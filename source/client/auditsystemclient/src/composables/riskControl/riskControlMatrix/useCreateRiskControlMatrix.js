import { ref } from 'vue';
import { createRiskControlMatrixRepository } from '@/services/riskControl/riskControlMatrix/createRiskControlMatrixRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useCreateRiskControlMatrix() {
    const isCreating = ref(false);
    const error = ref(null);
    const createdRiskControlMatrix = ref(null);
    const { showNotification } = useNotification();

    const handleCreateSuccess = (response) => {
        createdRiskControlMatrix.value = response.result;
        return {
            success: true,
            data: response.result,
            message: response.successMessage || 'Risk Control Matrix created successfully.'
        };
    };

    const handleCreateFailure = (response, defaultErrorMessage = 'Failed to create risk control matrix') => {
        const errorMessage = response?.errors?.length
            ? response.errors[0].message
            : response?.message || defaultErrorMessage;
        
        error.value = errorMessage;
        showNotification(errorMessage, 'error');
        return {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }]
        };
    };

    const createRiskControlMatrix = async (riskControlMatrixData) => {
        try {
            isCreating.value = true;
            error.value = null;
            createdRiskControlMatrix.value = null;

            const response = await createRiskControlMatrixRepository.createRiskControlMatrix(riskControlMatrixData);

            if (response?.success) {
                return handleCreateSuccess(response);
            } else {
                return handleCreateFailure(response);
            }

        } catch (err) {
            console.error('useCreateRiskControlMatrix: Create error:', err);
            return handleCreateFailure(err, err.message || 'Failed to create risk control matrix');
            
        } finally {
            isCreating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        createdRiskControlMatrix.value = null;
        isCreating.value = false;
    };

    return {
        isCreating,
        error,
        createdRiskControlMatrix,
        createRiskControlMatrix,
        resetState
    };
}