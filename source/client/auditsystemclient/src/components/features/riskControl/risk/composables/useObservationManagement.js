/**
 * Composable to manage observations selection and fetching details.
 *
 * @param {Array|import('vue').Ref<Array>} observations - List of observations or reactive ref.
 * @returns {Object} - Reactive state and helper functions:
 *   @property {import('vue').Ref<string>} selectedObservationId - Currently selected Observation ID.
 *   @property {import('vue').Ref<Object|null>} selectedObservation - Selected observation from the list.
 *   @property {import('vue').Ref<Object|null>} currentObservation - Full details of selected observation.
 *   @property {import('vue').Ref<boolean>} isObservationLoading - Loading state for fetching.
 *   @property {import('vue').Ref<Array>} observationsRef - Normalized observations array.
 *   @property {Function} getObservationById - Fetch observation by ID from backend.
 *   @property {Function} clearObservation - Clears fetched observation.
 */

import { ref, computed, watch } from 'vue';
import { useGetObservationById } from '@/composables/riskControl/observation/useGetObservationById';

export function useObservationManagement(observations) {
  // Initialize from localStorage
  const selectedObservationId = ref(localStorage.getItem('riskControlSelectedObservationId') || '');

  // Persist selectedObservationId to localStorage
  watch(selectedObservationId, (newValue) => {
    if (newValue) {
      localStorage.setItem('riskControlSelectedObservationId', newValue);
    } else {
      localStorage.removeItem('riskControlSelectedObservationId');
    }
  });

  // Fetch Observation details from backend
  const {
    observation: fetchObservation,
    isLoading: isObservationLoading,
    getObservationById,
    clearObservation
  } = useGetObservationById();

  // Fetch observation when selectedObservationId changes
  watch(selectedObservationId, async (newObservationId) => {
    if (newObservationId) {
      try {
        await getObservationById(newObservationId);
      } catch (error) {
        console.error('Error fetching Observation details:', error);
      }
    } else {
      clearObservation();
    }
  }, { immediate: true });

  // Normalize observations into a computed ref
  const observationsRef = computed(() => {
    return observations && 'value' in observations ? observations.value || [] : observations || [];
  });

  // Get the selected observation from the list
  const selectedObservation = computed(() => {
    const observationId = selectedObservationId.value;
    const observationsList = observationsRef.value;
    
    if (!observationId || !observationsList.length) return null;
    return observationsList.find(obj => obj.id === observationId) || null;
  });

  // Look for the observation from the server (getObservation.value).
  // If the server one is not there, use the selected one from the list (selectedObservation.value).
  // If the chosen object has a property called observationModel, return that instead. 
  // If not, just return the object itself.
  const currentObservation = computed(() => {
    const observation = fetchObservation?.value || selectedObservation?.value;
    return observation?.observationModel || observation || null;
  });

  return {
    selectedObservationId,
    selectedObservation,
    currentObservation,
    isObservationLoading,
    observationsRef,
    getObservationById,
    clearObservation
  };
}
