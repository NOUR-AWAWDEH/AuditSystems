<template>
    <div>
        <!-- SubDepartment Table -->
        <div class="bg-white rounded-xl shadow-lg border border-gray-200 overflow-hidden">
            <div class="p-3 sm:p-4 md:p-6">
                <!-- Header with Department Selection and Filters -->
                <div class="flex flex-col gap-4 mb-6">
                    <SubDepartmentHeader />

                    <!-- Department Selector and Filters - Stack on mobile -->
                    <div class="flex flex-col sm:flex-row sm:items-center gap-4">
                        <div class="w-full sm:min-w-64 sm:max-w-xs">
                            <DepartmentSelector 
                                :departments="departments" 
                                :selected-department-id="selectedDepartmentId"
                                @department-changed="handleDepartmentChange" />
                        </div>
                        <div class="w-full sm:w-auto">
                            <SearchFilter 
                                :search="searchQuery"
                                :searchPlaceholder="'Search sub departments...'" 
                                @update:search="handleSearch" 
                                class="w-full sm:w-auto"
                            />
                        </div>
                    </div>
                </div>

                <!-- Selected Department Info - Show when department is selected -->
                <div v-if="selectedDepartmentId"
                    class="mb-6 p-4 bg-gradient-to-r from-blue-50 to-indigo-50 rounded-lg border border-blue-200">
                    <div class="flex items-center justify-between">
                        <div class="flex items-center">
                            <LocationIcon size="lg" custom-class="text-blue-600 mr-2" />

                            <p class="text-sm text-gray-700">
                                Showing subDepartments for: &nbsp;&nbsp;
                                <span class="font-semibold text-blue-800">
                                    <span v-if="isDepartmentLoading">Loading department...</span>
                                    <span v-else>{{ currentDepartment?.name || 'Loading...' }}</span>
                                </span>
                                <span v-if="searchQuery" class="ml-2 text-blue-600">| Search: "{{ searchQuery }}"</span>
                            </p>
                        </div>
                        <button @click="$emit('add-sub-department')"
                            class="px-4 py-2 rounded-md text-sm font-medium bg-secondary-base hover:bg-secondary-shade text-white">
                            <PlusIcon :stroke-width="3" />
                            Add SubDepartment
                        </button>
                    </div>
                </div>

                <!-- Table Container - Show when department is selected -->
                <div v-if="selectedDepartmentId" class="bg-gray-50 rounded-lg border border-gray-200 overflow-hidden">
                    <ReusableTable 
                        :items="subDepartments"
                        :columns="enhancedColumns"
                        :is-loading="isLoading"
                        :actions="[]"
                        :no-data-message="noDataMessage"
                        class="sub-department-table"
                        >
                        <template #cell-actions="{ item }">
                            <div class="flex items-center gap-2">
                                <button @click.stop="handleSelectSubDepartment(item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-1 transition-all duration-200"
                                    :class="{ 'bg-green-200': selectedSubDepartmentId === item.id }">
                                    <CheckIcon v-if="selectedSubDepartmentId === item.id" size="sm" custom-class="mr-1" />
                                    <SelectIcon v-else size="sm" custom-class="mr-1" />
                                    {{ selectedSubDepartmentId === item.id ? 'Selected' : 'Select' }}
                                </button>
                                <button @click.stop="$emit('edit-sub-department', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200">
                                    <EditIcon size="sm" custom-class="mr-1" />
                                    Edit
                                </button>
                                <button @click.stop="$emit('delete-sub-department', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition-all duration-200">
                                    <DeleteIcon size="sm" custom-class="mr-1" />
                                    Delete
                                </button>
                            </div>
                        </template>
                    </ReusableTable>

                    <!-- Clear Search Button (shown when no results and searching) -->
                    <div v-if="searchQuery && subDepartments.length === 0 && !isLoading"
                        class="text-center py-4 border-t border-gray-200">
                        <button @click="handleSearch('')"
                            class="inline-flex items-center px-4 py-2 text-sm font-medium text-blue-600 bg-blue-50 rounded-md hover:bg-blue-100 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition-all duration-200">
                            <CloseIcon size="sm" stroke-width="3" custom-class="mr-1" />
                            Clear Search
                        </button>
                    </div>
                </div>

                <!-- No Department Selected - Show when no department is selected -->
                <div v-else class="text-center py-16">
                    <LocationIcon size="xxxl" custom-class="text-secondary-base mx-auto mb-6" />

                    <h3 class="text-xl font-semibold text-secondary-shade mb-4">Please select a department to view
                        sub departments</h3>
                    <p class="text-gray-500 mb-6">Choose a department from the dropdown above to get started</p>

                    <!-- No Departments Available -->
                    <div v-if="!departments || departments.length === 0" class="text-center">
                        <p class="text-sm text-gray-400">Please add a department first to manage sub departments</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { computed, ref, watch } from 'vue';
import { useRouter } from 'vue-router';
import ReusableTable from '@/components/common/ReusableTable.vue';
import SearchFilter from '@/components/common/SearchFilter.vue';
import DepartmentSelector from '@/components/features/Organization/department/components/DepartmentSelector.vue';
import SubDepartmentHeader from './SubDepartmentHeader.vue';
import LocationIcon from '@/components/common/icons/LocationIcon.vue';
import PlusIcon from '@/components/common/icons/PlusIcon.vue';
import EditIcon from '@/components/common/icons/EditIcon.vue';
import DeleteIcon from '@/components/common/icons/DeleteIcon.vue';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';
import CheckIcon from '@/components/common/icons/CheckIcon.vue';
import SelectIcon from '@/components/common/icons/SelectIcon.vue';

// Initialize router at the top level
const router = useRouter();

const props = defineProps({
    departments: Array,
    subDepartments: Array,
    isLoading: Boolean,
    selectedDepartmentId: String,
    selectedDepartment: Object,
    currentDepartment: Object,
    isDepartmentLoading: Boolean,
    searchQuery: String,
    handleDepartmentChange: Function,
    handleSearch: Function
});

const emit = defineEmits([
    'add-sub-department',
    'edit-sub-department',
    'delete-sub-department',
    'sub-department-selected'
]);

const internalSelectedDepartmentId = ref(props.selectedDepartmentId);

watch(() => props.selectedDepartmentId, (newVal) => {
    internalSelectedDepartmentId.value = newVal;
});

watch(() => internalSelectedDepartmentId.value, (newVal) => {
    props.handleDepartmentChange(newVal);
}, { immediate: true });

// SubDepartment selection state
const selectedSubDepartmentId = ref(null);

// Handle subdepartment selection from table row or button
const handleSelectSubDepartment = (subDepartment) => {
    selectedSubDepartmentId.value = subDepartment.id;
    emit('sub-department-selected', subDepartment.id);
    
    // Navigate to RiskControl with selected subdepartment
    router.push({
        path: '/risk-control',
        query: { subDepartmentId: subDepartment.id }
    });
};


const subDepartmentColumns = [
    { key: 'name', label: 'Name' },
    { key: 'createdAt', label: 'Created Date', type: 'date' }
];

const enhancedColumns = computed(() => {
    return [...subDepartmentColumns, { key: 'actions', label: 'Actions' }];
});

const noDataMessage = computed(() => {
    if (props.searchQuery) {
        return `No sub departments found for "${props.searchQuery}".`;
    }
    return 'No sub departments found for this department.';
});

</script>

<style scoped>
.sub-department-table .table-row {
    transition: all 0.2s ease;
    cursor: pointer;
}

.sub-department-table .table-row:hover {
    background-color: #f8fafc;
    transform: translateY(-1px);
    box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.sub-department-table .table-row.selected {
    background-color: #eff6ff;
    border-left: 4px solid #3b82f6;
}
</style>