import { ref } from "vue";
import { updateJobTitleByIdRepository } from "@/services/organization/JobTitle/updateJobTitleByIdRepository";

export function useUpdateJobTitle() {
  const isUpdating = ref(false);
  const error = ref(null);

  const updateJobTitle = async (jobTitleData) => {
    isUpdating.value = true;
    error.value = null;

    try {
      const result = await updateJobTitleByIdRepository.updateJobTitle(jobTitleData);

      if (result.success) {
        return { success: true };
      } else {
        const errorMsg = result.message || "Failed to update job title";
        error.value = errorMsg;
        return { success: false, message: errorMsg, errors: result.errors };
      }
    } catch (err) {
      const errorMsg = err.message || "An error occurred while updating job title";
      error.value = errorMsg;
      console.error("Error updating job title:", err);
      return { success: false, message: errorMsg };
    } finally {
      isUpdating.value = false;
    }
  };

  return {
    isUpdating,
    error,
    updateJobTitle,
  };
}
