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
    const { 
        department: fetchedDepartment,
        isLoading: isDepartmentLoading,
        fetchDepartmentById,
        clearDepartment 
    } = useGetDepartmentById();
    
    const {
        searchResults,
        isSearching,
        totalCount,
        searchSubDepartments,
        clearSearch
    } = useSearchSubDepartmentByUserId();
    
    const { 
        subdepartments: allSubDepartments, 
        isLoading: isLoadingAll,
        getSubDepartmentsByUserId 
    } = useGetSubDepartmentsByUserId();
    
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
        const department = fetchedDepartment.value || selectedDepartment.value;
        // Handle nested departmentModel structure
        return department?.departmentModel || department;
    });
    
    const subdepartments = computed(() => {
        if (searchQuery.value.trim() || selectedDepartmentId.value) {
            return searchResults.value;
        } else {
            return allSubDepartments.value;
        }
    });
    
    const isLoading = computed(() => isSearching.value || isLoadingAll.value || isInitializing.value);
    
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
        if (isInitializing.value) return;
        
        if (search.trim() || selectedDepartmentId.value) {
            await searchSubDepartments({
                searchTerm: search,
                departmentId: selectedDepartmentId.value,
                page: currentPage.value,
                pageSize: pageSize.value
            });
        } else {
            clearSearch();
            if (!selectedDepartmentId.value) {
                await getSubDepartmentsByUserId();
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
        }
    });
    
    return {
        subdepartments,
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
        // Add the missing functions
        fetchDepartmentById,
        clearDepartment
    };
}