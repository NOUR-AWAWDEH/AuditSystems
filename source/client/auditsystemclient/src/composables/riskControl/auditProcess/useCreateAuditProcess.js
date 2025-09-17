import { ref } from 'vue';
import { createAuditProcessRepository } from '@/services/riskControl/auditProcess/createAuditProcessRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useCreateAuditProcess() {
    const isCreating = ref(false);
    const error = ref(null);
    const createdAuditProcess = ref(null);
    const { showNotification } = useNotification();

    const handleCreateSuccess = (response) => {
        createdAuditProcess.value = response.result;
        return {
            success: true,
            data: response.result,
            message: response.successMessage || 'Process created successfully.'
        };
    };

    const handleCreateFailure = (response, defaultErrorMessage = 'Failed to create audit process') => {
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

    const createAuditProcess = async (auditProcessData) => {
        try {
            isCreating.value = true;
            error.value = null;
            createdAuditProcess.value = null;

            const response = await createAuditProcessRepository.createAuditProcess(auditProcessData);

            if (response?.success) {
                return handleCreateSuccess(response);
            } else {
                return handleCreateFailure(response);
            }

        } catch (err) {
            console.error('useCreateAuditProcess: Create error:', err);
            return handleCreateFailure(err, err.message || 'Failed to create audit process');
            
        } finally {
            isCreating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        createdAuditProcess.value = null;
        isCreating.value = false;
    };

    return {
        isCreating,
        error,
        createdAuditProcess,
        createAuditProcess,
        resetState
    };
}