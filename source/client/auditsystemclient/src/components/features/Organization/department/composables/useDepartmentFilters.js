import { ref, computed } from 'vue';

export function useDepartmentFilters(departments) {
    const searchQuery = ref('');
    
    const filteredDepartments = computed(() => {
        let filtered = [...departments.value];
        
        // Apply search filter
        if (searchQuery.value.trim()) {
            const query = searchQuery.value.toLowerCase().trim();
            filtered = filtered.filter(department => 
                department.name.toLowerCase().includes(query)
            );
        }
        
        return filtered;
    });
    
    return {
        searchQuery,
        filteredDepartments
    };
}