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
    getSubDepartmentById,
    initializeData,
    resetToDefaultView
}) {
    // Watch subDepartments and initialize data or reset view if sub-department doesn't exist
    watch(subDepartmentsRef, async (newSubDepartments) => {
        if (!newSubDepartments?.length || !selectedSubDepartment.value || isInitialized.value || isInitializing.value) {
            return;
        }

        const subDepartmentExists = newSubDepartments.some(sub => sub.id === selectedSubDepartmentId.value);
        if (subDepartmentExists) {
            await nextTick();
            await initializeData();  // Initialize the data if sub-department exists
        } else {
            resetToDefaultView();  // Reset view if sub-department is not found
        }
    }, { immediate: true });

    // Watch the search query and trigger search or reset accordingly
    watch(searchQuery, async (search, oldSearch) => {
        if (search === oldSearch) return;  // Only trigger when search term changes

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
            // Clear search results and reload data when search term is empty
            if (clearSearch) {
                clearSearch();
            }

            // Reload data based on selected sub-department
            if (selectedSubDepartmentId.value) {
                // Reload for selected sub-department
                if (searchAuditProcesses) {
                    await searchAuditProcesses({
                        searchTerm: '',
                        subDepartmentId: selectedSubDepartmentId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                // Load all audit processes when no sub-department is selected
                if (getAuditProcessesByUserId) {
                    await getAuditProcessesByUserId();
                }
            }
        }
    }, { debounce: 300 });  // Debounce to prevent excessive API calls

    // Watch selected sub-department ID and handle data fetching accordingly
    watch(selectedSubDepartmentId, async (newSubDepartmentId) => {
        if (clearSearch) clearSearch();  // Clear search when sub-department changes

        if (allAuditProcesses?.value !== undefined) {
            allAuditProcesses.value = [];  // Clear the existing audit processes
        }

        if (!newSubDepartmentId) {
            // Clear sub-department data if there's no selected sub-department
            if (clearSubDepartment) {
                clearSubDepartment();
            }
        }

        // Fetch the new sub-department data if a new sub-department is selected
        if (newSubDepartmentId) {
            try {
                await getSubDepartmentById(newSubDepartmentId);
            } catch (error) {
                console.error('[Process] Error fetching sub-department:', error);
            }
        }

        // If no sub-department is selected, reload all audit processes
        if (!newSubDepartmentId && !searchQuery.value.trim() && getAuditProcessesByUserId) {
            await getAuditProcessesByUserId();
            return;
        }

        // Fetch audit processes for the selected sub-department
        if (newSubDepartmentId) {
            await searchAuditProcesses({
                searchTerm: searchQuery.value || '',  // Search term is passed as an empty string if there's no query
                subDepartmentId: newSubDepartmentId,
                page: currentPage.value,
                pageSize: pageSize.value,
            });
        }
    }, { immediate: true });  // Immediate trigger for initial load
}