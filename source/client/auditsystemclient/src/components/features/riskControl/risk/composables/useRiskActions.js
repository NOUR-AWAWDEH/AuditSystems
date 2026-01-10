/**
 * Composable to manage actions related to user risks.
 *
 * Handles fetching, refreshing, and resetting risk data based on user interactions.
 *
 * @param {Object} params - Object containing reactive refs and functions:
 * @param {Function} params.searchRisks - Function to search risks based on query and objective.
 * @param {Function} params.getRisksByUserId - Function to fetch all risks for the current user.
 * @param {Function} params.clearSearch - Function to clear search results.
 * @param {Function} params.clearObjective - Function to clear the selected objective.
 * @param {Ref<Array>} params.allRisks - Reactive array holding all risks.
 * @param {Ref<string>} params.searchQuery - Reactive search query string.
 * @param {Ref<string|null>} params.selectedObjectiveId - Currently selected objective ID.
 * @param {Ref<number>} params.currentPage - Current pagination page.
 * @param {Ref<number>} params.pageSize - Number of items per page.
 *
 * Returns functions to handle risk actions:
 * - `refreshData` → Refreshes risk data based on search query or selected objective.
 * - `handleRiskAdded` → Refreshes the risk list when a new risk is added.
 * - `handleRiskChange` → Handles switching to a different objective, clears search, and resets risk list.
 * - `resetToDefaultView` → Resets all filters, search, and objectives to default state.
 *
 * Notes:
 * - All functions are safe and wrapped in try/catch to handle errors gracefully.
 * - Reactive: automatically updates risk data and UI when refs change.
 */

export function useRiskActions({
    searchRisks,
    getRisksByUserId,
    clearSearch,
    clearObjective,
    allRisks,
    searchQuery,
    selectedObjectiveId,
    currentPage,
    pageSize
}) {
    // Refreshes the risk list depending on whether the user has a search term or a selected objective.
    const refreshData = async () => {
        try {
            const hasSearchOrFilter = searchQuery.value.trim() || selectedObjectiveId.value;

            if (hasSearchOrFilter) {
                if (searchRisks) {
                    await searchRisks({
                        searchTerm: searchQuery.value || '',
                        observationId: selectedObjectiveId.value,
                        pageNumber: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                if (getRisksByUserId) {
                    await getRisksByUserId();
                }
            }
            
        } catch (error) {
            console.error('Error refreshing risk data:', error);
        }
    };

    // Called when a new risk is added.
    const handleRiskAdded = async () => {
        try {
            await refreshData();
        } catch (error) {
            console.error('Error handling risk addition:', error);
        }
    };

    // Called when the user changes the selected objective.
    // Handles switching to a different objective and refreshing the risk list.
    const handleRiskChange = async (newObjectiveId) => {
        try {
            if (clearSearch) {
                clearSearch();
            }

            if (allRisks?.value !== undefined) {
                allRisks.value = [];
            }

            selectedObjectiveId.value = newObjectiveId;
        } catch (error) {
            console.error('Error handling risk change:', error);
        }
    };

    // Resets everything to its default state
    const resetToDefaultView = () => {
        try {
            selectedObjectiveId.value = null;
            searchQuery.value = '';
            currentPage.value = 1;

            if (clearSearch) {
                clearSearch();
            }

            if (allRisks?.value !== undefined) {
                allRisks.value = [];
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
        handleRiskAdded,
        handleRiskChange,
        resetToDefaultView
    };
}
