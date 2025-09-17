import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { updateAuditProgramByIdRepository } from '@/services/riskControl/auditProgram/updateAuditProgramByIdRepository';

export function useUpdateAuditProgramById() {
    const isUpdating =ref(false);
    const error = ref(null);
    const updatedAuditProgram = ref(null);
    const { showNotification } = useNotification();

    const handleUpdateSuccess = (response) => {
        updatedAuditProgram.value = response.result;
        const message = response?.successMessage || 'Audit Program updated successfully';
        showNotification(message, 'success');
        return { success: true, data: response.result };
    };

    const handleUpdateFailure = (response, defaultErrorMessage = 'Failed to update audit program') => {    
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

    const validateUpdateAuditProgram = (auditProgramData) => {
        if (!auditProgramData.id) {
            return 'Audit Program ID is required';
        }
        
        if (!auditProgramData.description) {
            return 'Audit Program description is required';
        }

        if (!auditProgramData.exitControlId) {
            return 'Exit Control ID is required';
        }

        return null;
    };

    const updateAuditProgram = async (auditProgramData) => {
        try {
            isUpdating.value = true;
            error.value = null;
            updatedAuditProgram.value = null;
            
            const validationError = validateUpdateAuditProgram(auditProgramData);   
            if (validationError) {
                showNotification(validationError, 'error');
                return handleUpdateFailure({ message: validationError });
            }

            const response = await updateAuditProgramByIdRepository.updateAuditProgramById(auditProgramData);
            

            if (response?.success) {
                return handleUpdateSuccess(response);
            } else {
                return handleUpdateFailure(response);
            }

        } catch (error) {
            return handleUpdateFailure(error, error.message || 'Failed to update audit program');

        } finally {
            isUpdating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        updatedAuditProgram.value = null;
        isUpdating.value = false;
    };

    return {
        isUpdating,
        error,
        updatedAuditProgram,
        updateAuditProgram,
        resetState,
    };
};