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
        const hasSearchQuery = searchQuery?.value && searchQuery.value.trim();
        const hasSearchResults = searchResults && searchResults.value && searchResults.value.length > 0;
        const hasAll = allAuditProcesses && allAuditProcesses.value && allAuditProcesses.value.length > 0;

        // ✅ If user typed something → always show search results
        if (hasSearchQuery) {
            return searchResults?.value || [];
        }

        // ✅ If sub-department selected but no search → fallback to all processes
        if (selectedSubDepartmentId?.value) {
            return hasSearchResults ? searchResults.value : (allAuditProcesses?.value || []);
        }

        // ✅ Default → all processes
        return allAuditProcesses?.value || [];
    });

    const isLoading = computed(() => {
        return Boolean(
            (isSearching && isSearching.value) ||
            (isLoadingAll && isLoadingAll.value) ||
            (isInitializing && isInitializing.value)
        );
    });
    
    return {
        auditProcesses,
        isLoading
    };
}
