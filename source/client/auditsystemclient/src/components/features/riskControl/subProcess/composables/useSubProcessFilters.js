// composables/useSubProcessFilters.js
import { ref, computed, watch } from 'vue';

export function useSubProcessFilters(subProcesses) {
    const searchQuery = ref('');

    const filteredSubProcesses = computed(() => {
        if (!Array.isArray(subProcesses.value)) return [];
        let filtered = subProcesses.value;

        if (searchQuery.value.trim()) {
            const query = searchQuery.value.toLowerCase().trim();
            filtered = filtered.filter((subProcess) =>
                subProcess.particular?.toLowerCase().includes(query)
            );
        }

        return filtered;
    });

    // Optional: auto-clear search if the list changes drastically
    watch(subProcesses, () => {
        if (searchQuery.value && filteredSubProcesses.value.length === 0) {
            searchQuery.value = '';
        }
    });

    return {
        searchQuery,
        filteredSubProcesses,
    };
}