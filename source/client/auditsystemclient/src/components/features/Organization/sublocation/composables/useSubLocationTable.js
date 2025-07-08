import { ref, watch, computed } from 'vue';
import { useSearchSubLocationByUserId } from '@/composables/organization/sublocation/useSearchSubLocationByUserId';
import { useGetSubLocationsByUserId } from '@/composables/organization/sublocation/useGetSubLocationsByUserId';

export function useSubLocationTable(locations) {
    const searchQuery = ref('');
    const sortBy = ref('name');
    const selectedLocationId = ref('');
    const currentPage = ref(1);
    const pageSize = ref(10);
    
    // Use backend search with locationId support
    const {
        searchResults,
        isSearching,
        totalCount,
        searchSubLocations,
        clearSearch
    } = useSearchSubLocationByUserId();
    
    // Fallback to get all sublocations when no search
    const { 
        sublocations: allSublocations, 
        isLoading: isLoadingAll,
        getSubLocationsByUserId 
    } = useGetSubLocationsByUserId();
    
    // Computed property for selected location
    const selectedLocation = computed(() => {
        if (!locations || !selectedLocationId.value) return null;
        return locations.find(location => location.id === selectedLocationId.value) || null;
    });
    
    // Server-side filtering: No need for client-side filtering anymore
    const sublocations = computed(() => {
    
    if (searchQuery.value.trim()) {
        return searchResults.value;
    } else {
        return allSublocations.value; // Fixed: was filteredSublocations.value
    }
});
    
    // Combined loading state
    const isLoading = computed(() => isSearching.value || isLoadingAll.value);
    
    // Watch for search and sort changes
    watch([searchQuery, sortBy], async ([search, sort]) => {
        if (!selectedLocationId.value) return;
        
        if (search.trim()) {
            await searchSubLocations({
                searchTerm: search,
                locationId: selectedLocationId.value, // Server-side location filtering
                page: currentPage.value,
                pageSize: pageSize.value,
                sortBy: sort
            });
        } else {
            clearSearch();
            // Only fetch if we don't have data yet
            if (allSublocations.value.length === 0) {
                await getSubLocationsByUserId();
            }
        }
    });
    
    // Separate watch for location changes
    watch(selectedLocationId, async (newLocationId) => {
        if (!newLocationId) {
            clearSearch();
            return;
        }
        
        // If we have search query, search with new location
        if (searchQuery.value.trim()) {
            await searchSubLocations({
                searchTerm: searchQuery.value,
                locationId: newLocationId, // Server-side location filtering
                page: currentPage.value,
                pageSize: pageSize.value,
                sortBy: sortBy.value
            });
        } else {
            // Only fetch all sublocations if we don't have them yet
            if (allSublocations.value.length === 0) {
                await getSubLocationsByUserId();
            }
        }
    });
    
    const refreshData = async () => {
        // Add cache busting parameter
        const cacheBuster = Date.now();
        
        if (searchQuery.value.trim()) {
            await searchSubLocations(searchQuery.value, currentPage.value, pageSize.value, cacheBuster);
        } else {
            await getSubLocationsByUserId(cacheBuster);
            // Remove this line - computed property will update automatically
            // sublocations.value = allSublocations.value;
        }
    };
    
    // Add this function
    const handleSublocationAdded = async (newSublocation) => {
        await refreshData(); // Refresh the data to show the new sublocation immediately
    };
    
    const handleLocationChange = async (newLocationId) => {
        selectedLocationId.value = newLocationId;
        // The watch will handle the API call
    };
    
    const handleSearch = async (newSearchQuery) => {
        searchQuery.value = newSearchQuery;
        currentPage.value = 1; // Reset to first page
        // The watch will handle the API call
    };
    
    const handleSort = async (newSortBy) => {
        sortBy.value = newSortBy;
        // The watch will handle the API call
    };
    
    // Add method to reset to default view
    const resetToDefaultView = () => {
        selectedLocationId.value = '';
        searchQuery.value = '';
        currentPage.value = 1;
        clearSearch();
    };
    
    // Watch for location changes to handle deleted locations
    watch([locations, selectedLocationId], ([newLocations, currentLocationId]) => {
        // If a location was selected but no longer exists in the locations array
        if (currentLocationId && newLocations && newLocations.length > 0) {
            const locationExists = newLocations.some(loc => loc.id === currentLocationId);
            if (!locationExists) {
                resetToDefaultView();
            }
        }
    }, { deep: true });
    
    return {
        sublocations,
        isLoading,
        selectedLocationId,
        selectedLocation,
        searchQuery,
        sortBy,
        currentPage,
        pageSize,
        totalCount,
        refreshData,
        handleSublocationAdded, // Add this to the return object
        handleLocationChange,
        handleSearch,
        handleSort,
        resetToDefaultView
    };
}