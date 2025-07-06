import { ref, computed, watch } from 'vue';
import { useGetSubLocationsByUserId } from '@/composables/organization/sublocation/useGetSubLocationsByUserId';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';

export function useSubLocationTable(locations) {
    const { isLoading, sublocations, getSubLocationsByUserId } = useGetSubLocationsByUserId();
    const { initCheck } = useAuthStatus();
    
    const selectedLocationId = ref('');
    
    const selectedLocation = computed(() => {
        return locations.find(location => location.id === selectedLocationId.value) || null;
    });
    
    const locationSublocations = computed(() => {
        if (!selectedLocationId.value) return [];
        return sublocations.value.filter(sublocation => 
            sublocation.locationId === selectedLocationId.value
        );
    });
    
    const refreshData = async () => {
        console.log('🔄 Refreshing sublocation data...');
        await initCheck();
        await getSubLocationsByUserId();
        console.log('🏢 Sublocations refreshed:', sublocations.value);
    };
    
    const handleLocationChange = async () => {
        if (selectedLocationId.value) {
            await refreshData();
        }
    };
    
    const handleSublocationAdded = async (newSublocation) => {
        await getSubLocationsByUserId();
        return newSublocation;
    };
    
    // Watch for location changes
    watch(selectedLocationId, (newLocationId) => {
        if (newLocationId) {
            handleLocationChange();
        }
    });
    
    return {
        sublocations: locationSublocations,
        isLoading,
        selectedLocationId,
        selectedLocation,
        refreshData,
        handleLocationChange,
        handleSublocationAdded
    };
}