import { ref, onMounted, onBeforeUnmount } from 'vue';
import { useGetLocationsByUserId } from '@/composables/organization/location/useGetLocationsByUserId';
import { useGetSubLocationsByUserId } from '@/composables/organization/sublocation/useGetSubLocationsByUserId';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';

export function useLocationTable() {
    const { isLoading, locations, fetchLocationsByUserId } = useGetLocationsByUserId();
    const { isLoading: isSublocationsLoading, sublocations, getSubLocationsByUserId } = useGetSubLocationsByUserId();
    const { initCheck } = useAuthStatus();

    // Function to refresh data
    const refreshData = async () => {
        console.log('🔄 Starting data refresh...');
        await initCheck();
        
        console.log('📍 Fetching locations...');
        await fetchLocationsByUserId();
        console.log('📍 Locations fetched:', locations.value);
        
        console.log('🏢 Fetching sublocations...');
        await getSubLocationsByUserId();
        console.log('🏢 Sublocations fetched:', sublocations.value);
    };

    // Handle page visibility change to refresh data when user returns to tab
    const handleVisibilityChange = () => {
        if (!document.hidden) {
            refreshData();
        }
    };

    // Handle focus event to refresh data when user returns to window
    const handleFocus = () => {
        refreshData();
    };

    onMounted(async () => {
        await refreshData();
        
        // Add event listeners for page visibility and focus
        document.addEventListener('visibilitychange', handleVisibilityChange);
        window.addEventListener('focus', handleFocus);
    });

    onBeforeUnmount(() => {
        // Clean up event listeners
        document.removeEventListener('visibilitychange', handleVisibilityChange);
        window.removeEventListener('focus', handleFocus);
    });

    const handleLocationAdded = async (newLocation) => {
        await fetchLocationsByUserId();
        await getSubLocationsByUserId();
        return newLocation;
    };

    const handleSublocationAdded = async (newSublocation) => {
        await getSubLocationsByUserId();
        return newSublocation;
    };

    return {
        locations,
        sublocations,
        isLoading,
        isSublocationsLoading,
        refreshData,
        handleLocationAdded,
        handleSublocationAdded
    };
}