import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';

const DELETE_ENDPOINT = 'api/supportingdocs';

export const deleteSupportingDocumentRepository = {
  async deleteSupportingDocument(documentId, token) {
    try {
      const response = await api.delete(DELETE_ENDPOINT, {
        data: { id: documentId },
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (response.status === 204 || response?.data?.success !== false) {
        return ApiResponseT.ok(null, 'Document deleted successfully');
      }

      const errors = Array.isArray(response?.data?.errors)
        ? response.data.errors
        : [{ message: response?.data?.message || 'Delete failed' }];
      return ApiResponseT.badRequest(errors, response?.data?.message);

    } catch (error) {
      console.error('Delete error:', error);
      let errorMessage = 'Delete failed';
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