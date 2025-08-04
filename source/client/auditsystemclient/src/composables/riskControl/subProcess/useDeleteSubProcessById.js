import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { deleteSubProcessByIdRepository } from '@/services/riskControl/subProcess/deleteSubProcessByIdRepository';

export function useDeleteSubProcessById() {
    const isDeleting = ref(false);
    const error = ref(null);
    const deletedSubProcess = ref(null);
    const { showNotification } = useNotification();

    const handleDeleteSuccess = (response) => {
        deletedSubProcess.value = true;
        const message = response?.successMessage || 'SubProcess deleted successfully';
        showNotification(message, 'success');
        return { success: true, message };
    };

    const handleDeleteFailure = (response, defaultErrorMessage = 'Failed to delete sub process') => {
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

    const validateDeleteSubProcess = (subProcessId) => {
        if (!subProcessId) {
            return 'SubProcess ID is required';
        }
        return null;
    };
    
    const deleteSubProcess = async (subProcessId) => {
        try {
            isDeleting.value = true;
            error.value = null;
            deletedSubProcess.value = null;
            
            const validationError = validateDeleteSubProcess(subProcessId); 
            if (validationError) {
                return handleDeleteFailure({ message: validationError });
            }

            const response = await deleteSubProcessByIdRepository.deleteSubProcessById(subProcessId);
            
            if (response?.success) {
                return handleDeleteSuccess(response);
            } else {
                return handleDeleteFailure(response);
            }

        } catch (error) {
            return handleDeleteFailure(error, error.message || 'Failed to delete sub process');
        } finally {
            isDeleting.value = false;
        }
    };

    const resetState = () => {
        isDeleting.value = false;
        error.value = null;
        deletedSubProcess.value = null;
    };

    return {
        isDeleting,
        error,
        deletedSubProcess,
        deleteSubProcess,
        resetState,
    };
}