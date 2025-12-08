import { ref } from "vue";
import { updateJobTitleByIdRepository } from "@/services/organization/JobTitle/updateJobTitleByIdRepository";

export function useUpdateJobTitleById() {
  const isUpdating = ref(false);
  const error = ref(null);

  const updateJobTitle = async (jobTitleData) => {
    if (!jobTitleData.id) {
      const errorMsg = "Job Title ID is required";
      error.value = errorMsg;
      return { success: false, message: errorMsg };
    }

    isUpdating.value = true;
    error.value = null;

    try {
      // Note: userId is automatically injected due to UserAware pattern
      const result = await updateJobTitleByIdRepository.updateJobTitleById(
        jobTitleData
      );

      if (result.success) {
        return {
          success: true,
          message: result.message || "Job Title updated successfully",
          data: result.result,
        };
      } else {
        const errorMsg = result.message || "Failed to update Job Title";
        error.value = errorMsg;
        return { success: false, message: errorMsg, errors: result.errors };
      }
    } catch (err) {
      const errorMsg =
        err.message || "An error occurred while updating Job Title";
      error.value = errorMsg;
      return { success: false, message: errorMsg };
    } finally {
      isUpdating.value = false;
    }
  };

  const clearState = () => {
    error.value = null;
  };

  return {
    isUpdating,
    error,
    updateJobTitle,
    clearState,
  };
}
