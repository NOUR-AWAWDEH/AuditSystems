import { ref } from "vue";
import { deleteJobTitleByIdRepository } from "@/services/organization/JobTitle/deleteJobTitleByIdRepository";

export function useDeleteJobTitleById() {
  const isDeleting = ref(false);
  const error = ref(null);
  const deletedJobTitle = ref(null);

  const handleDeleteSuccess = (response) => {
    deletedJobTitle.value = true;
    const message = response?.message || "Job Title deleted successfully";
    return { success: true, message };
  };

  const handleDeleteFailure = (response, defaultErrorMessage = "Failed to delete JobTitle") => {
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

  const validateDeleteJobTitle = (jobTitleId) => {
    if (!jobTitleId) {
      return "jobTitle ID is required";
    }
    return null;
  };

  const deleteJobTitle = async (jobTitleId) => {
    try {
      isDeleting.value = true;
      error.value = null;
      deletedJobTitle.value = null;

      const validationError = validateDeleteJobTitle(jobTitleId);
      if (validationError) {
        return handleDeleteFailure({ message: validationError });
      }

      const response = await deleteJobTitleByIdRepository.deleteJobTitleById(jobTitleId);

      if (response?.success) {
        return handleDeleteSuccess(response);
      } else {
        return handleDeleteFailure(response);
      }
      
    } catch (error) {
      return handleDeleteFailure(error, error.message || "Failed to delete JobTitle");
    } finally {
      isDeleting.value = false;
    }
  };

  const resetState = () => {
    isDeleting.value = false;
    error.value = null;
    deleteJobTitle.value = null;
  };

  return {
    isDeleting,
    error,
    deletedJobTitle,
    deleteJobTitle,
    resetState
  };
}
