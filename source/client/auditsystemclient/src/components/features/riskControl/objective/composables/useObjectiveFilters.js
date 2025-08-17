import { ref, computed, watch } from 'vue';

export function useObjectiveFilters(objectives) {
    const searchQuery = ref('');

    const filteredObjectives = computed(() => {
        if (!Array.isArray(objectives.value)) return [];
        let filtered = objectives.value;

        if (searchQuery.value.trim()) {
            const query = searchQuery.value.toLowerCase().trim();
            filtered = filtered.filter((objective) =>
                objective.description.toLowerCase().includes(query)
            );
        }

        return filtered;
    });

    // Optional: auto-clear search if the list changes drastically
    watch(objectives, () => {
        if (searchQuery.value && filteredObjectives.value.length === 0) {
            searchQuery.value = '';
        }
    });

    return {
        searchQuery,
        filteredObjectives
    };
}