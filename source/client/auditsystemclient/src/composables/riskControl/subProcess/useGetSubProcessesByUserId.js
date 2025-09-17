import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { getSubProcessesByUserIdRepository } from '@/services/riskControl/subProcess/getSubProcessesByUserIdRepository';

export function useGetSubProcessesByUserId() {
    const subProcesses = ref([]);
    const isLoading = ref(false);
    const error = ref(null);
    const { showNotification } = useNotification();

    const handleFetchSuccess = (response) => {
        const subProcessesData =  Array.isArray(response.result)
    ? response.result
    : response.result?.subProcesses || [];

        subProcesses.value = subProcessesData;
        return { success: true, data: subProcessesData };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch sub processes') => {
        const errorMessage = response?.errors?.length > 0 
            ? response.errors[0]?.message
            : response?.successMessage || defaultErrorMessage;

        error.value = errorMessage;
        showNotification(errorMessage, 'error');

        return { 
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const getSubProcessesByUserId = async () => {
        try {
            isLoading.value = true;
            error.value = null;
            subProcesses.value = [];

            const response = await getSubProcessesByUserIdRepository.getSubProcessesByUserId();
            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch sub processes');
            
        } finally {
            isLoading.value = false;
        }
    };

    return {
        subProcesses,
        isLoading,
        error,
        getSubProcessesByUserId,
    };
}