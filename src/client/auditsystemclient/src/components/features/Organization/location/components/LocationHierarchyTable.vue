<template>
    <HierarchicalTable
        :data="locations"
        :columns="locationColumns"
        :is-loading="isLoading"
        title="Locations & Sublocations"
        hierarchy-column="name"
        children-key="sublocations"
        child-indicator-key="isSubLocation"
        parent-key="locationId"
        :parent-actions="locationActions"
        :child-actions="sublocationActions"
        @add-child="$emit('add-sublocation', $event)"
        @edit-parent="$emit('edit-location', $event)"
        @delete-parent="$emit('delete-location', $event)"
        @edit-child="$emit('edit-sublocation', $event)"
        @delete-child="$emit('delete-sublocation', $event)"
    />
</template>

<script setup>
import HierarchicalTable from '@/components/common/HierarchicalTable.vue';

defineProps({
    locations: {
        type: Array,
        required: true
    },
    isLoading: {
        type: Boolean,
        default: false
    }
});

defineEmits([
    'edit-location', 
    'delete-location', 
    'add-sublocation',
    'edit-sublocation', 
    'delete-sublocation'
]);

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