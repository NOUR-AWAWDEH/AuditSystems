import { computed } from 'vue';

export function useSubProcessComputed({
    searchQuery,
    selectedAuditProcessId,
    searchResults,
    allSubProcesses,
    isSearching,
    isLoadingAll,
    isInitializing
}) {
    const subProcesses = computed(() => {
        if (searchQuery?.value && searchQuery.value.trim()) {
            return (searchResults && searchResults.value)
                ? searchResults.value
                : [];
        }
        
        if (selectedAuditProcessId?.value) {
            return (searchResults && searchResults.value)
                ? searchResults.value
                : [];
        }
        
        return (allSubProcesses && allSubProcesses.value)
            ? allSubProcesses.value
            : [];
    });

    const isLoading = computed(() => {
        const searching = (isSearching && isSearching.value) ? isSearching.value : false;
        const loadingAll = (isLoadingAll && isLoadingAll.value) ? isLoadingAll.value : false;
        const initializing = (isInitializing && isInitializing.value) ? isInitializing.value : false;
        
        return searching || loadingAll || initializing;
    });

    return {
        subProcesses,
        isLoading
    };
}