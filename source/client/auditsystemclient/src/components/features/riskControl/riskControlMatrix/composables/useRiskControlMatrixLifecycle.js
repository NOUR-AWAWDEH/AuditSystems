import { ref, onMounted, nextTick } from 'vue';

export function useRiskControlMatrixLifecycle({
    selectedSubProcessId,
    currentPage,
    pageSize,
    searchRiskControlMatrices,
    subProcessesRef,
    searchQuery,
    getRiskControlMatricesByUserId,
    resetToDefaultView
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const initializeData = async () => {
        if (!selectedSubProcessId.value || isInitialized.value || isInitializing.value) {
            return;
        }

        try {
            isInitializing.value = true;
            if (searchRiskControlMatrices) {
                await searchRiskControlMatrices({
                    subProcessId: selectedSubProcessId.value,
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

        if (selectedSubProcessId.value && !isInitialized.value && !isInitializing.value) {
            if (subProcessesRef.value?.length > 0) {
                const subProcessExists = subProcessesRef.value.some(
                    subProcess => subProcess.id === selectedSubProcessId.value);
                if (subProcessExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedSubProcessId.value 
            && !searchQuery.value.trim() 
            && getRiskControlMatricesByUserId) {
                await getRiskControlMatricesByUserId();
        }
    });

    return {
        isInitialized,
        isInitializing,
        initializeData
    };
}