import { ref } from 'vue';
import { deleteObjectiveByIdRepository } from '@/services/riskControl/objective/deleteObjectiveByIdRepository';


export function useDeleteObjectiveById() {
    const isDeleting = ref(false);
    const error = ref(null);
    const deletedObjective = ref(null);

    const handleDeleteSuccess = (response) => {
        deletedObjective.value = true;
        const message = response?.successMessage || 'Objective deleted successfully';
        return { success: true, message };
    };

    const handleDeleteFailure = (response, defaultErrorMessage = 'Failed to delete objective') => {
        const errorMessage = response?.errors?.length > 0 
            ? response.errors[0]?.message
            : response?.successMessage || defaultErrorMessage;

        error.value = errorMessage;

        const failureResult = {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
        
        return failureResult;
    };

    const validateDeleteObjective = (objectiveId) => {
        if (!objectiveId) {
            return 'Objective ID is required';
        }
        return null;
    };
    
    const deleteObjective = async (objectiveId) => {
        try {
            isDeleting.value = true;
            error.value = null;
            deletedObjective.value = null;
            
            const validationError = validateDeleteObjective(objectiveId); 
            if (validationError) {
                return handleDeleteFailure({ message: validationError });
            }

            const response = await deleteObjectiveByIdRepository.deleteObjectiveById(objectiveId);

            if (response?.success) {
                return handleDeleteSuccess(response);
            } else {
                return handleDeleteFailure(response);
            }

        } catch (error) {
            return handleDeleteFailure(error, error.message || 'Failed to delete objective');
        } finally {
            isDeleting.value = false;
        }
    };

    const resetState = () => {
        isDeleting.value = false;
        error.value = null;
        deletedObjective.value = null;
    };

    return {
        isDeleting,
        error,
        deletedObjective,
        deleteObjective,
        resetState,
    };
}