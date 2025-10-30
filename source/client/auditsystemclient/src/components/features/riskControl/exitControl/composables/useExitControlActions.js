export function useExitControlActions({
    searchExitControls,
    getExitControlsByUserId,
    clearSearch,
    clearObjective,
    allExitControls,
    searchQuery,
    selectedObjectiveId,
    currentPage,
    pageSize
}) {

    const refreshData = async () => {
        try {
            const hasSearchOrFilter = searchQuery.value.trim() || selectedObjectiveId.value;

            if (hasSearchOrFilter) {
                if (searchExitControls) {
                    await searchExitControls({
                        searchTerm: searchQuery.value || '',
                        objectiveId: selectedObjectiveId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                if (getExitControlsByUserId) {
                    await getExitControlsByUserId();
                }
            }
            
        } catch (error) {
            console.error('Error refreshing Exit Control data:', error);
        }
    };

    const handleExitControlAdded = async () => {
        try {
            await refreshData();
        } catch (error) {
            console.error('Error handling Exit Control addition:', error);
        }
    };

    const handleExitControlChange = async (newObjectiveId) => {
        try {
            if (clearSearch) {
                clearSearch();
            }

            if (allExitControls?.value !== undefined) {
                allExitControls.value = [];
            }

            selectedObjectiveId.value = newObjectiveId;
        } catch (error) {
            console.error('Error handling Exit Control change:', error);
        }
    };

    const resetToDefaultView = () => {
        try {
            selectedObjectiveId.value = null;
            searchQuery.value = '';
            currentPage.value = 1;

            if (clearSearch) {
                clearSearch();
            }

            if (allExitControls?.value !== undefined) {
                allExitControls.value = [];
            }

            if (clearObjective) {
                clearObjective();
            }
        } catch (error) {
            console.error('Error resetting to default view:', error);
        }
    };

    return {
        refreshData,
        handleExitControlAdded,
        handleExitControlChange,
        resetToDefaultView
    };
}