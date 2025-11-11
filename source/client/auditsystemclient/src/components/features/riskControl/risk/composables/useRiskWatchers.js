import { watch, nextTick } from 'vue';

/**
 * Composable that sets up watchers for risk-related state in order to keep data
 * in sync with user actions such as selecting an objective, changing the search query,
 * or switching between objectives.
 *
 * @param {Object} params - Reactive references and actions for managing risks.
 * @param {Ref<Array>} params.objectivesRef - List of available objectives (risk control matrices).
 * @param {Ref<Object|null>} params.selectedObjective - The currently selected objective object.
 * @param {Ref<string|null>} params.selectedObjectiveId - ID of the selected objective.
 * @param {Ref<string>} params.searchQuery - Current search term for filtering risks.
 * @param {Ref<number>} params.currentPage - Current pagination page number.
 * @param {Ref<number>} params.pageSize - Number of items per page.
 * @param {Ref<boolean>} params.isInitialized - Flag indicating whether data has been initialized.
 * @param {Ref<boolean>} params.isInitializing - Flag indicating if initialization is in progress.
 *
 * @param {Function} params.searchRisks - Function to search risks by term/objective/page.
 * @param {Function} params.clearSearch - Function to clear search results.
 * @param {Function} params.getRisksByUserId - Function to load all risks for the user.
 * @param {Ref<Array>} params.allRisks - Reactive list of all risks (mutable).
 * @param {Function} params.clearObjective - Function to reset selected objective.
 * @param {Function} params.getObjectiveById - Function to fetch a specific objective by ID.
 * @param {Function} params.initializeData - Function to initialize data when an objective is valid.
 * @param {Function} params.resetToDefaultView - Function to reset view when objective is invalid.
 *
 * Behavior:
 * - Watches `selectedObjective`:
 *   → Initializes data if a valid objective is chosen and not already initialized.
 *   → Resets to default view if the objective no longer exists.
 *
 * - Watches `searchQuery`:
 *   → Performs a filtered search when query has text.
 *   → Clears search and reloads data if query is empty.
 *     • Reloads current objective risks if one is selected.
 *     • Loads all risks if no objective is selected.
 *
 * - Watches `selectedObjectiveId`:
 *   → Clears search and resets risks list.
 *   → Clears objective if none is selected.
 *   → Fetches objective by ID when selected.
 *   → Refreshes risks based on new objective and current search query.
 */
export function useRiskWatchers({
    objectivesRef,
    selectedObjective,
    selectedObjectiveId,
    searchQuery,
    currentPage,
    pageSize,
    isInitialized,
    isInitializing,
    searchRisks,
    clearSearch,
    getRisksByUserId,
    allRisks,
    clearObjective,
    getObjectiveById,
    initializeData,
    resetToDefaultView
}) {
    // Watch for changes in the selected objective object
    watch(
        selectedObjective,
        async (newObjective) => {
            if (!newObjective || isInitialized.value || isInitializing.value) {
                return;
            }

            const objectiveExists = objectivesRef.value.some(
                matrix => matrix.id === selectedObjectiveId.value
            );

            if (objectiveExists) {
                await nextTick();
                await initializeData();
            } else {
                resetToDefaultView();
            }
        },
        { immediate: true }
    );

    // Watch for changes in the search query
    watch(searchQuery, async (search) => {
        const trimmed = search.trim();

        if (trimmed) {
            // Perform search when query is not empty
            if (searchRisks) {
                await searchRisks({
                    searchTerm: trimmed,
                    objectiveId: selectedObjectiveId.value,
                    page: currentPage.value,
                    pageSize: pageSize.value
                });
            }
        } else {
            // Clear results when search is empty
            if (clearSearch) clearSearch();

            if (selectedObjectiveId.value) {
                // Reload risks for the selected objective
                if (searchRisks) {
                    await searchRisks({
                        searchTerm: '',
                        objectiveId: selectedObjectiveId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value
                    });
                }
            } else if (getRisksByUserId) {
                // Load all risks if no objective is selected
                await getRisksByUserId();
            }
        }
    });

    // Watch for changes in the selected objective ID
    watch(selectedObjectiveId, async (newObjectiveId) => {
        if (clearSearch) clearSearch();
        if (allRisks?.value !== undefined) allRisks.value = [];

        if (!newObjectiveId) {
            if (clearObjective) clearObjective();

            if (!searchQuery.value.trim() && getRisksByUserId) {
                await getRisksByUserId();
            }
            return;
        }

        try {
            await getObjectiveById(newObjectiveId);
        } catch (error) {
            console.error('[Objective] Error fetching objective:', error);
        }

        if (searchRisks) {
            await searchRisks({
                searchTerm: searchQuery.value || '',
                objectiveId: newObjectiveId,
                page: currentPage.value,
                pageSize: pageSize.value
            });
        }
    });
}