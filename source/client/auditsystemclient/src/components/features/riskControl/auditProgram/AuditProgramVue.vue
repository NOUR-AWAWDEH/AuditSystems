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
      v-if="filteredExitControls && filteredExitControls.length > 0"
      :exit-controls="filteredExitControls"
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

    <!-- Show big exitControl icon when No Exit Control Exist -->
    <NoExitControlsExist v-else />

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
import { watch, computed } from "vue"; // Added 'computed' import
import ConfirmDialog from "@/components/base/ConfirmDialog.vue";
import AuditProgramTable from "./components/AuditProgramTable.vue";
import AddAuditProgramModal from "./components/AddAuditProgramModal.vue";
import EditAuditProgramModal from "./components/EditAuditProgramModal.vue";
import NoExitControlsExist from "./components/NoExitControlsExist.vue";
import { useAuditProgramTable } from "./composables/useAuditProgramTable";
import { useAuditProgramModals } from "./composables/useAuditProgramModals";

const props = defineProps({
  exitControls: {
    type: Array,
    default: () => [],
  },
  selectedExitControlId: {
    type: [String, Number],
    default: null,
  },
});

// Define emit first
const emit = defineEmits([
  "audit-program-selected",
  "audit-program-added",
  "audit-program-updated",
  "audit-program-deleted",
]);

// Computed property to filter exitControls for display
const filteredExitControls = computed(() => {
  return props.exitControls.filter((control) => !control.inadequate);
});

// Use composables - now with integrated backend search
const {
  auditPrograms,
  isLoading,
  selectedExitControlId: internalSelectedExitControlId,
  selectedExitControl,
  currentExitControl,
  isExitControlLoading,
  searchQuery,
  handleAuditProgramChange,
  handleSearch,
  resetToDefaultView,
} = useAuditProgramTable(props.exitControls);

// Watch for prop changes and update internal state
watch(
  () => props.selectedExitControlId,
  (newExitControlId) => {
    // Ensure the newExitControlId is valid for the filtered list
    if (newExitControlId && !filteredExitControls.value.some((ec) => ec.id === newExitControlId)) {
      // If the previously selected ID is no longer in the filtered list, clear it
      internalSelectedExitControlId.value = ""; // Set to empty string
      localStorage.removeItem("riskControlSelectedExitControlId");
    } else {
      internalSelectedExitControlId.value = newExitControlId || ""; // Use new ID or empty string
    }

    // If no exit control is selected, and there are filtered exit controls, select the first one
    if (!internalSelectedExitControlId.value && filteredExitControls.value.length > 0) {
      internalSelectedExitControlId.value = filteredExitControls.value[0].id;
      localStorage.setItem("riskControlSelectedExitControlId", filteredExitControls.value[0].id);
    }
    handleAuditProgramChange(internalSelectedExitControlId.value); // Trigger search with the (potentially updated) ID
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
  cancelDelete,
} = useAuditProgramModals(auditPrograms, emit);

// Handle AuditProgram selection and forward to parent
const handleAuditProgramSelected = (auditProgramId) => {
  emit("audit-program-selected", auditProgramId);
};

defineExpose({
  auditPrograms,
  selectedExitControlId: internalSelectedExitControlId,
  resetToDefaultView,
});
</script>

<style scoped>
.audit-program-table .table-row {
  transition: all 0.2s ease;
}
</style>
