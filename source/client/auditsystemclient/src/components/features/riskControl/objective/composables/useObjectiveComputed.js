import { computed } from 'vue';

export function useObjectiveComputed({
    searchQuery,
    selectedRiskControlMatrixId,
    searchResults,
    allObjectives,
    isSearching,
    isLoadingAll,
    isInitializing
}) {
    const objectives = computed(() => {
        if (searchQuery?.value && searchQuery.value.trim()) {
            return (searchResults && searchResults.value)
                ? searchResults.value
                : [];
        }
        
        if (selectedRiskControlMatrixId?.value) {
            return (searchResults && searchResults.value)
                ? searchResults.value
                : [];
        }
        
        return (allObjectives && allObjectives.value)
            ? allObjectives.value
            : [];
    });

    const isLoading = computed(() => {
        const searching = (isSearching && isSearching.value) ? isSearching.value : false;
        const loadingAll = (isLoadingAll && isLoadingAll.value) ? isLoadingAll.value : false;
        const initializing = (isInitializing && isInitializing.value) ? isInitializing.value : false;
        
        return searching || loadingAll || initializing;
    });

    return {
        objectives,
        isLoading
    };
}