import {ref, computed } from 'vue';

export function useAuditProcessFilters(auditProcesses) {
    const searchQuery = ref('');

    const filteredAuditProcesses = computed(() => {
        let filtered = auditProcesses.value;
        
        if (searchQuery.value.trim()) {
            const query = searchQuery.value.toLowerCase().trim();
            filtered = filtered.filter((auditProcess) =>
                auditProcess.processName.toLowerCase().includes(query)
            );
        }

        return filtered;
    });

    return {
        searchQuery,
        filteredAuditProcesses
    };
}