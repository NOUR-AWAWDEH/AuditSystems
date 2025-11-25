<template>
  <!--RiskVue.vue-->
  <div class="space-y-6">
    <!-- Add Risk Modal -->
    <AddRiskModal
      :is-visible="showAddRiskModal"
      :objective-id="internalSelectedObjectiveId"
      @close="handleCloseAddRiskModal"
      @risk-added="handleRiskAdded"
    />

    <!-- Show RiskTable only when objectives exist -->
    <RiskTable
      v-if="props.objectives && props.objectives.length > 0"
      :objectives="props.objectives"
      :risks="risks"
      :is-loading="isLoading"
      :selected-objective-id="internalSelectedObjectiveId"
      :selected-objective="selectedObjective"
      :current-objective="currentObjective"
      :is-objective-loading="isObjectiveLoading"
      :search-query="searchQuery"
      :handle-objective-change="handleRiskChange"
      :handle-search="handleSearch"
      @add-risk="handleAddRisk"
      @edit-risk="handleEditRisk"
      @delete-risk="handleDeleteRisk"
    />

    <!-- Show big Objective icon when no objectives exist -->
    <NoObjectiveExist v-else />

    <!-- Edit Risk Modal -->
    <EditRiskModal
      v-if="showEditRiskModal"
      :is-visible="showEditRiskModal"
      :risk="riskToEdit"
      @close="handleCloseEditRiskModal"
      @risk-updated="handleRiskUpdated"
    />

    <!-- Confirmation Dialog -->
    <ConfirmDialog
      v-model="showDeleteDialog"
      :title="deleteDialogTitle"
      :message="deleteDialogMessage"
      :highlight-text="itemToDelete?.riskName || ''"
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
import RiskTable from "./components/RiskTable.vue";
import AddRiskModal from "./components/AddRiskModal.vue";
import EditRiskModal from "./components/EditRiskModal.vue";
import { useRiskTable } from "./composables/useRiskTable";
import { useRiskModals } from "./composables/useRiskModals";
import NoObjectiveExist from "./components/NoObjectiveExist.vue";

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
const emit = defineEmits(["risk-added", "risk-updated", "risk-deleted"]);

// Use composables - now with integrated backend search
const {
  risks,
  isLoading,
  selectedObjectiveId: internalSelectedObjectiveId,
  selectedObjective,
  currentObjective,
  isObjectiveLoading,
  searchQuery,
  handleRiskChange,
  handleSearch,
  resetToDefaultView,
} = useRiskTable(props.objectives);

// Watch for prop changes and update internal state
watch(
  () => props.selectedObjectiveId,
  (newObjectiveId) => {
    if (newObjectiveId !== internalSelectedObjectiveId.value) {
      handleRiskChange(newObjectiveId);
    }
  },
  { immediate: true }
);

// Use modal composable for dialog management
const {
  showAddRiskModal,
  showEditRiskModal,
  riskToEdit,
  showDeleteDialog,
  deleteDialogTitle,
  deleteDialogMessage,
  itemToDelete,
  handleAddRisk,
  handleCloseAddRiskModal,
  handleEditRisk,
  handleCloseEditRiskModal,
  handleRiskAdded,
  handleRiskUpdated,
  handleDeleteRisk,
  confirmDelete,
  cancelDelete,
} = useRiskModals(risks, emit);

defineExpose({
  risks,
  selectedObjectiveId: internalSelectedObjectiveId,
  resetToDefaultView,
});
</script>
