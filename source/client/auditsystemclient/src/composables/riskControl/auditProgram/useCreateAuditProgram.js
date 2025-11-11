import { ref } from 'vue';
import { createAuditProgramRepository } from '@/services/riskControl/auditProgram/createAuditProgramRepository.js';
import { useNotification } from '@/composables/notification/useNotification';

export function useCreateAuditProgram() {
    const isCreating = ref(false);
    const error = ref(null);
    const createdAuditProgram = ref(null);
    const { showNotification } = useNotification();

    const handleCreateSuccess = (response) => {
        createdAuditProgram.value = response.result;
        return {
            success: true,
            data: response.result,
            message: response.successMessage || 'Audit Program created successfully.'
        };
    };

    const handleCreateFailure = (response, defaultErrorMessage = 'Failed to create audit program') => {
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

    const createAuditProgram = async (auditProgramData) => {
        try {
            isCreating.value = true;
            error.value = null;
            createdAuditProgram.value = null;

            const response = await createAuditProgramRepository.createAuditProgram(auditProgramData);
            
            if (response?.success) {
                return handleCreateSuccess(response);
            } else {
                return handleCreateFailure(response);
            }

        } catch (err) {
            console.error('useCreateAuditProgram: Create error:', err);
            return handleCreateFailure(err, err.message || 'Failed to create audit program');

        } finally {
            isCreating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        createdAuditProgram.value = null;
        isCreating.value = false;
    };

    return {
        isCreating,
        error,
        createdAuditProgram,
        createAuditProgram,
        resetState
    };
}