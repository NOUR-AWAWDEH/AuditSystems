import { ref, computed } from 'vue';

export function useLocationFilters(locations, sublocations) {
    const searchQuery = ref('');
    const sortBy = ref('name');

    const filteredLocations = computed(() => {
        if (!Array.isArray(locations.value) || !Array.isArray(sublocations.value)) {
            return locations.value || [];
        }

        let filtered = locations.value.map(location => {
            const locationSublocations = sublocations.value.filter(
                sublocation => sublocation.locationId === location.id
            );
            
            return {
                ...location,
                sublocations: locationSublocations,
                hasSublocations: locationSublocations.length > 0
            };
        });

        // Apply search filter
        if (searchQuery.value) {
            const query = searchQuery.value.toLowerCase();
            filtered = filtered.filter(location => 
                location.name.toLowerCase().includes(query) ||
                location.address?.toLowerCase().includes(query) ||
                location.sublocations.some(sub => 
                    sub.name.toLowerCase().includes(query)
                )
            );
        }

        // Apply sorting
        filtered.sort((a, b) => {
            const aValue = a[sortBy.value] || '';
            const bValue = b[sortBy.value] || '';
            return aValue.localeCompare(bValue);
        });

        return filtered;
    });

    return {
        searchQuery,
        sortBy,
        filteredLocations
    };
}