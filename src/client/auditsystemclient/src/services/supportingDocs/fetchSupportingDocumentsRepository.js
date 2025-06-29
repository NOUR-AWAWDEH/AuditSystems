import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUPPORTING_DOCS_BASE_ENDPOINT = 'api/supportingdocs';

export const supportingDocsRepository = {
  async getSupportingDocumentsByUserId() {
    try {
      const token = authService.getLocalToken();
      
      const response = await api.get(`${SUPPORTING_DOCS_BASE_ENDPOINT}/get-supporting-docs-by-user-id`, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      if (response.data && response.status === 200) {
        if (response.data.success && response.data.result) {
            const documents = response.data.result.supportingDocs || [];
            return ApiResponseT.ok(documents, response.data.message || 'Successfully retrieved supporting documents.');
        } else if (response.data.success === false) {
            return ApiResponseT.badRequest(response.data.errors || [{ message: response.data.message || 'Failed to retrieve documents.' }], response.data.message || 'Failed to retrieve documents.');
        }

        return ApiResponseT.ok([], 'Successfully retrieved supporting documents, but response structure might differ.');
      } else {
        return ApiResponseT.badRequest(
          [{ message: response.data?.message || 'Failed to retrieve supporting documents.' }],
          response.data?.message || 'Failed to retrieve supporting documents.'
        );
      }
    } catch (error) {
      let errorMessage = 'Failed to retrieve supporting documents.';
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