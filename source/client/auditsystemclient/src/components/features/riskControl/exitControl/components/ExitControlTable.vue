<template>
    <div class="space-y-6">
        <div class="bg-white rounded-lg shadow-sm border border-gray-200">
            <div class="p-6">

                <!-- Objective and Filters - Stack on mobile -->
                <div class="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-4 mb-6">
                    <div class="flex-1 max-w-md">
                        <ExitControlHeader/>
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
                                :search-placeholder="'Search exit controls...'"
                                @update:search="handleSearch"
                                class="w-full sm:w-auto"
                            />
                        </div>
                    </div>
                </div>

                <!-- Keep only this Selected objective Info section -->
                <div v-if="selectedObjectiveId"
                     class="mb-6 p-4 bg-gradient-to-r from-blue-50 to-indigo-50 rounded-lg border border-blue-200">
                     <div class="flex items-center justify-between">
                         <div class="flex items-center">
                             <ObjectiveIcon size="xxl" custom-class="text-blue-600 mr-2" />

                             <p class="text-sm text-gray-700">
                                 Showing Exit Controls for: &nbsp;&nbsp;
                                 <span class="font-semibold text-blue-800">
                                     <span v-if="isObjectiveLoading">Loading objective...</span>
                                     <span v-else>{{ currentObjective?.description || 'Loading...' }}</span>
                                 </span>
                                 <span v-if="searchQuery" class="ml-2 text-blue-600">| Search: "{{ searchQuery }}"</span>
                             </p>
                         </div>
                         <button @click="$emit('add-exit-control')"
                             class="px-4 py-2 rounded-md text-sm font-medium bg-secondary-base hover:bg-secondary-shade text-white">
                             <PlusIcon :stroke-width="3" />
                             Add Exit Control
                         </button>
                     </div>
                 </div>

                 <!-- Table Container - Show when objective is Selected -->
                <div v-if="selectedObjectiveId" class="bg-gray-50 rounded-lg border border-gray-200 overflow-hidden">
                    <ReusableTable
                        :items="exitControls"
                        :columns="enhancedColumns"
                        :is-loading="isLoading"
                        :actions="[]"
                        :no-data-message="noDataMessage"
                        class="exit-control-table"
                        >
                        <template #cell-inadequate="{ item }">
                            <span 
                                :class="inadequateColorMap[item.inadequate]" 
                                class="px-2 py-1 rounded-full text-xs font-semibold">
                                {{ inadequateLabelMap[item.inadequate] }}
                            </span>
                        </template>

                        <template #cell-actions="{ item }">
                            <div class="flex items-center gap-2">
                                <button @click.stop="handleSelectExitControl(item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium
                                         text-green-700 bg-green-100 rounded-md hover:bg-green-200
                                         focus:outline-none focus:ring-2 focus:ring-green-500
                                         focus:ring-offset-1 transition-all duration-200"
                                    :class="{ 'bg-green-200': selectedExitControlId === item.id }">
                                    <CheckIcon v-if="selectedExitControlId === item.id" size="sm" custom-class="mr-1" />
                                    <SelectIcon v-else size="sm" custom-class="mr-1" />
                                    {{ selectedExitControlId === item.id ? 'Selected' : 'Select' }}
                                </button>
                                <button @click.stop="$emit('edit-exit-control', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium
                                     text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200
                                     focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1
                                     transition-all duration-200">
                                    <EditIcon size="sm" custom-class="mr-1" />
                                    Edit
                                </button>
                                <button @click.stop="$emit('delete-exit-control', item)"
                                    class="inline-flex items-center px-3 py-1.5 text-sm font-medium
                                     text-red-700 bg-red-100 rounded-md hover:bg-red-200
                                     focus:outline-none focus:ring-2 focus:ring-red-500
                                     focus:ring-offset-1 transition-all duration-200">
                                    <DeleteIcon size="sm" custom-class="mr-1" />
                                    Delete
                                </button>
                            </div>
                        </template>
                    </ReusableTable>

                    <!-- Clear Search Button (shown when no results and searching) -->
                    <div v-if="searchQuery && exitControls.length === 0 && !isLoading"
                        class="text-center py-4 border-t border-gray-200">
                        <button @click="handleSearch('')"
                            class="inline-flex items-center px-4 py-2 text-sm font-medium
                                text-blue-600 bg-blue-50 rounded-md hover:bg-blue-100
                                focus:outline-none focus:ring-2 focus:ring-blue-500
                                focus:ring-offset-2 transition-all duration-200">
                            <CloseIcon size="sm" stroke-width="3" custom-class="mr-1" />
                            Clear Search
                        </button>
                    </div>
                </div>

                <!-- No Objective Selected - Show when no objective is selected -->

                <div v-else class="text-center py-20">
                    <!-- When objectives are available but none selected -->
                    <div v-if="objectives && objectives.length > 0" class="max-w-md mx-auto">
                        <div class="bg-gradient-to-br from-blue-50 to-indigo-100
                            rounded-full w-32 h-32 mx-auto mb-8 flex items-center
                            justify-center shadow-lg">
                            <ObjectiveIcon size="xxxl" custom-class="text-blue-600" />
                        </div>
                        <h3 class="text-2xl font-bold text-gray-800 mb-4">Select an Objective</h3>
                        <p class="text-gray-600 mb-6 leading-relaxed">Choose an objective from the dropdown above to view and manage exit controls</p>
                        <div class="bg-blue-50 border border-blue-200 rounded-lg p-4">
                            <p class="text-sm text-blue-700">
                                <span class="font-medium">💡 Tip:</span> Use the dropdown menu above to select an objective and start managing exit controls
                            </p>
                        </div>
                    </div>

                    <!-- When no objectives are available -->
                    <div v-else class="max-w-lg mx-auto">
                        <div class="bg-gradient-to-br from-gray-50 to-gray-100 rounded-full w-32 h-32
                             mx-auto mb-8 flex items-center justify-center shadow-lg border-2 border-dashed border-gray-300">
                            <ObjectiveIcon size="xxxl" custom-class="text-gray-400" />
                        </div>
                        <h3 class="text-2xl font-bold text-gray-800 mb-4">No Objectives Available</h3>
                        <p class="text-gray-600 mb-6 leading-relaxed">You need to create objectives first before you can manage exit controls</p>
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
import ObjectiveSelector from '@/components/features/riskControl/objective/components/ObjectiveSelector.vue';
import ObjectiveIcon from '@/components/common/icons/ObjectiveIcon.vue';
import PlusIcon from '@/components/common/icons/PlusIcon.vue';
import EditIcon from '@/components/common/icons/EditIcon.vue';
import DeleteIcon from '@/components/common/icons/DeleteIcon.vue';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';
import CheckIcon from '@/components/common/icons/CheckIcon.vue';
import SelectIcon from '@/components/common/icons/SelectIcon.vue';
import ExitControlHeader from './ExitControlHeader.vue';

const props = defineProps({
    objectives: {
        type: Array,
        default: () => []
    },
    exitControls: {
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
    'add-exit-control',
    'edit-exit-control',
    'delete-exit-control',
    'exit-control-selected'
]);

// Exit control selection state
const selectedExitControlId = ref(null);

// Handle exit control selection from table row or button
const handleSelectExitControl = (exitControl) => {
    selectedExitControlId.value = exitControl.id;
    emit('exit-control-selected', exitControl); 
};


const exitControlColumns = [
    { key: 'description', label: 'Description' },
    { key: 'inadequate', label: 'Inadequate' },
    { key: 'createdAt', label: 'Created Date', type: 'date' }
];

const enhancedColumns = computed(() => {
    return [...exitControlColumns, { key: 'actions', label: 'Actions' }];
});

const noDataMessage = computed(() => {
    if (props.searchQuery) {
        return `No Exit Controls found for "${props.searchQuery}".`;
    }
    return 'No Exit Controls found for this Objective.';
});

const inadequateColorMap = {
  true: 'bg-red-100 text-red-800',     // Bad → Inadequate
  false: 'bg-green-100 text-green-800' // Good → Adequate
};

const inadequateLabelMap = {
  true: 'Inadequate',
  false: 'Adequate'
};


</script>

<style scoped>
.exit-control-table .table-row {
    transition: all 0.2s ease;
    cursor: pointer;
}

.exit-control-table .table-row:hover {
    background-color: #f8fafc;
    transform: translateY(-1px);
    box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.exit-control-table .table-row.selected {
    background-color: #e6f7ff;
    border-left: 4px solid #3b82f6;
}
</style>