import { ref } from 'vue';
import { getLocationsByUserIdRepository } from '@/services/organization/location/getLocationsByUserIdRepository';

export function useGetLocationsByUserId() {

  const locations = ref([]);
  const isLoading = ref(false);
  const error = ref(null);

  const fetchLocationsByUserId = async () => {
    isLoading.value = true;
    error.value = null;

    try {
      
      const result = await getLocationsByUserIdRepository.getLocationsByUserId();
      
      if (result.success) {
        locations.value = result.result || [];
      } else {
        const errorMsg = result.message || 'Failed to fetch locations';
        error.value = errorMsg;
        console.error('Failed to fetch locations:', errorMsg);
        console.error('Full error result:', result); // Add this line
      }
    } catch (err) {
      error.value = err.message || 'An error occurred while fetching locations';
      console.error('Error fetching locations:', err);
    } finally {
      isLoading.value = false;
    }
  };

  const addLocation = (newLocation) => {
    locations.value.push(newLocation);
  };

  const removeLocation = (locationId) => {
    locations.value = locations.value.filter(location => location.id !== locationId);
  };

  const updateLocation = (updatedLocation) => {
    const index = locations.value.findIndex(location => location.id === updatedLocation.id);
    if (index !== -1) {
      locations.value[index] = updatedLocation;
    }
  };

  return {
    locations,
    isLoading,
    error,
    fetchLocationsByUserId,
    addLocation,
    removeLocation,
    updateLocation
  };
}