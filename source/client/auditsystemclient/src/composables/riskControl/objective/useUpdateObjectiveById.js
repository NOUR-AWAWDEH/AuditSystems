import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { updateObjectiveByIdRepository } from '@/services/riskControl/objective/updateObjectiveByIdRepository';

export function useUpdateObjectiveById() {
    const isUpdating =ref(false);
    const error = ref(null);
    const updatedObjective = ref(null);
    const { showNotification } = useNotification();

    const handleUpdateSuccess = (response) => {
        updatedObjective.value = response.result;
        const message = response?.successMessage || 'Objective updated successfully';
        showNotification(message, 'success');
        return { success: true, data: response.result };
    };

    const handleUpdateFailure = (response, defaultErrorMessage = 'Failed to update objective') => {    
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

    const validateUpdateObjective = (objectiveData) => {
        if (!objectiveData.id) {
            return 'Objective ID is required';
        }
        
        if (!objectiveData.description) {
            return 'Objective description is required';
        }

        if (!objectiveData.riskControlMatrixId) {
            return 'Risk Control Matrix ID is required';
        }

        return null;
    };

    const updateObjective = async (objectiveData) => {
        try {
            isUpdating.value = true;
            error.value = null;
            updatedObjective.value = null;
            
            const validationError = validateUpdateObjective(objectiveData);
            if (validationError) {
                showNotification(validationError, 'error');
                return handleUpdateFailure({ message: validationError });
            }

            const response = await updateObjectiveByIdRepository.updateObjectiveById(objectiveData);
            

            if (response?.success) {
                return handleUpdateSuccess(response);
            } else {
                return handleUpdateFailure(response);
            }

        } catch (error) {
            return handleUpdateFailure(error, error.message || 'Failed to update objective');

        } finally {
            isUpdating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        updatedObjective.value = null;
        isUpdating.value = false;
    };

    return {
        isUpdating,
        error,
        updatedObjective,
        updateObjective,
        resetState,
    };
};