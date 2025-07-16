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
    
    const { 
        sublocation: fetchedSubLocation,
        isLoading: isSubLocationLoading,
        fetchSubLocationById,
        clearSubLocation
    } = useGetSubLocationById();
    
    const {
        searchResults,
        isSearching,
        totalCount,
        searchDepartments,
        clearSearch
    } = useSearchDepartmentByUserId();
    
    const { 
        departments: allDepartments,
        isLoading: isLoadingAll,
        getDepartmentsByUserId 
    } = useGetDepartmentsByUserId();
    
    console.log('[Department] useDepartmentTable initialized', { sublocations });
    
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
        console.log('[Department] selectedSubLocation computed', { subLocationId, sublocation });
        return sublocation || null;
    });
    
    // Add currentSubLocation computed property
    const currentSubLocation = computed(() => {
        const sublocation = fetchedSubLocation.value || selectedSubLocation.value;
        return sublocation;
    });
    
    const departments = computed(() => {
        if (searchQuery.value.trim() || selectedSubLocationId.value) {
            console.log('[Department] departments computed: using searchResults', searchResults.value);
            return searchResults.value || [];
        } else {
            console.log('[Department] departments computed: using allDepartments', allDepartments.value);
            return allDepartments.value || [];
        }
    });
    
    const isLoading = computed(() => isSearching.value || isLoadingAll.value || isInitializing.value);
    
    const initializeData = async () => {
        if (!selectedSubLocationId.value || isInitialized.value || isInitializing.value) {
            return;
        }
        
        try {
            console.log('[Department] Initializing data', {
                searchTerm: searchQuery.value || '',
                subLocationId: selectedSubLocationId.value,
                page: currentPage.value,
                pageSize: pageSize.value
            });
            
            isInitializing.value = true;
            await searchDepartments({
                subLocationId: selectedSubLocationId.value,
                page: currentPage.value,
                pageSize: pageSize.value
            });
            
            console.log('[Department] Data initialized', searchResults.value);
            isInitialized.value = true;
        } catch (error) {
            console.error('[Department] Failed to initialize data:', error);
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
    
    watch(searchQuery, async (search) => {
        if (isInitializing.value) return;
        
        console.log('[Department] searchQuery changed, searching departments', {
            search,
            subLocationId: selectedSubLocationId.value
        });
        
        if (search.trim() || selectedSubLocationId.value) {
            await searchDepartments({
                searchTerm: search,
                subLocationId: selectedSubLocationId.value,
                page: currentPage.value,
                pageSize: pageSize.value
            });
        } else {
            console.log('[Department] searchQuery cleared, loading all departments');
            clearSearch();
            if (!selectedSubLocationId.value) {
                await getDepartmentsByUserId();
            }
        }
    });
    
    // Watch for selectedSubLocationId changes and fetch sublocation details
    watch(selectedSubLocationId, async (newSubLocationId) => {
        console.log('[Department] selectedSubLocationId changed:', newSubLocationId);
        clearSearch();
        allDepartments.value = [];
        
        if (!newSubLocationId) {
            clearSubLocation();
        }
        
        if (newSubLocationId) {
            try {
                await fetchSubLocationById(newSubLocationId);
            } catch (error) {
                console.error('[Department] Error fetching sublocation:', error);
            }
        }
        
        if (!newSubLocationId) {
            if (!searchQuery.value.trim()) {
                console.log('[Department] Refreshing data: loading all departments');
                await getDepartmentsByUserId();
            }
            return;
        }
        
        await searchDepartments({
            searchTerm: searchQuery.value || '',
            subLocationId: newSubLocationId,
            page: currentPage.value,
            pageSize: pageSize.value
        });
        
        console.log('[Department] SubLocation changed', newSubLocationId);
    });
    
    const refreshData = async () => {
        if (searchQuery.value.trim() || selectedSubLocationId.value) {
            await searchDepartments({
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
        console.log('[Department] Department added, refreshing data');
        await refreshData();
    };
    
    const handleSubLocationChange = async (newSubLocationId) => {
        console.log('[Department] SubLocation changed:', newSubLocationId);
        clearSearch();
        allDepartments.value = [];
        selectedSubLocationId.value = newSubLocationId;        
        
        if (newSubLocationId) {
            isInitialized.value = true;
        }
    };

    const handleSearch = async (newSearchQuery) => {
        console.log('[Department] Search triggered:', newSearchQuery);
        searchQuery.value = newSearchQuery;
        currentPage.value = 1;
    };

    const resetToDefaultView = () => {
        console.log('[Department] Resetting to default view');
        selectedSubLocationId.value = null;
        searchQuery.value = '';
        currentPage.value = 1;
        clearSearch();
        allDepartments.value = [];
        clearSubLocation();
        isInitialized.value = false;
        isInitializing.value = false;
    };
    
    onMounted(async () => {
        console.log('[Department] Component mounted');
        await nextTick();
        
        if (selectedSubLocationId.value && !isInitialized.value && !isInitializing.value) {
            if (sublocationsRef.value?.length > 0) {
                const sublocationExists = sublocationsRef.value.some(sub => sub.id === selectedSubLocationId.value);
                if (sublocationExists) {
                    console.log('[Department] SubLocation exists on mount, initializing data');
                    await initializeData();
                } else {
                    console.log('[Department] SubLocation does not exist on mount, resetting view');
                    resetToDefaultView();
                }
            }
        } else if (!selectedSubLocationId.value && !searchQuery.value.trim()) {
            console.log('[Department] No sublocation or search query on mount, loading all departments');
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