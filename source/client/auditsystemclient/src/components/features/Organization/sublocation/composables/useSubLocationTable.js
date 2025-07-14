import { ref, watch, computed, onMounted, nextTick } from 'vue';
import { useSearchSubLocationByUserId } from '@/composables/organization/sublocation/useSearchSubLocationByUserId';
import { useGetSubLocationsByUserId } from '@/composables/organization/sublocation/useGetSubLocationsByUserId';
import { useGetLocationById } from '@/composables/organization/location/useGetLocationById';

export function useSubLocationTable(locations) {
    const searchQuery = ref('');
    const selectedLocationId = ref('');
    const currentPage = ref(1);
    const pageSize = ref(10);
    const isInitialized = ref(false);
    const isInitializing = ref(false);
    
    // Add useGetLocationById composable
    const { location: fetchedLocation, isLoading: isLocationLoading, fetchLocationById, clearLocation } = useGetLocationById();
    
    const {
        searchResults,
        isSearching,
        totalCount,
        searchSubLocations,
        clearSearch
    } = useSearchSubLocationByUserId();
    
    const { 
        sublocations: allSublocations, 
        isLoading: isLoadingAll,
        getSubLocationsByUserId 
    } = useGetSubLocationsByUserId();
    
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
    
    // Add currentLocation computed property
    const currentLocation = computed(() => {
        const location = fetchedLocation.value || selectedLocation.value;
        // Handle nested locationModel structure
        return location?.locationModel || location;
    });
    
    const sublocations = computed(() => {
        if (searchQuery.value.trim() || selectedLocationId.value) {
            return searchResults.value;
        } else {
            return allSublocations.value;
        }
    });
    
    const isLoading = computed(() => isSearching.value || isLoadingAll.value || isInitializing.value);
    
    const initializeData = async () => {
        if (!selectedLocationId.value || isInitialized.value || isInitializing.value) {
            return;
        }
        
        try {
            isInitializing.value = true;
            await searchSubLocations({
                searchTerm: searchQuery.value || '',
                locationId: selectedLocationId.value,
                page: currentPage.value,
                pageSize: pageSize.value
            });
            isInitialized.value = true;
        } catch (error) {
            console.error('Failed to initialize data:', error);
        } finally {
            isInitializing.value = false;
        }
    };
    
    watch(locationsRef, async (newLocations) => {
        
        if (!newLocations?.length || !selectedLocationId.value || isInitialized.value || isInitializing.value) {
            return;
        }
        
        const locationExists = newLocations.some(loc => loc.id === selectedLocationId.value);
        if (locationExists) {
            await nextTick();
            await initializeData();
        } else {
            resetToDefaultView();
        }
    }, { immediate: true });
    
    watch(searchQuery, async (search) => {
        if (isInitializing.value) return;
        
        if (search.trim() || selectedLocationId.value) {
            await searchSubLocations({
                searchTerm: search,
                locationId: selectedLocationId.value,
                page: currentPage.value,
                pageSize: pageSize.value
            });
        } else {
            clearSearch();
            if (!selectedLocationId.value) {
                await getSubLocationsByUserId();
            }
        }
    });
    
    // Watch for selectedLocationId changes and fetch location details
    watch(selectedLocationId, async (newLocationId) => {
        clearSearch();
        allSublocations.value = [];
        
        if (!newLocationId) {
            clearLocation();
        }
        
        if (newLocationId) {
            try {
                await fetchLocationById(newLocationId);
            } catch (error) {
                console.error('Error fetching location:', error);
            }
        }
        
        if (!newLocationId) {
            if (!searchQuery.value.trim()) {
                await getSubLocationsByUserId();
            }
            return;
        }
        
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
        
        clearSearch();
        allSublocations.value = [];
        
        selectedLocationId.value = newLocationId;
        
        if (newLocationId) {
            isInitialized.value = true;
        }
    };
    
    const handleSearch = async (newSearchQuery) => {
        searchQuery.value = newSearchQuery;
        currentPage.value = 1;
    };
    
    
    const resetToDefaultView = () => {
        selectedLocationId.value = '';
        searchQuery.value = '';
        currentPage.value = 1;
        clearSearch();
        allSublocations.value = [];
        clearLocation();
        isInitialized.value = false;
        isInitializing.value = false;
    };
    
    onMounted(async () => {
        
        await nextTick();
        
        if (selectedLocationId.value && !isInitialized.value && !isInitializing.value) {
            if (locationsRef.value?.length > 0) {
                const locationExists = locationsRef.value.some(loc => loc.id === selectedLocationId.value);
                if (locationExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedLocationId.value && !searchQuery.value.trim()) {
            await getSubLocationsByUserId();
        }
    });
    
    return {
        sublocations,
        isLoading,
        selectedLocationId,
        selectedLocation, 
        currentLocation, // Add currentLocation to return
        isLocationLoading, // Add isLocationLoading to return
        searchQuery,
        currentPage,
        pageSize,
        totalCount,
        refreshData,
        handleSublocationAdded,
        handleLocationChange,
        handleSearch,
        resetToDefaultView,
        initializeData
    };
}