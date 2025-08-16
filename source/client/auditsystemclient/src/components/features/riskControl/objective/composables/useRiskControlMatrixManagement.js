import { ref, computed, watch } from 'vue';
import { useGetRiskControlMatrixById } from '@/composables/riskControl/riskControlMatrix/useGetRiskControlMatrixById';

export function useRiskControlMatrixManagement(riskControlMatrices) {
    // Initialize from localStorage - USE THE SAME KEY AS RiskControl.vue
    const selectedRiskControlMatrixId = ref(localStorage.getItem('riskControlSelectedRiskControlMatrixId') || '');

    // Watch for changes and persist to localStorage
    watch(selectedRiskControlMatrixId, (newValue) => {
        if (newValue) {
            localStorage.setItem('riskControlSelectedRiskControlMatrixId', newValue);
        } else {
            localStorage.removeItem('riskControlSelectedRiskControlMatrixId');
        }
    });

    const {
        riskControlMatrix: fetchRiskControlMatrix,
        isLoading: isRiskControlMatrixLoading,
        fetchRiskControlMatrixById,
        clearRiskControlMatrix
    } = useGetRiskControlMatrixById();

    // Add watcher to fetch RiskControlMatrix details when selectedRiskControlMatrixId changes
    watch(selectedRiskControlMatrixId, async (newRiskControlMatrixId) => {
        if (newRiskControlMatrixId) {
            try {
                await fetchRiskControlMatrixById(newRiskControlMatrixId);
            } catch (error) {
                console.error('Error fetching RiskControlMatrix details:', error);
            }
        } else {
            clearRiskControlMatrix();
        }
    }, { immediate: true });

    const riskControlMatricesRef = computed(() => {
        if (riskControlMatrices && typeof riskControlMatrices === 'object' && 'value' in riskControlMatrices) {
            return riskControlMatrices.value || [];
        }
        return riskControlMatrices || [];
    });

    const selectedRiskControlMatrix = computed(() => {
        const riskControlMatrixId = selectedRiskControlMatrixId.value;
        const riskControlMatricesList = riskControlMatricesRef.value;

        if (!riskControlMatrixId || !riskControlMatricesList || riskControlMatricesList.length === 0) {
            return null;
        }
        const riskControlMatrix = riskControlMatricesList.find(riskControlMatrix => riskControlMatrix.id === riskControlMatrixId);
        return riskControlMatrix || null;
    });

    const currentRiskControlMatrix = computed(() => {
        const riskControlMatrix = fetchRiskControlMatrix?.value || selectedRiskControlMatrix?.value;
        return riskControlMatrix?.riskControlMatrixModel || riskControlMatrix;
    });

    return {
        selectedRiskControlMatrixId,
        selectedRiskControlMatrix,
        currentRiskControlMatrix,
        isRiskControlMatrixLoading,
        riskControlMatricesRef,
        fetchRiskControlMatrixById,
        clearRiskControlMatrix
    };
}