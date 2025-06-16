import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import axios from 'axios'; // Import axios directly

const UPLOAD_ENDPOINT = 'api/supportingdocs/upload-supporting-document';

export const supportingDocRepository = {
  async uploadSupportingDoc({ file, userId, fileName, token }) {
    try {
      // Prepare form data - EXACT parameter names matter!
      const formData = new FormData();
      formData.append('File', file);
      formData.append('UserId', userId);
      formData.append('FileName', fileName);

      // Use direct axios call to ensure proper headers
      const response = await axios({
        method: 'post',
        url: `${api.defaults.baseURL}/${UPLOAD_ENDPOINT}`,
        data: formData,
        headers: {
          'Content-Type': 'multipart/form-data',
          'Authorization': `Bearer ${token}`
        }
      });

      if (!response?.data?.success) {
        const errors = Array.isArray(response?.data?.errors)
          ? response.data.errors
          : [{ message: response?.data?.message || 'Upload failed' }];
        return ApiResponseT.badRequest(errors, response?.data?.message);
      }

      return ApiResponseT.ok(response.data.result, response.data.message);

    } catch (error) {
      // Detailed error logging
      console.error('Upload error details:');
      console.error('- Status:', error.response?.status);
      console.error('- StatusText:', error.response?.statusText);
      console.error('- Data:', error.response?.data);

      let errorMessage = 'Upload failed';
      let errors = [{ message: errorMessage }];

      if (error?.response?.data) {
        errorMessage = error.response.data.message || errorMessage;
        errors = Array.isArray(error.response.data.errors)
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
