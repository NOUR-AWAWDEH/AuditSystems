import { watch, nextTick } from 'vue';

export function useRecommendationWatchers({
    observationsRef,
    selectedObservation,
    selectedObservationId,
    searchQuery,
    currentPage,
    pageSize,
    isInitialized,
    isInitializing,
    searchRecommendations,
    clearSearch,
    getRecommendationsByUserId,
    allRecommendations,
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
            if (searchRecommendations) {
                await searchRecommendations({
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
                // Reload recommendations for the selected observation
                if (searchRecommendations) {
                    await searchRecommendations({
                        searchTerm: '',
                        observationId: selectedObservationId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value
                    });
                }
            } else if (getRecommendationsByUserId) {
                // Load all recommendations if no observation is selected
                await getRecommendationsByUserId();
            }
        }
    });

    // Watch for changes in the selected observation ID
    watch(selectedObservationId, async (newObservationId) => {
        if (clearSearch) clearSearch();
        if (allRecommendations?.value !== undefined) allRecommendations.value = [];

        if (!newObservationId) {
            if (clearObservation) clearObservation();

            if (!searchQuery.value.trim() && getRecommendationsByUserId) {
                await getRecommendationsByUserId();
            }
            return;
        }

        try {
            await getObservationById(newObservationId);
        } catch (error) {
            console.error('[Observation] Error fetching observation:', error);
        }

        if (searchRecommendations) {
            await searchRecommendations({
                searchTerm: searchQuery.value || '',
                observationId: newObservationId,
                page: currentPage.value,
                pageSize: pageSize.value
            });
        }
    });
}