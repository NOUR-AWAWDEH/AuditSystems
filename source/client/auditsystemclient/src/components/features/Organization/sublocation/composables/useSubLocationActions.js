export function useSubLocationActions({
    searchSubLocations,
    getSubLocationsByUserId,
    clearSearch,
    clearLocation,
    allSubLocations,
    searchQuery,
    selectedLocationId,
    currentPage,
    pageSize
}) {
    const refreshData = async () => {
        const hasSearchOrFilter = searchQuery.value.trim() || selectedLocationId.value;
        if (hasSearchOrFilter) {
            if (searchSubLocations) {
                await searchSubLocations({
                    searchTerm: searchQuery.value || '',
                    locationId: selectedLocationId.value,
                    page: currentPage.value,
                    pageSize: pageSize.value,
                });
            }
        } else {
            if (getSubLocationsByUserId) {
                await getSubLocationsByUserId();
            }
        }
    };

    const handleSubLocationChange = async (newLocationId) => {
        if (clearSearch) clearSearch();
        if (allSubLocations?.value !== undefined) allSubLocations.value = [];
        selectedLocationId.value = newLocationId;
    };

    const resetToDefaultView = () => {
        selectedLocationId.value = null;
        searchQuery.value = '';
        currentPage.value = 1;
        if (clearSearch) clearSearch();
        if (allSubLocations?.value !== undefined) allSubLocations.value = [];
        if (clearLocation) clearLocation();
    };

    return { refreshData, handleSubLocationChange, resetToDefaultView };
}
