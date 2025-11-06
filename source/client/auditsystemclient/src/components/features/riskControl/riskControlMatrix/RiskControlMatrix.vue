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
        <div v-else class="text-center py-20">
            <div class="max-w-lg mx-auto">
                <div class="bg-gradient-to-br from-primary-soft to-primary-tint rounded-full w-32 h-32 mx-auto mb-8 flex items-center justify-center shadow-lg border-2 border-dashed border-primary-base">
                    <ProcessIcon size="xxxl" custom-class="text-primary-shade" />
                </div>
                <h3 class="text-2xl font-bold text-secondary-shade mb-4">No Sub Processes Available</h3>
                <p class="text-gray-base mb-6 text-sm leading-relaxed">You need to create sub processes first before you can manage risk control matrices</p>
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
                                <li>Create or select a sub process</li>
                                <li>Return here to manage risk control matrices</li>
                            </ol>
                        </div>
                    </div>
                </div>
            </div>
        </div>
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
import ProcessIcon from '@/components/common/icons/ProcessIcon.vue';

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
