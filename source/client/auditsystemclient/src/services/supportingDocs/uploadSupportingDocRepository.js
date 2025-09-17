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

      const { data, status } = response;
      
      if (status !== 200) {
        const errors = Array.isArray(data?.errors)
          ? data.errors
          : [{ message: data?.successMessage || 'Upload failed' }];
        return ApiResponseT.badRequest(errors);
      }

      return ApiResponseT.ok(data?.result, data?.successMessage);

    } catch (error) {
      let errorMessage = 'Upload failed';
      let errors = [{ message: errorMessage }];

      if (error?.response) {
        errorMessage = error.response.data?.successMessage || error.response.statusText || errorMessage;
        errors = Array.isArray(error.response.data?.errors)
          ? error.response.data.errors
          : [{ message: errorMessage }];
      } else if (error?.message) {
        errorMessage = error.message;
        errors = [{ message: errorMessage }];
      }

      return ApiResponseT.badRequest(errors);
    }
  }
};