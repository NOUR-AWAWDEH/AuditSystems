import { ref } from "vue";
import { updateJobTitleByIdRepository } from "@/services/organization/JobTitle/updateJobTitleByIdRepository";

export function useUpdateJobTitleById() {
  const isUpdating = ref(false);
  const error = ref(null);
  const updatedJobTitle = ref(null);

  const handleUpdateSuccess = async (response) => {
    updatedJobTitle.value = response.result;
    return { success: true, data: response.result }
  };

  const handleUpdateFailure = (response, defaultErrorMessage = 'Failed to update job title') => {
        const errorMessage = response?.errors?.length > 0
            ? response.errors[0]?.message
            : response?.successMessage || defaultErrorMessage;

        error.value = errorMessage;

        return {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const validateUpdateJobTitle = (jobTitleData) => {
        if (!jobTitleData.id) {
            return 'Job Title ID is required';
        }

        if (!jobTitleData.name) {
            return 'Job Title name is required';
        }

        if (!jobTitleData.companyId) {
            return 'Company ID is required';
        }

        return null;
    };

    const updateJobTitle = async (jobTitleData) => {
        try {
            isUpdating.value = true;
            error.value = null;
            updateJobTitle.value = null;

            const validationError = validateUpdateJobTitle(jobTitleData);
            if (validationError) {
                return handleUpdateFailure({ message: validationError });
            }

            const response = await updateJobTitleByIdRepository.updateJobTitleById(jobTitleData);

            if (response?.success) {
                return handleUpdateSuccess(response);
            } else {
                return handleUpdateFailure(response);
            }

        } catch (error) {
            return handleUpdateFailure(error, error.message || 'Failed to update JobTitle');
        } finally {
            isUpdating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        updateJobTitle.value = null;
        isUpdating.value = false;
    };

    return {
        isUpdating,
        error,
        updatedJobTitle,
        updateJobTitle,
        resetState,
    };
};
