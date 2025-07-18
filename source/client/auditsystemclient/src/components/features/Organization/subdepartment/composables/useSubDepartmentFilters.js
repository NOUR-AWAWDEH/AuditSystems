import { ref, computed } from 'vue';

export function useSubDepartmentFilters(subDepartments) {
    const searchQuery = ref('');
    
    const filteredSubDepartments = computed(() => {
        let filtered = [...subDepartments.value];
        
        // Apply search filter
        if (searchQuery.value.trim()) {
            const query = searchQuery.value.toLowerCase().trim();
            filtered = filtered.filter(subDepartment => 
                subDepartment.name.toLowerCase().includes(query)
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