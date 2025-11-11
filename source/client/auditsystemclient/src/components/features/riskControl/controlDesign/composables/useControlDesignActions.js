export function useControlDesignActions({
    searchControlDesigns,
    getControlDesignsByUserId,
    clearSearch,
    clearExitControl,
    allControlDesigns,
    searchQuery,
    selectedExitControlId,
    currentPage,
    pageSize
}) {

    const refreshData = async () => {
        try {
            const hasSearchOrFilter = searchQuery.value.trim() || selectedExitControlId.value;

            if (hasSearchOrFilter) {
                if (searchControlDesigns) {
                    await searchControlDesigns({
                        searchTerm: searchQuery.value || '',
                        exitControlId: selectedExitControlId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                if (getControlDesignsByUserId) {
                    await getControlDesignsByUserId();
                }
            }
            
        } catch (error) {
            console.error('Error refreshing Control Design data:', error);
        }
    };

    const handleControlDesignAdded = async () => {
        try {
            await refreshData();
        } catch (error) {
            console.error('Error handling Control Design addition:', error);
        }
    };

    const handleControlDesignChange = async (newExitControlId) => {
        try {
            if (clearSearch) {
                clearSearch();
            }

            if (allControlDesigns?.value !== undefined) {
                allControlDesigns.value = [];
            }

            selectedExitControlId.value = newExitControlId;
        } catch (error) {
            console.error('Error handling Control Design change:', error);
        }
    };

    const resetToDefaultView = () => {
        try {
            selectedExitControlId.value = null;
            searchQuery.value = '';
            currentPage.value = 1;

            if (clearSearch) {
                clearSearch();
            }

            if (allControlDesigns?.value !== undefined) {
                allControlDesigns.value = [];
            }

            if (clearExitControl) {
                clearExitControl();
            }
        } catch (error) {
            console.error('Error resetting to default view:', error);
        }
    };

    return {
        refreshData,
        handleControlDesignAdded,
        handleControlDesignChange,
        resetToDefaultView
    };
}