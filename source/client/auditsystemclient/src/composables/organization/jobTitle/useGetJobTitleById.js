import { ref } from "vue";
import { getJobTitleByIdRepository } from "@/services/organization/JobTitle/getJobTitleByIdRepository";

export function useGetJobTitleById() {
  const jobTitle = ref(null);
  const isLoading = ref(false);
  const error = ref(null);

  const getJobTitleById = async (id) => {
    isLoading.value = true;
    error.value = null;
    jobTitle.value = null;

    try {
      const result = await getJobTitleByIdRepository.getJobTitleById(id);

      if (result.success) {
        jobTitle.value = result.result;
        return { success: true, data: result.result };
      } else {
        const errorMsg = result.message || "Failed to fetch job title";
        error.value = errorMsg;
        console.error("Failed to fetch job title:", errorMsg);
        return { success: false, message: errorMsg };
      }
    } catch (err) {
      const errorMsg = err.message || "An error occurred while fetching job title";
      error.value = errorMsg;
      console.error("Error fetching job title:", err);
      return { success: false, message: errorMsg };
    } finally {
      isLoading.value = false;
    }
  };

  return {
    jobTitle,
    isLoading,
    error,
    getJobTitleById,
  };
}
