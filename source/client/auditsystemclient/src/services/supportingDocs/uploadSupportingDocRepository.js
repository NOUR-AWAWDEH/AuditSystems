import { ApiResponseT } from '@/utils/ApiResponse';
import api from '@/services/api/api';

const SUPPORTING_DOCS_BASE_ENDPOINT = 'api/supportingdocs';
const UPLOAD_SUPPORTING_DOC_ENDPOINT = '/upload-supporting-document';

export const uploadSupportingDocRepository = {
  async uploadSupportingDoc({ file, userId, fileName, token }) {
    try {
      // Prepare form data - EXACT parameter names matter!
      const formData = new FormData();
      formData.append('File', file);
      formData.append('UserId', userId);
      formData.append('FileName', fileName);

      // Use custom api service with proper headers
      const response = await api.post(
        `${SUPPORTING_DOCS_BASE_ENDPOINT}${UPLOAD_SUPPORTING_DOC_ENDPOINT}`,
        formData,
        {
          headers: {
            'Content-Type': 'multipart/form-data',
            'Authorization': `Bearer ${token}`
          }
        }
      );

      if (!response?.success) {
        const errors = Array.isArray(response?.errors)
          ? response.errors
          : [{ message: response?.data?.successMessage || 'Upload failed' }];
        return ApiResponseT.badRequest(errors, response?.data?.successMessage);
      }

      return ApiResponseT.ok(response?.result, response?.data?.successMessage);

    } catch (error) {
      let errorMessage = 'Upload failed';
      let errors = [{ message: errorMessage }];

      if (error?.response) {
        errorMessage = error.response.successMessage || errorMessage;
        errors = Array.isArray(error.response.errors)
          ? error.response.errors
          : [{ message: errorMessage }];
      } else if (error?.message) {
        errorMessage = error.message;
        errors = [{ message: errorMessage }];
      }

      return ApiResponseT.badRequest(errors, errorMessage);
    }
  }
};
