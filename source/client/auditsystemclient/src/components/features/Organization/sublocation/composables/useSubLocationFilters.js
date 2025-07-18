import { ref, computed } from 'vue';

export function useSubLocationFilters(subLocations) {
    const searchQuery = ref('');
    
    const filteredSubLocations = computed(() => {
        let filtered = [...subLocations.value];
        
        // Apply search filter
        if (searchQuery.value.trim()) {
            const query = searchQuery.value.toLowerCase().trim();
            filtered = filtered.filter(subLocation => 
                subLocation.name.toLowerCase().includes(query) ||
                (subLocation.address && subLocation.address.toLowerCase().includes(query))
            );
        }
        // Sort by name
        return filtered;
    });
    
    return {
        searchQuery,
        filteredSubLocations
    };
}