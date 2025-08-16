import { ref } from 'vue';
import { useDeleteObjectiveById } from '@/composables/riskControl/objective/useDeleteObjectiveById';

export function useObjectiveModals(refreshData, showNotification) {

    const { deleteObjective } = useDeleteObjectiveById();

    const showAddObjectiveModal = ref(false);
    const showEditObjectiveModal = ref(false);
    const objectiveToEdit = ref(null);
    const showDeleteDialog = ref(false);
    const deleteDialogTitle = ref('');
    const deleteDialogMessage = ref('');
    const itemToDelete = ref(null);

    const handleAddObjective = () => {
        showAddObjectiveModal.value = true;
    };

    const handleCloseAddObjectiveModal = () => {
        showAddObjectiveModal.value = false;
    };

    const handleObjectiveAdded = async () => {
        try {
            await refreshData();
            handleCloseAddObjectiveModal();
        } catch (error) {
            console.error('Error in handleObjectiveAdded:', error);
            showNotification('Failed to refresh data after adding objective', 'error');
        }
    };

    const handleEditObjective = (objective) => {
        objectiveToEdit.value = objective;
        showEditObjectiveModal.value = true;
    };

    const handleCloseEditObjectiveModal = () => {
        showEditObjectiveModal.value = false;
        objectiveToEdit.value = null;
    };

    const handleObjectiveUpdated = async () => {
        // Store the description objective name before clearing the reference
        const description = objectiveToEdit.value?.description || 'Unknown Objective';
        await refreshData();
        showNotification(`Objective "${description}" has been updated successfully!`, 'success');
        handleCloseEditObjectiveModal();
    };

    const handleDeleteObjective = (objective) => {
        itemToDelete.value = objective;
        deleteDialogTitle.value = 'Delete Objective';
        deleteDialogMessage.value = `Are you sure you want to delete the objective "${objective.description}"?`;
        showDeleteDialog.value = true;
    };

    const confirmDelete = async () => {
        if (itemToDelete.value) {
            const result = await deleteObjective(itemToDelete.value.id);
            if (result.success) {
                showNotification(`Objective "${itemToDelete.value.description}" has been deleted successfully!`, 'success');
                localStorage.removeItem('riskControlSelectedObjectiveId');
                setTimeout(async () => {
                    await refreshData();
                }, 100);
            }
            cancelDelete();
        }
    };

    const cancelDelete = () => {
        showDeleteDialog.value = false;
        itemToDelete.value = null;
        deleteDialogTitle.value = '';
        deleteDialogMessage.value = '';
    };

    return {
        showAddObjectiveModal,
        showEditObjectiveModal,
        objectiveToEdit,
        showDeleteDialog,
        deleteDialogTitle,
        deleteDialogMessage,
        itemToDelete,
        handleAddObjective,
        handleCloseAddObjectiveModal,
        handleObjectiveAdded,
        handleEditObjective,
        handleCloseEditObjectiveModal,
        handleObjectiveUpdated,
        handleDeleteObjective,
        confirmDelete,
        cancelDelete,
    };
}
