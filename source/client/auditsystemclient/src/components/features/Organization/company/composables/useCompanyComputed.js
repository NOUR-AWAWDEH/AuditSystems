import { computed } from 'vue';

export function useCompanyComputed({
    searchQuery,
    searchResults,
    allCompanies,
    isSearching,
    isLoadingAll,
    isInitializing
}) {
    const companies = computed(() => {
        // If there's an active search query, always return search results
        if (searchQuery?.value && searchQuery.value.trim()) {
            return (searchResults && searchResults.value)
                ? searchResults.value
                : [];
        }
        
        return (allCompanies && allCompanies.value)
            ? allCompanies.value
            : [];
    });

    const isLoading = computed(() => {
        const searching = (isSearching && isSearching.value) ? isSearching.value : false;
        const loadingAll = (isLoadingAll && isLoadingAll.value) ? isLoadingAll.value : false;
        const initializing = (isInitializing && isInitializing.value) ? isInitializing.value : false;
        
        return searching || loadingAll || initializing;
    });

    return {
        companies,
        isLoading
    };
}