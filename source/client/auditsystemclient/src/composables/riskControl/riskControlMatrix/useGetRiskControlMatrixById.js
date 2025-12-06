import { ref } from 'vue';
import { getRiskControlMatrixByIdRepository } from '@/services/riskControl/riskControlMatrix/getRiskControlMatrixByIdRepository';

export function useGetRiskControlMatrixById() {
    const riskControlMatrix = ref(null);
    const isLoading = ref(null);
    const error = ref(null);

    const handleFetchSuccess = (response) => {
        riskControlMatrix.value = response.result;
        return { success: true, data: response.result };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch risk control matrix!') => {
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

    const validateRiskControlMatrixId = (riskControlMatrixId) => {
        if (!riskControlMatrixId) {
            const message = 'RiskControlMatrix ID is required.';
            return { success: false, message };
        }
    };

    const getRiskControlMatrixById = async (riskControlMatrixId) => {
        try {
            isLoading.value = true;
            error.value = null;
            riskControlMatrix.value = null;

            const validationError = validateRiskControlMatrixId(riskControlMatrixId);
            if (validationError) {
                return handleFetchFailure({ message: validationError });
            }

            const response = await getRiskControlMatrixByIdRepository.getRiskControlMatrixById(riskControlMatrixId);

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch risk control matrix');
        
        } finally {
            isLoading.value = false;
        }
    };

    const clearRiskControlMatrix = () => {
        error.value = null;
        riskControlMatrix.value = null;
        isLoading.value = false;
    };

    return {
        riskControlMatrix,
        isLoading,
        error,
        getRiskControlMatrixById,
        clearRiskControlMatrix,
    };
}