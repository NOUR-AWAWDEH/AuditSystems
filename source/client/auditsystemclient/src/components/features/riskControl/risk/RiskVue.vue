<template>
  <!--RiskVue.vue-->
  <div class="space-y-6">
    <!-- Add Risk Modal -->
    <AddRiskModal
      :is-visible="showAddRiskModal"
      :observation-id="internalSelectedObservationId"
      @close="handleCloseAddRiskModal"
      @risk-added="handleRiskAdded"
    />

    <!-- Show RiskTable only when observations exist -->
    <RiskTable
      v-if="props.observations && props.observations.length > 0"
      :observations="props.observations"
      :risks="risks"
      :is-loading="isLoading"
      :selected-observation-id="internalSelectedObservationId"
      :selected-observation="selectedObservation"
      :current-observation="currentObservation"
      :is-observation-loading="isObservationLoading"
      :search-query="searchQuery"
      :handle-observation-change="handleRiskChange"
      :handle-search="handleSearch"
      @add-risk="handleAddRisk"
      @edit-risk="handleEditRisk"
      @delete-risk="handleDeleteRisk"
    />

    <!-- Show big Observation icon when no observations exist -->
    <NoObservationExist v-else />

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
import NoObservationExist from "./components/NoObservationExist.vue";

const props = defineProps({
  observations: {
    type: Array,
    default: () => [],
  },
  selectedObservationId: {
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
  selectedObservationId: internalSelectedObservationId,
  selectedObservation,
  currentObservation,
  isObservationLoading,
  searchQuery,
  handleRiskChange,
  handleSearch,
  resetToDefaultView,
} = useRiskTable(props.observations);

// Watch for prop changes and update internal state
watch(
  () => props.selectedObservationId,
  (newObservationId) => {
    if (newObservationId !== internalSelectedObservationId.value) {
      handleRiskChange(newObservationId);
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
  selectedObservationId: internalSelectedObservationId,
  resetToDefaultView,
});
</script>
