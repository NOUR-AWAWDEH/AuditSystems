import { ref } from 'vue';
import { useDeleteObjectiveById } from '@/composables/riskControl/objective/useDeleteObjectiveById';
import { useNotification } from '@/composables/notification/useNotification';

export function useObjectiveModals(objectives, emit) {

    const { deleteObjective } = useDeleteObjectiveById();
    const { showNotification } = useNotification();

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

    const handleObjectiveAdded = async (newObjective) => {
        try {
            objectives.value.push(newObjective);
            emit('objective-added', newObjective);
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

    const handleObjectiveUpdated = async (updatedObjective) => {
       try {
        const index = objectives.value.findIndex(obj => obj.id === updatedObjective.id);
       
        if(index !== -1){
           objectives.value[index] = { ...objectives.value[index], ...updatedObjective };
        }
        emit('objective-updated', updatedObjective);

        showNotification(
            `Objective "${updatedObjective.description}" has been updated successfully!`,
             'success'
        );

        handleCloseEditObjectiveModal();
       }catch (error) {
            console.error('Error in handleObjectiveUpdated:', error);
            showNotification('Failed to refresh data after updating objective', 'error');
       }
    };

    const handleDeleteObjective = (objective) => {
        itemToDelete.value = objective;
        deleteDialogTitle.value = 'Delete Objective';
        deleteDialogMessage.value = `Are you sure you want to delete the objective "${objective.description}"?`;
        showDeleteDialog.value = true;
    };

    const confirmDelete = async () => {
      if (!itemToDelete.value) return;
      
        const deletedObjective = { ...itemToDelete.value };
        const result = await deleteObjective(deletedObjective.id);

        if (result.success) {
            const index = objectives.value.findIndex(obj => obj.id === deletedObjective.id);
            if (index !== -1) {
                objectives.value.splice(index, 1);
            }

            showNotification(
                `Objective "${deletedObjective.description}" has been deleted successfully!`,
                'success'
            );

            localStorage.removeItem('riskControlSelectedObjectiveId');
            
            emit('objective-deleted', deletedObjective.id);
        }

        cancelDelete();
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
