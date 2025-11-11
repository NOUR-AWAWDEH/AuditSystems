import { ref, computed, watch } from 'vue';


export function useRiskControlMatrixFilters(riskControlMatrices) {
    const searchQuery = ref('');

    const filteredRiskControlMatrices = computed(() => {
        if (!Array.isArray(riskControlMatrices.value)) return [];
        let filtered = riskControlMatrices.value;

        if (searchQuery.value.trim()) {
            const query = searchQuery.value.toLowerCase().trim();
            filtered = filtered.filter((riskControlMatrix) =>
                riskControlMatrix.description.toLowerCase().includes(query)
            );
        }

        return filtered;
    });

    // Optional: auto-clear search if the list changes drastically
    watch(riskControlMatrices, () => {
        if (searchQuery.value && filteredRiskControlMatrices.value.length === 0) {
            searchQuery.value = '';
        }
    });

    return {
        searchQuery,
        filteredRiskControlMatrices
    };
}