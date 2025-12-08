import { ref } from "vue";
import { deleteJobTitleByIdRepository } from "@/services/organization/JobTitle/deleteJobTitleRepository";

export function useDeleteJobTitleById() {

  const isDeleting = ref(false);
  const error = ref(null);
  const deletedJobTitle = ref(null);

  const handleDeleteSuccess = (response) => {
    deletedLocation.value = true;
    const message = response?.message || "Location deleted successfully";
    return { success: true, message };
  };

  const handleDeleteFailure = (response, defaultErrorMessage = "Failed to delete location") => {
    const errorMessage =
      response?.errors?.length > 0
        ? response.errors[0]?.message
        : response?.successMessage || defaultErrorMessage;

    error.value = errorMessage;

    const failureResult = {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };

    return failureResult;
  };

  const deleteJobTitle = async (id) => {
    isDeleting.value = true;
    error.value = null;

    try {
      const result = await deleteJobTitleByIdRepository.deleteJobTitleById(id);

      if (result.success) {
        return { success: true };
      } else {
        const errorMsg = result.message || "Failed to delete job title";
        error.value = errorMsg;
        return { success: false, message: errorMsg, errors: result.errors };
      }
    } catch (err) {
      const errorMsg = err.message || "An error occurred while deleting job title";
      error.value = errorMsg;
      console.error("Error deleting job title:", err);
      return { success: false, message: errorMsg };
    } finally {
      isDeleting.value = false;
    }
  };

  return {
    isDeleting,
    error,
    deleteJobTitle,
  };
}
