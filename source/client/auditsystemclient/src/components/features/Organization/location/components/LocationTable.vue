<template>
    <div class="bg-white rounded-lg shadow-sm border border-gray-200">
        <div class="p-4 md:p-6">
            <h2 class="text-lg font-semibold text-secondary-base mb-4">
                Locations
            </h2>
            <ReusableTable
                :items="locations"
                :columns="enhancedColumns"
                :is-loading="isLoading"
                :actions="[]"
                class="location-table"
            >
                <template #cell-actions="{ item }">
                    <div class="flex flex-wrap gap-2">
                        <button
                            @click.stop="$emit('edit-location', item)"
                            class="font-medium text-blue-600 hover:text-blue-800 hover:underline mr-4"
                        >
                            Edit
                        </button>
                        <button
                            @click.stop="$emit('delete-location', item)"
                            class="font-medium text-red-600 hover:text-red-800 hover:underline"
                        >
                            Delete
                        </button>
                    </div>
                </template>
            </ReusableTable>
        </div>
    </div>
</template>

<script setup>
import { computed } from 'vue';
import ReusableTable from '@/components/common/ReusableTable.vue';

const props = defineProps({
    locations: Array,
    isLoading: Boolean
});

defineEmits([
    'edit-location',
    'delete-location'
]);

const locationColumns = [
    { key: 'name', label: 'Name' },
    { key: 'address', label: 'Address' },
    { key: 'createdAt', label: 'Created Date', type: 'date' }
];

const enhancedColumns = computed(() => {
    return [...locationColumns, { key: 'actions', label: 'Actions' }];
});
</script>

<style scoped>
.location-table .table-row {
    transition: all 0.2s ease;
}
</style>