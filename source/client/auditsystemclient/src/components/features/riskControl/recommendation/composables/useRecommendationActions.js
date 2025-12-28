export function useRecommendationActions({
    searchRecommendations,
    getRecommendationsByUserId,
    clearSearch,
    clearObservation,
    allRecommendations,
    searchQuery,
    selectedObservationId,
    currentPage,
    pageSize
}) {
    // Refreshes the recommendation list depending on whether the user has a search term or a selected observation.
    const refreshData = async () => {
        try {
            const hasSearchOrFilter = searchQuery.value.trim() || selectedObservationId.value;

            if (hasSearchOrFilter) {
                if (searchRecommendations) {
                    await searchRecommendations({
                        searchTerm: searchQuery.value || '',
                        observationId: selectedObservationId.value,
                        pageNumber: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                if (getRecommendationsByUserId) {
                    await getRecommendationsByUserId();
                }
            }
            
        } catch (error) {
            console.error('Error refreshing recommendation data:', error);
        }
    };

    // Called when a new recommendation is added.
    const handleRecommendationAdded = async () => {
        try {
            await refreshData();
        } catch (error) {
            console.error('Error handling recommendation addition:', error);
        }
    };

    // Called when the user changes the selected observation.
    // Handles switching to a different observation and refreshing the recommendation list.
    const handleRecommendationChange = async (newObservationId) => {
        try {
            if (clearSearch) {
                clearSearch();
            }

            if (allRecommendations?.value !== undefined) {
                allRecommendations.value = [];
            }

            selectedObservationId.value = newObservationId;
        } catch (error) {
            console.error('Error handling recommendation change:', error);
        }
    };

    // Resets everything to its default state
    const resetToDefaultView = () => {
        try {
            selectedObservationId.value = null;
            searchQuery.value = '';
            currentPage.value = 1;

            if (clearSearch) {
                clearSearch();
            }

            if (allRecommendations?.value !== undefined) {
                allRecommendations.value = [];
            }

            if (clearObservation) {
                clearObservation();
            }
        } catch (error) {
            console.error('Error resetting to default view:', error);
        }
    };

    return {
        refreshData,
        handleRecommendationAdded,
        handleRecommendationChange,
        resetToDefaultView
    };
}
