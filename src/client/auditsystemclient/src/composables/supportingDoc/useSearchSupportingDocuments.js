import { ref } from 'vue';
import { searchSupportingDocumentRepository } from '@/services/supportingDocs/searchSupportingDocument';

export function useSearch() {
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
      const response = await searchSupportingDocumentRepository.searchSupportingDocumentsByUserId(
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

  const goToPage = async (pageNumber) => {
    if (pageNumber >= 1 && pageNumber <= Math.ceil(totalCount.value / pageSize.value)) {
      return await searchSupportingDocuments(searchTerm.value, pageNumber, pageSize.value);
    }
  };

  const nextPage = async () => {
    const nextPageNumber = currentPage.value + 1;
    if (nextPageNumber <= Math.ceil(totalCount.value / pageSize.value)) {
      return await goToPage(nextPageNumber);
    }
  };

  const previousPage = async () => {
    const prevPageNumber = currentPage.value - 1;
    if (prevPageNumber >= 1) {
      return await goToPage(prevPageNumber);
    }
  };

  const changePageSize = async (newSize) => {
    pageSize.value = newSize;
    currentPage.value = 1; // Reset to first page when changing page size
    return await searchSupportingDocuments(searchTerm.value, 1, newSize);
  };

  return {
    // State
    searchResults,
    isSearching,
    searchError,
    totalCount,
    currentPage,
    pageSize,
    searchTerm,
    
    // Methods
    searchSupportingDocuments,
    clearSearch,
    goToPage,
    nextPage,
    previousPage,
    changePageSize
  };
}