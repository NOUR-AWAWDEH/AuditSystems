<template>
    <!--ObservationTable.vue-->
    <div class="space-y-6">
        <div class="bg-white rounded-lg shadow-sm border border-gray-200">
            <div class="p-6">

                <!-- AuditProgramSelector and Filters - Stack on mobile -->
                <div class="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-4 mb-6">
                    <div class="flex-1 max-w-md">
                      <SubProcessHeader/>
                      <AuditProgramSelector
                            :auditPrograms="auditPrograms"
                            :selectedAuditProgramId="internalSelectedAuditProgramId"
                            @update:selectedAuditProgram="(id) => internalSelectedAuditProgramId = id"
                            @audit-program-changed="(id) => internalSelectedAuditProgramId = id"
                        />
                    </div>

                    <div class="flex flex-col sm:flex-row gap-3">
                        <div class="flex-1 sm:flex-initial">
                            <SearchFilter
                                :search="searchQuery"
                                :search-placeholder="'Search observations...'"
                                @update:search="handleSearch"
                                class="w-full sm:w-auto"
                            />
                        </div>
                    </div>
                </div>

                <!-- Keep only this Selected AuditProgram Info section -->
                <div v-if="selectedAuditProgramId"
                     class="mb-6 p-4 bg-gradient-to-r from-blue-50 to-indigo-50 rounded-lg border border-blue-200">
                     <div class="flex items-center justify-between">
                         <div class="flex items-center">
                             <AuditProgramIcon size="xxl" custom-class="text-blue-600 mr-2" />

                             <p class="text-sm text-gray-700">
                                 Showing Observations for: &nbsp;&nbsp;
                                 <span class="font-semibold text-blue-800">
                                     <span v-if="isAuditProgramLoading">Loading audit program...</span>
                                     <span v-else>{{ currentAuditProgram?.description || 'Loading...' }}</span>
                                 </span>
                                 <span v-if="searchQuery" class="ml-2 text-blue-600">| Search: "{{ searchQuery }}"</span>
                             </p>
                         </div>
                         <button @click="$emit('add-observation')"
                             class="px-4 py-2 rounded-md text-sm font-medium bg-secondary-base hover:bg-secondary-shade text-white">
                             <PlusIcon :stroke-width="3" />
                             Add Observation
                         </button>
                     </div>
                 </div>

                <!-- Table Container - Show when AuditProgram is Selected -->
                <div v-if="selectedAuditProgramId" class="bg-gray-50 rounded-lg border border-gray-200 overflow-hidden">
                    <ReusableTable 
                        :items="observations" 
                        :columns="enhancedColumns" 
                        :is-loading="isLoading" 
                        :actions="[]"
                        :no-data-message="noDataMessage" 
                        class="observation-table" 
                        >
                        <template #cell-actions="{ item }">
                            <div class="flex items-center gap-2">
                                <button @click.stop="handleSelectObservation(item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-1 transition-all duration-200"
                                    :class="{ 'bg-green-200': selectedObservationId === item.id }">
                                    <CheckIcon v-if="selectedObservationId === item.id" size="sm" custom-class="mr-1" />
                                    <SelectIcon v-else size="sm" custom-class="mr-1" />
                                    {{ selectedObservationId === item.id ? 'Selected' : 'Select' }}
                                </button>
                                <button @click.stop="$emit('edit-observation', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200">
                                    <EditIcon size="sm" custom-class="mr-1" />
                                    Edit
                                </button>
                                <button @click.stop="$emit('delete-observation', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition-all duration-200">
                                    <DeleteIcon size="sm" custom-class="mr-1" />
                                    Delete
                                </button>
                            </div>
                        </template>
                    </ReusableTable>

                    <!-- Clear Search Button (shown when no results and searching) -->
                    <div v-if="searchQuery && observations.length === 0 && !isLoading"
                        class="text-center py-4 border-t border-gray-200">
                        <button @click="handleSearch('')"
                            class="inline-flex items-center px-4 py-2 text-sm font-medium text-blue-600 bg-blue-50 rounded-md hover:bg-blue-100 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition-all duration-200">
                            <CloseIcon size="sm" stroke-width="3" custom-class="mr-1" />
                            Clear Search
                        </button>
                    </div>
                </div>

                <!-- No AuditProgram Selected - Show when no auditProgram is selected -->
                 <NoAuditProgramSelected 
                    v-if="!isAuditProgramLoading && auditPrograms.length === 0"
                    :audit-programs="auditPrograms" 
                 />

            </div>
        </div>
    </div>
</template>

<script setup>
import { computed, ref, watch } from 'vue';
import ReusableTable from '@/components/common/ReusableTable.vue';
import SearchFilter from '@/components/common/SearchFilter.vue';
import AuditProgramIcon from '@/components/common/icons/AuditProgramIcon.vue';
import AuditProgramSelector from '@/components/features/riskControl/auditProgram/components/AuditProgramSelector.vue';
import PlusIcon from '@/components/common/icons/PlusIcon.vue';
import EditIcon from '@/components/common/icons/EditIcon.vue';
import DeleteIcon from '@/components/common/icons/DeleteIcon.vue';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';
import CheckIcon from '@/components/common/icons/CheckIcon.vue';
import SelectIcon from '@/components/common/icons/SelectIcon.vue';
import SubProcessHeader from './ObservationHeader.vue';
import NoAuditProgramSelected from './NoAuditProgramSelected.vue';



const props = defineProps({
    auditPrograms: {
        type: Array,
        default: () => []
    },
    observations: {
        type: Array,
        default: () => []
    },
    isLoading: {
        type: Boolean,
        default: false
    },
    selectedAuditProgramId: {
        type: String,
        default: ''
    },
    selectedAuditProgram: {
        type: Object,
        default: () => null
    },
    currentAuditProgram: {
        type: Object,
        default: () => null
    },
    isAuditProgramLoading: {
        type: Boolean,
        default: false
    },
    searchQuery: {
        type: String,
        default: ''
    },
    handleAuditProgramChange: {
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

const internalSelectedAuditProgramId = ref(props.selectedAuditProgramId);

watch(() => props.selectedAuditProgramId, (newVal) => {
    internalSelectedAuditProgramId.value = newVal;
});

watch(internalSelectedAuditProgramId, (newVal) => {
    props.handleAuditProgramChange(newVal);
});

const emit = defineEmits([
    'add-observation',
    'edit-observation',
    'delete-observation',
    'observation-selected'
]);

// Observation selection state
const selectedObservationId = ref(null);

// Handle observation selection from table row or button
const handleSelectObservation = (observation) => {
    selectedObservationId.value = observation.id;
    emit('observation-selected', observation.id);
};


const observationColumns = [
    { key: 'description', label: 'Description' },
    { key: 'background', label: 'Background' },
    { key: 'actionOwner', label: 'Action Owner' },
    { key: 'actionPlan', label: 'Action Plan' },
    { key: 'actionDate', label: 'Action Date', type: 'date' },
    { key: 'note', label: 'Note' },
    { key: 'createdAt', label: 'Created Date', type: 'date' }
];


const enhancedColumns = computed(() => {
    return [...observationColumns, { key: 'actions', label: 'Actions' }];
});

const noDataMessage = computed(() => {
    if (props.searchQuery) {
        return `No observations found for "${props.searchQuery}".`;
    }
    return 'No observations found for this audit process.';
});

</script>

<style scoped>
.observation-table .table-row {
    transition: all 0.2s ease;
    cursor: pointer;
}

.observation-table .table-row:hover {
    background-color: #f8fafc;
    transform: translateY(-1px);
    box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.observation-table .table-row.selected {
    background-color: #e6f7ff;
    border-left: 4px solid #3b82f6;
}
</style>
