import { computed } from 'vue';

export function useExitControlComputed({
    searchQuery,
    selectedRiskId,
    searchResults,
    allExitControls,
    isSearching,
    isLoadingAll,
    isInitializing
}) {
    const exitControls = computed(() => {
        if (searchQuery?.value && searchQuery.value.trim()) {
            return (searchResults && searchResults.value)
                ? searchResults.value
                : [];
        }
        
        if (selectedRiskId?.value) {
            return (allExitControls && allExitControls.value)
                ? allExitControls.value
                : [];
        }
        
        return (allExitControls && allExitControls.value)
            ? allExitControls.value
            : [];
    });

    const isLoading = computed(() => {
        const searching = (isSearching && isSearching.value) ? isSearching.value : false;
        const loadingAll = (isLoadingAll && isLoadingAll.value) ? isLoadingAll.value : false;
        const initializing = (isInitializing && isInitializing.value) ? isInitializing.value : false;
        
        return searching || loadingAll || initializing;
    });

    return {
        exitControls,
        isLoading
    };
}