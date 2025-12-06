import { ref } from "vue";
import { createJobTitleRepository } from "@/services/organization/JobTitle/createJobTitleRepository";

export function useCreateJobTitle() {
  const isCreating = ref(false);
  const error = ref(null);

  const createJobTitle = async (jobTitleData) => {
    isCreating.value = true;
    error.value = null;

    try {
      const result = await createJobTitleRepository.createJobTitle(jobTitleData);

      if (result.success) {
        return { success: true, data: result.result };
      } else {
        const errorMsg = result.message || "Failed to create job title";
        error.value = errorMsg;
        return { success: false, message: errorMsg, errors: result.errors };
      }
    } catch (err) {
      const errorMsg = err.message || "An error occurred while creating job title";
      error.value = errorMsg;
      console.error("Error creating job title:", err);
      return { success: false, message: errorMsg };
    } finally {
      isCreating.value = false;
    }
  };

  return {
    isCreating,
    error,
    createJobTitle,
  };
}
