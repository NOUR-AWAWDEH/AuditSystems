import { ref } from "vue";
import { searchUsersByAdminIdRepository } from "@/services/userManagement/users/searchUsersByAdminIdRepository";

export function useSearchUsersByAdminId() {
  const searchResults = ref([]);
  const isSearching = ref(false);
  const error = ref(null);
  const totalCount = ref(0);
  const currentPage = ref(1);
  const pageSize = ref(10);
  const searchTerm = ref("");

  const handleSearchSuccess = (response) => {
    const items = response.result?.items || [];
    const extractedTotalCount = response.result?.totalCount || 0;

    const isValidArray = Array.isArray(items);

    searchResults.value = isValidArray ? items : [];
    totalCount.value = extractedTotalCount;

    return { success: true, data: response.result };
  };

  const handleSearchFailure = (response, defaultErrorMessage = "Failed to search users") => {
    const errorMessage = response?.errors?.length
      ? response?.errors[0]?.message
      : response?.message || defaultErrorMessage;

    error.value = errorMessage;

    return {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };
  };

  const searchUsers = async (searchParams = {}) => {
    isSearching.value = true;
    error.value = null;

    try {
      const params = {
        searchTerm: searchParams.searchTerm || "",
        pageNumber: searchParams.pageNumber || currentPage.value,
        pageSize: searchParams.pageSize || pageSize.value,
      };

      if (searchParams.searchTerm !== undefined) searchTerm.value = searchParams.searchTerm;
      if (searchParams.page !== undefined) currentPage.value = searchParams.page;
      if (searchParams.pageSize !== undefined) pageSize.value = searchParams.pageSize;

      const response = await searchUsersByAdminIdRepository.searchUsersByAdminId(params);

      if (response?.success) {
        handleSearchSuccess(response);
      } else {
        handleSearchFailure(response);
      }
    } catch (err) {
      console.error("Error searching users:", err);
      return handleSearchFailure(err, err.message || "Failed to search users");
    } finally {
      isSearching.value = false;
    }
  };

  const clearSearch = () => {
    searchResults.value = [];
    searchTerm.value = "";
    currentPage.value = 1;
    error.value = null;
    totalCount.value = 0;
    pageSize.value = 10;
  };

  const resetState = () => {
    searchResults.value = [];
    isSearching.value = false;
    error.value = null;
    totalCount.value = 0;
    currentPage.value = 1;
    searchTerm.value = "";
  };

  return {
    searchResults,
    isSearching,
    error,
    totalCount,
    currentPage,
    pageSize,
    searchTerm,

    // Actions
    searchUsers,
    clearSearch,
    resetState,
  };
}
