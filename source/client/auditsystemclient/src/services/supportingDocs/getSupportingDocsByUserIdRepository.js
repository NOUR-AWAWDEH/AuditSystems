import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUPPORTING_DOCS_BASE_ENDPOINT = 'api/supportingdocs';
const SUPPORTING_DOCS_GET_BY_USER_ID_ENDPOINT = '/get-supporting-docs-by-user-id';

export const getSupportingDocsByUserIdRepository = {
  async getSupportingDocsByUserId(userId) {
    try {
      const token = authService.getLocalToken();
      
      const response = await api.get(
        `${SUPPORTING_DOCS_BASE_ENDPOINT}${SUPPORTING_DOCS_GET_BY_USER_ID_ENDPOINT}`, {
        params: { userId },
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      if (response && response.status === 200) {
        if (response.data?.success && response.data?.result) {
            const documents = response.data.result.supportingDocs || response.data.result || [];
            return ApiResponseT.ok(documents, response.data.successMessage || 'Successfully retrieved supporting documents.');
        } else if (response.data?.success === false) {
            return ApiResponseT.badRequest(response.data.errors || [{ message: response.data.successMessage || 'Failed to retrieve documents.' }]);
        }

        return ApiResponseT.ok([], 'Successfully retrieved supporting documents, but response structure might differ.');
      } else {
        return ApiResponseT.badRequest(
          [{ message: response?.data?.successMessage || 'Failed to retrieve supporting documents.' }]
        );
      }
    } catch (error) {
      console.error('Error fetching documents:', error);
      let errorMessage = 'Failed to retrieve supporting documents.';
      let errors = [{ message: errorMessage }];

      if (error?.response) {
        errorMessage = error.response?.data?.successMessage || error.response.statusText || errorMessage;
        errors = error.response?.data?.errors && Array.isArray(error.response?.data?.errors) && error.response?.data?.errors.length > 0
          ? error.response?.data?.errors
          : [{ message: errorMessage }];
      } else if (error?.message) {
        errorMessage = error.message;
        errors = [{ message: errorMessage }];
      }
      return ApiResponseT.badRequest(errors);
    }
  }
};