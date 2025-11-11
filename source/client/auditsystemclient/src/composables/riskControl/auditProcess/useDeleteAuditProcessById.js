import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { deleteAuditProcessByIdRepository } from '@/services/riskControl/auditProcess/deleteAuditProcessByIdRepository';

export function useDeleteAuditProcessById() {
    const isDeleting = ref(false);
    const error = ref(null);
    const deletedAuditProcess = ref(null);
    const { showNotification } = useNotification();

    const handleDeleteSuccess = (response) => {
        deletedAuditProcess.value = true;
        const message = response?.successMessage || 'Process deleted successfully';
        showNotification(message, 'success');
        return { success: true, message };
    };

    const handleDeleteFailure = (response, defaultErrorMessage = 'Failed to delete process') => {
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

    const validateDeleteAuditProcess = (auditProcessId) => {
        if (!auditProcessId) {
            return 'Process ID is required';
        }
        return null;
    };
    
    const deleteAuditProcess = async (auditProcessId) => {
        try {
            isDeleting.value = true;
            error.value = null;
            deletedAuditProcess.value = null;
            
            const validationError = validateDeleteAuditProcess(auditProcessId);
            if (validationError) {
                return handleDeleteFailure({ message: validationError });
            }

            const response = await deleteAuditProcessByIdRepository.deleteAuditProcessById(auditProcessId);
            
            if (response?.success) {
                return handleDeleteSuccess(response);
            } else {
                return handleDeleteFailure(response);
            }

        } catch (error) {
            return handleDeleteFailure(error, error.message || 'Failed to delete process');
        } finally {
            isDeleting.value = false;
        }
    };

    const resetState = () => {
        isDeleting.value = false;
        error.value = null;
        deletedAuditProcess.value = null;
    };

    return {
        isDeleting,
        error,
        deletedAuditProcess,
        deleteAuditProcess,
        resetState,
    };
}