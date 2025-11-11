import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUPPORTING_DOCS_BASE_ENDPOINT = 'api/supportingdocs';
const SUPPORTING_DOCS_SEARCH_BY_USER_ID_ENDPOINT = '/search-supporting-docs-by-user-id';

export const searchSupportingDocsByUserIdRepository = {
  async searchSupportingDocsByUserId(searchTerm = '', pageNumber = 1, pageSize = 10) {
    try {
      const token = authService.getLocalToken();
      
      const response = await api.get(`${SUPPORTING_DOCS_BASE_ENDPOINT}${SUPPORTING_DOCS_SEARCH_BY_USER_ID_ENDPOINT}`, {
        params: {
          searchTerm,
          pageNumber,
          pageSize
        },
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      if (response && response.status === 200) {
        if (response.success && response.result) {
            return ApiResponseT.ok(response.result, response.successMessage || 'Successfully retrieved supporting documents.');
        } else if (response.success === false) {
            return ApiResponseT.badRequest(
                response?.errors || [{ message: 'Failed to search supporting documents.' }]
            );
        }

        return ApiResponseT.ok({ items: [], totalCount: 0, pageNumber: 1, pageSize: 10 }, 'Successfully retrieved supporting documents, but response structure might differ.');
      } else {
        return ApiResponseT.badRequest(
          [{ message: response?.successMessage || 'Failed to search supporting documents.' }]
        );
      }
    } catch (error) {
      let errorMessage = 'Failed to search supporting documents.';
      let errors = [{ message: errorMessage }];

      if (error?.response) {
        errorMessage = error.response?.data?.successMessage || error.response.statusText || errorMessage;
        errors = error.response?.data?.errors || [{ message: error.response?.data?.successMessage || 'Bad request' }];
        return ApiResponseT.badRequest(errors);
      } else if (error?.message) {
        errorMessage = error.message;
        errors = [{ message: errorMessage }];
      }
      return ApiResponseT.badRequest(errors);
    }
  }
};