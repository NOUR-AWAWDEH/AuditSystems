import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { getRiskControlMatricesByUserIdRepository } from '@/services/riskControl/riskControlMatrix/getRiskControlMatricesByUserIdRepository';

export function useGetRiskControlMatricesByUserId() {
    const riskControlMatrices = ref([]);
    const isLoading = ref(false);
    const error = ref(null);
    const { showNotification } = useNotification();

    const handleFetchSuccess = (response) => {
        const riskControlMatricesData = Array.isArray(response.result) ? response.result : [];
        riskControlMatrices.value = riskControlMatricesData;
        return { success: true, data: riskControlMatricesData };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch risk control matrices') => {
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

    const getRiskControlMatricesByUserId = async () => {
        try {
            isLoading.value = true;
            error.value = null;
            riskControlMatrices.value = [];

            const response = await getRiskControlMatricesByUserIdRepository.getRiskControlMatricesByUserId();

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch risk control matrices');

        } finally {
            isLoading.value = false;
        }
    };

    return {
        riskControlMatrices,
        isLoading,
        error,
        getRiskControlMatricesByUserId,
    };
}
