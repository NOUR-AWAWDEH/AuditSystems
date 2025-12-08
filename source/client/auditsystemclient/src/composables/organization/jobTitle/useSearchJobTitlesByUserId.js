import { ref } from "vue";
import { searchJobTitlesByUserIdRepository } from "@/services/organization/JobTitle/searchJobTitlesByUserIdRepository";

export function useSearchJobTitlesByUserId() {
  const jobTitles = ref([]);
  const isLoading = ref(false);
  const error = ref(null);
  const totalCount = ref(0);
  const currentPage = ref(1);
  const pageSize = ref(10);
  const totalPages = ref(0);

  const searchJobTitles = async (searchParams = {}) => {
    isLoading.value = true;
    error.value = null;

    try {
      const params = {
        searchTerm: searchParams.searchTerm || "",
        pageNumber: searchParams.pageNumber || currentPage.value,
        pageSize: searchParams.pageSize || pageSize.value,
      };

      const result = await searchJobTitlesByUserIdRepository.searchJobTitlesByUserId(params);

      if (result.success) {
        jobTitles.value = result.result?.items || [];
        totalCount.value = result.result?.totalCount || 0;
        currentPage.value = result.result?.currentPage || 1;
        pageSize.value = result.result?.pageSize || 10;
        totalPages.value = result.result?.totalPages || 0;
      } else {
        const errorMsg = result.message || "Failed to search job titles";
        error.value = errorMsg;
        console.error("Failed to search job titles:", errorMsg);
      }
    } catch (err) {
      error.value = err.message || "An error occurred while searching job titles";
      console.error("Error searching job titles:", err);
    } finally {
      isLoading.value = false;
    }
  };

  return {
    jobTitles,
    isLoading,
    error,
    totalCount,
    currentPage,
    pageSize,
    totalPages,
    searchJobTitles,
  };
}
