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
    watch(searchQuery, async (search) => {
        if (searchAuditProcesses) {
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