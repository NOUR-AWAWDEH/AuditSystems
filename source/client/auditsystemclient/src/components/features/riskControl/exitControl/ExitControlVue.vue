<template>
  <!--ExitControlVue.vue-->
  <div class="space-y-6">
    <!-- Add Exit Control Modal -->
    <AddExitControlModal
      :is-visible="showAddExitControlModal"
      :objective-id="internalSelectedObjectiveId"
      @close="handleCloseAddExitControlModal"
      @exit-control-added="handleExitControlAdded"
    />

    <!-- Show Exit Control Table only when objectives exist -->
    <ExitControlTable
      v-if="props.objectives && props.objectives.length > 0"
      :objectives="props.objectives"
      :exit-controls="exitControls"
      :is-loading="isLoading"
      :selected-objective-id="internalSelectedObjectiveId"
      :selected-objective="selectedObjective"
      :current-objective="currentObjective"
      :is-objective-loading="isObjectiveLoading"
      :search-query="searchQuery"
      :handle-objective-change="handleExitControlChange"
      :handle-search="handleSearch"
      @add-ExitControl="handleAddExitControl"
      @edit-ExitControl="handleEditExitControl"
      @delete-ExitControl="handleDeleteExitControl"
      @exit-control-selected="handleExitControlSelected"
    />

    <!-- Show big Objective icon when No Objective Exist -->
     <NoObjectiveExist v-else />

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
import { watch } from "vue";
import ConfirmDialog from "@/components/base/ConfirmDialog.vue";
import ExitControlTable from "./components/ExitControlTable.vue";
import AddExitControlModal from "./components/AddExitControlModal.vue";
import EditExitControlModal from "./components/EditExitControlModal.vue";
import NoObjectiveExist from "./components/NoObjectiveExist.vue";
import { useExitControlTable } from "./composables/useExitControlTable";
import { useExitControlModals } from "./composables/useExitControlModals";

const props = defineProps({
  objectives: {
    type: Array,
    default: () => [],
  },
  selectedObjectiveId: {
    type: [String, Number],
    default: null,
  },
});

// Define emit first
const emit = defineEmits([
  "exit-control-selected",
  "exit-control-added",
  "exit-control-updated",
  "exit-control-deleted",
]);

// Use composables - now with integrated backend search
const {
  exitControls,
  isLoading,
  selectedObjectiveId: internalSelectedObjectiveId,
  selectedObjective,
  currentObjective,
  isObjectiveLoading,
  searchQuery,
  handleExitControlChange,
  handleSearch,
  resetToDefaultView,
} = useExitControlTable(props.objectives);

// Watch for prop changes and update internal state
watch(
  () => props.selectedObjectiveId,
  (newObjectiveId) => {
    if (newObjectiveId !== internalSelectedObjectiveId.value) {
      handleExitControlChange(newObjectiveId);
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
  cancelDelete,
} = useExitControlModals(exitControls, emit);

// Handle ExitControl selection and forward to parent
const handleExitControlSelected = (exitControl) => {
  emit("exit-control-selected", exitControl);
};

defineExpose({
  exitControls,
  selectedObjectiveId: internalSelectedObjectiveId,
  resetToDefaultView,
});
</script>

<style scoped>
.exit-control-table .table-row {
  transition: all 0.2s ease;
}
</style>
