import { ref, computed, watch } from 'vue';
import { useGetRiskById } from '@/composables/riskControl/risk/useGetRiskById';

export function useRiskManagement(risks) {
    // Initialize from localStorage - USE THE SAME KEY AS RiskControl.vue
    const selectedRiskId = ref(localStorage.getItem('riskControlSelectedRiskId') || '');

    // Watch for changes and persist to localStorage
    watch(selectedRiskId, (newValue) => {
        if (newValue) {
            localStorage.setItem('riskControlSelectedRiskId', newValue);
        } else {
            localStorage.removeItem('riskControlSelectedRiskId');
        }
    });

    const {
        risk: fetchRisk,
        isLoading: isRiskLoading,
        getRiskById,
        clearRisk
    } = useGetRiskById();

    // Add watcher to fetch Risk details when selectedRiskId changes
    watch(selectedRiskId, async (newRiskId) => {
        if (newRiskId) {
            try {
                await getRiskById(newRiskId);
            } catch (error) {
                console.error('Error fetching risk details:', error);
            }
        } else {
            clearRisk();
        }
    }, { immediate: true });

    const risksRef = computed(() => {
        if (risks && typeof risks === 'object' && 'value' in risks) {
            return risks.value || [];
        }
        return risks || [];
    });

    const selectedRisk = computed(() => {
        const riskId = selectedRiskId.value;
        const risksList = risksRef.value;

        if (!riskId || !risksList || risksList.length === 0) {
            return null;
        }
        const risk = risksList.find(risk => risk.id === riskId);
        return risk || null;
    });

    const currentRisk = computed(() => {
        const risk = fetchRisk?.value || selectedRisk?.value;
        return risk?.RiskModel || risk;
    });

    return {
        selectedRiskId,
        selectedRisk,
        currentRisk,
        isRiskLoading,
        risksRef,
        getRiskById,
        clearRisk
    };
}