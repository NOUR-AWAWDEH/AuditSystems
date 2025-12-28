<template>
  <!--ObservationVue.vue-->
  <div class="space-y-6">
    <!-- Add Observation Modal -->
    <AddObservationModal
      :is-visible="showAddObservationModal"
      :audit-program-id="internalSelectedAuditProgramId"
      @close="handleCloseAddObservationModal"
      @observation-added="handleObservationAdded"
    />

    <!-- Show ObservationTable only when audit Programs exist -->
    <ObservationTable
      v-if="props.auditPrograms && props.auditPrograms.length > 0"
      :audit-programs="props.auditPrograms"
      :observations="observations"
      :is-loading="isLoading"
      :selected-audit-program-id="internalSelectedAuditProgramId"
      :selected-audit-program="selectedAuditProgram"
      :current-audit-program="currentAuditProgram"
      :is-observation-loading="isObservationLoading"
      :search-query="searchQuery"
      :handle-audit-program-change="handleObservationChange"
      :handle-search="handleSearch"
      @add-observation="handleAddObservation"
      @edit-observation="handleEditObservation"
      @delete-observation="handleDeleteObservation"
      @observation-selected="handleObservationSelected"
      @recommendation-add="handleAddRecommendations"
    />

    <!-- No Observations Message -->
    <NoObservations v-else />

    <!-- Edit Observation Modal -->
    <EditObservationModal
      v-if="showEditObservationModal"
      :is-visible="showEditObservationModal"
      :observation="observationToEdit"
      @close="handleCloseEditObservationModal"
      @observation-updated="handleObservationUpdated"
    />

    <!-- Confirmation Dialog -->
    <ConfirmDialog
      v-model="showDeleteDialog"
      :title="deleteDialogTitle"
      :message="deleteDialogMessage"
      :highlight-text="itemToDelete?.highlight || ''"
      :confirm-text="'Delete'"
      :cancel-text="'Cancel'"
      @confirm="confirmDelete"
      @cancel="cancelDelete"
    />
  </div>
</template>

<script setup>
import { watch, computed } from "vue";
import ConfirmDialog from "@/components/base/ConfirmDialog.vue";
import ObservationTable from "./components/ObservationTable.vue";
import AddObservationModal from "./components/AddObservationModal.vue";
import EditObservationModal from "./components/EditObservationModal.vue";
import { useObservationTable } from "./composables/useObservationTable";
import { useObservationModals } from "./composables/useObservationModals";
import NoObservations from "./components/NoObservations.vue";

const props = defineProps({
  auditPrograms: {
    type: Array,
    default: () => [],
  },
  selectedAuditProgramId: {
    type: [String, Number],
    default: null,
  },
    selectedObservationId: {
    type: [String, Number],
    default: null,
  },
});

const emit = defineEmits([
  "observation-selected",
  "observation-added",
  "observation-updated",
  "observation-deleted",
  "recommendation-add",
  "update:selectedObsrvationId",
]);

const internalSelectedObservationId = computed({
  get: () => props.selectedObservationId,
  set: (value) => emit("update:selectedObservationId", value)
});

// Use composables - now with integrated backend search
const {
  observations,
  isLoading,
  selectedAuditProgramId: internalSelectedAuditProgramId,
  selectedAuditProgram,
  currentAuditProgram,
  isObservationLoading,
  searchQuery,
  handleObservationChange,
  handleSearch,
  resetToDefaultView,
} = useObservationTable(props.auditPrograms);

// Watch for prop changes and update internal state
watch(
  () => props.selectedAuditProgramId,
  (newAuditProgramId) => {
    if (newAuditProgramId !== internalSelectedAuditProgramId.value) {
      handleObservationChange(newAuditProgramId);
    }
  },
  { immediate: true }
);

// Use modal composable for dialog management
const {
  showAddObservationModal,
  showEditObservationModal,
  observationToEdit,
  showDeleteDialog,
  deleteDialogTitle,
  deleteDialogMessage,
  itemToDelete,
  handleAddObservation,
  handleCloseAddObservationModal,
  handleEditObservation,
  handleCloseEditObservationModal,
  handleObservationAdded,
  handleObservationUpdated,
  handleDeleteObservation,
  confirmDelete,
  cancelDelete,
} = useObservationModals(observations, emit);

// Handle observation selection and forward to parent
const handleObservationSelected = (observationId) => {
  internalSelectedObservationId.value = observationId;
  emit("observation-selected", observationId);
};

const handleAddRecommendations = (observation) => {
  internalSelectedObservationId.value = observation.id;
  emit("observation-selected", observation.id);
  emit("recommendation-add", observation.id);
};

defineExpose({
  observations,
  selectedAuditProgramId: internalSelectedAuditProgramId,
  resetToDefaultView,
  selectedObservationId: internalSelectedObservationId,
});
</script>