export function useDepartmentActions({
    searchDepartments,
    getDepartmentsByUserId,
    clearSearch,
    clearSubLocation,
    allDepartments,
    searchQuery,
    selectedSubLocationId,
    currentPage,
    pageSize
}) {
    const refreshData = async () => {
        const hasSearchOrFilter = searchQuery.value.trim() || selectedSubLocationId.value;
        if (hasSearchOrFilter) {
            if (searchDepartments) {
                await searchDepartments({
                    searchTerm: searchQuery.value || '',
                    subLocationId: selectedSubLocationId.value,
                    page: currentPage.value,
                    pageSize: pageSize.value,
                });
            }
        } else {
            if (getDepartmentsByUserId) {
                await getDepartmentsByUserId();
            }
        }
    };

    const handleDepartmentChange = async (newSubLocationId) => {
        if (clearSearch) clearSearch();
        if (allDepartments?.value !== undefined) allDepartments.value = [];
        selectedSubLocationId.value = newSubLocationId;
    };

    const resetToDefaultView = () => {
        selectedSubLocationId.value = null;
        searchQuery.value = '';
        currentPage.value = 1;
        if (clearSearch) clearSearch();
        if (allDepartments?.value !== undefined) allDepartments.value = [];
        if (clearSubLocation) clearSubLocation();
    };

    return { refreshData, handleDepartmentChange, resetToDefaultView };
}