import { ref } from 'vue';
import { searchSupportingDocsByUserIdRepository } from '@/services/supportingDocs/searchSupportingDocsByUserIdRepository';

export function useSearchSupportingDocsByUserId() {
  const searchResults = ref([]);
  const isSearching = ref(false);
  const searchError = ref(null);
  const totalCount = ref(0);
  const currentPage = ref(1);
  const pageSize = ref(10);
  const searchTerm = ref('');

  const searchSupportingDocuments = async (term = '', pageNumber = 1, size = 10) => {
    isSearching.value = true;
    searchError.value = null;
    
    try {
      const response = await searchSupportingDocsByUserIdRepository.searchSupportingDocsByUserId(
        term,
        pageNumber,
        size
      );

      if (response && response.success) {
        searchResults.value = response.result?.items || [];
        totalCount.value = response.result?.totalCount || 0;
        currentPage.value = response.result?.pageNumber || pageNumber;
        pageSize.value = response.result?.pageSize || size;
        searchTerm.value = term;
        return response;
      } else {
        searchError.value = response?.message || response?.errors?.[0]?.message || 'Failed to search supporting documents.';
        searchResults.value = [];
        totalCount.value = 0;
        return response;
      }
    } catch (err) {
      searchError.value = err.message || 'An unexpected error occurred while searching documents.';
      searchResults.value = [];
      totalCount.value = 0;
      return { success: false, message: err.message || 'An unexpected error occurred while searching documents.' };
    } finally {
      isSearching.value = false;
    }
  };

  const clearSearch = () => {
    searchResults.value = [];
    searchError.value = null;
    totalCount.value = 0;
    currentPage.value = 1;
    searchTerm.value = '';
  };

  const resetPagination = () => {
    currentPage.value = 1;
  };

  return {
    searchResults,
    isSearching,
    searchError,
    totalCount,
    currentPage,
    pageSize,
    searchTerm,
    searchSupportingDocuments,
    clearSearch,
    resetPagination,
  };
}