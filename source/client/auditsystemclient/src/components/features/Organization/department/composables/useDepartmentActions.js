export function useDepartmentActions({
    searchDepartments,
    getDepartmentsByUserId,
    clearSearch,
    clearLocation,
    allDepartments,
    searchQuery,
    selectedLocationId,
    currentPage,
    pageSize
}) {
    const refreshData = async () => {
        const hasSearchOrFilter = searchQuery.value.trim() || selectedLocationId.value;
        if (hasSearchOrFilter) {
            if (searchDepartments) {
                await searchDepartments({
                    searchTerm: searchQuery.value || '',
                    locationId: selectedLocationId.value,
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

    const handleDepartmentChange = async (newLocationId) => {
        if (clearSearch) clearSearch();
        if (allDepartments?.value !== undefined) allDepartments.value = [];
        selectedLocationId.value = newLocationId;
    };

    const resetToDefaultView = () => {
        selectedLocationId.value = null;
        searchQuery.value = '';
        currentPage.value = 1;
        if (clearSearch) clearSearch();
        if (allDepartments?.value !== undefined) allDepartments.value = [];
        if (clearLocation) clearLocation();
    };

    return { refreshData, handleDepartmentChange, resetToDefaultView };
}