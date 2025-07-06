<template>
    <div>
        <!-- Location Table -->
        <div class="bg-white rounded-xl shadow-lg border border-gray-200 overflow-hidden">
            <div class="p-6">
                <!-- Header with Title and Filters -->
                <div class="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-6 mb-6">
                    <div class="flex items-center"> 
                        <div>
                            <h2 class="text-2xl font-bold text-secondary-base">
                                Locations
                            </h2>
                            <p class="text-sm text-gray-500 mt-1">Manage your organization locations</p>
                        </div>
                    </div>
                    <div class=" p-4">
                        <LocationFilters
                            :search="search"
                            :sort="sort"
                            @update:search="$emit('update:search', $event)"
                            @update:sort="$emit('update:sort', $event)"
                            @refresh="$emit('refresh')"
                        />
                    </div>
                </div>
                
                <!-- Table Container -->
                <div class="bg-gray-50 rounded-lg border border-gray-200 overflow-hidden">
                    <ReusableTable
                        :items="locations"
                        :columns="enhancedColumns"
                        :is-loading="isLoading"
                        :actions="[]"
                        class="location-table"
                    >
                        <template #cell-actions="{ item }">
                            <div class="flex items-center gap-2">
                                <button
                                    @click.stop="$emit('edit-location', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200"
                                >
                                    <svg class="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z"></path>
                                    </svg>
                                    Edit
                                </button>
                                <button
                                    @click.stop="$emit('delete-location', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition-all duration-200"
                                >
                                    <svg class="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"></path>
                                    </svg>
                                    Delete
                                </button>
                            </div>
                        </template>
                    </ReusableTable>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { computed } from 'vue';
import ReusableTable from '@/components/common/ReusableTable.vue';
import LocationFilters from './LocationFilters.vue';

defineProps({
    locations: Array,
    isLoading: Boolean,
    search: String,
    sort: String
});

defineEmits([
    'edit-location',
    'delete-location',
    'update:search',
    'update:sort',
    'refresh'
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

.location-table .table-row:hover {
    background-color: #f8fafc;
    transform: translateY(-1px);
    box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}
</style>