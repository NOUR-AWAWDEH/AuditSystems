import { watch, onMounted } from "vue";
import { useSearchJobTitles } from "@/composables/organization/jobTitle/useSearchJobTitles";
import { useGetJobTitlesByUserId } from "@/composables/organization/jobTitle/useGetJobTitlesByUserId";
import { useSearchAndPagination } from "@/composables/common/pagination/useSearchAndPagination";

export function useJobTitleTable() {
  const {
    jobTitles,
    isLoading,
    totalCount,
    searchJobTitles,
  } = useSearchJobTitles();

  const { jobTitles: allJobTitles, fetchJobTitles } = useGetJobTitlesByUserId();

  const { searchQuery, currentPage, pageSize, handleSearch } = useSearchAndPagination();

  const refreshData = async () => {
    if (searchQuery.value.trim()) {
      await searchJobTitles({
        searchTerm: searchQuery.value,
        pageNumber: currentPage.value,
        pageSize: pageSize.value
      });
    } else {
      await fetchJobTitles();
      jobTitles.value = allJobTitles.value || [];
    }
  };

  onMounted(() => {
    refreshData();
  });

  watch([searchQuery, currentPage], async () => {
    await refreshData();
  });

  return {
    jobTitles,
    isLoading,
    searchQuery,
    currentPage,
    pageSize,
    totalCount,
    handleSearch,
    refreshData,
  };
}
