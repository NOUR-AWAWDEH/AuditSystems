import { watch, onMounted } from "vue";
import { useSearchUsersByAdminId } from "@/composables/userManagement/users/useSearchUsersByAdminId";
import { useGetUsersByAdminId } from "@/composables/userManagement/users/useGetUsersByAdminId";
import { useSearchAndPagination } from "@/composables/common/pagination/useSearchAndPagination";

export function useUserTable() {
  const { users, isLoading, totalCount, searchUsers } = useSearchUsersByAdminId();

  const { users: allUsers, getUsersByAdminId } = useGetUsersByAdminId();

  const { searchQuery, currentPage, pageSize, handleSearch } = useSearchAndPagination();

  const refreshData = async () => {
    if (searchQuery.value.trim()) {
      await searchUsers({
        searchTerm: searchQuery.value,
        pageNumber: currentPage.value,
        pageSize: pageSize.value,
      });
    } else {
      await getUsersByAdminId();
      users.value = allUsers.value || [];
    }
  };

  onMounted(() => {
    refreshData();
  });

  watch([searchQuery, currentPage], async () => {
    await refreshData();
  });

  return {
    users,
    isLoading,
    searchQuery,
    currentPage,
    pageSize,
    totalCount,
    handleSearch,
    refreshData,
  };
}
