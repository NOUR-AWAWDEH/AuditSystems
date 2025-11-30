import { ref } from 'vue';
import { searchSubDepartmentsByUserIdRepository } from '@/services/organization/subdepartment/searchSubDepartmentsByUserIdRepository';

// Create singleton state outside the function
const searchResults = ref([]);
const isSearching = ref(false);
const error = ref(null);
const totalCount = ref(0);
const currentPage = ref(1);
const pageSize = ref(10);
const searchTerm = ref('');
// Persist departmentId in localStorage
const departmentId = ref(localStorage.getItem('organizationSelectedDepartmentId') || null);

export function useSearchSubDepartmentsByUserId() {

  const handleSearchSuccess = (response) => {
    const items = response.result?.result?.items || response.result?.items || [];
    const extractedTotalCount = response.result?.result?.totalCount || response.result?.totalCount || 0;

    const isValidArray = Array.isArray(items);

    searchResults.value = isValidArray ? items : [];
    totalCount.value = extractedTotalCount;

    return { success: true, data: response.result };
  };

  const handleSearchFailure = (response, defaultErrorMessage = 'Failed to search subdepartments') => {
    const errorMessage = response?.errors?.length
      ? response.errors[0].message
      : response?.message || defaultErrorMessage;

    error.value = errorMessage;

    return {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };
  };

  const searchSubDepartments = async (searchParams = {}) => {
    try {
      isSearching.value = true;
      error.value = null;

      const params = {
        searchTerm: searchParams.searchTerm || searchTerm.value,
        departmentId: searchParams.departmentId || departmentId.value,
        page: searchParams.page || currentPage.value,
        pageSize: searchParams.pageSize || pageSize.value
      };

      // Update local state and persist departmentId
      if (searchParams.searchTerm !== undefined) searchTerm.value = searchParams.searchTerm;
      if (searchParams.departmentId !== undefined) {
        departmentId.value = searchParams.departmentId;
        // Persist to localStorage
        if (searchParams.departmentId) {
          localStorage.setItem('organizationSelectedDepartmentId', searchParams.departmentId);
        } else {
          localStorage.removeItem('organizationSelectedDepartmentId');
        }
      }
      if (searchParams.page !== undefined) currentPage.value = searchParams.page;
      if (searchParams.pageSize !== undefined) pageSize.value = searchParams.pageSize;

      const response = await searchSubDepartmentsByUserIdRepository.searchSubDepartmentsByUserId(params);

      if (response?.success) {
        return handleSearchSuccess(response);
      } else {
        return handleSearchFailure(response);
      }
    } catch (err) {
      return handleSearchFailure(err, err.message || 'Failed to search subdepartments');
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

  const clearDepartmentFilter = () => {
    departmentId.value = null;
    localStorage.removeItem('organizationSelectedDepartmentId');
    searchResults.value = [];
  };

  const nextPage = () => {
    if (currentPage.value * pageSize.value < totalCount.value) {
      currentPage.value++;
      return searchSubDepartments();
    }
  };

  const previousPage = () => {
    if (currentPage.value > 1) {
      currentPage.value--;
      return searchSubDepartments();
    }
  };

  const goToPage = (page) => {
    if (page >= 1 && page <= Math.ceil(totalCount.value / pageSize.value)) {
      currentPage.value = page;
      return searchSubDepartments();
    }
  };

  const resetState = () => {
    searchResults.value = [];
    isSearching.value = false;
    error.value = null;
    totalCount.value = 0;
    currentPage.value = 1;
    searchTerm.value = '';
    departmentId.value = null;
    localStorage.removeItem('organizationSelectedDepartmentId');
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
    departmentId,

    // Actions
    searchSubDepartments,
    clearSearch,
    clearDepartmentFilter,
    nextPage,
    previousPage,
    goToPage,
    resetState,
  };
}
