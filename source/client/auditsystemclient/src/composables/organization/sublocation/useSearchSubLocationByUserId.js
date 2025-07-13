import { ref } from 'vue';
import { searchSubLocationByUserIdRepository } from '@/services/organization/sublocation/searchSubLocationByUserIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

// Create singleton state outside the function
const searchResults = ref([]);
const isSearching = ref(false);
const error = ref(null);
const totalCount = ref(0);
const currentPage = ref(1);
const pageSize = ref(10);
const searchTerm = ref('');
// Persist locationId in localStorage
const locationId = ref(localStorage.getItem('searchLocationId') || null);

export function useSearchSubLocationByUserId() {
  const { showNotification } = useNotification();

  const handleSearchSuccess = (response) => {
    const items = response.result?.result?.items || response.result?.items || [];
    const extractedTotalCount = response.result?.result?.totalCount || response.result?.totalCount || 0;
        
    const isValidArray = Array.isArray(items);
    
    searchResults.value = isValidArray ? items : [];
    totalCount.value = extractedTotalCount;
        
    return { success: true, data: response.result };
  };
  
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

  const searchSubLocations = async (searchParams = {}) => {
    try {
      isSearching.value = true;
      error.value = null;

      const params = {
        searchTerm: searchParams.searchTerm || searchTerm.value,
        locationId: searchParams.locationId || locationId.value,
        page: searchParams.page || currentPage.value,
        pageSize: searchParams.pageSize || pageSize.value
      };

      // Update local state and persist locationId
      if (searchParams.searchTerm !== undefined) searchTerm.value = searchParams.searchTerm;
      if (searchParams.locationId !== undefined) {
        locationId.value = searchParams.locationId;
        // Persist to localStorage
        if (searchParams.locationId) {
          localStorage.setItem('searchLocationId', searchParams.locationId);
        } else {
          localStorage.removeItem('searchLocationId');
        }
      }
      if (searchParams.page !== undefined) currentPage.value = searchParams.page;
      if (searchParams.pageSize !== undefined) pageSize.value = searchParams.pageSize;

      const response = await searchSubLocationByUserIdRepository.searchSubLocationsByUserId(params);

      if (response?.success) {
        return handleSearchSuccess(response);
      } else {
        return handleSearchFailure(response);
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
    // DON'T clear locationId here - it should persist
    // locationId.value = null;  // ← REMOVED
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
    clearLocationFilter, // New method to clear location specifically
    nextPage,
    previousPage,
    goToPage,
    resetState,
  };
}