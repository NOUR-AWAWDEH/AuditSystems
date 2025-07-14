import { ref, computed } from 'vue';

export function useDepartmentFilters(departments) {
    const searchQuery = ref('');
    const sortBy = ref('name');
    
    const filteredDepartments = computed(() => {
        let filtered = [...departments.value];
        
        // Apply search filter
        if (searchQuery.value.trim()) {
            const query = searchQuery.value.toLowerCase().trim();
            filtered = filtered.filter(department => 
                department.name.toLowerCase().includes(query) ||
                (department.name && department.name.toLowerCase().includes(query))
            );
        }
        
        // Apply sorting
        filtered.sort((a, b) => {
            const aValue = a[sortBy.value];
            const bValue = b[sortBy.value];
            
            if (sortBy.value === 'createdAt') {
                return new Date(bValue) - new Date(aValue); // Newest first
            }
            
            if (typeof aValue === 'string' && typeof bValue === 'string') {
                return aValue.localeCompare(bValue);
            }
            
            return 0;
        });
        
        return filtered;
    });
    
    return {
        searchQuery,
        filteredDepartments
    };
}