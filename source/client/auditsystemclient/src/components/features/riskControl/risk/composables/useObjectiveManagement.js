/**
 * Composable to manage objectives selection and fetching details.
 *
 * @param {Array|import('vue').Ref<Array>} objectives - List of objectives or reactive ref.
 * @returns {Object} - Reactive state and helper functions:
 *   @property {import('vue').Ref<string>} selectedObjectiveId - Currently selected Objective ID.
 *   @property {import('vue').Ref<Object|null>} selectedObjective - Selected objective from the list.
 *   @property {import('vue').Ref<Object|null>} currentObjective - Full details of selected objective.
 *   @property {import('vue').Ref<boolean>} isObjectiveLoading - Loading state for fetching.
 *   @property {import('vue').Ref<Array>} objectivesRef - Normalized objectives array.
 *   @property {Function} getObjectiveById - Fetch objective by ID from backend.
 *   @property {Function} clearObjective - Clears fetched objective.
 */

import { ref, computed, watch } from 'vue';
import { useGetObjectiveById } from '@/composables/riskControl/objective/useGetObjectiveById';

export function useObjectiveManagement(objectives) {
  // Initialize from localStorage
  const selectedObjectiveId = ref(localStorage.getItem('riskControlSelectedObjectiveId') || '');

  // Persist selectedObjectiveId to localStorage
  watch(selectedObjectiveId, (newValue) => {
    if (newValue) {
      localStorage.setItem('riskControlSelectedObjectiveId', newValue);
    } else {
      localStorage.removeItem('riskControlSelectedObjectiveId');
    }
  });

  // Fetch Objective details from backend
  const {
    objective: fetchObjective,
    isLoading: isObjectiveLoading,
    getObjectiveById,
    clearObjective
  } = useGetObjectiveById();

  // Fetch objective when selectedObjectiveId changes
  watch(selectedObjectiveId, async (newObjectiveId) => {
    if (newObjectiveId) {
      try {
        await getObjectiveById(newObjectiveId);
      } catch (error) {
        console.error('Error fetching Objective details:', error);
      }
    } else {
      clearObjective();
    }
  }, { immediate: true });

  // Normalize objectives into a computed ref
  const objectivesRef = computed(() => {
    return objectives && 'value' in objectives ? objectives.value || [] : objectives || [];
  });

  // Get the selected objective from the list
  const selectedObjective = computed(() => {
    const objectiveId = selectedObjectiveId.value;
    const objectivesList = objectivesRef.value;
    
    if (!objectiveId || !objectivesList.length) return null;
    return objectivesList.find(obj => obj.id === objectiveId) || null;
  });

  // Look for the objective from the server (getObjective.value).
  // If the server one is not there, use the selected one from the list (selectedObjective.value).
  // If the chosen object has a property called objectiveModel, return that instead. 
  // If not, just return the object itself.
  const currentObjective = computed(() => {
    const objective = fetchObjective?.value || selectedObjective?.value;
    return objective?.objectiveModel || objective || null;
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
