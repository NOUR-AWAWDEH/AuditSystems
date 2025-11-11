import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { deleteRiskControlMatrixByIdRepository } from '@/services/riskControl/riskControlMatrix/deleteRiskControlMatrixByIdRepository';


export function useDeleteRiskControlMatrixById() {
    const isDeleting = ref(false);
    const error = ref(null);
    const deletedRiskControlMatrix = ref(null);
    const { showNotification } = useNotification();

    const handleDeleteSuccess = (response) => {
        deletedRiskControlMatrix.value = true;
        const message = response?.successMessage || 'Risk Control Matrix deleted successfully';
        showNotification(message, 'success');
        return { success: true, message };
    };

    const handleDeleteFailure = (response, defaultErrorMessage = 'Failed to delete risk control matrix') => {
        const errorMessage = response?.errors?.length > 0 
            ? response.errors[0]?.message
            : response?.successMessage || defaultErrorMessage;

        error.value = errorMessage;
        showNotification(errorMessage, 'error');

        const failureResult = {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
        
        return failureResult;
    };

    const validateDeleteRiskControlMatrix = (riskControlMatrixId) => {
        if (!riskControlMatrixId) {
            return 'RiskControlMatrix ID is required';
        }
        return null;
    };
    
    const deleteRiskControlMatrix = async (riskControlMatrixId) => {
        try {
            isDeleting.value = true;
            error.value = null;
            deletedRiskControlMatrix.value = null;
            
            const validationError = validateDeleteRiskControlMatrix(riskControlMatrixId); 
            if (validationError) {
                return handleDeleteFailure({ message: validationError });
            }

            const response = await deleteRiskControlMatrixByIdRepository.deleteRiskControlMatrixById(riskControlMatrixId);
            
            if (response?.success) {
                return handleDeleteSuccess(response);
            } else {
                return handleDeleteFailure(response);
            }

        } catch (error) {
            return handleDeleteFailure(error, error.message || 'Failed to delete risk control matrix');
        } finally {
            isDeleting.value = false;
        }
    };

    const resetState = () => {
        isDeleting.value = false;
        error.value = null;
        deletedRiskControlMatrix.value = null;
    };

    return {
        isDeleting,
        error,
        deletedRiskControlMatrix,
        deleteRiskControlMatrix,
        resetState,
    };
}