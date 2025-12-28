import { computed } from 'vue';

export function useRecommendationComputed({
    searchQuery,
    selectedObservationId,
    searchResults,
    allRecommendations,
    isSearching,
    isLoadingAll,
    isInitializing
}) {
    const recommendations = computed(() => {
        if ((searchQuery?.value?.trim()) || selectedObservationId?.value) {
            return searchResults?.value || [];
        }
        return allRecommendations?.value || [];
    });

    const isLoading = computed(() => {
        const searching = (isSearching && isSearching.value) ? isSearching.value : false;
        const loadingAll = (isLoadingAll && isLoadingAll.value) ? isLoadingAll.value : false;
        const initializing = (isInitializing && isInitializing.value) ? isInitializing.value : false;
        
        return searching || loadingAll || initializing;
    });

    return {
        recommendations,
        isLoading
    };
}