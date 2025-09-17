import { ref, onMounted, nextTick } from 'vue';

export function useObjectiveLifecycle({
    selectedRiskControlMatrixId,
    currentPage,
    pageSize,
    searchObjectives,
    riskControlMatricesRef,
    searchQuery,
    getObjectivesByUserId,
    resetToDefaultView
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const initializeData = async () => {
        if (!selectedRiskControlMatrixId.value || isInitialized.value || isInitializing.value) {
            return;
        }

        try {
            isInitializing.value = true;
            if (searchObjectives) {
                await searchObjectives({
                    riskControlMatrixId: selectedRiskControlMatrixId.value,
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

        if (selectedRiskControlMatrixId.value && !isInitialized.value && !isInitializing.value) {
            if (riskControlMatricesRef.value?.length > 0) {
                const riskControlMatrixExists = riskControlMatricesRef.value.some(
                    riskControlMatrix => riskControlMatrix.id === selectedRiskControlMatrixId.value);
                if (riskControlMatrixExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedRiskControlMatrixId.value 
            && !searchQuery.value.trim() 
            && getObjectivesByUserId) {
                await getObjectivesByUserId();
        }
    });

    return {
        isInitialized,
        isInitializing,
        initializeData
    };
}