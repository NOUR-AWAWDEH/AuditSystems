import { ref, watch, computed, onMounted, nextTick } from 'vue';
import { useSearchDepartmentByUserId } from '@/composables/organization/department/useSearchDepartmentByUserId';
import { useGetDepartmentsByUserId } from '@/composables/organization/department/useGetDepartmentsByUserId';
import { useGetSubLocationById } from '@/composables/organization/sublocation/useGetSubLocationById';

export function useDepartmentTable(sublocations) {
    const searchQuery = ref('');
    const selectedSubLocationId = ref('');
    const currentPage = ref(1);
    const pageSize = ref(10);
    const isInitialized = ref(false);
    const isInitializing = ref(false);
    
    // Add useGetSubLocationById composable
    // Update the destructuring to use the correct function name
    const { sublocation: fetchedSubLocation, isLoading: isSubLocationLoading, getSubLocationById, resetState: clearSubLocation } = useGetSubLocationById();
    
    const {
        searchResults,
        isSearching,
        totalCount,
        searchDepartmentByUserId,
        clearSearch
    } = useSearchDepartmentByUserId();
    
    const { 
        departments: allDepartments, 
        isLoading: isLoadingAll,
        getDepartmentsByUserId 
    } = useGetDepartmentsByUserId();
    
    const sublocationsRef = computed(() => {
        if (sublocations && typeof sublocations === 'object' && 'value' in sublocations) {
            return sublocations.value || [];
        }
        return sublocations || [];
    });
    
    const selectedSubLocation = computed(() => {
        const subLocationId = selectedSubLocationId.value;
        const sublocationsList = sublocationsRef.value;
        
        if (!subLocationId || !sublocationsList || sublocationsList.length === 0) {
            return null;
        }
        
        const sublocation = sublocationsList.find(sub => sub.id === subLocationId);
        return sublocation || null;
    });
    
    // Add currentSubLocation computed property
    const currentSubLocation = computed(() => {
        const sublocation = fetchedSubLocation.value || selectedSubLocation.value;
        // Handle nested sublocationModel structure
        return sublocation?.sublocationModel || sublocation;
    });
    
    const departments = computed(() => {
        if (searchQuery.value.trim() || selectedSubLocationId.value) {
            return searchResults.value;
        } else {
            return allDepartments.value;
        }
    });
    
    const isLoading = computed(() => isSearching.value || isLoadingAll.value || isInitializing.value);
    
    // Update initializeData function (around line 73)
    const initializeData = async () => {
        if (!selectedSubLocationId.value || isInitialized.value || isInitializing.value) {
            return;
        }
        
        try {
            isInitializing.value = true;
            await searchDepartmentByUserId({
                searchTerm: searchQuery.value || '',
                subLocationId: selectedSubLocationId.value,
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
    
    watch(sublocationsRef, async (newSubLocations) => {
        
        if (!newSubLocations?.length || !selectedSubLocationId.value || isInitialized.value || isInitializing.value) {
            return;
        }
        
        const sublocationExists = newSubLocations.some(sub => sub.id === selectedSubLocationId.value);
        if (sublocationExists) {
            await nextTick();
            await initializeData();
        } else {
            resetToDefaultView();
        }
    }, { immediate: true });
    
    // Update watch(searchQuery) function (around line 100)
    watch(searchQuery, async (search) => {
        if (isInitializing.value) return;
        
        if (search.trim() || selectedSubLocationId.value) {
            await searchDepartmentByUserId({
                searchTerm: search,
                subLocationId: selectedSubLocationId.value,
                page: currentPage.value,
                pageSize: pageSize.value
            });
        } else {
            clearSearch();
            if (!selectedSubLocationId.value) {
                await getDepartmentsByUserId();
            }
        }
    });
    
    // Watch for selectedSubLocationId changes and fetch sublocation details
    // Update watch(selectedSubLocationId) function (around line 133)
    watch(selectedSubLocationId, async (newSubLocationId) => {
        clearSearch();
        allDepartments.value = [];
        
        if (!newSubLocationId) {
            clearSubLocation();
        }
        
        if (newSubLocationId) {
            try {
                await getSubLocationById(newSubLocationId);
            } catch (error) {
                console.error('Error fetching sublocation:', error);
            }
        }
        
        if (!newSubLocationId) {
            if (!searchQuery.value.trim()) {
                await getDepartmentsByUserId();
            }
            return;
        }
        
        await searchDepartmentByUserId({
            searchTerm: searchQuery.value || '',
            subLocationId: newSubLocationId,
            page: currentPage.value,
            pageSize: pageSize.value
        });
    });
    
    // Update refreshData function (around line 160)
    const refreshData = async () => {
        if (searchQuery.value.trim() || selectedSubLocationId.value) {
            await searchDepartmentByUserId({
                searchTerm: searchQuery.value || '',
                subLocationId: selectedSubLocationId.value,
                page: currentPage.value,
                pageSize: pageSize.value
            });
        } else {
            await getDepartmentsByUserId();
        }
    };
    
    const handleDepartmentAdded = async () => {
        await refreshData();
    };
    
    const handleSubLocationChange = async (newSubLocationId) => {
        
        clearSearch();
        allDepartments.value = [];
        
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
        selectedSubLocationId.value = '';
        searchQuery.value = '';
        currentPage.value = 1;
        clearSearch();
        allDepartments.value = [];
        clearSubLocation();
        isInitialized.value = false;
        isInitializing.value = false;
    };
    
    onMounted(async () => {
        
        await nextTick();
        
        if (selectedSubLocationId.value && !isInitialized.value && !isInitializing.value) {
            if (sublocationsRef.value?.length > 0) {
                const sublocationExists = sublocationsRef.value.some(sub => sub.id === selectedSubLocationId.value);
                if (sublocationExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedSubLocationId.value && !searchQuery.value.trim()) {
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