import { ref } from 'vue';
import { getRiskByIdRepository } from '@/services/riskControl/risk/getRiskByIdRepository';

export function useGetRiskById() {
    const risk = ref(null);
    const isLoading = ref(null);
    const error = ref(null);

    const handleFetchSuccess = (response) => {
        risk.value = response.result;
        return { success: true, data: response.result };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch risk!') => {
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

    const validateRiskId = (riskId) => {
        if (!riskId) {
            const message = 'Risk ID is required.';
            return { success: false, message };
        }
    };

    const getRiskById = async (riskId) => {
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
        getRiskById,
        clearRisk,
    };
}
