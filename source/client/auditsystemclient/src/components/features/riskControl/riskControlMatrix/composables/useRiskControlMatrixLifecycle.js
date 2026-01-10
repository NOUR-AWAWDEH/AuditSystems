import { ref, onMounted, nextTick } from 'vue';

export function useRiskControlMatrixLifecycle({
    selectedAuditProcessId,
    currentPage,
    pageSize,
    searchRiskControlMatrices,
    searchQuery,
    getRiskControlMatricesByUserId,
    resetToDefaultView
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const initializeData = async () => {
        if (!selectedAuditProcessId.value || isInitialized.value || isInitializing.value) {
            return;
        }

        try {
            isInitializing.value = true;
            if (searchRiskControlMatrices) {
                await searchRiskControlMatrices({
                    auditProcessId: selectedAuditProcessId.value,
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

        if (selectedAuditProcessId.value && !isInitialized.value && !isInitializing.value) {
             await initializeData();
        } else if (!selectedAuditProcessId.value 
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