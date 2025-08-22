<template>
    <div class="space-y-6">
        <div class="bg-white rounded-lg shadow-sm border border-gray-200">
            <div class="p-6">

                <div class="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-4 mb-6">
                    <div class="flex-1 max-w-md">
                        <RiskHeader/>
                        <ObjectiveSelector
                            :objectives="objectives"
                            :selected-objective-id="internalSelectedObjectiveId"
                            @update:selected-objective="(id) => internalSelectedObjectiveId = id"
                            @objective-changed="(id) => internalSelectedObjectiveId = id"
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

                <div v-if="selectedObjectiveId"
                     class="mb-6 p-4 bg-gradient-to-r from-blue-50 to-indigo-50 rounded-lg border border-blue-200">
                    <div class="flex items-center justify-between">
                        <div class="flex items-center">
                            <ObjectiveIcon size="xxl" custom-class="text-blue-600 mr-2" />

                            <p class="text-sm text-gray-700">
                                Showing Objectives for: &nbsp;&nbsp;
                                <span class="font-semibold text-blue-800">
                                    <span v-if="isObjectiveLoading">Loading objective...</span>
                                    <span v-else>{{ currentObjective?.description || 'Loading...' }}</span>
                                </span>
                                <span v-if="searchQuery" class="ml-2 text-blue-600">| Search: "{{ searchQuery }}"</span>
                            </p>
                        </div>
                        <button @click="$emit('add-risk')"
                                class="px-4 py-2 rounded-md text-sm font-medium bg-secondary-base hover:bg-secondary-shade text-white">
                            <PlusIcon :stroke-width="3" />
                            Add Risk
                        </button>
                    </div>
                </div>

                <div v-if="selectedObjectiveId" class="bg-gray-50 rounded-lg border border-gray-200 overflow-hidden">
                    <ReusableTable
                        :items="transformedRisks"
                        :columns="enhancedColumns"
                        :is-loading="isLoading"
                        :actions="[]"
                        :no-data-message="noDataMessage"
                        class="risk-table"
                    >
                        <template #cell-rate="{ item }">
                            <span :class="rateColorMap[item.rate]" class="px-2 py-1 rounded-full text-xs font-semibold">
                                {{ item.rate }}
                            </span>
                        </template>

                        <template #cell-actions="{ item }">
                            <div class="flex items-center gap-2">
                                <button @click.stop="handleSelectRisk(item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-1 transition-all duration-200"
                                    :class="{ 'bg-green-200': selectedRiskId === item.id }">
                                    <CheckIcon v-if="selectedRiskId === item.id" size="sm" custom-class="mr-1" />
                                    <SelectIcon v-else size="sm" custom-class="mr-1" />
                                    {{ selectedRiskId === item.id ? 'Selected' : 'Select' }}
                                </button>
                                <button @click.stop="$emit('edit-risk', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200">
                                    <EditIcon size="sm" custom-class="mr-1" />
                                    Edit
                                </button>
                                <button @click.stop="$emit('delete-risk', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition-all duration-200">
                                    <DeleteIcon size="sm" custom-class="mr-1" />
                                    Delete
                                </button>
                            </div>
                        </template>
                    </ReusableTable>

                    <div v-if="searchQuery && risks.length === 0 && !isLoading"
                        class="text-center py-4 border-t border-gray-200">
                        <button @click="handleSearch('')"
                            class="inline-flex items-center px-4 py-2 text-sm font-medium text-blue-600 bg-blue-50 rounded-md hover:bg-blue-100 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition-all duration-200">
                            <CloseIcon size="sm" stroke-width="3" custom-class="mr-1" />
                            Clear Search
                        </button>
                    </div>
                </div>

                <div v-else class="text-center py-20">
                    <div v-if="objectives && objectives.length > 0" class="max-w-md mx-auto">
                        <div class="bg-gradient-to-br from-blue-50 to-indigo-100 rounded-full w-32 h-32 mx-auto mb-8 flex items-center justify-center shadow-lg">
                            <ObjectiveIcon size="xxxl" custom-class="text-blue-600" />
                        </div>
                        <h3 class="text-2xl font-bold text-gray-800 mb-4">Select an Objective</h3>
                        <p class="text-gray-600 mb-6 leading-relaxed">Choose an objective from the dropdown above to view and manage risks</p>
                        <div class="bg-blue-50 border border-blue-200 rounded-lg p-4">
                            <p class="text-sm text-blue-700">
                                <span class="font-medium">💡 Tip:</span> Use the dropdown menu above to select an objective and start managing risks
                            </p>
                        </div>
                    </div>

                    <div v-else class="max-w-lg mx-auto">
                        <div class="bg-gradient-to-br from-gray-50 to-gray-100 rounded-full w-32 h-32 mx-auto mb-8 flex items-center justify-center shadow-lg border-2 border-dashed border-gray-300">
                            <ObjectiveIcon size="xxxl" custom-class="text-gray-400" />
                        </div>
                        <h3 class="text-2xl font-bold text-gray-800 mb-4">No Objectives Available</h3>
                        <p class="text-gray-600 mb-6 leading-relaxed">You need to create objectives first before you can manage risks</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { computed, onMounted, ref, watch } from 'vue';
import ReusableTable from '@/components/common/ReusableTable.vue';
import SearchFilter from '@/components/common/SearchFilter.vue';
import PlusIcon from '@/components/common/icons/PlusIcon.vue';
import EditIcon from '@/components/common/icons/EditIcon.vue';
import DeleteIcon from '@/components/common/icons/DeleteIcon.vue';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';
import CheckIcon from '@/components/common/icons/CheckIcon.vue';
import SelectIcon from '@/components/common/icons/SelectIcon.vue';
import RiskHeader from './RiskHeader.vue';
import ObjectiveSelector from '../../objective/components/ObjectiveSelector.vue';
import ObjectiveIcon from '@/components/common/icons/ObjectiveIcon.vue';

const props = defineProps({
    objectives: {
        type: Array,
        default: () => []
    },
    risks: {
        type: Array,
        default: () => []
    },
    isLoading: {
        type: Boolean,
        default: false
    },
    selectedObjectiveId: {
        type: [String, Number],
        default: null
    },
    selectedObjective: {
        type: Object,
        default: () => null
    },
    currentObjective: {
        type: Object,
        default: () => null
    },
    isObjectiveLoading: {
        type: Boolean,
        default: false
    },
    searchQuery: {
        type: String,
        default: ''
    },
    handleObjectiveChange: {
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

const internalSelectedObjectiveId = ref(props.selectedObjectiveId);

watch(() => props.selectedObjectiveId, (newVal) => {
    internalSelectedObjectiveId.value = newVal;
});

watch(internalSelectedObjectiveId, (newVal) => {
    props.handleObjectiveChange(newVal);
});

const emit = defineEmits([
    'add-risk',
    'edit-risk',
    'delete-risk',
    'risk-selected'
]);

// Risk selection state
const selectedRiskId = ref(null);

// Handle risk selection from table row or button
const handleSelectRisk = (risk) => {
    selectedRiskId.value = risk.id;
    emit('risk-selected', risk.id);
};


const riskColumns = [
    { key: 'riskName', label: 'Name' },
    { key: 'description', label: 'Description' },
    { key: 'rate', label: 'Rate Level' },
    { key: 'createdAt', label: 'Created Date', type: 'date' }
];

const enhancedColumns = computed(() => {
    return [...riskColumns, { key: 'actions', label: 'Actions' }];
});

const noDataMessage = computed(() => {
    if (props.searchQuery) {
        return `No Risks found for "${props.searchQuery}".`;
    }
    return 'No Risks found for this Objective.';
});

const rateLevelMap = {
    '11111111-1111-1111-1111-111111111111': 'Critical',
    '22222222-2222-2222-2222-222222222222': 'High',
    '33333333-3333-3333-3333-333333333333': 'Medium',
    '44444444-4444-4444-4444-444444444444': 'Low',
    '55555555-5555-5555-5555-555555555555': 'Minimal'
};

const rateColorMap = {
    'Critical': 'bg-red-100 text-red-800',
    'High': 'bg-orange-100 text-orange-800',
    'Medium': 'bg-yellow-100 text-yellow-800',
    'Low': 'bg-green-100 text-green-800',
    'Minimal': 'bg-blue-100 text-blue-800',
    'Unknown': 'bg-gray-100 text-gray-800'
};


const transformedRisks = computed(() => {
    return props.risks.map(risk => {
        const rate = rateLevelMap[risk.rateId] || 'Unknown';
        return {
            ...risk,
            rate,
        };
    });
});


</script>

<style scoped>
.risk-table .table-row {
    transition: all 0.2s ease;
    cursor: pointer;
}

.risk-table .table-row:hover {
    background-color: #f8fafc;
    transform: translateY(-1px);
    box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.risk-table .table-row.selected {
    background-color: #e6f7ff;
    border-left: 4px solid #3b82f6;
}
</style>