export function useObservationActions({
    searchObservations,
    getObservationsByUserId,
    clearSearch,
    clearAuditProgram,
    allObservations,
    searchQuery,
    selectedAuditProgramId,
    currentPage,
    pageSize
}) {

    const refreshData = async () => {
        try {
            const hasSearchOrFilter = searchQuery.value.trim() || selectedAuditProgramId.value;

            if (hasSearchOrFilter) {
                if (searchObservations) {
                    await searchObservations({
                        searchTerm: searchQuery.value || '',
                        auditProgramId: selectedAuditProgramId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                if (getObservationsByUserId) {
                    await getObservationsByUserId();
                }
            }
            
        } catch (error) {
            console.error('Error refreshing observation data:', error);
        }
    };

    const handleObservationAdded = async () => {
        try {
            await refreshData();
        } catch (error) {
            console.error('Error handling observation addition:', error);
        }
    };

    const handleObservationChange = async (newAuditProgramId) => {
        try {
            if (clearSearch) {
                clearSearch();
            }

            if (allObservations?.value !== undefined) {
                allObservations.value = [];
            }

            selectedAuditProgramId.value = newAuditProgramId;
        } catch (error) {
            console.error('Error handling audit program change:', error);
        }
    };

    const resetToDefaultView = () => {
        try {
            selectedAuditProgramId.value = null;
            searchQuery.value = '';
            currentPage.value = 1;

            if (clearSearch) {
                clearSearch();
            }

            if (allObservations?.value !== undefined) {
                allObservations.value = [];
            }

            if (clearAuditProgram) {
                clearAuditProgram();
            }
        } catch (error) {
            console.error('Error resetting to default view:', error);
        }
    };

    return {
        refreshData,
        handleObservationAdded,
        handleObservationChange,
        resetToDefaultView
    };
}