<template>
  <!--ControlDesignVue.vue-->
  <div class="space-y-6">
    <!-- Add Control Design Modal -->
    <AddControlDesignModal
      :is-visible="showAddControlDesignModal"
      :exit-control-id="internalSelectedExitControlId"
      @close="handleCloseAddControlDesignModal"
      @control-design-added="handleControlDesignAdded"
    />

    <!-- Show Control Design Table only when exit controls exist -->
    <ControlDesignTable
      v-if="filteredExitControls && filteredExitControls.length > 0"
      :exit-controls="filteredExitControls"
      :control-designs="controlDesigns"
      :is-loading="isLoading"
      :selected-exit-control-id="internalSelectedExitControlId"
      :selected-exit-control="selectedExitControl"
      :current-exit-control="currentExitControl"
      :is-exit-control-loading="isExitControlLoading"
      :search-query="searchQuery"
      :handle-exit-control-change="handleControlDesignChange"
      :handle-search="handleSearch"
      @add-control-design="handleAddControlDesign"
      @edit-control-design="handleEditControlDesign"
      @delete-control-design="handleDeleteControlDesign"
      @control-design-selected="handleControlDesignSelected"
    />

    <!-- Empty state  No Exit Control Exist-->
    <NoExitControlExist v-else />

    <!-- Edit Control Design Modal -->
    <EditControlDesignModal
      v-if="showEditControlDesignModal"
      :is-visible="showEditControlDesignModal"
      :control-design="controlDesignToEdit"
      @close="handleCloseEditControlDesignModal"
      @control-design-updated="handleControlDesignUpdated"
    />

    <!-- Confirmation Dialog -->
    <ConfirmDialog
      v-model="showDeleteDialog"
      :title="deleteDialogTitle"
      :message="deleteDialogMessage"
      :highlight-text="itemToDelete?.description || ''"
      confirm-text="Delete"
      cancel-text="Cancel"
      @confirm="confirmDelete"
      @cancel="cancelDelete"
    />
  </div>
</template>

<script setup>
import { watch, computed } from "vue";
import ConfirmDialog from "@/components/base/ConfirmDialog.vue";
import ControlDesignTable from "./components/ControlDesignTable.vue";
import AddControlDesignModal from "./components/AddControlDesignModal.vue";
import EditControlDesignModal from "./components/EditControlDesignModal.vue";
import NoExitControlExist from "./components/NoExitControlExist.vue";
import { useControlDesignTable } from "./composables/useControlDesignTable.js";
import { useControlDesignModals } from "./composables/useControlDesignModals.js";

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

const emit = defineEmits([
  "control-design-selected",
  "control-design-added",
  "control-design-updated",
  "control-design-deleted",
]);

// Computed property to filter exitControls
const filteredExitControls = computed(() => {
  return props.exitControls.filter((control) => control.inadequate);
});

// Composable
const {
  controlDesigns,
  isLoading,
  selectedExitControlId: internalSelectedExitControlId,
  selectedExitControl,
  currentExitControl,
  isExitControlLoading,
  searchQuery,
  handleControlDesignChange,
  handleSearch,
  resetToDefaultView,
} = useControlDesignTable(props.exitControls);

// ✅ Single watcher (merged both into one)
watch(
  () => props.selectedExitControlId,
  (newExitControlId) => {
    if (newExitControlId && !filteredExitControls.value.some((ec) => ec.id === newExitControlId)) {
      // Clear invalid selection
      internalSelectedExitControlId.value = "";
      localStorage.removeItem("riskControlSelectedExitControlId");
    } else {
      internalSelectedExitControlId.value = newExitControlId || "";
    }

    // If still no selection, auto-select first valid exit control
    if (!internalSelectedExitControlId.value && filteredExitControls.value.length > 0) {
      const firstId = filteredExitControls.value[0].id;
      internalSelectedExitControlId.value = firstId;
      localStorage.setItem("riskControlSelectedExitControlId", firstId);
    }

    // Always trigger change with final ID
    handleControlDesignChange(internalSelectedExitControlId.value);
  },
  { immediate: true }
);

// Modal handling
const {
  showAddControlDesignModal,
  showEditControlDesignModal,
  controlDesignToEdit,
  showDeleteDialog,
  deleteDialogTitle,
  deleteDialogMessage,
  itemToDelete,
  handleAddControlDesign,
  handleEditControlDesign,
  handleControlDesignAdded,
  handleControlDesignUpdated,
  confirmDelete,
  handleCloseAddControlDesignModal,
  handleCloseEditControlDesignModal,
  handleDeleteControlDesign,
  cancelDelete,
} = useControlDesignModals(controlDesigns, emit);

// Forward selection event
const handleControlDesignSelected = (controlDesign) => {
  emit("control-design-selected", controlDesign);
};

defineExpose({
  controlDesigns,
  selectedExitControlId: internalSelectedExitControlId,
  resetToDefaultView,
});
</script>
