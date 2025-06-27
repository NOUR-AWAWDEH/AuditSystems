import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';

export function useGetSublocations() {
  const sublocations = ref([]);
  const isLoading = ref(false);
  const error = ref(null);
  const { showNotification } = useNotification();

  // Mock data for development
  const mockSublocations = [
    {
      id: '1',
      name: 'Building A - Floor 1',
      address: '123 Main St, Floor 1',
      locationId: 'location-1',
      createdAt: '2024-01-15T10:00:00Z'
    },
    {
      id: '2', 
      name: 'Building A - Floor 2',
      address: '123 Main St, Floor 2',
      locationId: 'location-1',
      createdAt: '2024-01-15T10:30:00Z'
    },
    {
      id: '3',
      name: 'Warehouse Section B',
      address: '456 Industrial Ave, Section B',
      locationId: 'location-2',
      createdAt: '2024-01-16T09:00:00Z'
    }
  ];

  const fetchSublocations = async () => {
    isLoading.value = true;
    error.value = null;

    try {
      // Simulate API delay
      await new Promise(resolve => setTimeout(resolve, 500));
      
      // Use mock data for now
      sublocations.value = [...mockSublocations];
      
    } catch (err) {
      error.value = 'An error occurred while fetching sublocations';
      showNotification('Failed to fetch sublocations', 'error');
    } finally {
      isLoading.value = false;
    }
  };

  const addSublocation = (newSublocation) => {
    sublocations.value.push({
      ...newSublocation,
      id: Date.now().toString(), // Generate temporary ID
      createdAt: new Date().toISOString()
    });
  };

  const removeSublocation = (sublocationId) => {
    sublocations.value = sublocations.value.filter(sublocation => sublocation.id !== sublocationId);
  };

  const updateSublocation = (updatedSublocation) => {
    const index = sublocations.value.findIndex(sublocation => sublocation.id === updatedSublocation.id);
    if (index !== -1) {
      sublocations.value[index] = updatedSublocation;
    }
  };

  const getSublocationsByLocationId = (locationId) => {
    return sublocations.value.filter(sublocation => sublocation.locationId === locationId);
  };

  return {
    sublocations,
    isLoading,
    error,
    fetchSublocations,
    addSublocation,
    removeSublocation,
    updateSublocation,
    getSublocationsByLocationId
  };
}