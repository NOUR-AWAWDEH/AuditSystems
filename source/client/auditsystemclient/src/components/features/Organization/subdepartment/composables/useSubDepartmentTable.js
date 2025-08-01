import { ref, watch, computed, onMounted, nextTick } from 'vue';
import { useSearchSubDepartmentByUserId } from '@/composables/organization/subdepartment/useSearchSubDepartmentByUserId';
import { useGetSubDepartmentsByUserId } from '@/composables/organization/subdepartment/useGetSubDepartmentsByUserId';
import { useGetDepartmentById } from '@/composables/organization/department/useGetDepartmentById';

export function useSubDepartmentTable(departments) {
    const searchQuery = ref('');
    const selectedDepartmentId = ref('');
    const currentPage = ref(1);
    const pageSize = ref(10);
    const isInitialized = ref(false);
    const isInitializing = ref(false);
    
    // Add useGetDepartmentById composable
    // Add useGetDepartmentById composable
    const { 
        department: fetchedDepartment = ref(null),
        isLoading: isDepartmentLoading = ref(false),
        fetchDepartmentById,
        clearDepartment 
    } = useGetDepartmentById() || {};
    
    const {
        searchResults = ref([]),
        isSearching = ref(false),
        totalCount = ref(0),
        searchSubDepartments,
        clearSearch
    } = useSearchSubDepartmentByUserId() || {};
    
    const { 
        subDepartments: allSubDepartments = ref([]), 
        isLoading: isLoadingAll = ref(false),
        getSubDepartmentsByUserId 
    } = useGetSubDepartmentsByUserId() || {};
    
    const departmentsRef = computed(() => {
        if (departments && typeof departments === 'object' && 'value' in departments) {
            return departments.value || [];
        }
        return departments || [];
    });
    
    const selectedDepartment = computed(() => {
        const departmentId = selectedDepartmentId.value;
        const departmentsList = departmentsRef.value;
        
        if (!departmentId || !departmentsList || departmentsList.length === 0) {
            return null;
        }
        
        const department = departmentsList.find(dep => dep.id === departmentId);
        return department || null;
    });
    
    // Add currentDepartment computed property
    const currentDepartment = computed(() => {
        const department = fetchedDepartment?.value || selectedDepartment?.value;
        return department?.departmentModel || department;
    });
    
    const subDepartments = computed(() => {
        if (searchQuery?.value?.trim() || selectedDepartmentId?.value) {
            return (searchResults && searchResults.value) ? searchResults.value : [];
        } else {
            return (allSubDepartments && allSubDepartments.value) ? allSubDepartments.value : [];
        }
    });
    
    const isLoading = computed(() => {
        const searching = (isSearching && isSearching.value) ? isSearching.value : false;
        const loadingAll = (isLoadingAll && isLoadingAll.value) ? isLoadingAll.value : false;
        const departmentLoading = (isDepartmentLoading && isDepartmentLoading.value) ? isDepartmentLoading.value : false;
        return searching || loadingAll || departmentLoading || isInitializing.value;
    });
    
    const initializeData = async () => {
        if (!selectedDepartmentId.value || isInitialized.value || isInitializing.value) {
            return;
        }
        
        try {
            isInitializing.value = true;
            await searchSubDepartments({
                searchTerm: searchQuery.value || '',
                departmentId: selectedDepartmentId.value,
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
    
    watch(departmentsRef, async (newDepartments) => {
        
        if (!newDepartments?.length || !selectedDepartmentId.value || isInitialized.value || isInitializing.value) {
            return;
        }
        
        const departmentExists = newDepartments.some(dep => dep.id === selectedDepartmentId.value);
        if (departmentExists) {
            await nextTick();
            await initializeData();
        } else {
            resetToDefaultView();
        }
    }, { immediate: true });
    
    watch(searchQuery, async (search) => {        
    if (search.trim()) {
            // Perform search when there's a search term
            await searchSubDepartments({
                searchTerm: search,
                departmentId: selectedDepartmentId.value,
                page: currentPage.value,
                pageSize: pageSize.value
            });
        } else {
            // Clear search results
            clearSearch();
            
            // Reload sub-departments based on current state
            if (selectedDepartmentId.value) {
                // If a department is selected, reload sub-departments for that department
                await searchSubDepartments({
                    searchTerm: '',
                    departmentId: selectedDepartmentId.value,
                    page: currentPage.value,
                    pageSize: pageSize.value
                });
            } else {
                // If no department is selected, load all sub-departments
                await getSubDepartmentsByUserId();
                searchResults.value = allSubDepartments.value || [];
            }
        }
    });
    
    // Watch for selectedDepartmentId changes and fetch department details
    watch(selectedDepartmentId, async (newDepartmentId) => {
        clearSearch();
        allSubDepartments.value = [];
        
        if (!newDepartmentId) {
            clearDepartment();
        }
        
        if (newDepartmentId) {
            try {
                await fetchDepartmentById(newDepartmentId);
            } catch (error) {
                console.error('Error fetching department:', error);
            }
        }
        
        if (!newDepartmentId) {
            if (!searchQuery.value.trim()) {
                await getSubDepartmentsByUserId();
            }
            return;
        }
        
        await searchSubDepartments({
            searchTerm: searchQuery.value || '',
            departmentId: newDepartmentId,
            page: currentPage.value,
            pageSize: pageSize.value
        });
    });
    
    const refreshData = async () => {
        if (searchQuery.value.trim() || selectedDepartmentId.value) {
            await searchSubDepartments({
                searchTerm: searchQuery.value || '',
                departmentId: selectedDepartmentId.value,
                page: currentPage.value,
                pageSize: pageSize.value
            });
        } else {
            await getSubDepartmentsByUserId();
            searchResults.value = allSubDepartments.value || [];
        }
    };
    
    const handleSubDepartmentAdded = async () => {
        await refreshData();
    };
    
    const handleDepartmentChange = async (newDepartmentId) => {
        
        clearSearch();
        allSubDepartments.value = [];
        
        selectedDepartmentId.value = newDepartmentId;
        
        if (newDepartmentId) {
            isInitialized.value = true;
        }
    };
    
    const handleSearch = async (newSearchQuery) => {
        searchQuery.value = newSearchQuery;
        currentPage.value = 1;
    };
    
    const resetToDefaultView = () => {
        selectedDepartmentId.value = '';
        searchQuery.value = '';
        currentPage.value = 1;
        clearSearch();
        allSubDepartments.value = [];
        clearDepartment();
        isInitialized.value = false;
        isInitializing.value = false;
    };
    
    onMounted(async () => {
        
        await nextTick();
        
        if (selectedDepartmentId.value && !isInitialized.value && !isInitializing.value) {
            if (departmentsRef.value?.length > 0) {
                const departmentExists = departmentsRef.value.some(dep => dep.id === selectedDepartmentId.value);
                if (departmentExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedDepartmentId.value && !searchQuery.value.trim()) {
            await getSubDepartmentsByUserId();
            // Add this line to ensure the table shows data on initial load
            searchResults.value = allSubDepartments.value || [];
        }
    });
    
    return {
        subDepartments,
        isLoading,
        selectedDepartmentId,
        selectedDepartment,
        currentDepartment,
        isDepartmentLoading,
        searchQuery,
        currentPage,
        pageSize,
        totalCount,
        refreshData,
        handleSubDepartmentAdded,
        handleDepartmentChange,
        handleSearch,
        resetToDefaultView,
        initializeData,
        fetchDepartmentById,
        clearDepartment
    };
}