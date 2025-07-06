<template>
    <HierarchicalTable
        :data="hierarchicalData"
        :columns="locationColumns"
        :is-loading="isLoading"
        title="Locations & Sublocations"
        hierarchy-column="name"
        children-key="sublocations"
        child-indicator-key="isSubLocation"
        parent-key="locationId"
        :parent-actions="locationActions"
        :child-actions="sublocationActions"
        :expand-by-default="true"
        @add-child="handleAddSublocation"
        @edit-parent="$emit('edit-location', $event)"
        @delete-parent="$emit('delete-location', $event)"
        @edit-child="$emit('edit-sublocation', $event)"
        @delete-child="$emit('delete-sublocation', $event)"
    />
</template>

<script setup>
import { computed } from 'vue';
import HierarchicalTable from '@/components/common/HierarchicalTable.vue';

const props = defineProps({
    locations: {
        type: Array,
        required: true
    },
    sublocations: {
        type: Array,
        default: () => []
    },
    isLoading: {
        type: Boolean,
        default: false
    }
});

const emit = defineEmits([
    'edit-location', 
    'delete-location', 
    'add-sublocation',
    'edit-sublocation', 
    'delete-sublocation'
]);

// Handle add sublocation event
const handleAddSublocation = (item) => {
    // Emit the location ID (use 'id' property of the location)
    emit('add-sublocation', item.id);
};

// Combine locations with their sublocations
const hierarchicalData = computed(() => {
    console.log('Locations:', props.locations);
    console.log('Sublocations:', props.sublocations);
    
    const result = props.locations.map(location => {
        // Find sublocations that belong to this location
        const locationSublocations = props.sublocations.filter(
            sublocation => sublocation.locationId === location.id
        );
        
        console.log(`Location ${location.name} (ID: ${location.id}) has ${locationSublocations.length} sublocations:`, locationSublocations);
        
        return {
            ...location,
            sublocations: locationSublocations
        };
    });
    
    console.log('Final hierarchical data:', result);
    return result;
});

const locationColumns = [
    { key: 'name', label: 'Name' },
    { key: 'address', label: 'Address' },
    { key: 'createdAt', label: 'Created Date', type: 'date' }
];

const locationActions = [
    { 
        label: 'Add Sublocation', 
        event: 'add-child', 
        class: 'font-medium text-green-600 hover:text-green-800 hover:underline mr-4' 
    },
    { 
        label: 'Edit', 
        event: 'edit-parent', 
        class: 'font-medium text-blue-600 hover:text-blue-800 hover:underline mr-4' 
    },
    { 
        label: 'Delete', 
        event: 'delete-parent', 
        class: 'font-medium text-red-600 hover:text-red-800 hover:underline' 
    }
];

const sublocationActions = [
    { 
        label: 'Edit', 
        event: 'edit-child', 
        class: 'font-medium text-blue-600 hover:text-blue-800 hover:underline mr-4' 
    },
    { 
        label: 'Delete', 
        event: 'delete-child', 
        class: 'font-medium text-red-600 hover:text-red-800 hover:underline' 
    }
];
</script>