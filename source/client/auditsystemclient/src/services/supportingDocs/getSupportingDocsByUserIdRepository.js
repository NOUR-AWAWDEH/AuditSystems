import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUPPORTING_DOCS_BASE_ENDPOINT = 'api/supportingdocs';
const SUPPORTING_DOCS_GET_BY_USER_ID_ENDPOINT = '/get-supporting-docs-by-user-id';

export const getSupportingDocsByUserIdRepository = {
  async getSupportingDocsByUserId() {
    try {
      const token = authService.getLocalToken();
      
      const response = await api.get(
        `${SUPPORTING_DOCS_BASE_ENDPOINT}${SUPPORTING_DOCS_GET_BY_USER_ID_ENDPOINT}`, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      if (response && response.status === 200) {
        if (response.success && response.result) {
            const documents = response.result.supportingDocs || [];
            return ApiResponseT.ok(documents, response.successMessage || 'Successfully retrieved supporting documents.');
        } else if (response.success === false) {
            return ApiResponseT.badRequest(response.errors || [{ message: response.successMessage || 'Failed to retrieve documents.' }], response.successMessage || 'Failed to retrieve documents.');
        }

        return ApiResponseT.ok([], 'Successfully retrieved supporting documents, but response structure might differ.');
      } else {
        return ApiResponseT.badRequest(
          [{ message: response?.data?.successMessage || 'Failed to retrieve supporting documents.' }],
          response?.data?.successMessage || 'Failed to retrieve supporting documents.'
        );
      }
    } catch (error) {
      let errorMessage = 'Failed to retrieve supporting documents.';
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