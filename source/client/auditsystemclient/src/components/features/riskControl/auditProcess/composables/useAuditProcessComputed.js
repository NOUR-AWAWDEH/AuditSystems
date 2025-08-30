import { computed } from 'vue';

export function useAuditProcessComputed({
    searchQuery,
    selectedSubDepartmentId,
    searchResults,
    allAuditProcesses,
    isSearching,
    isLoadingAll,
    isInitializing
}) {
    const auditProcesses = computed(() => {
        // If there's an active search query OR a sub-department is selected, use searchResults
        if (searchQuery?.value && searchQuery.value.trim()) {
            return searchResults?.value || [];
        }

        if (selectedSubDepartmentId?.value) {
            return searchResults?.value || [];
        }

        // Otherwise, use all audit processes
        return allAuditProcesses?.value || [];
    });

    const isLoading = computed(() => {
        const searching = isSearching?.value || false;
        const loadingAll = isLoadingAll?.value || false;
        const initializing = isInitializing?.value || false;

        return searching || loadingAll || initializing;
    });

    return {
        auditProcesses,
        isLoading
    };
}
