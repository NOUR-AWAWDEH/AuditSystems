<template>
    <div>
        <!-- Sublocation Filters -->
        <SubLocationFilters
            :locations="locations"
            :selected-location-id="selectedLocationId"
            :search="search"
            :sort="sort"
            @update:selected-location-id="$emit('update:selectedLocationId', $event)"
            @update:search="$emit('update:search', $event)"
            @update:sort="$emit('update:sort', $event)"
            @refresh="$emit('refresh')"
        />
        
        <!-- Sublocation Table -->
        <div class="bg-white rounded-lg shadow-sm border border-gray-200">
            <div class="p-4 md:p-6">
                <!-- Header -->
                <div class="flex flex-col lg:flex-row lg:items-center lg:justify-between mb-4">
                    <h2 class="text-lg font-semibold text-secondary-base mb-4 lg:mb-0">
                        Sublocations
                    </h2>
                    <BaseButton 
                        v-if="selectedLocationId"
                        @click="$emit('add-sublocation')"
                        class="text-white bg-primary-base px-4 py-2 rounded-md"
                    >
                        Add Sublocation
                    </BaseButton>
                </div>

                <!-- Selected Location Info -->
                <div v-if="selectedLocation" class="mb-4 p-3 bg-gray-50 rounded-md">
                    <p class="text-sm text-gray-600">
                        Showing sublocations for: <span class="font-medium text-gray-900">{{ selectedLocation.name }}</span>
                    </p>
                </div>

                <!-- Table -->
                <div v-if="selectedLocationId">
                    <ReusableTable
                        :items="sublocations"
                        :columns="enhancedColumns"
                        :is-loading="isLoading"
                        :actions="[]"
                        class="sublocation-table"
                    >
                        <template #cell-actions="{ item }">
                            <div class="flex flex-wrap gap-2">
                                <button
                                    @click.stop="$emit('edit-sublocation', item)"
                                    class="font-medium text-blue-600 hover:text-blue-800 hover:underline mr-4"
                                >
                                    Edit
                                </button>
                                <button
                                    @click.stop="$emit('delete-sublocation', item)"
                                    class="font-medium text-red-600 hover:text-red-800 hover:underline"
                                >
                                    Delete
                                </button>
                            </div>
                        </template>
                    </ReusableTable>

                    <div v-if="!isLoading && sublocations.length === 0" class="text-center py-8">
                        <p class="text-gray-500">
                            No sublocations found for this location.
                        </p>
                    </div>
                </div>

                <!-- No Location Selected -->
                <div v-else class="text-center py-12">
                    <p class="text-lg mb-2 text-gray-500">Select a location to view its sublocations</p>
                    <p class="text-sm text-gray-400">Choose a location from the filters above to get started</p>
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
    search: String,
    sort: String
});

defineEmits([
    'add-sublocation',
    'edit-sublocation',
    'delete-sublocation',
    'update:selectedLocationId',
    'update:search',
    'update:sort',
    'refresh'
]);

const sublocationColumns = [
    { key: 'name', label: 'Name' },
    { key: 'address', label: 'Address' },
    { key: 'createdAt', label: 'Created Date', type: 'date' }
];

const enhancedColumns = computed(() => {
    return [...sublocationColumns, { key: 'actions', label: 'Actions' }];
});
</script>

<style scoped>
.sublocation-table .table-row {
    transition: all 0.2s ease;
}
</style>