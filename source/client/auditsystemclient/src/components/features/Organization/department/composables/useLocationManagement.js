import { ref, computed, watch } from 'vue';
import { useGetLocationById } from '@/composables/organization/location/useGetLocationById';

export function useLocationManagement(locations) {
    // Initialize from localStorage
    const selectedLocationId = ref(localStorage.getItem('organizationSelectedLocationId') || '');

    //Watch for changes and persist to localStorage
    watch(selectedLocationId, (newValue) => {
        if (newValue) {
            localStorage.setItem('organizationSelectedLocationId', newValue);
        } else {
            localStorage.removeItem('organizationSelectedLocationId');
        }
    });

    const {
        location: fetchLocation,
        isLoading: isLocationLoading,
        getLocationById,
        clearLocation,
    } = useGetLocationById();

    // Add watcher to fetch Location details when selectedLocationId changes
    watch(selectedLocationId, async (newLocationId) => {
        if (newLocationId) {
            try {
                await getLocationById(newLocationId);
            } catch (error) {
                console.error('Error fetching Location details:', error);
            }
        } else {
            clearLocation();
        }
    }, { immediate: true });

    const locationsRef = computed(() => {
        if (locations && typeof locations === 'object' && 'value' in locations) {
            return locations.value || [];
        }
        return locations || [];
    });

    const selectedLocation = computed(() => {
        const locationId = selectedLocationId.value;
        const locationsList = locationsRef.value;

        if (!locationId || !locationsList || locationsList.length === 0) {
            return null;
        }
        const location = locationsList.find(loc => loc.id === locationId);
        return location || null;
    });

    const currentLocation = computed(() => {
        const location = fetchLocation?.value || selectedLocation?.value;
        return location?.locationModel || location;
    });

    return {
        selectedLocationId,
        selectedLocation,
        currentLocation,
        isLocationLoading,
        locationsRef,
        getLocationById,
        clearLocation,
    };
}