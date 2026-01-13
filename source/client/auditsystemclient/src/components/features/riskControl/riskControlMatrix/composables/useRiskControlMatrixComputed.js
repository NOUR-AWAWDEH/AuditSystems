import { computed } from 'vue';

export function useRiskControlMatrixComputed({
    searchQuery,
    selectedAuditProcessId,
    searchResults,
    allRiskControlMatrices,
    isSearching,
    isLoadingAll,
    isInitializing
}) {
    const riskControlMatrices = computed(() => {
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
        
        return (allRiskControlMatrices && allRiskControlMatrices.value)
            ? allRiskControlMatrices.value
            : [];
    });

    const isLoading = computed(() => {
        const searching = (isSearching && isSearching.value) ? isSearching.value : false;
        const loadingAll = (isLoadingAll && isLoadingAll.value) ? isLoadingAll.value : false;
        const initializing = (isInitializing && isInitializing.value) ? isInitializing.value : false;
        
        return searching || loadingAll || initializing;
    });

    return {
        riskControlMatrices,
        isLoading
    };
}