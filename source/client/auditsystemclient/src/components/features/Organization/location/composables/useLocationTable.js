import { ref, onMounted, onBeforeUnmount } from 'vue';
import { useGetLocationsByUserId } from '@/composables/organization/location/useGetLocationsByUserId';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';

export function useLocationTable() {
    const { isLoading, locations, fetchLocationsByUserId } = useGetLocationsByUserId();
    const { initCheck } = useAuthStatus();

    // Function to refresh data
    const refreshData = async () => {
        console.log('🔄 Starting data refresh...');
        await initCheck();
        
        console.log('📍 Fetching locations...');
        await fetchLocationsByUserId();
        console.log('📍 Locations fetched:', locations.value);
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
        return newLocation;
    };

    return {
        locations,
        isLoading,
        refreshData,
        handleLocationAdded
    };
}