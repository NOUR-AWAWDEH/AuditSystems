import { ref } from "vue";
import { createJobTitleRepository } from "@/services/organization/JobTitle/createJobTitleRepository";

export function useCreateJobTitle() {
  const isCreating = ref(false);
  const error = ref(null);
  const createdJobTitle = ref(null);

  const handleCreateSuccess = (response) => {
    createdJobTitle.value = response;
    return { success: true, data: response };
  };

  const handleCreateFailure = (response, defaultErrorMessage = "Failed to create location") => {
    const errorMessage = response?.errors?.length
      ? response.errors[0].message
      : response?.message || defaultErrorMessage;

    error.value = errorMessage;

    return {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };
  };

  const validateJobTitleData = (jobTitleData) => {
    if (!jobTitleData.name?.trim()) {
      return "job-Title name is required";
    }
    if (!jobTitleData.companyId) {
      return "Company ID is required";
    }
    return null;
  };

  const createJobTitle = async (jobTitleData) => {
    try {
      isCreating.value = true;
      error.value = null;
      createdJobTitle.value = null;

      // Validate input data
      const validationError = validateJobTitleData(jobTitleData);
      if (validationError) {
        return handleCreateFailure({ message: validationError });
      }

      const result = await createJobTitleRepository.createJobTitle(jobTitleData);

      if (result.success) {
        return handleCreateSuccess(result);
      } else {
        return handleCreateFailure(result);
      }

    } catch (err) {
      return handleCreateFailure(err, err.message || "Create failed");
    } finally {
      isCreating.value = false;
    }
  };

  const clearState = () => {
    isCreating.value = false;
    error.value = null;
    createdJobTitle.value = null;
  };

  return {
    isCreating,
    error,
    createdJobTitle,
    createJobTitle,
    clearState,
  };
}