export function useObjectiveActions({
    searchObjectives,
    getObjectivesByUserId,
    clearSearch,
    clearRiskControlMatrix,
    allObjectives,
    searchQuery,
    selectedRiskControlMatrixId,
    currentPage,
    pageSize
}) {

    const refreshData = async () => {
        try {
            const hasSearchOrFilter = searchQuery.value.trim() || selectedRiskControlMatrixId.value;

            if (hasSearchOrFilter) {
                if (searchObjectives) {
                    await searchObjectives({
                        searchTerm: searchQuery.value || '',
                        riskControlMatrixId: selectedRiskControlMatrixId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                if (getObjectivesByUserId) {
                    await getObjectivesByUserId();
                }
            }
            
        } catch (error) {
            console.error('Error refreshing objective data:', error);
        }
    };

    const handleObjectiveAdded = async () => {
        try {
            await refreshData();
        } catch (error) {
            console.error('Error handling objective addition:', error);
        }
    };

    const handleObjectiveChange = async (newRiskControlMatrixId) => {
        try {
            if (clearSearch) {
                clearSearch();
            }

            if (allObjectives?.value !== undefined) {
                allObjectives.value = [];
            }

            selectedRiskControlMatrixId.value = newRiskControlMatrixId;
        } catch (error) {
            console.error('Error handling objective change:', error);
        }
    };

    const resetToDefaultView = () => {
        try {
            selectedRiskControlMatrixId.value = null;
            searchQuery.value = '';
            currentPage.value = 1;

            if (clearSearch) {
                clearSearch();
            }

            if (allObjectives?.value !== undefined) {
                allObjectives.value = [];
            }

            if (clearRiskControlMatrix) {
                clearRiskControlMatrix();
            }
        } catch (error) {
            console.error('Error resetting to default view:', error);
        }
    };

    return {
        refreshData,
        handleObjectiveAdded,
        handleObjectiveChange,
        resetToDefaultView
    };
}