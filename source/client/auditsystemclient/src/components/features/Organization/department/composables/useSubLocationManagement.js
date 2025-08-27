import { ref, computed, watch } from 'vue';
import { useGetSubLocationById } from '@/composables/organization/sublocation/useGetSubLocationById';

export function useSubLocationManagement(subLocations) {
    // Initialize from localStorage
    const selectedSubLocationId = ref(localStorage.getItem('organizationSelectedSubLocationId') || '');

    //Watch for changes and persist to localStorage
    watch(selectedSubLocationId, (newValue) => {
        if (newValue) {
            localStorage.setItem('organizationSelectedSubLocationId', newValue);
        } else {
            localStorage.removeItem('organizationSelectedSubLocationId');
        }
    });

    const {
        subLocation: fetchSubLocation,
        isLoading: isSubLocationLoading,
        getSubLocationById,
        clearSubLocation,
    } = useGetSubLocationById();

    // Add watcher to fetch SubLocation details when selectedSubLocationId changes
    watch(selectedSubLocationId, async (newSubLocationId) => {
        if (newSubLocationId) {
            try {
                await getSubLocationById(newSubLocationId);
            } catch (error) {
                console.error('Error fetching SubLocation details:', error);
            }
        } else {
            clearSubLocation();
        }
    }, { immediate: true });

    const subLocationsRef = computed(() => {
        if (subLocations && typeof subLocations === 'object' && 'value' in subLocations) {
            return subLocations.value || [];
        }
        return subLocations || [];
    });

    const selectedSubLocation = computed(() => {
        const subLocationId = selectedSubLocationId.value;
        const subLocationsList = subLocationsRef.value;

        if (!subLocationId || !subLocationsList || subLocationsList.length === 0) {
            return null;
        }
        const subLocation = subLocationsList.find(loc => loc.id === subLocationId);
        return subLocation || null;
    });

    const currentSubLocation = computed(() => {
        const subLocation = fetchSubLocation?.value || selectedSubLocation?.value;
        return subLocation?.SubLocationModel || subLocation;
    });

    return {
        selectedSubLocationId,
        selectedSubLocation,
        currentSubLocation,
        isSubLocationLoading,
        subLocationsRef,
        getSubLocationById,
        clearSubLocation,
    };
}