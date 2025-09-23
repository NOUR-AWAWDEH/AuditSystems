<template>
  <!-- AuditProgram.vue -->
    <div class="space-y-6">
        <!-- Add AuditProgram Modal -->
        <AddAuditProgramModal
            :is-visible="showAddAuditProgramModal"
            :exit-control-id="internalSelectedExitControlId"
            @close="handleCloseAddAuditProgramModal"
            @audit-program-added="handleAuditProgramAdded"
        />

        <!-- Show Exit Control Table only when exitControls exist -->
        <AuditProgramTable
            v-if="props.exitControls && props.exitControls.length > 0"
            :exit-controls="props.exitControls"
            :audit-programs="auditPrograms"
            :is-loading="isLoading"
            :selected-exit-control-id="internalSelectedExitControlId"
            :selected-exit-control="selectedExitControl"
            :current-exit-control="currentExitControl"
            :is-exit-control-loading="isExitControlLoading"
            :search-query="searchQuery"
            :handle-exit-control-change="handleAuditProgramChange"
            :handle-search="handleSearch"
            @add-audit-program="handleAddAuditProgram"
            @edit-audit-program="handleEditAuditProgram"
            @delete-audit-program="handleDeleteAuditProgram"
            @audit-program-selected="handleAuditProgramSelected"
        />

        <!-- Show big exitControl icon when no ExitControl exist -->
        <div v-else class="text-center py-20">
            <div class="max-w-lg mx-auto">
                <div class="bg-gradient-to-br from-primary-soft to-primary-tint rounded-full w-32 h-32
                    mx-auto mb-8 flex items-center justify-center shadow-lg border-2 border-dashed border-primary-base">
                    <ExitControlIcon size="xxxl" custom-class="text-primary-shade" />
                </div>
                <h3 class="text-2xl font-bold text-secondary-shade mb-4">No Exit Controls Available</h3>
                <p class="text-gray-base mb-6 text-sm leading-relaxed">You need to create Exit Controls first before you can manage Exit Controls</p>
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
                                <li>Navigate to the Exit Control section</li>
                                <li>Create or select an Exit Control </li>
                                <li>Return here to manage Exit Controls</li>
                            </ol>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Edit AuditProgram Modal -->
        <EditAuditProgramModal
            v-if="showEditAuditProgramModal"
            :is-visible="showEditAuditProgramModal"
            :audit-program="auditProgramToEdit"
            @close="handleCloseEditAuditProgramModal"
            @audit-program-updated="handleAuditProgramUpdated"
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
import {onMounted, watch} from 'vue';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import AuditProgramTable from './components/AuditProgramTable.vue';
import AddAuditProgramModal from './components/AddAuditProgramModal.vue';
import EditAuditProgramModal from './components/EditAuditProgramModal.vue';
import { useAuditProgramTable } from './composables/useAuditProgramTable';
import { useAuditProgramModals } from './composables/useAuditProgramModals';
import ExitControlIcon from '@/components/common/icons/RiskIcon.vue';

const props = defineProps({
    exitControls: {
        type: Array,
        default: () => [],
    },
    selectedExitControlId: {
        type: [String, Number],
        default: null
    }
});

// Define emit first
const emit = defineEmits([
    'audit-program-selected',
    'audit-program-added',
    'audit-program-updated',
    'audit-program-deleted'
]);

// Use composables - now with integrated backend search
const {
    auditPrograms,
    isLoading,
    selectedExitControlId : internalSelectedExitControlId,
    selectedExitControl,
    currentExitControl,
    isExitControlLoading,
    searchQuery,
    handleAuditProgramChange,
    handleSearch,
    resetToDefaultView,
} = useAuditProgramTable(props.exitControls);

// Watch for prop changes and update internal state
watch( () => props.selectedExitControlId, (newExitControlId) => {
        if (newExitControlId !== internalSelectedExitControlId.value) {
            handleAuditProgramChange(newExitControlId);
        }
    },
    { immediate: true }
);

// Use modal composable for dialog management
const {
    showAddAuditProgramModal,
    showEditAuditProgramModal,
    auditProgramToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddAuditProgram,
    handleEditAuditProgram,
    handleAuditProgramAdded,
    handleAuditProgramUpdated,
    confirmDelete,
    handleCloseAddAuditProgramModal,
    handleCloseEditAuditProgramModal,
    handleDeleteAuditProgram,
    cancelDelete
} = useAuditProgramModals(auditPrograms, emit);


// Handle AuditProgram selection and forward to parent
const handleAuditProgramSelected = (auditProgramId) => {
    emit('audit-program-selected', auditProgramId);
};


defineExpose({
    auditPrograms,
    selectedExitControlId: internalSelectedExitControlId,
    resetToDefaultView
});
</script>

<style scoped>
.audit-program-table .table-row {
    transition: all 0.2s ease;
}
</style>