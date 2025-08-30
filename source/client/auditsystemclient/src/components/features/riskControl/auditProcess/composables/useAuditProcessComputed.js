import { computed, onMounted } from 'vue';

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
        if (searchQuery?.value && searchQuery.value.trim()) {
            return (searchResults && searchResults.value) ? searchResults.value : [];
        }

        if (selectedSubDepartmentId?.value) {
            return (searchResults && searchResults.value) ? searchResults.value : [];
        }

        return (allAuditProcesses && allAuditProcesses.value) ? allAuditProcesses.value : [];
    });


    const isLoading = computed(() => {
        const searching = (isSearching && isSearching.value) ? isSearching.value : false;
        const loadingAll = (isLoadingAll && isLoadingAll.value) ? isLoadingAll.value : false;
        const initializing = (isInitializing && isInitializing.value) ? isInitializing.value : false;

        return searching || loadingAll || initializing;
    });

    return {
        auditProcesses,
        isLoading
    };
}
