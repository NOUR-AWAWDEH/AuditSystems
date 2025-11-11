import { ref } from 'vue';
import { getExitControlByIdRepository } from '@/services/riskControl/exitControl/getExitControlByIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useGetExitControlById() {
    const exitControl = ref(null);
    const isLoading = ref(null);
    const error = ref(null);
    const { showNotification } = useNotification();

    const handleFetchSuccess = (response) => {
        exitControl.value = response.result;
        const message = response?.successMessage || 'Exit Control fetched successfully!';
        showNotification(message, 'success');
        return { success: true, data: exitControl.value };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch exit control!') => {
        const errorMessage = response?.errors?.length
            ? response?.errors[0]?.message
            : response?.message || defaultErrorMessage;

        error.value = errorMessage;
        showNotification(errorMessage, 'error');

        return {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const validateExitControlId = (exitControlId) => {
        if (!exitControlId) {
            const message = 'Exit Control ID is required.';
            showNotification(message, 'error');
            return { success: false, message };
        }
    };

    const getExitControlById = async (exitControlId) => {
        try {
            isLoading.value = true;
            error.value = null;
            exitControl.value = null;

            const validationError = validateExitControlId(exitControlId);
            if (validationError) {
                return handleFetchFailure({ message: validationError });
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