import { watch, nextTick } from 'vue';

/**
 * Composable that sets up watchers for risk-related state in order to keep data
 * in sync with user actions such as selecting an objective, changing the search query,
 * or switching between objectives.
 *
 * @param {Object} params - Reactive references and actions for managing risks.
 * @param {Ref<Array>} params.observationsRef - List of available observations (risk control matrices).
 * @param {Ref<Object|null>} params.selectedObservation - The currently selected observation object.
 * @param {Ref<string|null>} params.selectedObservationId - ID of the selected observation.
 * @param {Ref<string>} params.searchQuery - Current search term for filtering risks.
 * @param {Ref<number>} params.currentPage - Current pagination page number.
 * @param {Ref<number>} params.pageSize - Number of items per page.
 * @param {Ref<boolean>} params.isInitialized - Flag indicating whether data has been initialized.
 * @param {Ref<boolean>} params.isInitializing - Flag indicating if initialization is in progress.
 *
 * @param {Function} params.searchRisks - Function to search risks by term/observation/page.
 * @param {Function} params.clearSearch - Function to clear search results.
 * @param {Function} params.getRisksByUserId - Function to load all risks for the user.
 * @param {Ref<Array>} params.allRisks - Reactive list of all risks (mutable).
 * @param {Function} params.clearObservation - Function to reset selected observation.
 * @param {Function} params.getObservationById - Function to fetch a specific observation by ID.
 * @param {Function} params.initializeData - Function to initialize data when an observation is valid.
 * @param {Function} params.resetToDefaultView - Function to reset view when observation is invalid.
 *
 * Behavior:
 * - Watches `selectedObservation`:
 *   → Initializes data if a valid observation is chosen and not already initialized.
 *   → Resets to default view if the observation no longer exists.
 *
 * - Watches `searchQuery`:
 *   → Performs a filtered search when query has text.
 *   → Clears search and reloads data if query is empty.
 *     • Reloads current objective risks if one is selected.
 *     • Loads all risks if no objective is selected.
 *
 * - Watches `selectedObservationId`:
 *   → Clears search and resets risks list.
 *   → Clears observation if none is selected.
 *   → Fetches observation by ID when selected.
 *   → Refreshes risks based on new observation and current search query.
 */
export function useRiskWatchers({
    observationsRef,
    selectedObservation,
    selectedObservationId,
    searchQuery,
    currentPage,
    pageSize,
    isInitialized,
    isInitializing,
    searchRisks,
    clearSearch,
    getRisksByUserId,
    allRisks,
    clearObservation,
    getObservationById,
    initializeData,
    resetToDefaultView
}) {
    // Watch for changes in the selected observation object
    watch(
        selectedObservation,
        async (newObservation) => {
            if (!newObservation || isInitialized.value || isInitializing.value) {
                return;
            }

            const observationExists = observationsRef.value.some(
                matrix => matrix.id === selectedObservationId.value
            );

            if (observationExists) {
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
                    observationId: selectedObservationId.value,
                    page: currentPage.value,
                    pageSize: pageSize.value
                });
            }
        } else {
            // Clear results when search is empty
            if (clearSearch) clearSearch();

            if (selectedObservationId.value) {
                // Reload risks for the selected observation
                if (searchRisks) {
                    await searchRisks({
                        searchTerm: '',
                        observationId: selectedObservationId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value
                    });
                }
            } else if (getRisksByUserId) {
                // Load all risks if no observation is selected
                await getRisksByUserId();
            }
        }
    });

    // Watch for changes in the selected observation ID
    watch(selectedObservationId, async (newObservationId) => {
        if (clearSearch) clearSearch();
        if (allRisks?.value !== undefined) allRisks.value = [];

        if (!newObservationId) {
            if (clearObservation) clearObservation();

            if (!searchQuery.value.trim() && getRisksByUserId) {
                await getRisksByUserId();
            }
            return;
        }

        try {
            await getObservationById(newObservationId);
        } catch (error) {
            console.error('[Observation] Error fetching observation:', error);
        }

        if (searchRisks) {
            await searchRisks({
                searchTerm: searchQuery.value || '',
                observationId: newObservationId,
                page: currentPage.value,
                pageSize: pageSize.value
            });
        }
    });
}