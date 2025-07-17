import { ref, computed } from 'vue';

export function useSubDepartmentFilters(subdepartments) {
    const searchQuery = ref('');
    
    const filteredSubDepartments = computed(() => {
        let filtered = [...subdepartments.value];
        
        // Apply search filter
        if (searchQuery.value.trim()) {
            const query = searchQuery.value.toLowerCase().trim();
            filtered = filtered.filter(subdepartment => 
                subdepartment.name.toLowerCase().includes(query)
            );
        }
        // Sort by name
        return filtered.sort((a, b) => a.name.localeCompare(b.name));
    });
    
    return {
        searchQuery,
        filteredSubDepartments
    };
}