// src/composables/supportingDoc/useFetchSupportingDocuments.js

import { ref } from 'vue';
// Assuming the repository file will be kept as is, or adjust path if it's renamed/moved.
import { supportingDocsRepository } from '@/services/supportingDocs/fetchSupportingDocumentsRepository';

export function useFetchSupportingDocuments() {
  const documents = ref([]);
  const isLoading = ref(false);
  const error = ref(null);

  const fetchDocumentsByUserId = async (userId) => {
    isLoading.value = true;
    error.value = null;
    documents.value = []; // Clear previous documents

    try {
      const response = await supportingDocsRepository.getSupportingDocumentsByUserId(userId);

      if (response && response.success) {
        documents.value = response.result || [];
      } else {
        error.value = response?.message || response?.errors?.[0]?.message || 'Failed to fetch supporting documents.';
        documents.value = []; // Ensure documents are empty on error
      }
    } catch (err) {
      console.error('Error in useFetchSupportingDocuments:', err);
      error.value = err.message || 'An unexpected error occurred while fetching documents.';
      documents.value = []; // Ensure documents are empty on critical error
    }
    finally {
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