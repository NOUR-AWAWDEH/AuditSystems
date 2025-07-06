import { ref, computed } from 'vue';

export function useLocationFilters(locations) {
    const searchQuery = ref('');
    const sortBy = ref('name');

    const filteredLocations = computed(() => {
        if (!Array.isArray(locations.value)) {
            return locations.value || [];
        }

        let filtered = [...locations.value];

        // Apply search filter
        if (searchQuery.value) {
            const query = searchQuery.value.toLowerCase();
            filtered = filtered.filter(location => 
                location.name.toLowerCase().includes(query) ||
                (location.address && location.address.toLowerCase().includes(query))
            );
        }

        // Apply sorting
        filtered.sort((a, b) => {
            const aValue = a[sortBy.value] || '';
            const bValue = b[sortBy.value] || '';
            
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
        sortBy,
        filteredLocations
    };
}