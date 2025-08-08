import { ref, computed } from 'vue';

export function useSubProcessFilters(subProcesses) {
    const searchQuery = ref('');

    const filteredSubProcesses = computed(() => {
        let filtered = subProcesses.value ?? [];

        if (searchQuery.value.trim()) {
            const query = searchQuery.value.toLowerCase().trim();
            filtered = filtered.filter((subProcess) =>
                (subProcess.particular || '').toLowerCase().includes(query)
            );
        }

        return filtered;
    });

    return {
        searchQuery,
        filteredSubProcesses
    };
}