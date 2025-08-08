import { ref, computed, watch } from 'vue';
import { useGetAuditProcessById } from '@/composables/riskControl/auditProcess/useGetAuditProcessById';

export function useAuditProcessManagement(auditProcesses) {
    // Initialize from localStorage - USE THE SAME KEY AS RiskControl.vue
    const selectedAuditProcessId = ref(localStorage.getItem('riskControlSelectedAuditProcessId') || '');

    // Watch for changes and persist to localStorage
    watch(selectedAuditProcessId, (newValue) => {
        if (newValue) {
            localStorage.setItem('riskControlSelectedAuditProcessId', newValue);
        } else {
            localStorage.removeItem('riskControlSelectedAuditProcessId');
        }
    });

    const {
        auditProcess: fetchAuditProcess,
        isLoading: isAuditProcessLoading,
        fetchAuditProcessById,
        clearAuditProcess
    } = useGetAuditProcessById();

    // Add watcher to fetch AuditProcess details when selectedAuditProcessId changes
    watch(selectedAuditProcessId, async (newAuditProcessId) => {
        if (newAuditProcessId) {
            try {
                await fetchAuditProcessById(newAuditProcessId);
            } catch (error) {
                console.error('Error fetching AuditProcess details:', error);
            }
        } else {
            clearAuditProcess();
        }
    }, { immediate: true });

    const auditProcessesRef = computed(() => {
        if (auditProcesses && typeof auditProcesses === 'object' && 'value' in auditProcesses) {
            return auditProcesses.value || [];
        }
        return auditProcesses || [];
    });

    const selectedAuditProcess = computed(() => {
        const auditProcessId = selectedAuditProcessId.value;
        const auditProcessesList = auditProcessesRef.value;

        if (!auditProcessId || !auditProcessesList || auditProcessesList.length === 0) {
            return null;
        }
        const auditProcess = auditProcessesList.find(auditProcess => auditProcess.id === auditProcessId);
        return auditProcess || null;
    });

    const currentAuditProcess = computed(() => {
        const auditProcess = fetchAuditProcess?.value || selectedAuditProcess?.value;
        return auditProcess?.auditProcessModel || auditProcess;
    });

    return {
        selectedAuditProcessId,
        selectedAuditProcess,
        currentAuditProcess,
        isAuditProcessLoading,
        auditProcessesRef,
        fetchAuditProcessById,
        clearAuditProcess
    };
}