import { ref, computed, watch } from 'vue';
import { useGetSubProcessById } from '@/composables/riskControl/subProcess/useGetSubProcessById';

export function useSubProcessManagement(subProcesses) {
    // Initialize from localStorage - USE THE SAME KEY AS RiskControl.vue
    const selectedSubProcessId = ref(localStorage.getItem('riskControlSelectedSubProcessId') || '');

    // Watch for changes and persist to localStorage
    watch(selectedSubProcessId, (newValue) => {
        if (newValue) {
            localStorage.setItem('riskControlSelectedSubProcessId', newValue);
        } else {
            localStorage.removeItem('riskControlSelectedSubProcessId');
        }
    });

    const {
        subProcess: fetchSubProcess,
        isLoading: isSubProcessLoading,
        fetchSubProcessById,
        clearSubProcess
    } = useGetSubProcessById();

    // Add watcher to fetch SubProcess details when selectedSubProcessId changes
    watch(selectedSubProcessId, async (newSubProcessId) => {
        if (newSubProcessId) {
            try {
                await fetchSubProcessById(newSubProcessId);
            } catch (error) {
                console.error('Error fetching SubProcess details:', error);
            }
        } else {
            clearSubProcess();
        }
    }, { immediate: true });

    const subProcessesRef = computed(() => {
        if (subProcesses && typeof subProcesses === 'object' && 'value' in subProcesses) {
            return subProcesses.value || [];
        }
        return subProcesses || [];
    });

    const selectedSubProcess = computed(() => {
        const subProcessId = selectedSubProcessId.value;
        const subProcessesList = subProcessesRef.value;

        if (!subProcessId || !subProcessesList || subProcessesList.length === 0) {
            return null;
        }
        const subProcess = subProcessesList.find(subProcess => subProcess.id === subProcessId);
        return subProcess || null;
    });

    const currentSubProcess = computed(() => {
        const subProcess = fetchSubProcess?.value || selectedSubProcess?.value;
        return subProcess?.subProcessModel || subProcess;
    });

    return {
        selectedSubProcessId,
        selectedSubProcess,
        currentSubProcess,
        isSubProcessLoading,
        subProcessesRef,
        fetchSubProcessById,
        clearSubProcess
    };
}