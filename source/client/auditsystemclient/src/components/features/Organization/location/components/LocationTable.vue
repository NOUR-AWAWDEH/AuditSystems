<template>
    <!--LocationTable.vue-->
    <div>
        <!-- Location Table -->
        <div class="bg-white rounded-xl shadow border border-gray-200 overflow-hidden">
            <div class="p-3 sm:p-4 md:p-6">
                <!-- Header with Title and Filters -->
                <div class="flex flex-col gap-4 mb-6">
                    <LocationHeader />

                    <div class="flex flex-col sm:flex-row sm:items-end gap-3">
                        <SearchFilters
                            :search="searchQuery"
                            :searchPlaceholder="'Search locations...'"
                            @update:search="handleSearch"
                            class="w-full sm:w-auto"
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
                        :no-data-message="noDataMessage"
                        class="location-table"
                    >
                        <template #cell-actions="{ item }">
                            <div class="flex items-center gap-2">
                                <button
                                    @click.stop="handleSelectLocation(item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-1 transition-all duration-200"
                                    :class="{ 'bg-green-200': selectedLocationId === item.id }"
                                >
                                    <CheckIcon v-if="selectedLocationId === item.id" size="sm" custom-class="mr-1" />
                                    <SelectIcon v-else size="sm" custom-class="mr-1" />
                                    {{ selectedLocationId === item.id ? 'Selected' : 'Select' }}
                                </button>
                                <button
                                    @click.stop="$emit('edit-location', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200"
                                >
                                    <EditIcon size="sm" custom-class="mr-1" />
                                    Edit
                                </button>
                                <button
                                    @click.stop="$emit('delete-location', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition-all duration-200"
                                >
                                    <DeleteIcon size="sm" custom-class="mr-1" />
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
import { computed, ref, watch } from 'vue';
import ReusableTable from '@/components/common/ReusableTable.vue';
import SearchFilters from '@/components/common/SearchFilter.vue';
import EditIcon from '@/components/common/icons/EditIcon.vue';
import DeleteIcon from '@/components/common/icons/DeleteIcon.vue';
import CheckIcon from '@/components/common/icons/CheckIcon.vue';
import SelectIcon from '@/components/common/icons/SelectIcon.vue';
import LocationHeader from './LocationHeader.vue';

const props = defineProps({
    locations: Array,
    isLoading: Boolean,
    searchQuery: String,
    handleSearch: Function
});

const emit = defineEmits([
    'add-location',
    'edit-location',
    'delete-location',
    'location-selected'
]);

// Location selection state
const selectedLocationId = ref(null);

// Handle location selection from table row or button
const handleSelectLocation = (location) => {
    selectedLocationId.value = location.id;
    emit('location-selected', location.id);
};

const locationColumns = [
    { key: 'name', label: 'Name' },
    { key: 'address', label: 'Address' },
    { key: 'createdAt', label: 'Created Date', type: 'date' }
];

const enhancedColumns = computed(() => {
    return [...locationColumns, { key: 'actions', label: 'Actions' }];
});

// Message for empty data
const noDataMessage = computed(() => {
    return props.searchQuery
        ? `No Locations found for "${props.searchQuery}".`
        : 'No Locations found yet. Please add a location.';
});

// Watch for location changes to reset selection if location is deleted
watch(() => props.locations, (newLocations) => {
    if (selectedLocationId.value && newLocations) {
        const locationExists = newLocations.some(loc => loc.id === selectedLocationId.value);
        if (!locationExists) {
            selectedLocationId.value = null;
            emit('location-selected', null);
        }
    }
}, { deep: true });

</script>

<style scoped>
.location-table .table-row {
    transition: all 0.2s ease;
    cursor: pointer;
}

.location-table .table-row:hover {
    background-color: #f8fafc;
    transform: translateY(-1px);
    box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.location-table .table-row.selected {
    background-color: #eff6ff;
    border-left: 4px solid #3b82f6;
}
</style>