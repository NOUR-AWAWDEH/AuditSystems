import { ref } from 'vue';
import { searchSubLocationByUserIdRepository } from '@/services/organization/sublocation/searchSubLocationByUserIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useSearchSubLocationByUserId() {

// Create singleton state outside the function
const searchResults = ref([]);
const isSearching = ref(false);
const error = ref(null);
const totalCount = ref(0);
const currentPage = ref(1);
const pageSize = ref(10);
const searchTerm = ref(''); // Added missing searchTerm
const { showNotification } = useNotification();

const locationId = ref(localStorage.getItem('searchLocationId') || null);

  // Added missing handleSearchFailure function
  const handleSearchFailure = (response, defaultErrorMessage = 'Failed to search sublocations') => {
    const errorMessage = response?.errors?.length
      ? response.errors[0].message
      : response?.message || defaultErrorMessage;
    
    error.value = errorMessage;
    showNotification(errorMessage, 'error');
    
    return {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };
  };

  const searchSubLocations = async (params = {}) => {
      const {
          searchTerm = '',
          locationId = null,
          page = 1,
          pageSize: pageSizeParam = 10
      } = params;
      
      isSearching.value = true;
      error.value = null;
      currentPage.value = page;
      pageSize.value = pageSizeParam;
  
      try {
          // Create the search parameters object for the repository
          const searchParams = {
              searchTerm,
              locationId,
              page,
              pageSize: pageSizeParam
          };
          
          const result = await searchSubLocationByUserIdRepository.searchSubLocationsByUserId(
              searchParams
          );
          

          if (result.success) {
            const subLocations = result.result?.items || result.result || [];
            const fetchedTotalCount = result.result?.totalCount || 0;
  
            searchResults.value = subLocations;
            totalCount.value = fetchedTotalCount;
  
            return { 
              success: true, 
              message: result.message || 'Search completed successfully',
              data: {
                subLocations: searchResults.value,
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
              
              if (result.errors && result.errors.length > 0) {
                  showNotification(errorMsg, 'error');
              }
              
              return { success: false, message: errorMsg, errors: result.errors };
          }
      } catch (err) {
        return handleSearchFailure(err, err.message || 'Failed to search sublocations');
      } finally {
        isSearching.value = false;
      }
  };

  const clearSearch = () => {
    searchResults.value = [];
    searchTerm.value = '';
    currentPage.value = 1;
    error.value = null;
    totalCount.value = 0;
  };

  const clearLocationFilter = () => {
    locationId.value = null;
    localStorage.removeItem('searchLocationId');
    searchResults.value = [];
  };

  const nextPage = () => {
    if (currentPage.value * pageSize.value < totalCount.value) {
      currentPage.value++;
      return searchSubLocations();
    }
  };

  const previousPage = () => {
    if (currentPage.value > 1) {
      currentPage.value--;
      return searchSubLocations();
    }
  };

  const goToPage = (page) => {
    if (page >= 1 && page <= Math.ceil(totalCount.value / pageSize.value)) {
      currentPage.value = page;
      return searchSubLocations();
    }
  };

  const resetState = () => {
    searchResults.value = [];
    isSearching.value = false;
    error.value = null;
    totalCount.value = 0;
    currentPage.value = 1;
    searchTerm.value = '';
    locationId.value = null;
    localStorage.removeItem('searchLocationId');
  };

  return {
    // State
    searchResults,
    isSearching,
    error,
    totalCount,
    currentPage,
    pageSize,
    searchTerm,
    locationId,

    // Actions
    searchSubLocations,
    clearSearch,
    clearLocationFilter,
    nextPage,
    previousPage,
    goToPage,
    resetState,
  };
}