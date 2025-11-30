import { ref } from 'vue';
import { getExitControlByIdRepository } from '@/services/riskControl/exitControl/getExitControlByIdRepository';

export function useGetExitControlById() {
    const exitControl = ref(null);
    const isLoading = ref(null);
    const error = ref(null);

    const handleFetchSuccess = (response) => {
        exitControl.value = response.result;
        return { success: true, data: exitControl.value };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch exit control!') => {
        const errorMessage = response?.errors?.length
            ? response?.errors[0]?.message
            : response?.message || defaultErrorMessage;

        error.value = errorMessage;

        return {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const validateExitControlId = (exitControlId) => {
        if (!exitControlId) {
            return 'Exit Control ID is required';
        }
        return null;
    };

    const getExitControlById = async (exitControlId) => {
        try {
            isLoading.value = true;
            error.value = null;
            exitControl.value = null;

            const validationError = validateExitControlId(exitControlId);
            if (validationError) {
                return handleFetchFailure(validationError);
            }

            const response = await getExitControlByIdRepository.getExitControlById(exitControlId);

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch exit control');

        } finally {
            isLoading.value = false;
        }
    };

    const clearExitControl = () => {
        error.value = null;
        exitControl.value = null;
        isLoading.value = false;
    };

    return {
        exitControl,
        isLoading,
        error,
        getExitControlById,
        clearExitControl,
    };
}
