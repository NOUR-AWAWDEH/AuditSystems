import { ref } from "vue";
import { useDeleteRecommendationById } from "@/composables/riskControl/recommendation/useDeleteRecommendationById";
import { useNotification } from "@/composables/notification/useNotification";

export function useRecommendationModals(recommendations, emit) {
  const { deleteRecommendation } = useDeleteRecommendationById();
  const { showNotification } = useNotification();

  const showAddRecommendationModal = ref(false);
  const showEditRecommendationModal = ref(false);
  const recommendationToEdit = ref(null);
  const showDeleteDialog = ref(false);
  const deleteDialogTitle = ref("");
  const deleteDialogMessage = ref("");
  const itemToDelete = ref(null);

  const handleAddRecommendation = () => {
    showAddRecommendationModal.value = true;
  };

  const handleCloseAddRecommendationModal = () => {
    showAddRecommendationModal.value = false;
  };

  const handleRecommendationAdded = async (newRecommendation) => {
    try {
      recommendations.value.push(newRecommendation);
      emit("recommendation-added", newRecommendation);
      handleCloseAddRecommendationModal();
    } catch (error) {
      console.error("Error in handleRecommendationAdded:", error);
      showNotification("Failed to refresh data after adding recommendation", "error");
    }
  };

  const handleEditRecommendation = (recommendation) => {
    recommendationToEdit.value = recommendation;
    showEditRecommendationModal.value = true;
  };

  const handleCloseEditRecommendationModal = () => {
    showEditRecommendationModal.value = false;
    recommendationToEdit.value = null;
  };

  const handleRecommendationUpdated = async (updatedRecommendation) => {
    try {
      const index = recommendations.value.findIndex((r) => r.id === updatedRecommendation.id);
      if (index !== -1) {
        recommendations.value[index] = { ...recommendations.value[index], ...updatedRecommendation };
      }

      emit("recommendation-updated", updatedRecommendation);
      showNotification(`Recommendation updated successfully!`, "success");
      handleCloseEditRecommendationModal();
    } catch (error) {
      console.error("Error in handleRecommendationUpdated:", error);
      showNotification("Failed to update recommendation", "error");
    }
  };

  const handleDeleteRecommendation = (recommendation) => {
    itemToDelete.value = recommendation;
    deleteDialogTitle.value = "Delete Recommendation";
    deleteDialogMessage.value = `Are you sure you want to delete the recommendation "${recommendation.description}"?`;
    showDeleteDialog.value = true;
  };

  const confirmDelete = async () => {
    if (!itemToDelete.value) return;
    const deletedRecommendation = { ...itemToDelete.value };
    const result = await deleteRecommendation(deletedRecommendation.id);

    if (result.success) {
      const index = recommendations.value.findIndex((r) => r.id === deletedRecommendation.id);
      if (index !== -1) {
        recommendations.value.splice(index, 1);
      }

      showNotification(`Recommendation deleted successfully!`, "success");

      localStorage.removeItem("recommendationControlSelectedRecommendationId");

      emit("recommendation-deleted", deletedRecommendation.id);
    } else {
      showNotification(result.message, "error");
    }
    cancelDelete();
  };

  const cancelDelete = () => {
    showDeleteDialog.value = false;
    itemToDelete.value = null;
    deleteDialogTitle.value = "";
    deleteDialogMessage.value = "";
  };

  return {
    showAddRecommendationModal,
    showEditRecommendationModal,
    recommendationToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddRecommendation,
    handleCloseAddRecommendationModal,
    handleRecommendationAdded,
    handleEditRecommendation,
    handleCloseEditRecommendationModal,
    handleRecommendationUpdated,
    handleDeleteRecommendation,
    confirmDelete,
    cancelDelete,
  };
}
