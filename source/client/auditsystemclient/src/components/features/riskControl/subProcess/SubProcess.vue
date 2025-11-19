<template>
    <!--SubProcess.vue-->
  <div class="space-y-6">
    <!-- Add SubProcess Modal -->
    <AddSubProcessModal
      :is-visible="showAddSubProcessModal"
      :audit-process-id="internalSelectedAuditProcessId"
      @close="handleCloseAddSubProcessModal"
      @sub-process-added="handleSubProcessAdded"
    />

    <!-- Show SubProcessTable only when audit Processes exist -->
    <SubProcessTable
      v-if="props.auditProcesses && props.auditProcesses.length > 0"
      :audit-processes="props.auditProcesses"
      :sub-processes="subProcesses"
      :is-loading="isLoading"
      :selected-audit-process-id="internalSelectedAuditProcessId"
      :selected-audit-process="selectedAuditProcess"
      :current-audit-process="currentAuditProcess"
      :is-audit-process-loading="isAuditProcessLoading"
      :search-query="searchQuery"
      :handle-audit-process-change="handleSubProcessChange"
      :handle-search="handleSearch"
      @add-sub-process="handleAddSubProcess"
      @edit-sub-process="handleEditSubProcess"
      @delete-sub-process="handleDeleteSubProcess"
      @sub-process-selected="handleSubProcessSelected"
    />


    <!-- Show big Process icon when no Audit-processes exist -->
    <NoAuditProcessExist v-else />
   
    <!-- Edit Sub Process Modal -->
    <EditSubProcessModal
      v-if="showEditSubProcessModal"
      :is-visible="showEditSubProcessModal"
      :sub-process="subProcessToEdit"
      @close="handleCloseEditSubProcessModal"
      @sub-process-updated="handleSubProcessUpdated"
    />

    <!-- Confirmation Dialog -->
    <ConfirmDialog
      v-model="showDeleteDialog"
      :title="deleteDialogTitle"
      :message="deleteDialogMessage"
      :highlight-text="itemToDelete?.particular || ''"
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
import SubProcessTable from './components/SubProcessTable.vue';
import AddSubProcessModal from './components/AddSubProcessModal.vue';
import EditSubProcessModal from './components/EditSubProcessModal.vue';
import { useSubProcessTable } from './composables/useSubProcessTable';
import { useSubProcessModals } from './composables/useSubProcessModals';
import NoAuditProcessExist from './components/NoAuditProcessExist.vue';const props = defineProps({
  auditProcesses: {
    type: Array,
    default: () => [],
  },
  selectedAuditProcessId: {
    type: [String, Number],
    default: null,
  },
});

const emit = defineEmits([
    'sub-process-selected',
    'sub-process-added',
    'sub-process-updated',
    'sub-process-deleted'
]);


// Use composables -now with integrated backend search
const {
  subProcesses,
  isLoading,
  selectedAuditProcessId: internalSelectedAuditProcessId,
  selectedAuditProcess,
  currentAuditProcess,
  isAuditProcessLoading,
  searchQuery,
  handleSubProcessChange,
  handleSearch,
  resetToDefaultView,
} = useSubProcessTable(props.auditProcesses);


// Watch for prop changes and update internal state
watch(() => props.selectedAuditProcessId, (newAuditProcessId) => {
    if (newAuditProcessId !== internalSelectedAuditProcessId.value) {
       handleSubProcessChange(newAuditProcessId);
    }
  },
  { immediate: true },
);

// Use modal composable for dialog management
const {
  showAddSubProcessModal,
  showEditSubProcessModal,
  subProcessToEdit,
  showDeleteDialog,
  deleteDialogTitle,
  deleteDialogMessage,
  itemToDelete,
  handleAddSubProcess,
  handleCloseAddSubProcessModal,
  handleEditSubProcess,
  handleCloseEditSubProcessModal,
  handleSubProcessAdded,
  handleSubProcessUpdated,
  handleDeleteSubProcess,
  confirmDelete,
  cancelDelete,
} = useSubProcessModals(subProcesses, emit);

// Handle sub-process selection and forward to parent
const handleSubProcessSelected = (subProcessId) => {
  emit('sub-process-selected', subProcessId);
};

defineExpose({
  subProcesses,
  selectedAuditProcessId: internalSelectedAuditProcessId,
  resetToDefaultView,
});
</script>

<style scoped>
.sub-process-table .table-row {
  transition: all 0.2s ease;
}
</style>
