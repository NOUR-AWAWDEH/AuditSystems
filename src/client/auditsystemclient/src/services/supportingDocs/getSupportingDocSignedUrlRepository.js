import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const SUPPORTING_DOCS_BASE_URL = 'api/supportingdocs';

export const getSupportingDocSignedUrlRepository = {
  
  async getSupportingDocSignedUrl(fileId) {
    
    try {
      const token = authService.getLocalToken();
      
      const url = `${SUPPORTING_DOCS_BASE_URL}/get-supporting-doc-url-by-id`;
      
      const response = await api.get(url, {
        params: { id: fileId },
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
            
      if (response.status === 200 && response.data?.success) {
        return ApiResponseT.ok(response.data.result, 'Signed URL retrieved successfully.');
      }
      
      return ApiResponseT.badRequest(
        [{ message: 'Failed to get signed URL.' }],
        'Failed to get signed URL.'
      );
    } catch (error) {
      
      let errorMessage = 'Failed to get signed URL.';
      let errors = [{ message: errorMessage }];

      if (error?.response?.data) {
        errorMessage = error.response.data.message || errorMessage;
        errors = error.response.data.errors && Array.isArray(error.response.data.errors) && error.response.data.errors.length > 0
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
