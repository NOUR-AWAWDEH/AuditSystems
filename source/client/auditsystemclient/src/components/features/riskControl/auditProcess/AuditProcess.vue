<template>
  <!--AuditProcess.vue-->
  <div class="space-y-6">
    <!-- Add Audit Process Modal -->
    <AddAuditProcessModal
      :is-visible="showAddAuditProcessModal"
      :sub-department-id="internalSelectedSubDepartmentId"
      @close="handleCloseAddAuditProcessModal"
      @audit-process-added="handleAuditProcessAdded"
    />

    <!-- Show AuditProcessTable only when sub-departments exist -->
    <AuditProcessTable
      v-if="props.subDepartments && props.subDepartments.length > 0"
      :sub-departments="props.subDepartments"
      :audit-processes="computedAuditProcesses"
      :is-loading="isLoading"
      :selected-sub-department-id="internalSelectedSubDepartmentId"
      :selected-sub-department="selectedSubDepartment"
      :current-sub-department="currentSubDepartment"
      :is-sub-department-loading="isSubDepartmentLoading"
      :search-query="searchQuery"
      :handle-sub-department-change="handleAuditProcessChange"
      :handle-search="handleSearch"
      @add-audit-process="handleAddAuditProcess"
      @edit-audit-process="handleEditAuditProcess"
      @delete-audit-process="handleDeleteAuditProcess"
      @audit-process-selected="handleAuditProcessSelected"
    />

    <!-- Show big department icon when no sub-departments exist -->
      <NoSubDepartmentExist v-else
      />

    <!-- Edit Audit Process Modal -->
    <EditAuditProcessModal
      v-if="showEditAuditProcessModal"
      :is-visible="showEditAuditProcessModal"
      :audit-process="auditProcessToEdit"
      @close="handleCloseEditAuditProcessModal"
      @audit-process-updated="handleAuditProcessUpdated"
    />

    <!-- Confirmation Dialog -->
    <ConfirmDialog
      v-model="showDeleteDialog"
      :title="deleteDialogTitle"
      :message="deleteDialogMessage"
      :highlight-text="itemToDelete?.processName || ''"
      :confirm-text="'Delete'"
      :cancel-text="'Cancel'"
      @confirm="confirmDelete"
      @cancel="cancelDelete"
    />
  </div>
</template>

<script setup>
import ConfirmDialog from "@/components/base/ConfirmDialog.vue";
import AuditProcessTable from "./components/AuditProcessTable.vue";
import AddAuditProcessModal from "./components/AddAuditProcessModal.vue";
import EditAuditProcessModal from "./components/EditAuditProcessModal.vue";
import { useAuditProcessTable } from "./composables/useAuditProcessTable";
import { useAuditProcessModals } from "./composables/useAuditProcessModals";
import NoSubDepartmentExist from "./components/NoSubDepartmentExist.vue";

const props = defineProps({
  subDepartments: {
    type: Array,
    default: () => [],
  },
  selectedSubDepartmentId: {
    type: [String, Number],
    default: null,
  },
});

// Define emit first
const emit = defineEmits([
  "audit-process-selected",
  "audit-process-added",
  "audit-process-updated",
  "audit-process-deleted",
]);

// Use composables - now with integrated backend search
const {
  isLoading,
  computedAuditProcesses,
  selectedSubDepartmentId: internalSelectedSubDepartmentId,
  selectedSubDepartment,
  currentSubDepartment,
  isSubDepartmentLoading,
  searchQuery,
  handleAuditProcessChange,
  handleSearch,
  resetToDefaultView,
} = useAuditProcessTable(props.subDepartments);

// Use modal composable for dialog management
const {
  showAddAuditProcessModal,
  showEditAuditProcessModal,
  auditProcessToEdit,
  showDeleteDialog,
  deleteDialogTitle,
  deleteDialogMessage,
  itemToDelete,
  handleAddAuditProcess,
  handleEditAuditProcess,
  handleDeleteAuditProcess,
  handleCloseAddAuditProcessModal,
  handleAuditProcessAdded,
  handleAuditProcessUpdated,
  confirmDelete,
  handleCloseEditAuditProcessModal,
  cancelDelete,
} = useAuditProcessModals(computedAuditProcesses, emit);

// Handle audit process selection and forward to parent
const handleAuditProcessSelected = (auditProcessId) => {
  emit("audit-process-selected", auditProcessId);
};

defineExpose({
  auditProcesses: computedAuditProcesses,
  selectedSubDepartmentId: internalSelectedSubDepartmentId,
  resetToDefaultView,
});
</script>
