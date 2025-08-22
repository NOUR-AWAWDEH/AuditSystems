<template>
  <!-- Objective.vue -->
    <div class="space-y-6">
        <!-- Add Objective Modal -->
        <AddObjectiveModal
            :is-visible="showAddObjectiveModal"
            :risk-control-matrix-id="internalSelectedRiskControlMatrixId"
            @close="handleCloseAddObjectiveModal"
            @objective-added="handleObjectiveAdded"
        />

        <!-- Show ObjectiveTable only when risk control matrices exist -->
        <ObjectiveTable
            v-if="props.riskControlMatrices && props.riskControlMatrices.length > 0"
            :risk-control-matrices="props.riskControlMatrices"
            :objectives="objectives"
            :is-loading="isLoading"
            :selected-risk-control-matrix-id="internalSelectedRiskControlMatrixId"
            :selected-risk-control-matrix="selectedRiskControlMatrix"
            :current-risk-control-matrix="currentRiskControlMatrix"
            :is-risk-control-matrix-loading="isRiskControlMatrixLoading"
            :search-query="searchQuery"
            :handle-risk-control-matrix-change="handleObjectiveChange"
            :handle-search="handleSearch"
            :refresh-data="refreshData"
            @add-objective="handleAddObjective"
            @edit-objective="handleEditObjective"
            @delete-objective="handleDeleteObjective"
            @objective-selected="handleObjectiveSelected"
        />

        <!-- Show big RiskControlMatrix icon when no Risk Control Matrix  exist -->
        <div v-else class="text-center py-20">
            <div class="max-w-lg mx-auto">
                <div class="bg-gradient-to-br from-primary-soft to-primary-tint rounded-full w-32 h-32 mx-auto mb-8 flex items-center justify-center shadow-lg border-2 border-dashed border-primary-base">
                    <RiskControlMatrixIcon size="xxxl" custom-class="text-primary-shade" />
                </div>
                <h3 class="text-2xl font-bold text-secondary-shade mb-4">No Risk Control Matrices Available</h3>
                <p class="text-gray-base mb-6 text-sm leading-relaxed">You need to create risk control matrices first before you can manage objectives</p>
                <div class="bg-white border border-tertiary-shade rounded-lg p-6">
                    <div class="flex items-start space-x-3">
                        <div class="flex-shrink-0">
                            <svg class="w-6 h-6 text-tertiary-shade" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 0L3.732 16.5c-.77.833.192 2.5 1.732 2.5z"></path>
                            </svg>
                        </div>
                        <div class="text-left">
                            <h4 class="text-sm font-medium text-tertiary-shade mb-2">Getting Started</h4>
                            <ol class="text-sm text-gray-base space-y-1 list-decimal list-inside">
                                <li>Navigate to the Risk Control section</li>
                                <li>Create or select a risk control matrix</li>
                                <li>Return here to manage objectives</li>
                            </ol>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Edit Objective Modal -->
        <EditObjectiveModal
            v-if="showEditObjectiveModal"
            :is-visible="showEditObjectiveModal"
            :objective="objectiveToEdit"
            @close="handleCloseEditObjectiveModal"
            @updated="handleObjectiveUpdated"
        />

        <!-- Confirmation Dialog -->
        <ConfirmDialog
            v-model="showDeleteDialog"
            :title="deleteDialogTitle"
            :message="deleteDialogMessage"
            :highlight-text="itemToDelete?.description || ''"
            :confirm-text="'Delete'"
            :cancel-text="'Cancel'"
            @confirm="confirmDelete"
            @cancel="cancelDelete"
        />
    </div>
</template>

<script setup>
import { watch, onMounted, ref  } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import ObjectiveTable from './components/ObjectiveTable.vue';
import AddObjectiveModal from './components/AddObjectiveModal.vue';
import EditObjectiveModal from './components/EditObjectiveModal.vue';
import { useObjectiveTable } from './composables/useObjectiveTable';
import { useObjectiveModals } from './composables/useObjectiveModals';
import RiskControlMatrixIcon from '@/components/common/icons/RiskControlMatrixIcon.vue';

const props = defineProps({
    riskControlMatrices: {
        type: Array,
        default: () => [],
    },
    selectedRiskControlMatrixId: {
        type: [String, Number],
        default: null
    }
});



// Define emit first
const emit = defineEmits(['objective-selected', 'objective-added']);


const { showNotification } = useNotification();

// Use composables - now with integrated backend search

const {
    objectives,
    isLoading,
    selectedRiskControlMatrixId : internalSelectedRiskControlMatrixId,
    selectedRiskControlMatrix,
    currentRiskControlMatrix,
    isRiskControlMatrixLoading,
    searchQuery,
    refreshData,
    handleObjectiveChange,
    handleSearch,
    resetToDefaultView,
} = useObjectiveTable(props.riskControlMatrices);

// Local ref to manage selectedRiskControlMatrixId
const localSelectedRiskControlMatrixId = ref(props.selectedRiskControlMatrixId);

// Add onMounted to refresh data on page load
onMounted(async () => {
    if (localSelectedRiskControlMatrixId.value) {
        await refreshData();
    }
});

// Watch for prop changes and update internal state
watch(
    () => props.selectedRiskControlMatrixId,
    async (newRiskControlMatrixId) => {
        localSelectedRiskControlMatrixId.value = newRiskControlMatrixId;
        if (newRiskControlMatrixId !== internalSelectedRiskControlMatrixId.value) {
            await handleObjectiveChange(newRiskControlMatrixId);
        }
    },
    { immediate: true }
);

// Watch for RiskControlMatrices prop changes
watch(
    () => props.riskControlMatrices,
    async (newRiskControlMatrices) => {
        if (newRiskControlMatrices.length > 0 && localSelectedRiskControlMatrixId.value) {
            await refreshData();
        }
    },
    { immediate: true, deep: true }
);

// Use modal composable for dialog management
const {
    showAddObjectiveModal,
    showEditObjectiveModal,
    objectiveToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddObjective,
    handleCloseAddObjectiveModal,
    handleEditObjective,
    handleCloseEditObjectiveModal,
    handleObjectiveUpdated,
    handleDeleteObjective,
    confirmDelete,
    cancelDelete
} = useObjectiveModals(refreshData, showNotification);


// Handle objective selection and forward to parent
const handleObjectiveSelected = (objectiveId) => {
    emit('objective-selected', objectiveId);
};

const handleObjectiveAdded = () => {
  // refresh local table
  refreshData();
  // bubble up to parent (RiskControl.vue)
  emit('objective-added');
};

defineExpose({
    objectives,
    selectedRiskControlMatrixId: localSelectedRiskControlMatrixId,
    resetToDefaultView
});
</script>

<style scoped>
.objective-table .table-row {
    transition: all 0.2s ease;
}
</style>
