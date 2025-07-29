import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const SUPPORTING_DOCS_BASE_URL = 'api/supportingdocs';
const SUPPORTING_DOCS_GET_BY_ID_ENDPOINT = '/get-supporting-doc-url-by-id';


export const getSupportingDocSignedUrlRepository = {
  
  async getSupportingDocSignedUrl(fileId) {
    
    try {
      const token = authService.getLocalToken();
      
      const url = `${SUPPORTING_DOCS_BASE_URL}${SUPPORTING_DOCS_GET_BY_ID_ENDPOINT}`;
      
      const response = await api.get(url, {
        params: { id: fileId },
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
            
      if (response.status === 200 && response?.success) {
        return ApiResponseT.ok(response.result, response?.data?.successMessage || 'Signed URL retrieved successfully.');
      }
      
      return ApiResponseT.badRequest(
        [{ message: 'Failed to get signed URL.' }],
        'Failed to get signed URL.'
      );
    } catch (error) {
      
      let errorMessage = 'Failed to get signed URL.';
      let errors = [{ message: errorMessage }];

      if (error?.response) {
        errorMessage = error.response?.data?.successMessage || errorMessage;
        errors = error.response?.errors && Array.isArray(error.response?.errors) && error.response?.errors.length > 0
          ? error.response?.errors
          : [{ message: errorMessage }];
      } else if (error?.message) {
        errorMessage = error.message;
        errors = [{ message: errorMessage }];
      }
     
      return ApiResponseT.badRequest(errors, errorMessage);
    }
  }
};
