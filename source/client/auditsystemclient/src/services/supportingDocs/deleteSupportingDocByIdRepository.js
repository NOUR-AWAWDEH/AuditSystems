import { ApiResponseT } from '@/utils/ApiResponse';
import api from '@/services/api/api';

const SUPPORTING_DOCS_BASE_ENDPOINT = 'api/supportingdocs';
const DELETE_SUPPORTIONG_DOC_BY_ID ='/delete-supporting-doc-by-id';

export const deleteSupportingDocByIdRepository = {
  async deleteSupportingDocById(documentId, token) {
    try {
      const response = await api.delete(`${SUPPORTING_DOCS_BASE_ENDPOINT}${DELETE_SUPPORTIONG_DOC_BY_ID}`, {
        data: { id: documentId },
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (response.status === 204 || response?.success !== false) {
        return ApiResponseT.ok(null, 'Document deleted successfully');
      }

      const errors = Array.isArray(response?.errors)
        ? response.errors
        : [{ message: response?.successMessage || 'Delete failed' }];
      return ApiResponseT.badRequest(errors);

    } catch (error) {
      let errorMessage = 'Delete failed';
      let errors = [{ message: errorMessage }];

      if (error?.response) {
        errorMessage = error.response?.data?.successMessage || error.response.statusText || errorMessage;
        errors = Array.isArray(error.response?.data?.errors)
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