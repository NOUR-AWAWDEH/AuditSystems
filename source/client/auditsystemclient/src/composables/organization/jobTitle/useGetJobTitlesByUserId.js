import { ref } from "vue";
import { getJobTitlesByUserIdRepository } from "@/services/organization/JobTitle/getJobTitlesByUserIdRepository";

export function useGetJobTitlesByUserId() {

  const jobTitles = ref([]);
  const isLoading = ref(false);
  const error = ref(null);

  const getJobTitlesByUserId = async () => {
    isLoading.value = true;
    error.value = null;

    try {

      const result = await getJobTitlesByUserIdRepository.getJobTitlesByUserId();

      if (result.success) {
        jobTitles.value = result.result || [];
      } else {
        const errorMsg = result.message || "Failed to fetch job titles";
        error.value = errorMsg;
        console.error("Failed to fetch job titles:", errorMsg);
        console.error("Full error result:", result);
      }
    } catch (err) {
      error.value = err.message || "An error occurred while fetching job titles";
      console.error("Error fetching job titles:", err);
    } finally {
      isLoading.value = false;
    }
  };


  const addJobTitle = (newJobTitle) => {
    jobTitles.value.push(newJobTitle);
  };

  const removeJobTitle = (jobTitleId) => {
    jobTitles.value = jobTitles.value.filter((jobTitle) => jobTitle.id !== jobTitleId);
  };

  const updateJobTitle = (updatedJobTitle) => {
    const index = jobTitles.value.findIndex((jobTitle) => jobTitle.id === updatedJobTitle.id);
    if (index !== -1) {
      jobTitles.value[index] = updatedJobTitle;
    }
  };

  return {
    jobTitles,
    isLoading,
    error,
    getJobTitlesByUserId,
    addJobTitle,
    removeJobTitle,
    updateJobTitle
  };
}
