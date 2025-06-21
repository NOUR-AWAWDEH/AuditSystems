// src/services/download/decodeDownloadRepository.js

import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const DECODE_DOWNLOAD_BASE_ENDPOINT = 'DecodeDownload';

export const decodeDownloadRepository = {
  async downloadFileFromSignedUrl(signedUrl) {
    try {
      const token = authService.getLocalToken();

      const config = {
        params: { signedUrl },
        responseType: 'blob',
        headers: {
          'Authorization': `Bearer ${token}`,
        }
      };

      const response = await api.get(DECODE_DOWNLOAD_BASE_ENDPOINT, config);

      if (response.data && response.status === 200) {
        let filename = null;

        // Try headers directly
        filename = response.headers.filename ||
                   response.headers.Filename ||
                   response.headers['filename'] ||
                   response.headers['Filename'];

        // Try Content-Disposition
        if (!filename) {
          const contentDispositionHeader = Object.entries(response.headers)
            .find(([key]) => key.toLowerCase() === 'content-disposition');

          if (contentDispositionHeader) {
            const contentDisposition = contentDispositionHeader[1];
            const match = contentDisposition.match(/filename=([^;]+)/i);
            if (match) {
              filename = match[1].trim().replace(/^["']|["']$/g, '');
            }
          }
        }

        // Try to extract from signed URL
        if (!filename) {
          try {
            const urlParams = new URLSearchParams(new URL(signedUrl).search);
            const fileParam = urlParams.get('file');
            if (fileParam) {
              const decodedPath = decodeURIComponent(fileParam);
              const pathParts = decodedPath.split(/[\\/]/);
              const rawFilename = pathParts[pathParts.length - 1];
              filename = rawFilename;
            }
          } catch (err) {
            // Silent error handling
          }
        }

        if (!filename) {
          return ApiResponseT.badRequest(
            [{ message: 'No filename found in response headers or URL' }],
            'No filename found in response headers or URL'
          );
        }

        // Final clean-up of the filename
        filename = getCleanFileName(filename);

        // Download logic
        const blob = new Blob([response.data], {
          type: response.headers['content-type'] || 'application/octet-stream'
        });

        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);

        return ApiResponseT.ok({ filename }, 'File downloaded successfully');
      } else {
        return ApiResponseT.badRequest(
          [{ message: response.data?.message || 'Failed to download file.' }],
          response.data?.message || 'Failed to download file.'
        );
      }
    } catch (error) {
      let errorMessage = 'Failed to download file.';
      let errors = [{ message: errorMessage }];

      if (error?.response?.data) {
        errorMessage = error.response.data.message || errorMessage;
        errors = error.response.data.errors?.length
          ? error.response.data.errors
          : [{ message: errorMessage }];
      } else if (error?.message) {
        errorMessage = error.message;
        errors = [{ message: errorMessage }];
      }

      return ApiResponseT.badRequest(errors, errorMessage);
    }
  }
};

// 🔧 Utility function to clean the filename
function getCleanFileName(fileName) {
  if (!fileName) return 'Unknown';

  // Match filename_32charhash.ext => filename.ext
  const hashPattern = /^(.+)_([a-f0-9]{32})(\.[^.]+)$/i;
  const hashMatch = fileName.match(hashPattern);
  if (hashMatch) return hashMatch[1] + hashMatch[3];

  // Match GUID_Category_Filename_Hash.ext => Filename.ext
  const detailedPattern = /^[a-f0-9-]+_\w+_(.+?)_[a-f0-9]+\.(\w+)$/i;
  const detailedMatch = fileName.match(detailedPattern);
  if (detailedMatch) return `${detailedMatch[1]}.${detailedMatch[2]}`;

  return fileName;
}
