import { ref, watch } from 'vue';
import { useSearchLocationByUserId } from '@/composables/organization/location/useSearchLocationByUserId';
import { useGetLocationsByUserId } from '@/composables/organization/location/useGetLocationsByUserId';

export function useLocationTable() {
    const searchQuery = ref('');
    const sortBy = ref('name');
    const currentPage = ref(1);
    const pageSize = ref(10);
    
    // Use backend search instead of local filtering
    const {
        searchResults: locations,
        isSearching: isLoading,
        totalCount,
        searchLocations
    } = useSearchLocationByUserId();
    
    const { 
        locations: allLocations, 
        fetchLocationsByUserId 
    } = useGetLocationsByUserId();
    
    // Watch for search changes and trigger backend search
    watch([searchQuery, sortBy, currentPage], async ([search, sort, page]) => {
        if (search.trim()) {
            await searchLocations(search, page, pageSize.value);
        } else {
            await fetchLocationsByUserId();
            locations.value = allLocations.value;
        }
    }, { immediate: true });
    
    const refreshData = async () => {
        if (searchQuery.value.trim()) {
            await searchLocations(searchQuery.value, currentPage.value, pageSize.value);
        } else {
            await fetchLocationsByUserId();
            locations.value = allLocations.value;
        }
    };
    
    return {
        locations,
        isLoading,
        searchQuery,
        sortBy,
        currentPage,
        pageSize,
        totalCount,
        refreshData
    };
}