import { ref, onMounted, nextTick } from 'vue';

export function useExitControlLifecycle({
    selectedObjectiveId,
    currentPage,
    pageSize,
    searchExitControls,
    objectivesRef,
    searchQuery,
    getExitControlsByUserId,
    resetToDefaultView
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const initializeData = async () => {
        if (!selectedObjectiveId.value || isInitialized.value || isInitializing.value) {
            return;
        }

        try {
            isInitializing.value = true;
            if (searchExitControls) {
                await searchExitControls({
                    objectiveId: selectedObjectiveId.value,
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

        if (selectedObjectiveId.value && !isInitialized.value && !isInitializing.value) {
            if (objectivesRef.value?.length > 0) {
                const objectiveExists = objectivesRef.value.some(
                    objective => objective.id === selectedObjectiveId.value);
                if (objectiveExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedObjectiveId.value 
            && !searchQuery.value.trim() 
            && getExitControlsByUserId) {
                await getExitControlsByUserId();
        }
    });

    return {
        isInitialized,
        isInitializing,
        initializeData
    };
}