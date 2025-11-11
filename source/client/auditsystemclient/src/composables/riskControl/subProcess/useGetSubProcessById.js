import { ref } from 'vue';
import { getSubProcessByIdRepository } from '@/services/riskControl/subProcess/getSubProcessByIdRepository';
import { useNotification } from '@/composables/notification/useNotification';


export function useGetSubProcessById() {
    const subProcess = ref(null);
    const isLoading = ref(null);
    const error = ref(null);
    const { showNotification } = useNotification();

    const handleFetchSuccess = (response) => {
        subProcess.value = response.result;
        const message = response?.successMessage || 'SubProcess fetched successfully!';
        showNotification(message, 'success');
        return { success: true, data: response.result };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch sub process!') => {
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

    const validateSubProcessId = (subProcessId) => {
        if (!subProcessId) {
            const message = 'SubProcess ID is required.';
            showNotification(message, 'error');
            return { success: false, message };
        }
    };

    const getSubProcessById = async (subProcessId) => {
        try {
            isLoading.value = true;
            error.value = null;
            subProcess.value = null;

            const validationError = validateSubProcessId(subProcessId);
            if (validationError) {
                return handleFetchFailure({ message: validationError });
            }

            const response = await getSubProcessByIdRepository.getSubProcessById(subProcessId);

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch sub process');
        
        } finally {
            isLoading.value = false;
        }
    };

    const clearSubProcess = () => {
        error.value = null;
        subProcess.value = null;
        isLoading.value = false;
    };

    return {
        subProcess,
        isLoading,
        error,
        getSubProcessById,
        clearSubProcess,
    };
}