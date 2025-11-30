import { ref } from 'vue';
import { searchLocationsByUserIdRepository } from '@/services/organization/location/searchLocationsByUserIdRepository';

export function useSearchLocationsByUserId() {

  const searchResults = ref([]);
  const isSearching = ref(false);
  const error = ref(null);
  const totalCount = ref(0);
  const currentPage = ref(1);
  const pageSize = ref(10);

  const searchLocations = async (searchTerm = '', pageNumber = 1, pageSizeParam = 10) => {
    isSearching.value = true;
    error.value = null;
    currentPage.value = pageNumber;
    pageSize.value = pageSizeParam;

    try {
        const result = await searchLocationsByUserIdRepository.searchLocationsByUserId(
            searchTerm, 
            pageNumber, 
            pageSizeParam
        );
        

        if (result.success) {
          const locations = result.result?.items || result.result || [];
          const fetchedTotalCount = result.result?.totalCount || 0;

          searchResults.value = locations;
          totalCount.value = fetchedTotalCount;

          return { 
            success: true, 
            message: result.message || 'Search completed successfully',
            data: {
              locations: searchResults.value,
              totalCount: totalCount.value,
              currentPage: currentPage.value,
              pageSize: pageSize.value
            }
          };
        } else {
            const errorMsg = result.message || 'Search failed';
            error.value = errorMsg;
            searchResults.value = [];
            totalCount.value = 0;
            return { success: false, message: errorMsg, errors: result.errors };
        }
    } catch (err) {
        const errorMsg = err.message || 'An error occurred during search';
        error.value = errorMsg;
        searchResults.value = [];
        totalCount.value = 0;
        return { success: false, message: errorMsg };
    } finally {
        isSearching.value = false;
    }
  };

  const clearSearch = () => {
    searchResults.value = [];
    error.value = null;
    totalCount.value = 0;
    currentPage.value = 1;
  };

  const nextPage = async (searchTerm = '') => {
    if (currentPage.value * pageSize.value < totalCount.value) {
      return await searchLocations(searchTerm, currentPage.value + 1, pageSize.value);
    }
    return { success: false, message: 'No more pages available' };
  };

  const previousPage = async (searchTerm = '') => {
    if (currentPage.value > 1) {
      return await searchLocations(searchTerm, currentPage.value - 1, pageSize.value);
    }
    return { success: false, message: 'Already on first page' };
  };

  return {
    searchResults,
    isSearching,
    error,
    totalCount,
    currentPage,
    pageSize,
    searchLocations,
    clearSearch,
    nextPage,
    previousPage
  };
}