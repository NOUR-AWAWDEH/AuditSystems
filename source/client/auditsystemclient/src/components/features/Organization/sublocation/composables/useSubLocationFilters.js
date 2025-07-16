import { ref, computed } from 'vue';

export function useSubLocationFilters(sublocations) {
    const searchQuery = ref('');
    
    const filteredSublocations = computed(() => {
        let filtered = [...sublocations.value];
        
        // Apply search filter
        if (searchQuery.value.trim()) {
            const query = searchQuery.value.toLowerCase().trim();
            filtered = filtered.filter(sublocation => 
                sublocation.name.toLowerCase().includes(query) ||
                (sublocation.address && sublocation.address.toLowerCase().includes(query))
            );
        }
        // Sort by name
        return filtered;
    });
    
    return {
        searchQuery,
        filteredSublocations
    };
}