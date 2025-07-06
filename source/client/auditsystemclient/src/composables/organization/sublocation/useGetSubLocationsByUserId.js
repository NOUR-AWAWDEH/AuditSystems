import { ref } from 'vue';
import { getSubLocationsByUserIdRepository } from '@/services/organization/sublocation/getSubLocationsByUserIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useGetSubLocationsByUserId() {
  const sublocations = ref([]);
  const isLoading = ref(false);
  const error = ref(null);
  const { showNotification } = useNotification();

  const handleFetchSuccess = (response) => {
    // Fix: Extract subLocations array from the nested result object
    const sublocationData = response.result?.subLocations || [];
    sublocations.value = sublocationData;
    console.log('🎯 Mapped sublocation data:', sublocationData);
    return { success: true, data: sublocationData };
  };
  
  const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch sublocations') => {
    const errorMessage = response?.errors?.length
      ? response.errors[0].message
      : response?.message || defaultErrorMessage;
    
    error.value = errorMessage;
    showNotification(errorMessage, 'error');
    
    return {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };
  };

  const getSubLocationsByUserId = async () => {
    try {
      isLoading.value = true;
      error.value = null;
      sublocations.value = [];

      console.log('🔍 Calling sublocation API...');
      const response = await getSubLocationsByUserIdRepository.getSubLocationsByUserId();
      console.log('📡 Sublocation API response:', response);

      if (response?.success) {
        console.log('✅ Sublocation API success:', response.result);
        return handleFetchSuccess(response);
      } else {
        console.log('❌ Sublocation API failed:', response);
        return handleFetchFailure(response);
      }
    } catch (err) {
      console.log('💥 Sublocation API error:', err);
      return handleFetchFailure(err, err.message || 'Failed to fetch sublocations');
    } finally {
      isLoading.value = false;
    }
  };

  const addSublocation = (newSublocation) => {
    sublocations.value.push(newSublocation);
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

  // NEW: Create hierarchical data structure
  const createHierarchicalData = (locations) => {
    if (!Array.isArray(locations) || !Array.isArray(sublocations.value)) {
      return locations || [];
    }

    return locations.map(location => {
      const locationSublocations = sublocations.value.filter(
        sublocation => sublocation.locationId === location.id
      );
      
      return {
        ...location,
        sublocations: locationSublocations,
        hasSublocations: locationSublocations.length > 0
      };
    });
  };

  // NEW: Get hierarchical data grouped by location
  const getHierarchicalDataByLocation = (locations) => {
    const hierarchicalData = createHierarchicalData(locations);
    
    console.log('📊 Created hierarchical data:', hierarchicalData);
    
    return hierarchicalData;
  };

  const resetState = () => {
    error.value = null;
    sublocations.value = [];
    isLoading.value = false;
  };

  return {
    // State
    sublocations,
    isLoading,
    error,

    // Actions
    getSubLocationsByUserId,
    addSublocation,
    removeSublocation,
    updateSublocation,
    getSublocationsByLocationId,
    resetState,
    
    // NEW: Hierarchical data helpers
    createHierarchicalData,
    getHierarchicalDataByLocation,
  };
}