<template>
    <div class="space-y-6">
        <div class="bg-white rounded-lg shadow-sm border border-gray-200">
            <div class="p-6">

                <!-- AuditProcessSelector and Filters - Stack on mobile -->
                <div class="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-4 mb-6">
                    <div class="flex-1 max-w-md">
                      <SubProcessHeader/>
                      <AuditProcessSelector
                            :auditProcesses="auditProcesses"
                            :selectedAuditProcessId="internalSelectedAuditProcessId"
                            @update:selectedAuditProcess="(id) => internalSelectedAuditProcessId = id"
                            @audit-process-changed="(id) => internalSelectedAuditProcessId = id"
                        />
                    </div>

                    <div class="flex flex-col sm:flex-row gap-3">
                        <div class="flex-1 sm:flex-initial">
                            <SearchFilter
                                :search="searchQuery"
                                :search-placeholder="'Search sub processes...'"
                                @update:search="handleSearch"
                                class="w-full sm:w-auto"
                            />
                        </div>
                    </div>
                </div>

                <!-- Keep only this Selected AuditProcess Info section -->
                <div v-if="selectedAuditProcessId"
                     class="mb-6 p-4 bg-gradient-to-r from-blue-50 to-indigo-50 rounded-lg border border-blue-200">
                     <div class="flex items-center justify-between">
                         <div class="flex items-center">
                             <DepartmentIcon size="lg" custom-class="text-blue-600 mr-2" />

                             <p class="text-sm text-gray-700">
                                 Showing Sub Processes for: &nbsp;&nbsp;
                                 <span class="font-semibold text-blue-800">
                                     <span v-if="isAuditProcessLoading">Loading audit process...</span>
                                     <span v-else>{{ currentAuditProcess?.processName || 'Loading...' }}</span>
                                 </span>
                                 <span v-if="searchQuery" class="ml-2 text-blue-600">| Search: "{{ searchQuery }}"</span>
                             </p>
                         </div>
                         <button @click="$emit('add-sub-process')"
                             class="px-4 py-2 rounded-md text-sm font-medium bg-secondary-base hover:bg-secondary-shade text-white">
                             <PlusIcon :stroke-width="3" />
                             Add Sub Process
                         </button>
                     </div>
                 </div>

                <!-- Table Container - Show when AuditProcess is Selected -->
                <div v-if="selectedAuditProcessId" class="bg-gray-50 rounded-lg border border-gray-200 overflow-hidden">
                    <ReusableTable :items="subProcesses" :columns="enhancedColumns" :is-loading="isLoading" :actions="[]"
                        :no-data-message="noDataMessage" class="sub-process-table" @row-click="handleRowClick">
                        <template #cell-actions="{ item }">
                            <div class="flex items-center gap-2">
                                <button @click.stop="handleSelectSubProcess(item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-1 transition-all duration-200"
                                    :class="{ 'bg-green-200': selectedSubProcessId === item.id }">
                                    <CheckIcon v-if="selectedSubProcessId === item.id" size="sm" custom-class="mr-1" />
                                    <SelectIcon v-else size="sm" custom-class="mr-1" />
                                    {{ selectedSubProcessId === item.id ? 'Selected' : 'Select' }}
                                </button>
                                <button @click.stop="$emit('edit-sub-process', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200">
                                    <EditIcon size="sm" custom-class="mr-1" />
                                    Edit
                                </button>
                                <button @click.stop="$emit('delete-sub-process', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition-all duration-200">
                                    <DeleteIcon size="sm" custom-class="mr-1" />
                                    Delete
                                </button>
                            </div>
                        </template>
                    </ReusableTable>

                    <!-- Clear Search Button (shown when no results and searching) -->
                    <div v-if="searchQuery && subProcesses.length === 0 && !isLoading"
                        class="text-center py-4 border-t border-gray-200">
                        <button @click="handleSearch('')"
                            class="inline-flex items-center px-4 py-2 text-sm font-medium text-blue-600 bg-blue-50 rounded-md hover:bg-blue-100 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition-all duration-200">
                            <CloseIcon size="sm" stroke-width="3" custom-class="mr-1" />
                            Clear Search
                        </button>
                    </div>
                </div>

                <!-- No AuditProcess Selected - Show when no auditProcess is selected -->
                <div v-else class="text-center py-20">
                    <!-- When audit processes are available but none selected -->
                    <div v-if="auditProcesses && auditProcesses.length > 0" class="max-w-md mx-auto">
                        <div class="bg-gradient-to-br from-blue-50 to-indigo-100 rounded-full w-32 h-32 mx-auto mb-8 flex items-center justify-center shadow-lg">
                            <DepartmentIcon size="xxxl" custom-class="text-blue-600" />
                        </div>
                        <h3 class="text-2xl font-bold text-gray-800 mb-4">Select an Audit Process</h3>
                        <p class="text-gray-600 mb-6 leading-relaxed">Choose an audit process from the dropdown above to view and manage sub processes</p>
                        <div class="bg-blue-50 border border-blue-200 rounded-lg p-4">
                            <p class="text-sm text-blue-700">
                                <span class="font-medium">💡 Tip:</span> Use the dropdown menu above to select an audit process and start managing sub processes
                            </p>
                        </div>
                    </div>

                    <!-- When no audit processes are available -->
                    <div v-else class="max-w-lg mx-auto">
                        <div class="bg-gradient-to-br from-gray-50 to-gray-100 rounded-full w-32 h-32 mx-auto mb-8 flex items-center justify-center shadow-lg border-2 border-dashed border-gray-300">
                            <DepartmentIcon size="xxxl" custom-class="text-gray-400" />
                        </div>
                        <h3 class="text-2xl font-bold text-gray-800 mb-4">No Audit Processes Available</h3>
                        <p class="text-gray-600 mb-6 leading-relaxed">You need to create audit processes first before you can manage sub processes</p>
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
import AuditProcessSelector from '@/components/features/riskControl/auditProcess/components/AuditProcessSelector.vue';
import DepartmentIcon from '@/components/common/icons/DepartmentIcon.vue';
import PlusIcon from '@/components/common/icons/PlusIcon.vue';
import EditIcon from '@/components/common/icons/EditIcon.vue';
import DeleteIcon from '@/components/common/icons/DeleteIcon.vue';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';
import CheckIcon from '@/components/common/icons/CheckIcon.vue';
import SelectIcon from '@/components/common/icons/SelectIcon.vue';
import SubProcessHeader from './SubProcessHeader.vue';

const props = defineProps({
    auditProcesses: {
        type: Array,
        default: () => []
    },
    subProcesses: {
        type: Array,
        default: () => []
    },
    isLoading: {
        type: Boolean,
        default: false
    },
    selectedAuditProcessId: {
        type: String,
        default: ''
    },
    selectedAuditProcess: {
        type: Object,
        default: () => null
    },
    currentAuditProcess: {
        type: Object,
        default: () => null
    },
    isAuditProcessLoading: {
        type: Boolean,
        default: false
    },
    searchQuery: {
        type: String,
        default: ''
    },
    handleAuditProcessChange: {
        type: Function,
        default: () => {}
    },
    handleSearch: {
        type: Function,
        default: () => {}
    },
    refreshData: {
        type: Function,
        default: () => {}
    }
});

const internalSelectedAuditProcessId = ref(props.selectedAuditProcessId);

watch(() => props.selectedAuditProcessId, (newVal) => {
    internalSelectedAuditProcessId.value = newVal;
});

watch(internalSelectedAuditProcessId, (newVal) => {
    props.handleAuditProcessChange(newVal);
});

const emit = defineEmits([
    'add-sub-process',
    'edit-sub-process',
    'delete-sub-process',
    'sub-process-selected'
]);

// Sub Process selection state
const selectedSubProcessId = ref(null);

// Handle sub process selection from table row or button
const handleSelectSubProcess = (subProcess) => {
    selectedSubProcessId.value = subProcess.id;
    emit('sub-process-selected', subProcess.id);
};

// Handle row click to select sub process
const handleRowClick = (subProcess) => {
    handleSelectSubProcess(subProcess);
};

const subProcessColumns = [
    { key: 'particular', label: 'Particular' },
    { key: 'createdAt', label: 'Created Date', type: 'date' }
];

const enhancedColumns = computed(() => {
    return [...subProcessColumns, { key: 'actions', label: 'Actions' }];
});

const noDataMessage = computed(() => {
    if (props.searchQuery) {
        return `No sub processes found for "${props.searchQuery}".`;
    }
    return 'No sub processes found for this audit process.';
});

</script>

<style scoped>
.sub-process-table .table-row {
    transition: all 0.2s ease;
    cursor: pointer;
}

.sub-process-table .table-row:hover {
    background-color: #f8fafc;
    transform: translateY(-1px);
    box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.sub-process-table .table-row.selected {
    background-color: #e6f7ff;
    border-left: 4px solid #3b82f6;
}
</style>
