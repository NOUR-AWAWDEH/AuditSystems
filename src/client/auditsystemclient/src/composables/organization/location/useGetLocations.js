import { ref } from 'vue';
import { getLocationsRepository } from '@/services/organization/location/getLocationsRepository';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';

export function useGetLocations() {
  const locations = ref([]);
  const isLoading = ref(false);
  const error = ref(null);
  const { authStatus } = useAuthStatus();

  const fetchLocations = async () => {
    if (!authStatus.value?.userId) {
      error.value = 'User not authenticated';
      return;
    }

    isLoading.value = true;
    error.value = null;

    try {
      const result = await getLocationsRepository.getLocationsByUserId(authStatus.value.userId);
      
      if (result.success) {
        locations.value = result.data || [];
      } else {
        error.value = result.message || 'Failed to fetch locations';
      }
    } catch (err) {
      console.error('Error fetching locations:', err);
      error.value = 'An error occurred while fetching locations';
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
    fetchLocations,
    addLocation,
    removeLocation,
    updateLocation
  };
}