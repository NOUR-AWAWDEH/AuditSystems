<template>
    <div class="space-y-6">
        <div class="bg-white rounded-lg shadow-sm border border-gray-200">
            <div class="p-6">
                <!-- SubDepartmentSelector and Filters - Stack on mobile -->
                <div class="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-4 mb-6">
                    <div class="flex-1 max-w-md">
                        <AuditProcessHeader/>
                        <SubDepartmentSelector
                            :subDepartments="subDepartments"
                            :selectedSubDepartmentId="internalSelectedSubDepartmentId"
                            @update:selectedSubDepartment="(id) => internalSelectedSubDepartmentId = id"
                            @sub-department-changed="(id) => internalSelectedSubDepartmentId = id"
                        />
                    </div>

                    <div class="flex flex-col sm:flex-row gap-3">
                        <div class="flex-1 sm:flex-initial">
                            <SearchFilter
                                :search="searchQuery"
                                :search-placeholder="'Search audit processes...'"
                                @update:search="handleSearch"
                                class="w-full sm:w-auto"
                            />
                        </div>
                    </div>
                </div>

                <!-- Keep only this Selected SubDepartment Info section -->
                <div v-if="selectedSubDepartmentId"
                     class="mb-6 p-4 bg-gradient-to-r from-blue-50 to-indigo-50 rounded-lg border border-blue-200">
                     <div class="flex items-center justify-between">
                         <div class="flex items-center">
                             <DepartmentIcon size="lg" custom-class="text-blue-600 mr-2" />

                             <p class="text-sm text-gray-700">
                                 Showing Audit Processes for: &nbsp;&nbsp;
                                 <span class="font-semibold text-blue-800">
                                     <span v-if="isSubDepartmentLoading">Loading sub-department...</span>
                                     <span v-else>{{ currentSubDepartment?.name || 'Loading...' }}</span>
                                 </span>
                                 <span v-if="searchQuery" class="ml-2 text-blue-600">| Search: "{{ searchQuery }}"</span>
                             </p>
                         </div>
                         <button @click="$emit('add-audit-process')"
                             class="px-4 py-2 rounded-md text-sm font-medium bg-secondary-base hover:bg-secondary-shade text-white">
                             <PlusIcon :stroke-width="3" />
                             Add Process
                         </button>
                     </div>
                 </div>

                <!-- Table Container - Show when SubDepartment is Selected -->
                <div v-if="selectedSubDepartmentId" class="bg-gray-50 rounded-lg border border-gray-200 overflow-hidden">
                    <ReusableTable :items="auditProcesses" :columns="enhancedColumns" :is-loading="isLoading" :actions="[]"
                        :no-data-message="noDataMessage" class="audit-process-table" @row-click="handleRowClick">
                        <template #cell-actions="{ item }">
                            <div class="flex items-center gap-2">
                                <button @click.stop="handleSelectAuditProcess(item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-1 transition-all duration-200"
                                    :class="{ 'bg-green-200': selectedAuditProcessId === item.id }">
                                    <CheckIcon v-if="selectedAuditProcessId === item.id" size="sm" custom-class="mr-1" />
                                    <SelectIcon v-else size="sm" custom-class="mr-1" />
                                    {{ selectedAuditProcessId === item.id ? 'Selected' : 'Select' }}
                                </button>
                                <button @click.stop="$emit('edit-audit-process', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200">
                                    <EditIcon size="sm" custom-class="mr-1" />
                                    Edit
                                </button>
                                <button @click.stop="$emit('delete-audit-process', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition-all duration-200">
                                    <DeleteIcon size="sm" custom-class="mr-1" />
                                    Delete
                                </button>
                            </div>
                        </template>
                    </ReusableTable>

                    <!-- Clear Search Button (shown when no results and searching) -->
                    <div v-if="searchQuery && auditProcesses.length === 0 && !isLoading"
                        class="text-center py-4 border-t border-gray-200">
                        <button @click="handleSearch('')"
                            class="inline-flex items-center px-4 py-2 text-sm font-medium text-blue-600 bg-blue-50 rounded-md hover:bg-blue-100 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition-all duration-200">
                            <CloseIcon size="sm" stroke-width="3" custom-class="mr-1" />
                            Clear Search
                        </button>
                    </div>
                </div>

                <!-- No SubDepartment Selected - Show when no subDepartment is selected -->
                <div v-else class="text-center py-20">
                    <!-- When sub-departments are available but none selected -->
                    <div v-if="subDepartments && subDepartments.length > 0" class="max-w-md mx-auto">
                        <div class="bg-gradient-to-br from-blue-50 to-indigo-100 rounded-full w-32 h-32 mx-auto mb-8 flex items-center justify-center shadow-lg">
                            <DepartmentIcon size="xxxl" custom-class="text-blue-600" />
                        </div>
                        <h3 class="text-2xl font-bold text-gray-800 mb-4">Select a Sub-Department</h3>
                        <p class="text-gray-600 mb-6 leading-relaxed">Choose a sub-department from the dropdown above to view and manage audit processes</p>
                        <div class="bg-blue-50 border border-blue-200 rounded-lg p-4">
                            <p class="text-sm text-blue-700">
                                <span class="font-medium">💡 Tip:</span> Use the dropdown menu above to select a sub-department and start managing audit processes
                            </p>
                        </div>
                    </div>

                    <!-- When no sub-departments are available -->
                    <div v-else class="max-w-lg mx-auto">
                        <div class="bg-gradient-to-br from-gray-50 to-gray-100 rounded-full w-32 h-32 mx-auto mb-8 flex items-center justify-center shadow-lg border-2 border-dashed border-gray-300">
                            <DepartmentIcon size="xxxl" custom-class="text-gray-400" />
                        </div>
                        <h3 class="text-2xl font-bold text-gray-800 mb-4">No Sub-Departments Available</h3>
                        <p class="text-gray-600 mb-6 leading-relaxed">You need to create sub-departments first before you can manage audit processes</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { computed, ref, watch } from 'vue';
import ReusableTable from '@/components/common/ReusableTable.vue';
import SearchFilter from '@/components/common/SearchFilter.vue';
import SubDepartmentSelector from '@/components/features/Organization/subdepartment/components/SubDepartmentSelector.vue';
import DepartmentIcon from '@/components/common/icons/DepartmentIcon.vue';
import PlusIcon from '@/components/common/icons/PlusIcon.vue';
import EditIcon from '@/components/common/icons/EditIcon.vue';
import DeleteIcon from '@/components/common/icons/DeleteIcon.vue';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';
import CheckIcon from '@/components/common/icons/CheckIcon.vue';
import SelectIcon from '@/components/common/icons/SelectIcon.vue';
import AuditProcessHeader from './AuditProcessHeader.vue';

const props = defineProps({
    subDepartments: Array,
    auditProcesses: Array,
    isLoading: Boolean,
    selectedSubDepartmentId: String,
    selectedSubDepartment: Object,
    currentSubDepartment: Object,
    isSubDepartmentLoading: Boolean,
    searchQuery: String,
    handleSubDepartmentChange: Function,
    handleSearch: Function,
    refreshData: Function
});

const internalSelectedSubDepartmentId = ref(props.selectedSubDepartmentId);

watch(() => props.selectedSubDepartmentId, (newVal) => {
    internalSelectedSubDepartmentId.value = newVal;
});

watch(internalSelectedSubDepartmentId, (newVal) => {
    props.handleSubDepartmentChange(newVal);
});

const emit = defineEmits([
    'add-audit-process',
    'edit-audit-process',
    'delete-audit-process',
    'audit-process-selected'
]);

// Audit Process selection state
const selectedAuditProcessId = ref(null);

// Handle audit process selection from table row or button
const handleSelectAuditProcess = (auditProcess) => {
    selectedAuditProcessId.value = auditProcess.id;
    emit('audit-process-selected', auditProcess.id);
};

// Handle row click to select audit process
const handleRowClick = (auditProcess) => {
    handleSelectAuditProcess(auditProcess);
};

const auditProcessColumns = [
    { key: 'processName', label: 'Name' },
    { key: 'createdAt', label: 'Created Date', type: 'date' }
];

const enhancedColumns = computed(() => {
    return [...auditProcessColumns, { key: 'actions', label: 'Actions' }];
});

const noDataMessage = computed(() => {
    if (props.searchQuery) {
        return `No processes found for "${props.searchQuery}".`;
    }
    return 'No processes found for this sub department.';
});

</script>

<style scoped>
.audit-process-table .table-row {
    transition: all 0.2s ease;
    cursor: pointer;
}

.audit-process-table .table-row:hover {
    background-color: #f8fafc;
    transform: translateY(-1px);
    box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.audit-process-table .table-row.selected {
    background-color: #e6f7ff;
    border-left: 4px solid #3b82f6;
}
</style>
