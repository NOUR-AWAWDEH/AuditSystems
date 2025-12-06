import { ref } from 'vue';
import { updateRiskControlMatrixByIdRepository } from '@/services/riskControl/riskControlMatrix/updateRiskControlMatrixByIdRepository';

export function useUpdateRiskControlMatrixById() {
    const isUpdating =ref(false);
    const error = ref(null);
    const updatedRiskControlMatrix = ref(null);

    const handleUpdateSuccess = (response) => {
        updatedRiskControlMatrix.value = response.result;
        return { success: true, data: response.result };
    };

    const handleUpdateFailure = (response, defaultErrorMessage = 'Failed to update risk control matrix') => {    
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

    const validateUpdateRiskControlMatrix = (riskControlMatrixData) => {
        if (!riskControlMatrixData.id) {
            return 'Risk Control Matrix ID is required';
        }
        
        if (!riskControlMatrixData.description) {
            return 'Risk Control Matrix description is required';
        }

        if (!riskControlMatrixData.subProcessId) {
            return 'SubProcess ID is required';
        }

        return null;
    };

    const updateRiskControlMatrix = async (riskControlMatrixData) => {
        try {
            isUpdating.value = true;
            error.value = null;
            updatedRiskControlMatrix.value = null;
            
            const validationError = validateUpdateRiskControlMatrix(riskControlMatrixData);
            if (validationError) {
                return handleUpdateFailure({ message: validationError });
            }

            const response = await updateRiskControlMatrixByIdRepository.updateRiskControlMatrixById(riskControlMatrixData);
            
            if (response?.success) {
                return handleUpdateSuccess(response);
            } else {
                return handleUpdateFailure(response);
            }

        } catch (error) {
            return handleUpdateFailure(error, error.message || 'Failed to update risk control matrix');

        } finally {
            isUpdating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        updatedRiskControlMatrix.value = null;
        isUpdating.value = false;
    };

    return {
        isUpdating,
        error,
        updatedRiskControlMatrix,
        updateRiskControlMatrix,
        resetState,
    };
};