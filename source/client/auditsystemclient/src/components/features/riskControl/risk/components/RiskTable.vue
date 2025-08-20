<template>
    <div class="space-y-6">
        <div class="bg-white rounded-lg shadow-sm border border-gray-200">
            <div class="p-6">

                <!-- Risk Control Matrix and Filters - Stack on mobile -->
                <div class="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-4 mb-6">
                    <div class="flex-1 max-w-md">
                        <ObjectiveHeader/>
                        <RiskControlMatrixSelector
                            :risk-control-matrices="riskControlMatrices"
                            :selected-risk-control-matrix-id="internalSelectedRiskControlMatrixId"
                            @update:selected-risk-control-matrix="(id) => internalSelectedRiskControlMatrixId = id"
                            @risk-control-matrix-changed="(id) => internalSelectedRiskControlMatrixId = id"
                        />
                    </div>

                    <div class="flex flex-col sm:flex-row gap-3">
                        <div class="flex-1 sm:flex-initial">
                            <SearchFilter
                                :search="searchQuery"
                                :search-placeholder="'Search objectives...'"
                                @update:search="handleSearch"
                                class="w-full sm:w-auto"
                            />
                        </div>
                    </div>
                </div>

                <!-- Keep only this Selected risk control matrix Info section -->
                <div v-if="selectedRiskControlMatrixId"
                     class="mb-6 p-4 bg-gradient-to-r from-blue-50 to-indigo-50 rounded-lg border border-blue-200">
                     <div class="flex items-center justify-between">
                         <div class="flex items-center">
                             <RiskControlMatrixIcon size="xxl" custom-class="text-blue-600 mr-2" />

                             <p class="text-sm text-gray-700">
                                 Showing Objectives for: &nbsp;&nbsp;
                                 <span class="font-semibold text-blue-800">
                                     <span v-if="isRiskControlMatrixLoading">Loading risk control matrix...</span>
                                     <span v-else>{{ currentRiskControlMatrix?.description || 'Loading...' }}</span>
                                 </span>
                                 <span v-if="searchQuery" class="ml-2 text-blue-600">| Search: "{{ searchQuery }}"</span>
                             </p>
                         </div>
                         <button @click="$emit('add-objective')"
                             class="px-4 py-2 rounded-md text-sm font-medium bg-secondary-base hover:bg-secondary-shade text-white">
                             <PlusIcon :stroke-width="3" />
                             Add Objective
                         </button>
                     </div>
                 </div>

                <!-- Table Container - Show when risk control matrix is Selected -->
                <div v-if="selectedRiskControlMatrixId" class="bg-gray-50 rounded-lg border border-gray-200 overflow-hidden">
                    <ReusableTable :items="objectives" :columns="enhancedColumns" :is-loading="isLoading" :actions="[]"
                        :no-data-message="noDataMessage" class="objective-table" @row-click="handleRowClick">
                        <template #cell-actions="{ item }">
                            <div class="flex items-center gap-2">
                                <button @click.stop="handleSelectObjective(item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-1 transition-all duration-200"
                                    :class="{ 'bg-green-200': selectedObjectiveId === item.id }">
                                    <CheckIcon v-if="selectedObjectiveId === item.id" size="sm" custom-class="mr-1" />
                                    <SelectIcon v-else size="sm" custom-class="mr-1" />
                                    {{ selectedObjectiveId === item.id ? 'Selected' : 'Select' }}
                                </button>
                                <button @click.stop="$emit('edit-objective', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200">
                                    <EditIcon size="sm" custom-class="mr-1" />
                                    Edit
                                </button>
                                <button @click.stop="$emit('delete-objective', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition-all duration-200">
                                    <DeleteIcon size="sm" custom-class="mr-1" />
                                    Delete
                                </button>
                            </div>
                        </template>
                    </ReusableTable>

                    <!-- Clear Search Button (shown when no results and searching) -->
                    <div v-if="searchQuery && objectives.length === 0 && !isLoading"
                        class="text-center py-4 border-t border-gray-200">
                        <button @click="handleSearch('')"
                            class="inline-flex items-center px-4 py-2 text-sm font-medium text-blue-600 bg-blue-50 rounded-md hover:bg-blue-100 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition-all duration-200">
                            <CloseIcon size="sm" stroke-width="3" custom-class="mr-1" />
                            Clear Search
                        </button>
                    </div>
                </div>

                <!-- No Risk Control Matrix Selected - Show when no risk control matrix is selected -->

                <div v-else class="text-center py-20">
                    <!-- When risk Control matrices are available but none selected -->
                    <div v-if="riskControlMatrices && riskControlMatrices.length > 0" class="max-w-md mx-auto">
                        <div class="bg-gradient-to-br from-blue-50 to-indigo-100 rounded-full w-32 h-32 mx-auto mb-8 flex items-center justify-center shadow-lg">
                            <RiskControlMatrixIcon size="xxxl" custom-class="text-blue-600" />
                        </div>
                        <h3 class="text-2xl font-bold text-gray-800 mb-4">Select a Risk Control Matrix</h3>
                        <p class="text-gray-600 mb-6 leading-relaxed">Choose a risk control matrix from the dropdown above to view and manage objectives</p>
                        <div class="bg-blue-50 border border-blue-200 rounded-lg p-4">
                            <p class="text-sm text-blue-700">
                                <span class="font-medium">💡 Tip:</span> Use the dropdown menu above to select a risk control matrix and start managing objectives
                            </p>
                        </div>
                    </div>

                    <!-- When no risk control matrices are available -->
                    <div v-else class="max-w-lg mx-auto">
                        <div class="bg-gradient-to-br from-gray-50 to-gray-100 rounded-full w-32 h-32 mx-auto mb-8 flex items-center justify-center shadow-lg border-2 border-dashed border-gray-300">
                            <RiskControlMatrixIcon size="xxxl" custom-class="text-gray-400" />
                        </div>
                        <h3 class="text-2xl font-bold text-gray-800 mb-4">No Risk Control Matrices Available</h3>
                        <p class="text-gray-600 mb-6 leading-relaxed">You need to create risk control matrices first before you can manage objectives</p>
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
import RiskControlMatrixSelector from '@/components/features/riskControl/riskControlMatrix/components/RiskControlMatrixSelector.vue';
import RiskControlMatrixIcon from '@/components/common/icons/RiskControlMatrixIcon.vue';
import PlusIcon from '@/components/common/icons/PlusIcon.vue';
import EditIcon from '@/components/common/icons/EditIcon.vue';
import DeleteIcon from '@/components/common/icons/DeleteIcon.vue';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';
import CheckIcon from '@/components/common/icons/CheckIcon.vue';
import SelectIcon from '@/components/common/icons/SelectIcon.vue';
import ObjectiveHeader from './RiskHeader.vue';

const props = defineProps({
    riskControlMatrices: {
        type: Array,
        default: () => []
    },
    objectives: {
        type: Array,
        default: () => []
    },
    isLoading: {
        type: Boolean,
        default: false
    },
    selectedRiskControlMatrixId: {
        type: [String, Number],
        default: null
    },
    selectedRiskControlMatrix: {
        type: Object,
        default: () => null
    },
    currentRiskControlMatrix: {
        type: Object,
        default: () => null
    },
    isRiskControlMatrixLoading: {
        type: Boolean,
        default: false
    },
    searchQuery: {
        type: String,
        default: ''
    },
    handleRiskControlMatrixChange: {
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

const internalSelectedRiskControlMatrixId = ref(props.selectedRiskControlMatrixId);

watch(() => props.selectedRiskControlMatrixId, (newVal) => {
    internalSelectedRiskControlMatrixId.value = newVal;
});

watch(internalSelectedRiskControlMatrixId, (newVal) => {
    props.handleRiskControlMatrixChange(newVal);
});

const emit = defineEmits([
    'add-objective',
    'edit-objective',
    'delete-objective',
    'objective-selected'
]);

// Objective selection state
const selectedObjectiveId = ref(null);

// Handle objective selection from table row or button
const handleSelectObjective = (objective) => {
    selectedObjectiveId.value = objective.id;
    emit('objective-selected', objective.id);
};

// Handle row click to select objective
const handleRowClick = (objective) => {
    handleSelectObjective(objective);
};

const objectiveColumns = [
    { key: 'description', label: 'Description' },
    { key: 'createdAt', label: 'Created Date', type: 'date' }
];

const enhancedColumns = computed(() => {
    return [...objectiveColumns, { key: 'actions', label: 'Actions' }];
});

const noDataMessage = computed(() => {
    if (props.searchQuery) {
        return `No Objectives found for "${props.searchQuery}".`;
    }
    return 'No Objectives found for this Risk Control Matrix.';
});

</script>

<style scoped>
.objective-table .table-row {
    transition: all 0.2s ease;
    cursor: pointer;
}

.objective-table .table-row:hover {
    background-color: #f8fafc;
    transform: translateY(-1px);
    box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.objective-table .table-row.selected {
    background-color: #e6f7ff;
    border-left: 4px solid #3b82f6;
}
</style>
