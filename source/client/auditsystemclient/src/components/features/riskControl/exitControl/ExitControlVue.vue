<template>
  <!-- ExitControl.vue -->
    <div class="space-y-6">
        <!-- Add Exit Control Modal -->
        <AddExitControlModal
            :is-visible="showAddExitControlModal"
            :risk-id="internalSelectedRiskId"
            @close="handleCloseAddExitControlModal"
            @exit-control-added="handleExitControlAdded"
        />

        <!-- Show Exit Control Table only when risks exist -->
        <ExitControlTable
            v-if="props.risks && props.risks.length > 0"
            :risks="props.risks"
            :exit-controls="exitControls"
            :is-loading="isLoading"
            :selected-risk-id="internalSelectedRiskId"
            :selected-risk="selectedRisk"
            :current-risk="currentRisk"
            :is-risk-loading="isRiskLoading"
            :search-query="searchQuery"
            :handle-risk-change="handleExitControlChange"
            :handle-search="handleSearch"
            @add-ExitControl="handleAddExitControl"
            @edit-ExitControl="handleEditExitControl"
            @delete-ExitControl="handleDeleteExitControl"
            @exit-control-selected="handleExitControlSelected"
        />

        <!-- Show big Risk icon when no Risk exist -->
        <div v-else class="text-center py-20">
            <div class="max-w-lg mx-auto">
                <div class="bg-gradient-to-br from-primary-soft to-primary-tint rounded-full w-32 h-32
                    mx-auto mb-8 flex items-center justify-center shadow-lg border-2 border-dashed border-primary-base">
                    <RiskIcon size="xxxl" custom-class="text-primary-shade" />
                </div>
                <h3 class="text-2xl font-bold text-secondary-shade mb-4">No Risks Available</h3>
                <p class="text-gray-base mb-6 text-sm leading-relaxed">You need to create risks first before you can manage Exit Controls</p>
                <div class="bg-white border border-tertiary-shade rounded-lg p-6">
                    <div class="flex items-start space-x-3">
                        <div class="flex-shrink-0">
                            <svg class="w-6 h-6 text-tertiary-shade" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 
                                    4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 0L3.732 16.5c-.77.833.192 2.5 1.732 2.5z">
                                </path>
                            </svg>
                        </div>
                        <div class="text-left">
                            <h4 class="text-sm font-medium text-tertiary-shade mb-2">Getting Started</h4>
                            <ol class="text-sm text-gray-base space-y-1 list-decimal list-inside">
                                <li>Navigate to the Risk Control section</li>
                                <li>Create or select a risk </li>
                                <li>Return here to manage Exit Controls</li>
                            </ol>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Edit Exit Control Modal -->
        <EditExitControlModal
            v-if="showEditExitControlModal"
            :is-visible="showEditExitControlModal"
            :exit-control="exitControlToEdit"
            @close="handleCloseEditExitControlModal"
            @exit-control-updated="handleExitControlUpdated"
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
import { onMounted, watch} from 'vue';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import ExitControlTable from './components/ExitControlTable.vue';
import AddExitControlModal from './components/AddExitControlModal.vue';
import EditExitControlModal from './components/EditExitControlModal.vue';
import { useExitControlTable } from './composables/useExitControlTable';
import { useExitControlModals } from './composables/useExitControlModals';
import RiskIcon from '@/components/common/icons/RiskIcon.vue';

const props = defineProps({
    risks: {
        type: Array,
        default: () => [],
    },
    selectedRiskId: {
        type: [String, Number],
        default: null
    }
});

// Define emit first
const emit = defineEmits([
    'exit-control-selected',
    'exit-control-added',
    'exit-control-updated',
    'exit-control-deleted'
]);

// Use composables - now with integrated backend search
const {
    exitControls,
    isLoading,
    selectedRiskId : internalSelectedRiskId,
    selectedRisk,
    currentRisk,
    isRiskLoading,
    searchQuery,
    handleExitControlChange,
    handleSearch,
    resetToDefaultView,
} = useExitControlTable(props.risks);

// Watch for prop changes and update internal state
watch( () => props.selectedRiskId, (newRiskId) => {
        if (newRiskId !== internalSelectedRiskId.value) {
             handleExitControlChange(newRiskId);
        }
    },
    { immediate: true }
);

// Use modal composable for dialog management
const {
    showAddExitControlModal,
    showEditExitControlModal,
    exitControlToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddExitControl,
    handleEditExitControl,
    handleExitControlAdded,
    handleExitControlUpdated,
    confirmDelete,
    handleCloseAddExitControlModal,
    handleCloseEditExitControlModal,
    handleDeleteExitControl,
    cancelDelete
} = useExitControlModals(exitControls, emit);


// Handle ExitControl selection and forward to parent
const handleExitControlSelected = (exitControl) => {
    emit('exit-control-selected', exitControl);
};


defineExpose({
    exitControls,
    selectedRiskId: internalSelectedRiskId,
    resetToDefaultView
});
</script>

<style scoped>
.exit-control-table .table-row {
    transition: all 0.2s ease;
}
</style>
