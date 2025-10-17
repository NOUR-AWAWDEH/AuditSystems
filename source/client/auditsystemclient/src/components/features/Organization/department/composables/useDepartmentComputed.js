import { computed } from 'vue';

export function useDepartmentComputed({
    searchQuery,
    selectedLocationId,
    searchResults,
    allDepartments,
    isSearching,
    isLoadingAll,
    isInitializing
}) {
    const departments = computed(() => {
        // If there's an active search query, always return search results
        if (searchQuery?.value && searchQuery.value.trim()) {
            return (searchResults && searchResults.value)
                ? searchResults.value
                : [];
        }
        
        if (selectedLocationId?.value) {
            return (searchResults && searchResults.value)
                ? searchResults.value
                : [];
        }
        
        return (allDepartments && allDepartments.value)
            ? allDepartments.value
            : [];
    });

    const isLoading = computed(() => {
        const searching = (isSearching && isSearching.value) ? isSearching.value : false;
        const loadingAll = (isLoadingAll && isLoadingAll.value) ? isLoadingAll.value : false;
        const initializing = (isInitializing && isInitializing.value) ? isInitializing.value : false;
        
        return searching || loadingAll || initializing;
    });

    return {
        departments,
        isLoading
    };
}