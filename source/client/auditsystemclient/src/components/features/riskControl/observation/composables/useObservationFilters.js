import { ref, computed, watch } from 'vue';

export function useObservationFilters(observations) {
    const searchQuery = ref('');

    const filteredObservations = computed(() => {
        if (!Array.isArray(observations.value)) return [];
        let filtered = observations.value;

        if (searchQuery.value.trim()) {
            const query = searchQuery.value.toLowerCase().trim();
            filtered = filtered.filter((observation) =>
                observation.particular?.toLowerCase().includes(query)
            );
        }

        return filtered;
    });

    // Optional: auto-clear search if the list changes drastically
    watch(observations, () => {
        if (searchQuery.value && filteredObservations.value.length === 0) {
            searchQuery.value = '';
        }
    });

    return {
        searchQuery,
        filteredObservations,
    };
}