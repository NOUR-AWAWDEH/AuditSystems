import { ref } from 'vue';
import { getLocationsRepository } from '@/services/organization/location/getLocationsRepository';
import { useNotification } from '@/composables/notification/useNotification';
import authService from '@/services/auth/authRepos/authService';

export function useGetLocations() {
  const isLoading = ref(false);
  const error = ref(null);
  const locations = ref([]);
  const { showNotification } = useNotification();

  const fetchLocations = async () => {
    try {
      isLoading.value = true;
      error.value = null;

      const currentUser = authService.getCurrentUser();
      if (!currentUser?.id) {
        error.value = 'User not authenticated';
        showNotification('User not authenticated', 'error');
        return;
      }

      const response = await getLocationsRepository.getLocationsByUserId(currentUser.id);

      if (response?.success) {
        locations.value = response.result || [];
      } else {
        const errorMessage = response?.errors?.[0]?.message || response?.message || 'Failed to fetch locations';
        error.value = errorMessage;
        showNotification(errorMessage, 'error');
      }
    } catch (err) {
      console.error('Fetch locations error:', err);
      const errorMessage = err.message || 'Failed to fetch locations';
      error.value = errorMessage;
      showNotification(errorMessage, 'error');
    } finally {
      isLoading.value = false;
    }
  };

  const addLocation = (newLocation) => {
    locations.value.unshift(newLocation);
  };

  const removeLocation = (locationId) => {
    locations.value = locations.value.filter(loc => loc.id !== locationId);
  };

  const updateLocation = (updatedLocation) => {
    const index = locations.value.findIndex(loc => loc.id === updatedLocation.id);
    if (index !== -1) {
      locations.value[index] = updatedLocation;
    }
  };

  return {
    // State
    isLoading,
    error,
    locations,

    // Actions
    fetchLocations,
    addLocation,
    removeLocation,
    updateLocation,
  };
}