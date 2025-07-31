import { ref, watch, computed, onMounted, nextTick } from 'vue';
import { useSearchAuditProcessByUserId } from '@/composables/riskControl/auditProcess/useSearchAuditProcessByUserId';
import { useGetAuditProcessesByUserId } from '@/composables/riskControl/auditProcess/useGetAuditProcessesByUserId';
import { useGetSubDepartmentById } from '@/composables/organization/subdepartment/useGetSubDepartmentById';

export function useAuditProcessTable(subDepartments) {
    const searchQuery = ref('');
    const selectedSubDepartmentId = ref('');
    const currentPage = ref(1);
    const pageSize = ref(10);
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const {
        subDepartment: fetchSubDepartment,
        isLoading : isSubDepartmentLoading,
        fetchSubDepartmentById,
        clearSubDepartment
    } = useGetSubDepartmentById();

    const {
        searchResults = ref([]),
        isSearching =ref(false),
        totalCount = ref(0),
        searchAuditProcesses,
        clearSearch
    } = useSearchAuditProcessByUserId() || {};

    const {
        auditProcesses: allAuitProcesses = ref([]),
        isLoading: isLoadingAll = ref(false),
        getAuditProcessesByUserId
    } = useGetAuditProcessesByUserId() || {};

    const subDepartmentsRef = computed(() => {
        if (subDepartments && typeof subDepartments === 'object' && 'value' in subDepartments) {
            return subDepartments.value || [];
        }
        return subDepartments || [];
    });

    const selectedSubDepartment = computed(() =>{
        const subDepartmentId = selectedSubDepartmentId.value;
        const subDepartmentsList = subDepartmentsRef.value;
        
        if(!subDepartmentId || !subDepartmentsList || subDepartmentsList.length === 0) {
            return null;
        }
        const subDepartment = subDepartmentsList.find(sub => sub.id === subDepartmentId);
        return subDepartment || null;
    });

    const currentSubDepartment = computed(() => {
        const subDepartment = fetchSubDepartment?.value || selectedSubDepartment?.value;
        return subDepartment?.subDepartmentModel || subDepartment;
    });

    const auditProcesses = computed(() => {
        if ((searchQuery?.value && searchQuery.value.trim()) || (selectedSubDepartmentId?.value)) {
            return (searchResults && searchResults.value) 
                ? searchResults.value
                : []; 
        } else {
            return (allAuditProcesses && allAuditProcesses.value) // Fixed typo
                ? allAuditProcesses.value
                : [];
        }
    });

    const isLoading = computed(() => {
        const searching = (isSearching && isSearching.value) ? isSearching.value : false;
        const loadingAll = (isLoadingAll && isLoadingAll.value) ? isLoadingAll.value : false;
        return searching || loadingAll || isInitializing.value;
    });

    const initializeData = async () => {
        if (!selectedSubDepartmentId.value || isInitialized.value || isInitializing.value) {
            return;
        }

        try{
            isInitializing.value = true;
            if (searchAuditProcesses) {
                await searchAuditProcesses({
                    subDepartmentId: selectedSubDepartmentId.value,
                    page: currentPage.value,
                    pageSize: pageSize.value,
                });
            }
            isInitialized.value = true;
        } catch(error) {
            console.error('Error initializing data:', error);
        } finally {
            isInitializing.value = false;
        }
    };
    
    watch(subDepartmentsRef, async (newSubDepartments) => {
        if (!newSubDepartments?.length || !selectedSubDepartment.value || isInitialized.value || isInitializing.value) {
            return;
        }

        const subDepartmentExists = newSubDepartments.some(sub => sub.id === selectedSubDepartmentId.value);
        if (subDepartmentExists) {
            await nextTick();
            await initializeData();
        } else {
            resetToDefaultView();
        }
    }, { immediate: true });

    watch(searchQuery, async (search) => {
        if(searchAuditProcesses) {
            await searchAuditProcesses({
                searchTerm: search,
                subDepartmentId: selectedSubDepartmentId.value,
                page: currentPage.value,
                pageSize: pageSize.value,
            });
        } else {
            if (clearSearch) {
                clearSearch();
            }
            if (!selectedSubDepartment.value && getAuditProcessesByUserId) {
                await getAuditProcessesByUserId();
            }
        }
    });
    
    watch(selectedSubDepartmentId, async (newSubDepartmentId) => {
        if (clearSearch) {
            clearSearch();
        }

        if (allAuitProcesses?.value !== undefined) {
            allAuitProcesses.value = [];
        }

        if (!newSubDepartmentId) {
            if ( clearSubDepartment) {
                clearSubDepartment();
            }
        }
        
        if (newSubDepartmentId) {
            try {
                await fetchSubDepartmentById(newSubDepartmentId);
            } catch (error) {
                console.error('[Process] Error fetching subdepartment:', error);
            }
        }

        if(!newSubDepartmentId) {
            if (!searchQuery.value.trim() && getAuditProcessesByUserId) {
                getAuditProcessesByUserId();
            }
            return;
        }

        if (newSubDepartmentId) {
            await searchAuditProcesses({
                searchTerm: searchQuery.value || '',
                subDepartmentId: newSubDepartmentId,
                page: currentPage.value,
                pageSize: pageSize.value,
            });
        }
    });
    
    const refreshData =async () => {
        if (searchQuery.value.trim() || selectedSubDepartmentId.value) {
            if (searchAuditProcesses) {
                await searchAuditProcesses({
                    searchTerm: searchQuery.value || '',
                    subDepartmentId: selectedSubDepartmentId.value,
                    page: currentPage.value,
                    pageSize: pageSize.value,
                });
            }
        } else {
            if(getAuditProcessesByUserId) {
                await getAuditProcessesByUserId();
            }
        }
    };

    const handleAuditProcessAdded = async () => {
        await refreshData();
    };

    const handleAuditProcessChange = async (newSubDepartmentId) => {
        if (clearSearch) {
            clearSearch();
        }
        if (allAuitProcesses?.value !== undefined) {
            allAuitProcesses.value = [];
        }

        selectedSubDepartmentId.value = newSubDepartmentId;

        if (newSubDepartmentId) {
            isInitialized.value = true;
        }
    };

    const handleSearch = async (newSearchQuery) => { // Fixed function name
        searchQuery.value = newSearchQuery;
        currentPage.value = 1;
    };

    const resetToDefaultView = () => {
        selectedSubDepartmentId.value = null;
        searchQuery.value = '';
        currentPage.value = 1;
        if (clearSearch) {
            clearSearch();
        }
        if (allAuditProcesses?.value !== undefined) { // Fixed typo
            allAuditProcesses.value = [];
        }
        if (clearSubDepartment) {
            clearSubDepartment();
        }
        isInitialized.value = false;
        isInitializing.value = false;
    };

    onMounted( async () => {
        await nextTick();

        if (selectedSubDepartmentId.value && !isInitialized.value && !isInitializing.value) {
            if (subDepartmentsRef.value?.length > 0) {
                const subDepartmentExists = subDepartmentsRef.value.some(sub => sub.id === selectedSubDepartmentId.value);
                if (subDepartmentExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedSubDepartmentId.value && !searchQuery.value.trim() && getAuditProcessesByUserId) {
            await getAuditProcessesByUserId();
        }
    });

    return {
        auditProcesses,
        isLoading,
        selectedSubDepartmentId,
        selectedSubDepartment,
        currentSubDepartment,
        isSubDepartmentLoading,
        searchQuery,
        currentPage,
        pageSize,
        totalCount,
        refreshData,
        handleAuditProcessAdded,
        handleAuditProcessChange,
        handleSearch,
        resetToDefaultView,
    };
}

