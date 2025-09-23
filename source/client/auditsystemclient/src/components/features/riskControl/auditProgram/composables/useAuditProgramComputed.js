import { computed, onMounted } from 'vue';

export function useAuditProgramComputed({
    searchQuery,
    selectedExitControlId,
    searchResults,
    allAuditPrograms,
    isSearching,
    isLoadingAll,
    isInitializing
}) {
    const auditPrograms = computed(() => {
        if (searchQuery?.value && searchQuery.value.trim()) {
            return (searchResults && searchResults.value)
                ? searchResults.value
                : [];
        }
        
        if (selectedExitControlId?.value) {
            return (searchResults && searchResults.value)
                ? searchResults.value
                : [];
        }
        
        return (allAuditPrograms && allAuditPrograms.value)
            ? allAuditPrograms.value
            : [];
    });

    const isLoading = computed(() => {
        const searching = (isSearching && isSearching.value) ? isSearching.value : false;
        const loadingAll = (isLoadingAll && isLoadingAll.value) ? isLoadingAll.value : false;
        const initializing = (isInitializing && isInitializing.value) ? isInitializing.value : false;
        
        return searching || loadingAll || initializing;
    });

    return {
        auditPrograms,
        isLoading
    };
}