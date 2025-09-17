import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { updateRiskControlMatrixByIdRepository } from '@/services/riskControl/riskControlMatrix/updateRiskControlMatrixByIdRepository';

export function useUpdateRiskControlMatrixById() {
    const isUpdating =ref(false);
    const error = ref(null);
    const updatedRiskControlMatrix = ref(null);
    const { showNotification } = useNotification();

    const handleUpdateSuccess = (response) => {
        updatedRiskControlMatrix.value = response.result;
        const message = response?.successMessage || 'Risk control matrix updated successfully';
        showNotification(message, 'success');
        return { success: true, data: response.result };
    };

    const handleUpdateFailure = (response, defaultErrorMessage = 'Failed to update risk control matrix') => {    
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
                showNotification(validationError, 'error');
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