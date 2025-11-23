
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
            @add-objective="handleAddObjective"
            @edit-objective="handleEditObjective"
            @delete-objective="handleDeleteObjective"
            @objective-selected="handleObjectiveSelected"
            @add-risks="handleAddRisks"
        />

        <!-- Show big RiskControlMatrix icon when no Risk Control Matrix  exist -->

        <NoRiskControlMatricesExist v-else />

        <!-- Edit Objective Modal -->
        <EditObjectiveModal
            v-if="showEditObjectiveModal"
            :is-visible="showEditObjectiveModal"
            :objective="objectiveToEdit"
            @close="handleCloseEditObjectiveModal"
            @objective-updated="handleObjectiveUpdated"
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
import { watch, computed } from 'vue';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import ObjectiveTable from './components/ObjectiveTable.vue';
import AddObjectiveModal from './components/AddObjectiveModal.vue';
import EditObjectiveModal from './components/EditObjectiveModal.vue';
import { useObjectiveTable } from './composables/useObjectiveTable';
import { useObjectiveModals } from './composables/useObjectiveModals';
import NoRiskControlMatricesExist from './components/NoRiskControlMatricesExist.vue';

const props = defineProps({
    riskControlMatrices: {
        type: Array,
        default: () => [],
    },
    selectedRiskControlMatrixId: {
        type: [String, Number],
        default: null
    },
    selectedObjectiveId: {
        type: [String, Number],
        default: null
    }

});

// Define emit first
const emit = defineEmits([
    'objective-selected',
    'objective-added',
    'objective-updated',
    'objective-deleted',
    'add-risks',
    'update:selectedObjectiveId'
]);

// ✅ Add internal selected objective ID for v-model
const internalSelectedObjectiveId = computed({
    get: () => props.selectedObjectiveId,
    set: (value) => emit('update:selectedObjectiveId', value)
});


// Use composables - now with integrated backend search
const {
    objectives,
    isLoading,
    selectedRiskControlMatrixId : internalSelectedRiskControlMatrixId,
    selectedRiskControlMatrix,
    currentRiskControlMatrix,
    isRiskControlMatrixLoading,
    searchQuery,
    handleObjectiveChange,
    handleSearch,
    resetToDefaultView,
} = useObjectiveTable(props.riskControlMatrices);

// Watch for prop changes and update internal state
watch( () => props.selectedRiskControlMatrixId, (newRiskControlMatrixId) => {
        if (newRiskControlMatrixId !== internalSelectedRiskControlMatrixId.value) {
             handleObjectiveChange(newRiskControlMatrixId);
        }
    },
    { immediate: true }
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
    handleEditObjective,
    handleObjectiveAdded,
    handleObjectiveUpdated,
    confirmDelete,
    handleCloseAddObjectiveModal,
    handleCloseEditObjectiveModal,
    handleDeleteObjective,
    cancelDelete
} = useObjectiveModals(objectives, emit);


const handleObjectiveSelected = (objectiveId) => {
    internalSelectedObjectiveId.value = objectiveId;
    emit('objective-selected', objectiveId);
};

const handleAddRisks = (objective) => {
    internalSelectedObjectiveId.value = objective.id;

    emit('objective-selected', objective.id);
    emit('add-risks', objective.id);
};

defineExpose({
    objectives,
    selectedRiskControlMatrixId: internalSelectedRiskControlMatrixId,
    resetToDefaultView
});
</script>

<style scoped>
.objective-table .table-row {
    transition: all 0.2s ease;
}
</style>
