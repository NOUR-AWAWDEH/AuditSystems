import { ref, onMounted, nextTick } from 'vue';

export function useRecommendationLifecycle({
    selectedObservationId,
    currentPage,
    pageSize,
    searchRecommendations,
    observationsRef,
    searchQuery,
    getRecommendationsByUserId,
    resetToDefaultView
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const initializeData = async () => {
        if (!selectedObservationId.value || isInitialized.value || isInitializing.value) {
            return;
        }

        try {
            isInitializing.value = true;
            if (searchRecommendations) {
                await searchRecommendations({
                    observationId: selectedObservationId.value,
                    page: currentPage.value,
                    pageSize: pageSize.value,
                });
            }
            isInitialized.value = true;
        } catch (error) {
            console.error('Error initializing data:', error);
        } finally {
            isInitializing.value = false;
        }
    };

    onMounted(async () => {
        await nextTick();

        if (selectedObservationId.value && !isInitialized.value && !isInitializing.value) {
            if (observationsRef.value?.length > 0) {
                const observationExists = observationsRef.value.some(
                    observation => observation.id === selectedObservationId.value);
                if (observationExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedObservationId.value 
            && !searchQuery.value.trim() 
            && getRecommendationsByUserId) {
                await getRecommendationsByUserId();
        }
    });

    return {
        isInitialized,
        isInitializing,
        initializeData
    };
}