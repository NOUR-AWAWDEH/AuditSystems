import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { updateRiskByIdRepository } from '@/services/riskControl/risk/updateRiskByIdRepository';

export function useUpdateRiskById() {
    const isUpdating =ref(false);
    const error = ref(null);
    const updatedRisk = ref(null);
    const { showNotification } = useNotification();

    const handleUpdateSuccess = (response) => {
        updatedRisk.value = response.result;
        const message = response?.successMessage || 'Risk updated successfully';
        showNotification(message, 'success');
        return { success: true, data: response.result };
    };

    const handleUpdateFailure = (response, defaultErrorMessage = 'Failed to update risk') => {    
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

    const validateUpdateRisk = (riskData) => {
        if (!riskData.id) {
            return 'Risk ID is required';
        }
        
        if (!riskData.description) {
            return 'Risk description is required';
        }

        if (!riskData.objectiveId) {
            return 'Objective ID is required';
        }
        
        if (!riskData.rateId) {
            return 'Rate ID is required';
        }

        return null;
    };

    const updateRisk = async (riskData) => {
        try {
            isUpdating.value = true;
            error.value = null;
            updatedRisk.value = null;
            
            const validationError = validateUpdateRisk(riskData);
            if (validationError) {
                showNotification(validationError, 'error');
                return handleUpdateFailure({ message: validationError });
            }

            const response = await updateRiskByIdRepository.updateRiskById(riskData);
            
            if (response?.success) {
                return handleUpdateSuccess(response);
            } else {
                return handleUpdateFailure(response);
            }

        } catch (error) {
            return handleUpdateFailure(error, error.message || 'Failed to update risk');

        } finally {
            isUpdating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        updatedRisk.value = null;
        isUpdating.value = false;
    };

    return {
        isUpdating,
        error,
        updatedRisk,
        updateRisk,
        resetState,
    };
};