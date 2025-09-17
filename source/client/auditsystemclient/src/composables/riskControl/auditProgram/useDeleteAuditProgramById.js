import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { deleteAuditProgramByIdRepository } from '@/services/riskControl/auditProgram/deleteAuditProgramByIdRepository';


export function useDeleteAuditProgramById() {
    const isDeleting = ref(false);
    const error = ref(null);
    const deletedAuditProgram = ref(null);
    const { showNotification } = useNotification();

    const handleDeleteSuccess = (response) => {
        deletedAuditProgram.value = true;
        const message = response?.successMessage || 'Audit Program deleted successfully';
        showNotification(message, 'success');
        return { success: true, message };
    };

    const handleDeleteFailure = (response, defaultErrorMessage = 'Failed to delete audit program') => {
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

    const validateDeleteAuditProgram = (auditProgramId) => {
        if (!auditProgramId) {
            return 'Audit Program ID is required';
        }
        return null;
    };
    
    const deleteAuditProgram = async (auditProgramId) => {
        try {
            isDeleting.value = true;
            error.value = null;
            deletedAuditProgram.value = null;
            
            const validationError = validateDeleteAuditProgram(auditProgramId); 
            if (validationError) {
                return handleDeleteFailure({ message: validationError });
            }

            const response = await deleteAuditProgramByIdRepository.deleteAuditProgramById(auditProgramId);

            if (response?.success) {
                return handleDeleteSuccess(response);
            } else {
                return handleDeleteFailure(response);
            }

        } catch (error) {
            return handleDeleteFailure(error, error.message || 'Failed to delete audit program');
        } finally {
            isDeleting.value = false;
        }
    };

    const resetState = () => {
        isDeleting.value = false;
        error.value = null;
        deletedAuditProgram.value = null;
    };

    return {
        isDeleting,
        error,
        deletedAuditProgram,
        deleteAuditProgram,
        resetState,
    };
}