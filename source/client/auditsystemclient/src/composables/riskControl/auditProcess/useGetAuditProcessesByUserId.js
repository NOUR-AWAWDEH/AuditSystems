import { ref } from 'vue';
import { getAuditProcessesByUserIdRepository } from '@/services/riskControl/auditProcess/getAuditProcessesByUserIdRepository';

export function useGetAuditProcessesByUserId() {
    const auditProcesses = ref([]);
    const isLoading = ref(false);
    const error = ref(null);

    const handleFetchSuccess = (response) => {
        let auditProcessesData = response.result || [];
        
        // Normalize to a flat array
        if (!Array.isArray(auditProcessesData)) {
            if (auditProcessesData.processes && Array.isArray(auditProcessesData.processes)) {
                // Handle case: { processes: [...] }
                auditProcessesData = auditProcessesData.processes;
            } else if (auditProcessesData && typeof auditProcessesData === 'object') {
                // Handle case: { "id1": {...}, "id2": {...} }
                auditProcessesData = Object.values(auditProcessesData);
            } else {
                // Fallback to empty array
                auditProcessesData = [];
            }
        }

        // Flatten the array if nested (e.g., [ [ {...} ] ] -> [ {...} ])
        auditProcessesData = Array.isArray(auditProcessesData) 
            ? auditProcessesData.flat()
            : [];

        auditProcesses.value = auditProcessesData;
        return { success: true, data: auditProcessesData };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch processes') => {
        const errorMessage = response?.errors?.length > 0 
            ? response.errors[0]?.message
            : response?.successMessage || defaultErrorMessage;

        error.value = errorMessage;

        return { 
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const getAuditProcessesByUserId = async () => {
        try {
            isLoading.value = true;
            error.value = null;
            auditProcesses.value = [];

            const response = await getAuditProcessesByUserIdRepository.getAuditProcessesByUserId();

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch processes');
            
        } finally {
            isLoading.value = false;
        }
    };

    return {
        auditProcesses,
        isLoading,
        error,
        getAuditProcessesByUserId,
    };
}