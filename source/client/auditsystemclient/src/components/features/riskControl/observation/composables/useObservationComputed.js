import { computed } from 'vue';

export function useObservationComputed({
    searchQuery,
    selectedAuditProgramId,
    searchResults,
    allObservations,
    isSearching,
    isLoadingAll,
    isInitializing
}) {
    const observations = computed(() => {
        if (searchQuery?.value && searchQuery.value.trim()) {
            return (searchResults && searchResults.value)
                ? searchResults.value
                : [];
        }
        
        if (selectedAuditProgramId?.value) {
            return (searchResults && searchResults.value)
                ? searchResults.value
                : [];
        }
        
        return (allObservations && allObservations.value)
            ? allObservations.value
            : [];
    });

    const isLoading = computed(() => {
        const searching = (isSearching && isSearching.value) ? isSearching.value : false;
        const loadingAll = (isLoadingAll && isLoadingAll.value) ? isLoadingAll.value : false;
        const initializing = (isInitializing && isInitializing.value) ? isInitializing.value : false;
        
        return searching || loadingAll || initializing;
    });

    return {
        observations,
        isLoading
    };
}