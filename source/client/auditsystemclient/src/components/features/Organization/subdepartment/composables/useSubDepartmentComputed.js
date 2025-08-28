import { computed } from 'vue';

export function useSubDepartmentComputed({
    searchQuery,
    selectedDepartmentId,
    searchResults,
    allSubDepartments,
    isSearching,
    isLoadingAll,
    isInitializing
}) {
    const subDepartments = computed(() => {
        if (searchQuery?.value && searchQuery.value.trim()) {
            return (searchResults && searchResults.value) ? searchResults.value : [];
        }

        if (selectedDepartmentId?.value) {
            return (searchResults && searchResults.value) ? searchResults.value : [];
        }

        return (allSubDepartments && allSubDepartments.value) ? allSubDepartments.value : [];
    });


    const isLoading = computed(() => {
        const searching = (isSearching && isSearching.value) ? isSearching.value : false;
        const loadingAll = (isLoadingAll && isLoadingAll.value) ? isLoadingAll.value : false;
        const initializing = (isInitializing && isInitializing.value) ? isInitializing.value : false;

        return searching || loadingAll || initializing;
    });

    return {
        subDepartments,
        isLoading
    };
}
