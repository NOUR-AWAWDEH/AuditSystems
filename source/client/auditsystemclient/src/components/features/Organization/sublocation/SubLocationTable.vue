<template>
    <div>
        <!-- Sublocation Table -->
        <div class="bg-white rounded-xl shadow-lg border border-gray-200 overflow-hidden">
            <div class="p-6">
                <!-- Header with Title and Filters -->
                <div class="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-6 mb-6">
                    <div class="flex items-center"> 
                        <div>
                            <h2 class="text-2xl font-bold text-secondary-base">
                                Sublocations
                            </h2>
                            <p class="text-sm text-gray-500 mt-1">Manage your organization sublocations</p>
                        </div>
                    </div>
                    <SubLocationFilters
                        :locations="locations"
                        :selected-location-id="selectedLocationId"
                        :search="searchQuery"
                        :sort="sortBy"
                        @update:selected-location-id="handleLocationChange"
                        @update:search="handleSearch"
                        @update:sort="handleSort"
                        @refresh="refreshData"
                    />
                </div>

                <!-- Selected Location Info -->
                <div v-if="selectedLocationId" class="mb-6 p-4 bg-gradient-to-r from-blue-50 to-indigo-50 rounded-lg border border-blue-200">
                    <div class="flex items-center justify-between">
                        <div class="flex items-center">
                            <svg class="w-5 h-5 text-blue-600 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z"></path>
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 11a3 3 0 11-6 0 3 3 0 016 0z"></path>
                            </svg>
                            <p class="text-sm text-gray-700">
                                Showing sublocations for: <span class="font-semibold text-blue-800">{{ selectedLocation?.name || 'Selected Location' }}</span>
                                <span v-if="searchQuery" class="ml-2 text-blue-600">| Search: "{{ searchQuery }}"</span>
                            </p>
                        </div>
                        <BaseButton 
                            @click="$emit('add-sublocation')"
                            class="inline-flex items-center px-4 py-2 bg-green-600 text-white font-medium rounded-lg shadow-md hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-2 transition-all duration-200 transform hover:scale-105 active:scale-95"
                        >
                            <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6"></path>
                            </svg>
                            Add Sublocation
                        </BaseButton>
                    </div>
                </div>
                
                <!-- Table Container -->
                <div v-if="selectedLocationId" class="bg-gray-50 rounded-lg border border-gray-200 overflow-hidden">
                    <ReusableTable
                        :items="sublocations"
                        :columns="enhancedColumns"
                        :is-loading="isLoading"
                        :actions="[]"
                        :no-data-message="noDataMessage"
                        class="sublocation-table"
                    >
                        <template #cell-actions="{ item }">
                            <div class="flex items-center gap-2">
                                <button
                                    @click.stop="$emit('edit-sublocation', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200"
                                >
                                    <svg class="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z"></path>
                                    </svg>
                                    Edit
                                </button>
                                <button
                                    @click.stop="$emit('delete-sublocation', item)"
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
                    
                    <!-- Clear Search Button (shown when no results and searching) -->
                    <div v-if="searchQuery && sublocations.length === 0 && !isLoading" class="text-center py-4 border-t border-gray-200">
                        <button
                            @click="handleSearch('')"
                            class="inline-flex items-center px-4 py-2 text-sm font-medium text-blue-600 bg-blue-50 rounded-md hover:bg-blue-100 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition-all duration-200"
                        >
                            <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
                            </svg>
                            Clear Search
                        </button>
                    </div>
                </div>

                <!-- No Location Selected -->
                <div v-else class="text-center py-16">
                    <svg class="w-20 h-20 text-gray-300 mx-auto mb-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z"></path>
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 11a3 3 0 11-6 0 3 3 0 016 0z"></path>
                    </svg>
                    <h3 class="text-xl font-semibold text-gray-700 mb-2">Select a location to view sublocations</h3>
                    <p class="text-gray-500">Choose a location from the filters above to get started</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { computed } from 'vue';
import ReusableTable from '@/components/common/ReusableTable.vue';
import BaseButton from '@/components/base/BaseButton.vue';
import SubLocationFilters from './SubLocationFilters.vue';

const props = defineProps({
    locations: Array,
    sublocations: Array,
    isLoading: Boolean,
    selectedLocationId: String,
    selectedLocation: Object,
    searchQuery: String,
    sortBy: String,
    handleLocationChange: Function,
    handleSearch: Function,
    handleSort: Function,
    refreshData: Function
});

defineEmits([
    'add-sublocation',
    'edit-sublocation',
    'delete-sublocation'
]);

const sublocationColumns = [
    { key: 'name', label: 'Name' },
    { key: 'address', label: 'Address' },
    { key: 'createdAt', label: 'Created Date', type: 'date' }
];

const enhancedColumns = computed(() => {
    return [...sublocationColumns, { key: 'actions', label: 'Actions' }];
});

// Computed property for dynamic no data message
const noDataMessage = computed(() => {
    if (props.searchQuery) {
        return `No sublocations found for "${props.searchQuery}".`;
    }
    return 'No sublocations found for this location.';
});
</script>

<style scoped>
.sublocation-table .table-row {
    transition: all 0.2s ease;
}

.sublocation-table .table-row:hover {
    background-color: #f8fafc;
    transform: translateY(-1px);
    box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}
</style>