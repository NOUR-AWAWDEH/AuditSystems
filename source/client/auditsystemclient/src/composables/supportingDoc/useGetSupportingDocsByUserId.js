import { ref } from 'vue';
import { getSupportingDocsByUserIdRepository } from '@/services/supportingDocs/getSupportingDocsByUserIdRepository';

export function useGetSupportingDocsByUserId() {
  const documents = ref([]);
  const isLoading = ref(false);
  const error = ref(null);

  const fetchDocumentsByUserId = async (userId) => {
    isLoading.value = true;
    error.value = null;
    documents.value = [];

    try {
      const response = await getSupportingDocsByUserIdRepository.getSupportingDocsByUserId(userId);

      if (response && response.success) {
        documents.value = response.result || [];
        return response;
      } else {
        error.value = response?.message || response?.errors?.[0]?.message || 'Failed to fetch supporting documents.';
        documents.value = [];
        return response;
      }
    } catch (err) {
      error.value = err.message || 'An unexpected error occurred while fetching documents.';
      documents.value = [];
      return { success: false, message: err.message || 'An unexpected error occurred while fetching documents.' };
    } finally {
      isLoading.value = false;
    }
  };

  return {
    documents,
    isLoading,
    error,
    fetchDocumentsByUserId,
  };
}