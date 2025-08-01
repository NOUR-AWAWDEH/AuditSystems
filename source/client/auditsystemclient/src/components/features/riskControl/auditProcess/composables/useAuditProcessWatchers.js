import { watch, nextTick } from 'vue';

export function useAuditProcessWatchers({
    subDepartmentsRef,
    selectedSubDepartment,
    selectedSubDepartmentId,
    searchQuery,
    currentPage,
    pageSize,
    isInitialized,
    isInitializing,
    searchAuditProcesses,
    clearSearch,
    getAuditProcessesByUserId,
    allAuditProcesses,
    clearSubDepartment,
    fetchSubDepartmentById,
    initializeData,
    resetToDefaultView
}) {
    // Watch subDepartments
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

    // Watch search query
    // Watch search query
watch(searchQuery, async (search) => {
        if (search.trim()) {
            // Perform search when there's a search term
            if (searchAuditProcesses) {
                await searchAuditProcesses({
                    searchTerm: search,
                    subDepartmentId: selectedSubDepartmentId.value,
                    page: currentPage.value,
                    pageSize: pageSize.value,
                });
            }
        } else {
            // Clear search results
            if (clearSearch) {
                clearSearch();
            }
            
            // Reload audit processes based on current state
            if (selectedSubDepartmentId.value) {
                // If a sub-department is selected, reload audit processes for that sub-department
                if (searchAuditProcesses) {
                    await searchAuditProcesses({
                        searchTerm: '',
                        subDepartmentId: selectedSubDepartmentId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                // If no sub-department is selected, load all audit processes
                if (getAuditProcessesByUserId) {
                    await getAuditProcessesByUserId();
                }
            }
        }
    });

    // Watch selected sub department
    watch(selectedSubDepartmentId, async (newSubDepartmentId) => {
        if (clearSearch) {
            clearSearch();
        }

        if (allAuditProcesses?.value !== undefined) {
            allAuditProcesses.value = [];
        }

        if (!newSubDepartmentId) {
            if (clearSubDepartment) {
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

        if (!newSubDepartmentId) {
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
}