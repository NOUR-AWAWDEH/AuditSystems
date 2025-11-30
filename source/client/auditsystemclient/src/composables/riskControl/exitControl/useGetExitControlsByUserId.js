import { ref } from 'vue';
import { getExitControlsByUserIdRepository } from '@/services/riskControl/exitControl/getExitControlsByUserIdRepository';

export function useGetExitControlsByUserId() {
    const exitControls = ref([]);
    const isLoading = ref(false);
    const error = ref(null);

    const handleFetchSuccess = (response) => {
        const exitControlsData = Array.isArray(response.result) ? response.result : [];
        exitControls.value = exitControlsData;
        return { success: true, data: exitControlsData };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch exit controls') => {
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

    const getExitControlsByUserId = async () => {
        try {
            isLoading.value = true;
            error.value = null;
            exitControls.value = [];

            const response = await getExitControlsByUserIdRepository.getExitControlsByUserId();

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch exit controls');

        } finally {
            isLoading.value = false;
        }
    };

    return {
        exitControls,
        isLoading,
        error,
        getExitControlsByUserId,
    };
}
