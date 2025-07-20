import { ref, watch, computed, onMounted, nextTick } from 'vue';
import { useSearchDepartmentByUserId } from '@/composables/organization/department/useSearchDepartmentByUserId';
import { useGetDepartmentsByUserId } from '@/composables/organization/department/useGetDepartmentsByUserId';
import { useGetSubLocationById } from '@/composables/organization/sublocation/useGetSubLocationById';

export function useDepartmentTable(subLocations) {
    const searchQuery = ref('');
    const selectedSubLocationId = ref('');
    const currentPage = ref(1);
    const pageSize = ref(10);
    const isInitialized = ref(false);
    const isInitializing = ref(false);
    
    const { 
        subLocation: fetchedSubLocation,
        isLoading: isSubLocationLoading,
        fetchSubLocationById,
        clearSubLocation
    } = useGetSubLocationById();
    
    // Safer destructuring with default values
    const {
        searchResults = ref([]),
        isSearching = ref(false),
        totalCount = ref(0),
        searchDepartments,
        clearSearch
    } = useSearchDepartmentByUserId() || {};
    
    const { 
        departments: allDepartments = ref([]),
        isLoading: isLoadingAll = ref(false),
        getDepartmentsByUserId 
    } = useGetDepartmentsByUserId() || {};
    
    
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
        
        const subLocation = subLocationsList.find(sub => sub.id === subLocationId);
        return subLocation || null;
    });
    
    // Add currentSubLocation computed property
    const currentSubLocation = computed(() => {
        const subLocation = fetchedSubLocation?.value || selectedSubLocation?.value;
        return subLocation?.subLocationModel || subLocation;
    });
    
    const departments = computed(() => {
        if ((searchQuery?.value && searchQuery.value.trim()) || (selectedSubLocationId?.value)) {
            return (searchResults && searchResults.value) ? searchResults.value : [];
        } else {
            return (allDepartments && allDepartments.value) ? allDepartments.value : [];
        }
    });
    
    const isLoading = computed(() => {
        const searching = (isSearching && isSearching.value) ? isSearching.value : false;
        const loadingAll = (isLoadingAll && isLoadingAll.value) ? isLoadingAll.value : false;
        return searching || loadingAll || isInitializing.value;
    });
    
    const initializeData = async () => {
        if (!selectedSubLocationId.value || isInitialized.value || isInitializing.value) {
            return;
        }
        
        try {
            isInitializing.value = true;
            if (searchDepartments) {
                await searchDepartments({
                    subLocationId: selectedSubLocationId.value,
                    page: currentPage.value,
                    pageSize: pageSize.value
                });
            }
            isInitialized.value = true;
        } catch (error) {
            console.error('[Department] Failed to initialize data:', error);
        } finally {
            isInitializing.value = false;
        }
    };
    
    watch(subLocationsRef, async (newSubLocations) => {
        if (!newSubLocations?.length || !selectedSubLocationId.value || isInitialized.value || isInitializing.value) {
            return;
        }
        
        const subLocationExists = newSubLocations.some(sub => sub.id === selectedSubLocationId.value);
        if (subLocationExists) {
            await nextTick();
            await initializeData();
        } else {
            resetToDefaultView();
        }
    }, { immediate: true });
    
    watch(searchQuery, async (search) => {
        if (isInitializing.value) return;
        
        if (search.trim() || selectedSubLocationId.value) {
            if (searchDepartments) {
                await searchDepartments({
                    searchTerm: search,
                    subLocationId: selectedSubLocationId.value,
                    page: currentPage.value,
                    pageSize: pageSize.value
                });
            }
        } else {
            if (clearSearch) {
                clearSearch();
            }
            if (!selectedSubLocationId.value && getDepartmentsByUserId) {
                await getDepartmentsByUserId();
            }
        }
    });
    
    // Watch for selectedSubLocationId changes and fetch SubLocation details
    watch(selectedSubLocationId, async (newSubLocationId) => {
        if (clearSearch) {
            clearSearch();
        }
        if (allDepartments?.value !== undefined) {
            allDepartments.value = [];
        }
        
        if (!newSubLocationId) {
            if (clearSubLocation) {
                clearSubLocation();
            }
        }
        
        if (newSubLocationId) {
            try {
                await fetchSubLocationById(newSubLocationId);
            } catch (error) {
                console.error('[Department] Error fetching sub location:', error);
            }
        }
        
        if (!newSubLocationId) {
            if (!searchQuery.value.trim() && getDepartmentsByUserId) {
                await getDepartmentsByUserId();
            }
            return;
        }
        
        if (searchDepartments) {
            await searchDepartments({
                searchTerm: searchQuery.value || '',
                subLocationId: newSubLocationId,
                page: currentPage.value,
                pageSize: pageSize.value
            });
        }
    });
    
    const refreshData = async () => {
        if (searchQuery.value.trim() || selectedSubLocationId.value) {
            if (searchDepartments) {
                await searchDepartments({
                    searchTerm: searchQuery.value || '',
                    subLocationId: selectedSubLocationId.value,
                    page: currentPage.value,
                    pageSize: pageSize.value
                });
            }
        } else {
            if (getDepartmentsByUserId) {
                await getDepartmentsByUserId();
            }
        }
    };
    
    const handleDepartmentAdded = async () => {
        await refreshData();
    };
    
    const handleSubLocationChange = async (newSubLocationId) => {
        if (clearSearch) {
            clearSearch();
        }
        if (allDepartments?.value !== undefined) {
            allDepartments.value = [];
        }
        selectedSubLocationId.value = newSubLocationId;
        
        if (newSubLocationId) {
            isInitialized.value = true;
        }
    };

    const handleSearch = async (newSearchQuery) => {
        searchQuery.value = newSearchQuery;
        currentPage.value = 1;
    };

    const resetToDefaultView = () => {
        selectedSubLocationId.value = null;
        searchQuery.value = '';
        currentPage.value = 1;
        if (clearSearch) {
            clearSearch();
        }
        if (allDepartments?.value !== undefined) {
            allDepartments.value = [];
        }
        if (clearSubLocation) {
            clearSubLocation();
        }
        isInitialized.value = false;
        isInitializing.value = false;
    };
    
    onMounted(async () => {
        await nextTick();
        
        if (selectedSubLocationId.value && !isInitialized.value && !isInitializing.value) {
            if (subLocationsRef.value?.length > 0) {
                const subLocationExists = subLocationsRef.value.some(sub => sub.id === selectedSubLocationId.value);
                if (subLocationExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedSubLocationId.value && !searchQuery.value.trim() && getDepartmentsByUserId) {
            await getDepartmentsByUserId();
        }
    });
    
    return {
        departments,
        isLoading,
        selectedSubLocationId,
        selectedSubLocation, 
        currentSubLocation,
        isSubLocationLoading,
        searchQuery,
        currentPage,
        pageSize,
        totalCount,
        refreshData,
        handleDepartmentAdded,
        handleSubLocationChange,
        handleSearch,
        resetToDefaultView,
        initializeData
    };
}
