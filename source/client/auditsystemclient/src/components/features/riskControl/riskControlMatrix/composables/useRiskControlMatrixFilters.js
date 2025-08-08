import { ref, computed } from 'vue';

export function useRiskControlMatrixFilters(riskControlMatrices) {
    const searchQuery = ref('');

    const filteredRiskControlMatrices = computed(() => {
        let filtered = riskControlMatrices.value;

        if (searchQuery.value.trim()) {
            const query = searchQuery.value.toLowerCase().trim();
            filtered = filtered.filter((riskControlMatrix) =>
                riskControlMatrix.description.toLowerCase().includes(query)
            );
        }

        return filtered;
    });

    return {
        searchQuery,
        filteredRiskControlMatrices
    };
}