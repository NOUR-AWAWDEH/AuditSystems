<template>
    <!--RiskControlMatrix.vue-->
    <div class="space-y-6">
        <!-- Add RiskControlMatrix Modal -->
        <AddRiskControlMatrixModal
            :is-visible="showAddRiskControlMatrixModal"
            :audit-process-id="internalAuditProcessId"
            @close="handleCloseAddRiskControlMatrixModal"
            @risk-control-matrix-added="handleRiskControlMatrixAdded"
        />

        <!-- Show RiskControlMatrixTable only when audit processes exist -->
        <RiskControlMatrixTable
            v-if="props.auditProcessId"
            :audit-process-id="internalAuditProcessId"
            :risk-control-matrices="riskControlMatrices"
            :is-loading="isLoading"
            :search-query="searchQuery"
            :handle-audit-process-change="handleRiskControlMatrixChange"
            :handle-search="handleSearch"
            :current-audit-process="auditProcess"
            :is-audit-process-loading="isAuditProcessLoading"
            @add-risk-control-matrix="handleAddRiskControlMatrix"
            @edit-risk-control-matrix="handleEditRiskControlMatrix"
            @delete-risk-control-matrix="handleDeleteRiskControlMatrix"
            @risk-control-matrix-selected="handleRiskControlMatrixSelected"
        />

        <!-- Show big process icon when no audit process selected -->
         <NoAuditProcessExist v-else/>

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
import { watch, toRef } from 'vue';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import RiskControlMatrixTable from './components/RiskControlMatrixTable.vue';
import AddRiskControlMatrixModal from './components/AddRiskControlMatrixModal.vue';
import EditRiskControlMatrixModal from './components/EditRiskControlMatrixModal.vue';
import { useRiskControlMatrixTable } from './composables/useRiskControlMatrixTable';
import { useRiskControlMatrixModals } from './composables/useRiskControlMatrixModals';
import { useGetAuditProcessById } from '@/composables/riskControl/auditProcess/useGetAuditProcessById';
import NoAuditProcessExist from './components/NoAuditProcessExist.vue';

const props = defineProps({
    auditProcessId: {
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
    selectedAuditProcessId: internalAuditProcessId,
    searchQuery,
    handleRiskControlMatrixChange,
    handleSearch,
    resetToDefaultView,
} = useRiskControlMatrixTable(toRef(props, "auditProcessId"));

const { auditProcess, isLoading: isAuditProcessLoading, getAuditProcessById, clearAuditProcess } = useGetAuditProcessById();

watch(internalAuditProcessId, async (newVal) => {
    if (newVal) {
        await getAuditProcessById(newVal);
    } else {
        clearAuditProcess();
    }
}, { immediate: true });


// Watch for prop changes and update internal state


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
    resetToDefaultView
});

</script>

<style scoped>
.risk-control-matrix-table .table-row {
    transition: all 0.2s ease;
}
</style>
