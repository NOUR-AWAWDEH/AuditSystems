import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { getAuditProgramsByUserIdRepository } from '@/services/riskControl/auditProgram/getAuditProgramsByUserIdRepository';

export function useGetAuditProgramsByUserId() {
    const auditPrograms = ref([]);
    const isLoading = ref(false);
    const error = ref(null);
    const { showNotification } = useNotification();

    const handleFetchSuccess = (response) => {
        const auditProgramsData = Array.isArray(response.result) ? response.result : [];
        auditPrograms.value = auditProgramsData;
        return { success: true, data: auditProgramsData };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch audit programs') => {
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

    const getAuditProgramsByUserId = async () => {
        try {
            isLoading.value = true;
            error.value = null;
            auditPrograms.value = [];

            const response = await getAuditProgramsByUserIdRepository.getAuditProgramsByUserId();

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch audit programs');

        } finally {
            isLoading.value = false;
        }
    };

    return {
        auditPrograms,
        isLoading,
        error,
        getAuditProgramsByUserId,
    };
}