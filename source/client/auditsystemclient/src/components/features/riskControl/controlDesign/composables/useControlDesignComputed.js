import { computed } from 'vue';

export function useControlDesignComputed({
    searchQuery,
    selectedExitControlId,
    searchResults,
    allControlDesigns,
    isSearching,
    isLoadingAll,
    isInitializing
}) {
    const controlDesigns = computed(() => {
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
        
        return (allControlDesigns && allControlDesigns.value)
            ? allControlDesigns.value
            : [];
    });

    const isLoading = computed(() => {
        const searching = (isSearching && isSearching.value) ? isSearching.value : false;
        const loadingAll = (isLoadingAll && isLoadingAll.value) ? isLoadingAll.value : false;
        const initializing = (isInitializing && isInitializing.value) ? isInitializing.value : false;
        
        return searching || loadingAll || initializing;
    });

    return {
        controlDesigns,
        isLoading
    };
}