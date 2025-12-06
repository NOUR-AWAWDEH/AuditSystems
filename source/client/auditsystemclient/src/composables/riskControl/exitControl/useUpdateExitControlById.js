import { ref } from 'vue';
import { updateExitControlByIdRepository } from '@/services/riskControl/exitControl/updateExitControlByIdRepository';

export function useUpdateExitControlById() {
    const isUpdating =ref(false);
    const error = ref(null);
    const updatedExitControl = ref(null);

    const handleUpdateSuccess = (response) => {
        updatedExitControl.value = response.result;
        return { success: true, data: response.result };
    };

    const handleUpdateFailure = (response, defaultErrorMessage = 'Failed to update exit control') => {    
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

    const validateUpdateExitControl = (exitControlData) => {
        if (!exitControlData.id) {
            return 'Exit Control ID is required';
        }
        
        if (!exitControlData.description) {
            return 'Exit Control description is required';
        }

        if (!exitControlData.objectiveId) {
            return 'Objective ID is required';
        }

        return null;
    };

    const updateExitControl = async (exitControlData) => {
        try {
            isUpdating.value = true;
            error.value = null;
            updatedExitControl.value = null;
            
            const validationError = validateUpdateExitControl(exitControlData);
            if (validationError) {
                return handleUpdateFailure({ message: validationError });
            }

            const response = await updateExitControlByIdRepository.updateExitControlById(exitControlData);
            

            if (response?.success) {
                return handleUpdateSuccess(response);
            } else {
                return handleUpdateFailure(response);
            }

        } catch (error) {
            return handleUpdateFailure(error, error.message || 'Failed to update exit control');

        } finally {
            isUpdating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        updatedExitControl.value = null;
        isUpdating.value = false;
    };

    return {
        isUpdating,
        error,
        updatedExitControl,
        updateExitControl,
        resetState,
    };
};