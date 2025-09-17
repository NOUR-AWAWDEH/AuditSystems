import { ref, onMounted, nextTick } from 'vue';

export function useExitControlLifecycle({
    selectedRiskId,
    currentPage,
    pageSize,
    searchExitControls,
    risksRef,
    searchQuery,
    getExitControlsByUserId,
    resetToDefaultView
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const initializeData = async () => {
        if (!selectedRiskId.value || isInitialized.value || isInitializing.value) {
            return;
        }

        try {
            isInitializing.value = true;
            if (searchExitControls) {
                await searchExitControls({
                    riskId: selectedRiskId.value,
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

        if (selectedRiskId.value && !isInitialized.value && !isInitializing.value) {
            if (risksRef.value?.length > 0) {
                const riskExists = risksRef.value.some(
                    risk => risk.id === selectedRiskId.value);
                if (riskExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedRiskId.value 
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