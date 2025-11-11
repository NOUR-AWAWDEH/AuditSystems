<template>
    <!--DepartmentTable.vue-->
    <div>
        <!-- Department Header -->
        <div class="bg-white rounded-xl shadow-lg border border-gray-200 overflow-hidden">
            <div class="p-3 sm:p-4 md:p-6">
                <!-- Header with Location Selection and Filters -->
                <div class="flex flex-col gap-4 mb-6">
                    <DepartmentHeader />

                    <!-- LocationSelector and Filters - Stack on mobile -->
                    <div class="flex flex-col sm:flex-row sm:items-center gap-4">
                        <div class="w-full sm:min-w-64 sm:max-w-xs">
                            <LocationSelector
                                :locations="locations"
                                :selected-location-id="selectedLocationId"
                                @location-changed="handleLocationChange"
                            />
                        </div>
                        <div class="w-full sm:w-auto">
                            <SearchFilter
                                :search="searchQuery"
                                :searchPlaceholder="'Search departments...'"
                                @update:search="handleSearch"
                                class="w-full sm:w-auto"
                            />
                        </div>

                    </div>
                </div>

                <!-- Selected Location Info - Show when Location is Selected -->
                <div v-if="selectedLocationId"
                    class="mb-6 p-4 bg-gradient-to-r from-blue-50 to-indigo-50 rounded-lg border border-blue-200">
                    <div class="flex items-center justify-between">
                        <div class="flex items-center">
                            <LocationIcon size="lg" custom-class="text-blue-600 mr-2" />

                            <p class="text-sm text-gray-700">
                                Showing departments for: &nbsp;&nbsp;
                                <span class="font-semibold text-blue-800">
                                    <span v-if="isLocationLoading">Loading location...</span>
                                    <span v-else>{{ currentLocation?.name || 'Loading...' }}</span>
                                </span>
                                <span v-if="searchQuery" class="ml-2 text-blue-600">| Search: "{{ searchQuery }}"</span>
                            </p>
                        </div>
                        <button @click="$emit('add-department')"
                            class="px-4 py-2 rounded-md text-sm font-medium bg-secondary-base hover:bg-secondary-shade text-white">
                            <PlusIcon :stroke-width="3" />
                            Add Department
                        </button>
                    </div>
                </div>

                <!-- Table Container - Show when Location is Selected -->
                <div v-if="selectedLocationId" class="bg-gray-50 rounded-lg border border-gray-200 overflow-hidden">
                    <ReusableTable
                        :items="departments"
                        :columns="enhancedColumns"
                        :is-loading="isLoading"
                        :actions="[]"
                        :no-data-message="noDataMessage"
                        class="department-table"
                        >
                        <template #cell-actions="{ item }">
                            <div class="flex items-center gap-2">
                                <button @click.stop="handleSelectDepartment(item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-1 transition-all duration-200"
                                    :class="{ 'bg-green-200': selectedDepartmentId === item.id }">
                                    <CheckIcon v-if="selectedDepartmentId === item.id" size="sm" custom-class="mr-1" />
                                    <SelectIcon v-else size="sm" custom-class="mr-1" />
                                    {{ selectedDepartmentId === item.id ? 'Selected' : 'Select' }}
                                </button>
                                <button @click.stop="$emit('edit-department', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200">
                                    <EditIcon size="sm" custom-class="mr-1" />
                                    Edit
                                </button>
                                <button @click.stop="$emit('delete-department', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition-all duration-200">
                                    <DeleteIcon size="sm" custom-class="mr-1" />
                                    Delete
                                </button>
                            </div>
                        </template>
                    </ReusableTable>

                    <!-- Clear Search Button (shown when no results and searching) -->
                    <div v-if="searchQuery && departments.length === 0 && !isLoading"
                        class="text-center py-4 border-t border-gray-200">
                        <button @click="handleSearch('')"
                            class="inline-flex items-center px-4 py-2 text-sm font-medium text-blue-600 bg-blue-50 rounded-md hover:bg-blue-100 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition-all duration-200">
                            <CloseIcon size="sm" stroke-width="3" custom-class="mr-1" />
                            Clear Search
                        </button>
                    </div>
                </div>

                <!-- No Department Selected - Show when no Department is selected -->
                <div v-else class="text-center py-16">
                    <LocationIcon size="xxxl" custom-class="text-secondary-base mx-auto mb-6" />

                    <h3 class="text-xl font-semibold text-secondary-shade mb-4">Please select a Department to view
                        departments</h3>
                    <p class="text-gray-500 mb-6">Choose a location from the dropdown above to get started</p>

                    <!-- No Locations Available -->
                    <div v-if="!locations || locations.length === 0" class="text-center">
                        <p class="text-sm text-gray-400">Please add a location first to manage departments</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { computed, ref, watch} from 'vue';
import ReusableTable from '@/components/common/ReusableTable.vue';
import SearchFilter from '@/components/common/SearchFilter.vue';
import LocationSelector from '@/components/features/Organization/location/components/LocationSelector.vue';
import DepartmentHeader from './DepartmentHeader.vue';
import LocationIcon from '@/components/common/icons/LocationIcon.vue';
import PlusIcon from '@/components/common/icons/PlusIcon.vue';
import EditIcon from '@/components/common/icons/EditIcon.vue';
import DeleteIcon from '@/components/common/icons/DeleteIcon.vue';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';
import CheckIcon from '@/components/common/icons/CheckIcon.vue';
import SelectIcon from '@/components/common/icons/SelectIcon.vue';

const props = defineProps({
    locations: Array,
    departments: Array,
    isLoading: Boolean,
    selectedLocationId: String,
    selectedLocation: Object,
    currentLocation: Object,
    isLocationLoading: Boolean,
    searchQuery: String,
    handleLocationChange: Function,
    handleSearch: Function,
});

const emit = defineEmits([
    'add-department',
    'edit-department',
    'delete-department',
    'department-selected'
]);

const internalSelectedLocationId = ref(props.selectedLocationId);

watch(() => props.selectedLocationId, (newVal) => {
  internalSelectedLocationId.value = newVal;
});

watch(internalSelectedLocationId, (newVal) => {
  props.handleLocationChange(newVal);
}, { immediate: true });

// Department selection state
const selectedDepartmentId = ref(null);

// Handle department selection from table row or button
const handleSelectDepartment = (department) => {
    selectedDepartmentId.value = department.id;
    emit('department-selected', department.id);
};

const departmentColumns = [
    { key: 'name', label: 'Name' },
    { key: 'createdAt', label: 'Created Date', type: 'date' }
];

const enhancedColumns = computed(() => {
    return [...departmentColumns, { key: 'actions', label: 'Actions' }];
});

const noDataMessage = computed(() => {
    if (props.searchQuery) {
        return `No departments found for "${props.searchQuery}".`;
    }
    return 'No departments found for this location.';
});

</script>

<style scoped>
.department-table .table-row {
    transition: all 0.2s ease;
    cursor: pointer;
}

.department-table .table-row:hover {
    background-color: #f8fafc;
    transform: translateY(-1px);
    box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.department-table .table-row.selected {
    background-color: #e6f7ff;
    border-left: 4px solid #3b82f6;
}
</style>
