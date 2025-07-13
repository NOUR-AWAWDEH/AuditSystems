import { ref, watch, computed, toRef } from 'vue';
import { useSearchSubLocationByUserId } from '@/composables/organization/sublocation/useSearchSubLocationByUserId';
import { useGetSubLocationsByUserId } from '@/composables/organization/sublocation/useGetSubLocationsByUserId';

export function useSubLocationTable(locations) {
    const searchQuery = ref('');
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
    
    // Fallback to get all sublocations when no search and no location selected
    const { 
        sublocations: allSublocations, 
        isLoading: isLoadingAll,
        getSubLocationsByUserId 
    } = useGetSubLocationsByUserId();
    
    // Convert locations prop to reactive ref
    const locationsRef = toRef(() => locations);
    
    // Computed property for selected location
    const selectedLocation = computed(() => {
        if (!locations || !selectedLocationId.value) return null;
        return locations.find(location => location.id === selectedLocationId.value) || null;
    });
    
    // Server-side filtering: No need for client-side filtering anymore
    const sublocations = computed(() => {
        if (searchQuery.value.trim() || selectedLocationId.value) {
            return searchResults.value;
        } else {
            return allSublocations.value;
        }
    });
    
    // Combined loading state
    const isLoading = computed(() => isSearching.value || isLoadingAll.value);
    
    // Watch for search changes (removed sorting)
    watch(searchQuery, async (search) => {
        if (search.trim() || selectedLocationId.value) {
            await searchSubLocations({
                searchTerm: search,
                locationId: selectedLocationId.value,
                page: currentPage.value,
                pageSize: pageSize.value
            });
        } else {
            clearSearch();
            // Only fetch all sublocations if no location is selected
            if (!selectedLocationId.value) {
                await getSubLocationsByUserId();
            }
        }
    });
    
    // Separate watch for location changes
    watch(selectedLocationId, async (newLocationId) => {
        // Clear all data immediately when location changes
        clearSearch();
        allSublocations.value = [];
        
        if (!newLocationId) {
            // If no location selected, get all sublocations
            if (!searchQuery.value.trim()) {
                await getSubLocationsByUserId();
            }
            return;
        }
        
        // Always use search API when a location is selected (with or without search term)
        await searchSubLocations({
            searchTerm: searchQuery.value || '',
            locationId: newLocationId,
            page: currentPage.value,
            pageSize: pageSize.value
        });
    });
    
    const refreshData = async () => {
        if (searchQuery.value.trim() || selectedLocationId.value) {
            await searchSubLocations({
                searchTerm: searchQuery.value || '',
                locationId: selectedLocationId.value,
                page: currentPage.value,
                pageSize: pageSize.value
            });
        } else {
            await getSubLocationsByUserId();
        }
    };
    
    const handleSublocationAdded = async () => {
        await refreshData();
    };
    
    const handleLocationChange = async (newLocationId) => {
        // Clear data immediately before setting new location
        clearSearch();
        allSublocations.value = [];
        selectedLocationId.value = newLocationId;
    };
    
    const handleSearch = async (newSearchQuery) => {
        searchQuery.value = newSearchQuery;
        currentPage.value = 1; // Reset to first page
        // The watch will handle the API call
    };
    
    // Add method to reset to default view
    const resetToDefaultView = () => {
        selectedLocationId.value = '';
        searchQuery.value = '';
        currentPage.value = 1;
        clearSearch();
        allSublocations.value = [];
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
        currentPage,
        pageSize,
        totalCount,
        refreshData,
        handleSublocationAdded,
        handleLocationChange,
        handleSearch,
        resetToDefaultView
    };
}