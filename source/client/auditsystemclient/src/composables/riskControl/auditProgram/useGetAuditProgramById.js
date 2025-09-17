import { ref } from 'vue';
import { getAuditProgramByIdRepository } from '@/services/riskControl/auditProgram/getAuditProgramByIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useGetAuditProgramById() {
    const auditProgram = ref(null);
    const isLoading = ref(null);
    const error = ref(null);
    const { showNotification } = useNotification();

    const handleFetchSuccess = (response) => {
        auditProgram.value = response.result;
        const message = response?.successMessage || 'Audit Program fetched successfully!';
        showNotification(message, 'success');
        return { success: true, data: response.result };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch audit program!') => {
        const errorMessage = response?.errors?.length
            ? response?.errors[0]?.message
            : response?.message || defaultErrorMessage;

        error.value = errorMessage;
        showNotification(errorMessage, 'error');

        return {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const validateAuditProgramId = (auditProgramId) => {
        if (!auditProgramId) {
            const message = 'Audit Program ID is required.';
            showNotification(message, 'error');
            return { success: false, message };
        }
    };

    const getAuditProgramById = async (auditProgramId) => {
        try {
            isLoading.value = true;
            error.value = null;
            auditProgram.value = null;

            const validationError = validateAuditProgramId(auditProgramId);
            if (validationError) {
                return handleFetchFailure({ message: validationError });
            }

            const response = await getAuditProgramByIdRepository.getAuditProgramById(auditProgramId);

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch audit program');
        
        } finally {
            isLoading.value = false;
        }
    };

    const clearAuditProgram = () => {
        error.value = null;
        auditProgram.value = null;
        isLoading.value = false;
    };

    return {
        auditProgram,
        isLoading,
        error,
        getAuditProgramById,
        clearAuditProgram,
    };
}