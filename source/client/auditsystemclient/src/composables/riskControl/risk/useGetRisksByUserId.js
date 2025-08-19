import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { getRisksByUserIdRepository } from '@/services/riskControl/risk/getRisksByUserIdRepository';

export function useGetRisksByUserId() {
    const risks = ref([]);
    const isLoading = ref(false);
    const error = ref(null);
    const { showNotification } = useNotification();

    const handleFetchSuccess = (response) => {
        const risksData = response.result?.risks || [];
        risks.value = risksData;
        return { success: true, data: risksData };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch risks') => {
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

    const getRisksByUserId = async () => {
        try {
            isLoading.value = true;
            error.value = null;
            risks.value = [];

            const response = await getRisksByUserIdRepository.getRisksByUserId();
            console.log("useGetRisksByUserId: ", response);

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch risks');
        } finally {
            isLoading.value = false;
        }
    };

    return {
        risks,
        isLoading,
        error,
        getRisksByUserId,
    };
}