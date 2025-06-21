import { ref } from 'vue';
import { deleteSupportingDocumentRepository } from '@/services/supportingDocs/deleteSupportingDocumentRepository';
import authService from '@/services/auth/authRepos/authService';

export function useDeleteSupportingDocument() {
  const isDeleting = ref(false);
  const error = ref(null);
  const deleteStatus = ref(null);

  const handleDeleteSuccess = () => {
    deleteStatus.value = { success: true, message: 'Document deleted successfully' };
    return { success: true, message: 'Document deleted successfully' };
  };

  const handleDeleteFailure = (response, defaultErrorMessage = 'Delete failed') => {
    const errorMessage = response?.errors?.length
      ? response.errors[0].message
      : defaultErrorMessage;
    error.value = errorMessage;
    deleteStatus.value = {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };
    return deleteStatus.value;
  };

  const deleteSupportingDocument = async (documentId) => {
    try {
      isDeleting.value = true;
      error.value = null;
      deleteStatus.value = null;

      const token = authService.getLocalToken();
      if (!token) {
        throw new Error('No authentication token found');
      }

      const response = await deleteSupportingDocumentRepository.deleteSupportingDocument(documentId, token);
      
      if (response?.success) {
        return handleDeleteSuccess();
      } else {
        return handleDeleteFailure(response);
      }
    } catch (err) {
      return handleDeleteFailure(err, err.message || 'Delete failed');
    } finally {
      isDeleting.value = false;
    }
  };

  return {
    isDeleting,
    error,
    deleteStatus,
    deleteSupportingDocument,
  };
}