import { ref, computed, watch } from 'vue';
import { useGetExitControlById } from '@/composables/riskControl/exitControl/useGetExitControlById';

export function useExitControlManagement(exitControls) {
    // Initialize from localStorage - USE THE SAME KEY AS RiskControl.vue
    const selectedExitControlId = ref(localStorage.getItem('riskControlSelectedExitControlId') || '');

    // Watch for changes and persist to localStorage
    watch(selectedExitControlId, (newValue) => {
        if (newValue) {
            localStorage.setItem('riskControlSelectedExitControlId', newValue);
        } else {
            localStorage.removeItem('riskControlSelectedExitControlId');
        }
    });

    const {
        exitControl: fetchExitControl,
        isLoading: isExitControlLoading,
        getExitControlById,
        clearExitControl
    } = useGetExitControlById();

    // Add watcher to fetch Exit Control details when selectedExitControlId changes
    watch(selectedExitControlId, async (newExitControlId) => {
        if (newExitControlId) {
            try {
                await getExitControlById(newExitControlId);
            } catch (error) {
                console.error('Error fetching exit control details:', error);
            }
        } else {
            clearExitControl();
        }
    }, { immediate: true });

    const exitControlsRef = computed(() => {
        if (exitControls && typeof exitControls === 'object' && 'value' in exitControls) {
            return exitControls.value || [];
        }
        return exitControls || [];
    });

    const selectedExitControl = computed(() => {
        const exitControlId = selectedExitControlId.value;
        const exitControlsList = exitControlsRef.value;

        if (!exitControlId || !exitControlsList || exitControlsList.length === 0) {
            return null;
        }
        const exitControl = exitControlsList.find(exitControl => exitControl.id === exitControlId);
        return exitControl || null;
    });

    const currentExitControl = computed(() => {
        const exitControl = fetchExitControl?.value || selectedExitControl?.value;
        return exitControl?.ExitControlModel || exitControl;
    });

    return {
        selectedExitControlId,
        selectedExitControl,
        currentExitControl,
        isExitControlLoading,
        exitControlsRef,
        getExitControlById,
        clearExitControl
    };
}