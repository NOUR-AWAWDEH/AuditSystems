import { ref, watch, computed } from "vue";
import { useSearchUsersByAdminId } from "@/composables/userManagement/users/useSearchUsersByAdminId";
import { useGetUsersByAdminId } from "@/composables/userManagement/users/useGetUsersByAdminId";
import { useSearchAndPagination } from "@/composables/common/pagination/useSearchAndPagination";

export function useUserTable() {
  const {
    searchResults: searchUsersList,
    isSearching,
    error: searchError,
    totalCount,
    searchUsers,
  } = useSearchUsersByAdminId();

  const {
    users: allUsers,
    isLoading: isGetLoading,
    error: getError,
    getUsersByAdminId,
  } = useGetUsersByAdminId();

  const { searchQuery, currentPage, pageSize, handleSearch, resetPagination } =
    useSearchAndPagination();

  const displayedUsers = ref([]);

  const isLoading = computed(() => isSearching.value || isGetLoading.value);
  const error = computed(() => searchError.value || getError.value);

  const refreshData = async () => {
    const query = searchQuery.value?.trim();

    if (query) {
      await searchUsers({
        searchTerm: query,
        pageNumber: currentPage.value,
        pageSize: pageSize.value,
      });
      displayedUsers.value = searchUsersList.value || [];
    } else {
      await getUsersByAdminId();
      displayedUsers.value = allUsers.value || [];
    }
  };

  const resetToDefaultView = () => {
    resetPagination();
    refreshData();
  };

  watch(
    [searchQuery, currentPage, pageSize],
    () => {
      refreshData();
    },
    { immediate: true }
  );

  return {
    users: displayedUsers,
    isLoading,
    error,
    totalCount,
    searchQuery,
    currentPage,
    pageSize,
    handleSearch,
    refreshData,
    resetToDefaultView,
  };
}
