<template>
    <div>
        <!-- Department Header -->
        <div class="bg-white rounded-xl shadow-lg border border-gray-200 overflow-hidden">
            <div class="p-3 sm:p-4 md:p-6">
                <!-- Header with SubLocation Selection and Filters -->
                <div class="flex flex-col gap-4 mb-6">
                    <DepartmentHeader />

                    <!-- SubLocationSelector and Filters - Stack on mobile -->
                    <div class="flex flex-col sm:flex-row sm:items-center gap-4">
                        <div class="w-full sm:min-w-64 sm:max-w-xs">
                            <SubLocationSelector 
                                :sub-locations="subLocations"
                                :selected-sub-location-id="selectedSubLocationId"
                                @sub-location-changed="handleSubLocationChange"
                            />
                        </div>
                        <div v-if="validSelectedSubLocation" class="w-full sm:w-auto">
                            <SearchFilter 
                                :search="searchQuery" 
                                @update:search="handleSearch" 
                                @refresh="refreshData" 
                            />
                        </div>

                    </div>
                </div>

                <!-- Selected SubLocation Info - Show when SubLocation is Selected -->
                <div v-if="selectedSubLocationId"
                    class="mb-6 p-4 bg-gradient-to-r from-blue-50 to-indigo-50 rounded-lg border border-blue-200">
                    <div class="flex items-center justify-between">
                        <div class="flex items-center">
                            <LocationIcon size="lg" custom-class="text-blue-600 mr-2" />

                            <p class="text-sm text-gray-700">
                                Showing departments for: &nbsp;&nbsp;
                                <span class="font-semibold text-blue-800">
                                    <span v-if="isSubLocationLoading">Loading subLocation...</span>
                                    <span v-else>{{ currentSubLocation?.name || 'Loading...' }}</span>
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

                <!-- Table Container - Show when SubLocation is Selected -->
                <div v-if="selectedSubLocationId" class="bg-gray-50 rounded-lg border border-gray-200 overflow-hidden">
                    <ReusableTable :items="departments" :columns="enhancedColumns" :is-loading="isLoading" :actions="[]"
                        :no-data-message="noDataMessage" class="department-table" @row-click="handleRowClick">
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
                                    <DeleteIcon size="sm" custom-class="mr1" />
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

                <!-- No SubLocation Selected - Show when no subLocation is selected -->
                <div v-else class="text-center py-16">
                    <LocationIcon size="xxxl" custom-class="text-secondary-base mx-auto mb-6" />

                    <h3 class="text-xl font-semibold text-secondary-shade mb-4">Please select a subLocation to view
                        departments</h3>
                    <p class="text-gray-500 mb-6">Choose a subLocation from the dropdown above to get started</p>

                    <!-- No SubLocations Available -->
                    <!-- No SubLocations Available -->
                    <div v-if="!subLocations || subLocations.length === 0" class="text-center">
                        <p class="text-sm text-gray-400">Please add a subLocation first to manage departments</p>
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
import SubLocationSelector from '@/components/features/Organization/sublocation/components/SubLocationSelector.vue';
import DepartmentHeader from './DepartmentHeader.vue';
import LocationIcon from '@/components/common/icons/LocationIcon.vue';
import PlusIcon from '@/components/common/icons/PlusIcon.vue';
import EditIcon from '@/components/common/icons/EditIcon.vue';
import DeleteIcon from '@/components/common/icons/DeleteIcon.vue';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';
import CheckIcon from '@/components/common/icons/CheckIcon.vue';
import SelectIcon from '@/components/common/icons/SelectIcon.vue';

const props = defineProps({
    subLocations: Array,
    departments: Array,
    isLoading: Boolean,
    selectedSubLocationId: String,
    selectedSubLocation: Object,
    currentSubLocation: Object,
    isSubLocationLoading: Boolean,
    searchQuery: String,
    handleSubLocationChange: Function,
    handleSearch: Function,
    refreshData: Function
});

const internalSelectedSubLocationId = ref(props.selectedSubLocationId);

watch(() => props.selectedSubLocationId, (newVal) => {
  internalSelectedSubLocationId.value = newVal;
});

watch(internalSelectedSubLocationId, (newVal) => {
  props.handleSubLocationChange(newVal);
});

const emit = defineEmits([
    'add-department',
    'edit-department',
    'delete-department',
    'department-selected'
]);

// Department selection state
const selectedDepartmentId = ref(null);

// Handle department selection from table row or button
const handleSelectDepartment = (department) => {
    selectedDepartmentId.value = department.id;
    emit('department-selected', department.id);
};

// Handle row click to select department
const handleRowClick = (department) => {
    handleSelectDepartment(department);
};

// Watch for department changes to reset selection if department is deleted
watch(() => props.departments, (newDepartments) => {
    if (selectedDepartmentId.value && newDepartments) {
        const departmentExists = newDepartments.some(dept => dept.id === selectedDepartmentId.value);
        if (!departmentExists) {
            selectedDepartmentId.value = null;
            emit('department-selected', null);
        }
    }
}, { deep: true });

// Reset selection when sublocation changes
watch(() => props.selectedSubLocationId, () => {
    selectedDepartmentId.value = null;
    emit('department-selected', null);
});

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
    return 'No departments found for this sub location.';
});

const validSelectedSubLocation = computed(() => {
   return props.selectedSubLocationId && props.currentSubLocation?.name;
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