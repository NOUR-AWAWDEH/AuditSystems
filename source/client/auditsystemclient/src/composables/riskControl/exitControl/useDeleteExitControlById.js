import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { deleteExitControlByIdRepository } from '@/services/riskControl/exitControl/deleteExitControlByIdRepository';


export function useDeleteExitControlById() {
    const isDeleting = ref(false);
    const error = ref(null);
    const deletedExitControl = ref(null);
    const { showNotification } = useNotification();

    const handleDeleteSuccess = (response) => {
        deletedExitControl.value = true;
        const message = response?.successMessage || 'Exit Control deleted successfully';
        showNotification(message, 'success');
        return { success: true, message };
    };

    const handleDeleteFailure = (response, defaultErrorMessage = 'Failed to delete exit control') => {
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

    const validateDeleteExitControl = (exitControlId) => {
        if (!exitControlId) {
            return 'Exit Control ID is required';
        }
        return null;
    };
    
    const deleteExitControl = async (exitControlId) => {
        try {
            isDeleting.value = true;
            error.value = null;
            deletedExitControl.value = null;
            
            const validationError = validateDeleteExitControl(exitControlId); 
            if (validationError) {
                return handleDeleteFailure({ message: validationError });
            }

            const response = await deleteExitControlByIdRepository.deleteExitControlById(exitControlId);

            console.log('Response of Delete Exit Control By ID Repositroy: ', response.data);
            if (response?.success) {
                return handleDeleteSuccess(response);
            } else {
                return handleDeleteFailure(response);
            }

        } catch (error) {
            return handleDeleteFailure(error, error.message || 'Failed to delete exit control');
        } finally {
            isDeleting.value = false;
        }
    };

    const resetState = () => {
        isDeleting.value = false;
        error.value = null;
        deletedExitControl.value = null;
    };

    return {
        isDeleting,
        error,
        deletedExitControl,
        deleteExitControl,
        resetState,
    };
}