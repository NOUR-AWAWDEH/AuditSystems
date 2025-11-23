<template>
    <!--RiskControlMatrix.vue-->
    <div class="space-y-6">
        <!-- Add RiskControlMatrix Modal -->
        <AddRiskControlMatrixModal
            :is-visible="showAddRiskControlMatrixModal"
            :sub-process-id="internalSelectedSubProcessId"
            @close="handleCloseAddRiskControlMatrixModal"
            @risk-control-matrix-added="handleRiskControlMatrixAdded"
        />

        <!-- Show RiskControlMatrixTable only when sub processes exist -->
        <RiskControlMatrixTable
            v-if="props.subProcesses && props.subProcesses.length > 0"
            :sub-processes="props.subProcesses"
            :risk-control-matrices="riskControlMatrices"
            :is-loading="isLoading"
            :selected-sub-process-id="internalSelectedSubProcessId"
            :selected-sub-process="selectedSubProcess"
            :current-sub-process="currentSubProcess"
            :is-sub-process-loading="isSubProcessLoading"
            :search-query="searchQuery"
            :handle-sub-process-change="handleRiskControlMatrixChange"
            :handle-search="handleSearch"
            @add-risk-control-matrix="handleAddRiskControlMatrix"
            @edit-risk-control-matrix="handleEditRiskControlMatrix"
            @delete-risk-control-matrix="handleDeleteRiskControlMatrix"
            @risk-control-matrix-selected="handleRiskControlMatrixSelected"
        />

        <!-- Show big process icon when no sub processes exist -->
         <NoSubProcessExist v-else/>

        <!-- Edit RiskControlMatrix Modal -->
        <EditRiskControlMatrixModal
            v-if="showEditRiskControlMatrixModal"
            :is-visible="showEditRiskControlMatrixModal"
            :risk-control-matrix="riskControlMatrixToEdit"
            @close="handleCloseEditRiskControlMatrixModal"
            @risk-control-matrix-updated="handleRiskControlMatrixUpdated"
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
import { watch } from 'vue';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import RiskControlMatrixTable from './components/RiskControlMatrixTable.vue';
import AddRiskControlMatrixModal from './components/AddRiskControlMatrixModal.vue';
import EditRiskControlMatrixModal from './components/EditRiskControlMatrixModal.vue';
import { useRiskControlMatrixTable } from './composables/useRiskControlMatrixTable';
import { useRiskControlMatrixModals } from './composables/useRiskControlMatrixModals';
import NoSubProcessExist from './components/NoSubProcessExist.vue';

const props = defineProps({
    subProcesses: {
        type: Array,
        default: () => [],
    },
    selectedSubProcessId: {
        type: [String, Number],
        default: null
    },
});

// Define emit first
const emit = defineEmits([
    'risk-control-matrix-selected',
    'risk-control-matrix-added',
    'risk-control-matrix-updated',
    'risk-control-matrix-deleted'
]);

// Use composables - now with integrated backend search
const {
    riskControlMatrices,
    isLoading,
    selectedSubProcessId: internalSelectedSubProcessId,
    selectedSubProcess,
    currentSubProcess,
    isSubProcessLoading,
    searchQuery,
    handleRiskControlMatrixChange,
    handleSearch,
    resetToDefaultView,
} = useRiskControlMatrixTable(props.subProcesses);


// Watch for prop changes and update internal state
watch(() => props.selectedSubProcessId, (newSubProcessId) => {
        if (newSubProcessId !== internalSelectedSubProcessId.value) {
            handleRiskControlMatrixChange(newSubProcessId);
        }
    },
    { immediate: true }
);


// Use modal composable for dialog management
const {
    showAddRiskControlMatrixModal,
    showEditRiskControlMatrixModal,
    riskControlMatrixToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddRiskControlMatrix,
    handleCloseAddRiskControlMatrixModal,
    handleEditRiskControlMatrix,
    handleCloseEditRiskControlMatrixModal,
    handleRiskControlMatrixAdded,
    handleRiskControlMatrixUpdated,
    handleDeleteRiskControlMatrix,
    confirmDelete,
    cancelDelete
} = useRiskControlMatrixModals(riskControlMatrices, emit);

// Handle risk control matrix selection and forward to parent
const handleRiskControlMatrixSelected = (riskControlMatrixId) => {
    emit('risk-control-matrix-selected', riskControlMatrixId);
};

defineExpose({
    riskControlMatrices,
    selectedSubProcessId: internalSelectedSubProcessId,
    resetToDefaultView
});

</script>

<style scoped>
.risk-control-matrix-table .table-row {
    transition: all 0.2s ease;
}
</style>
