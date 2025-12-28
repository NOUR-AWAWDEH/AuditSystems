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

  // Fetch Observation when selectedObservationId changes
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

  // Normalize Observations into a computed ref
  const observationsRef = computed(() => {
    return observations && 'value' in observations ? observations.value || [] : observations || [];
  });

  // Get the selected Observation from the list
  const selectedObservation = computed(() => {
    const observationId = selectedObservationId.value;
    const observationsList = observationsRef.value;
    
    if (!observationId || !observationsList.length) return null;
    return observationsList.find(obj => obj.id === observationId) || null;
  });

  const currentObservation = computed(() => {
    // fetchObservation is already a ref, so check it directly
    let observation = fetchObservation.value || selectedObservation.value;
    
    // Handle wrapped response from backend (has observationModel property)
    if (observation && observation.observationModel) {
      observation = observation.observationModel;
    }
    
    return observation || null;
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
