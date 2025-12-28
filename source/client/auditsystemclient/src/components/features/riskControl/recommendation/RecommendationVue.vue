<template>
  <!--RecommendationVue.vue-->
  <div class="space-y-6">
    <!-- Add Recommendation Modal -->
    <AddRecommendationModal
      :is-visible="showAddRecommendationModal"
      :observation-id="internalSelectedObservationId"
      @close="handleCloseAddRecommendationModal"
      @recommendation-added="handleRecommendationAdded"
    />

    <!-- Show RecommendationTable only when observations exist -->
    <RecommendationTable
      v-if="props.observations && props.observations.length > 0"
      :observations="props.observations"
      :recommendations="recommendations"
      :is-loading="isLoading"
      :selected-observation-id="internalSelectedObservationId"
      :selected-observation="selectedObservation"
      :current-observation="currentObservation"
      :is-observation-loading="isObservationLoading"
      :search-query="searchQuery"
      :handle-observation-change="handleRecommendationChange"
      :handle-search="handleSearch"
      @add-recommendation="handleAddRecommendation"
      @edit-recommendation="handleEditRecommendation"
      @delete-recommendation="handleDeleteRecommendation"
    />

    <!-- Show big Observation icon when no observations exist -->
    <NoObservationExist v-else />

    <!-- Edit Recommendation Modal -->
    <EditRecommendationModal
      v-if="showEditRecommendationModal"
      :is-visible="showEditRecommendationModal"
      :recommendation="recommendationToEdit"
      @close="handleCloseEditRecommendationModal"
      @recommendation-updated="handleRecommendationUpdated"
    />

    <!-- Confirmation Dialog -->
    <ConfirmDialog
      v-model="showDeleteDialog"
      :title="deleteDialogTitle"
      :message="deleteDialogMessage"
      :highlight-text="itemToDelete?.title || ''"
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
import RecommendationTable from "./components/RecommendationTable.vue";
import AddRecommendationModal from "./components/AddRecommendationModal.vue";
import EditRecommendationModal from "./components/EditRecommendationModal.vue";
import { useRecommendationTable } from "./composables/useRecommendationTable";
import { useRecommendationModals } from "./composables/useRecommendationModals";
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
const emit = defineEmits(["recommendation-added", "recommendation-updated", "recommendation-deleted"]);

// Use composables - now with integrated backend search
const {
  recommendations,
  isLoading,
  selectedObservationId: internalSelectedObservationId,
  selectedObservation,
  currentObservation,
  isObservationLoading,
  searchQuery,
  handleRecommendationChange,
  handleSearch,
  resetToDefaultView,
} = useRecommendationTable(props.observations);

// Watch for prop changes and update internal state
watch(
  () => props.selectedObservationId,
  (newObservationId) => {
    if (newObservationId !== internalSelectedObservationId.value) {
      handleRecommendationChange(newObservationId);
    }
  },
  { immediate: true }
);

// Use modal composable for dialog management
const {
  showAddRecommendationModal,
  showEditRecommendationModal,
  recommendationToEdit,
  showDeleteDialog,
  deleteDialogTitle,
  deleteDialogMessage,
  itemToDelete,
  handleAddRecommendation,
  handleCloseAddRecommendationModal,
  handleEditRecommendation,
  handleCloseEditRecommendationModal,
  handleRecommendationAdded,
  handleRecommendationUpdated,
  handleDeleteRecommendation,
  confirmDelete,
  cancelDelete,
} = useRecommendationModals(recommendations, emit);

defineExpose({
  recommendations,
  selectedObservationId: internalSelectedObservationId,
  resetToDefaultView,
});
</script>
