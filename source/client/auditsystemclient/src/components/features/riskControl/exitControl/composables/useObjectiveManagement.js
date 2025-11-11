import { ref, computed, watch } from 'vue';
import { useGetObjectiveById } from '@/composables/riskControl/objective/useGetObjectiveById';

export function useObjectiveManagement(objectives) {
    // Initialize from localStorage - USE THE SAME KEY AS RiskControl.vue
    const selectedObjectiveId = ref(localStorage.getItem('riskControlSelectedObjectiveId') || '');

    // Watch for changes and persist to localStorage
    watch(selectedObjectiveId, (newValue) => {
        if (newValue) {
            localStorage.setItem('riskControlSelectedObjectiveId', newValue);
        } else {
            localStorage.removeItem('riskControlSelectedObjectiveId');
        }
    });

    const {
        objective: fetchObjective,
        isLoading: isObjectiveLoading,
        getObjectiveById,
        clearObjective
    } = useGetObjectiveById();

    // Add watcher to fetch Objective details when selectedObjectiveId changes
    watch(selectedObjectiveId, async (newObjectiveId) => {
        if (newObjectiveId) {
            try {
                await getObjectiveById(newObjectiveId);
            } catch (error) {
                console.error('Error fetching objective details:', error);
            }
        } else {
            clearObjective();
        }
    }, { immediate: true });

    const objectivesRef = computed(() => {
        if (objectives && typeof objectives === 'object' && 'value' in objectives) {
            return objectives.value || [];
        }
        return objectives || [];
    });

    const selectedObjective = computed(() => {
        const objectiveId = selectedObjectiveId.value;
        const objectivesList = objectivesRef.value;

        if (!objectiveId || !objectivesList || objectivesList.length === 0) {
            return null;
        }
        const objective = objectivesList.find(objective => objective.id === objectiveId);
        return objective || null;
    });

    const currentObjective = computed(() => {
        const objective = fetchObjective?.value || selectedObjective?.value;
        return objective?.ObjectiveModel || objective;
    });

    return {
        selectedObjectiveId,
        selectedObjective,
        currentObjective,
        isObjectiveLoading,
        objectivesRef,
        getObjectiveById,
        clearObjective
    };
}