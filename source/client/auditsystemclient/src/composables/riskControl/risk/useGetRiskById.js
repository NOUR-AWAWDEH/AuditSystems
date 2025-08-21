import { ref } from 'vue';
import { getRiskByIdRepository } from '@/services/riskControl/risk/getRiskByIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useGetRiskById() {
    const risk = ref(null);
    const isLoading = ref(null);
    const error = ref(null);
    const { showNotification } = useNotification();

    const handleFetchSuccess = (response) => {
        risk.value = response.result;
        const message = response?.successMessage || 'Risk fetched successfully!';
        showNotification(message, 'success');
        return { success: true, data: response.result };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch risk!') => {
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

    const validateRiskId = (riskId) => {
        if (!riskId) {
            const message = 'Risk ID is required.';
            showNotification(message, 'error');
            return { success: false, message };
        }
    };

    const fetchRiskById = async (riskId) => {
        try {
            isLoading.value = true;
            error.value = null;
            risk.value = null;

            const validationError = validateRiskId(riskId);
            if (validationError) {
                return handleFetchFailure({ message: validationError });
            }

            const response = await getRiskByIdRepository.getRiskById(riskId);


            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch risk');

        } finally {
            isLoading.value = false;
        }
    };

    const clearRisk = () => {
        error.value = null;
        risk.value = null;
        isLoading.value = false;
    };

    return {
        risk,
        isLoading,
        error,
        fetchRiskById,
        clearRisk,
    };
}
