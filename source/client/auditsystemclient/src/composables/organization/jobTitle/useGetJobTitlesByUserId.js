import { ref } from "vue";
import { getJobTitlesByUserIdRepository } from "@/services/organization/JobTitle/getJobTitlesByUserIdRepository";

export function useGetJobTitlesByUserId() {
  const jobTitles = ref([]);
  const isLoading = ref(false);
  const error = ref(null);

  const fetchJobTitles = async () => {
    isLoading.value = true;
    error.value = null;

    try {
      const result = await getJobTitlesByUserIdRepository.getJobTitlesByUserId();

      if (result.success) {
        jobTitles.value = result.result?.items || [];
      } else {
        const errorMsg = result.message || "Failed to fetch job titles";
        error.value = errorMsg;
        console.error("Failed to fetch job titles:", errorMsg);
      }
    } catch (err) {
      error.value = err.message || "An error occurred while fetching job titles";
      console.error("Error fetching job titles:", err);
    } finally {
      isLoading.value = false;
    }
  };

  return {
    jobTitles,
    isLoading,
    error,
    fetchJobTitles,
  };
}
