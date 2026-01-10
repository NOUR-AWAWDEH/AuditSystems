import { ref } from 'vue';
import { getAuditProcessByIdRepository } from '@/services/riskControl/auditProcess/getAuditProcessByIdRepository';

export function useGetAuditProcessById() {
    const auditProcess = ref(null);
    const isLoading = ref(null);
    const error = ref(null);

    const handleFetchSuccess = (response) => {
        auditProcess.value = response.result?.auditProcessModel || response.result;
        return { success: true, data: auditProcess.value };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch process!') => {
        const errorMessage = response?.errors?.length
            ? response?.errors[0]?.message
            : response?.message || defaultErrorMessage;

        error.value = errorMessage;

        return {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const validateAuditProcessId = (auditProcessId) => {
        if (!auditProcessId) {
            const message = 'Audit process ID is required.';
            return { success: false, message };
        }
    };

    const getAuditProcessById = async (auditProcessId) => {
        try {
            isLoading.value = true;
            error.value = null;
            auditProcess.value = null;

            const validationError = validateAuditProcessId(auditProcessId);
            if (validationError) {
                return handleFetchFailure({ message: validationError });
            }

            const response = await getAuditProcessByIdRepository.getAuditProcessById(auditProcessId);

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch process');
        
        } finally {
            isLoading.value = false;
        }
    };

    const clearAuditProcess = () => {
        error.value = null;
        auditProcess.value = null;
        isLoading.value = false;
    };

    return {
        auditProcess,
        isLoading,
        error,
        getAuditProcessById,
        clearAuditProcess,
    };
}