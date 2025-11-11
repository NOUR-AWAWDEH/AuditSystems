import { ref } from 'vue';
import { useDeleteControlDesignById } from '@/composables/riskControl/controlDesign/useDeleteControlDesignById';
import { useNotification } from '@/composables/notification/useNotification';

export function useControlDesignModals(controlDesigns, emit) {

    const { deleteControlDesign } = useDeleteControlDesignById();
    const { showNotification } = useNotification();

    const showAddControlDesignModal = ref(false);
    const showEditControlDesignModal = ref(false);
    const controlDesignToEdit = ref(null);
    const showDeleteDialog = ref(false);
    const deleteDialogTitle = ref('');
    const deleteDialogMessage = ref('');
    const itemToDelete = ref(null);

    const handleAddControlDesign = () => {
        showAddControlDesignModal.value = true;
    };

    const handleCloseAddControlDesignModal = () => {
        showAddControlDesignModal.value = false;
    };

    const handleControlDesignAdded = async (newControlDesign) => {
        try {
            controlDesigns.value.push(newControlDesign);
            emit('control-design-added', newControlDesign);
            handleCloseAddControlDesignModal();
        } catch (error) {
            console.error('Error in handleControlDesignAdded:', error);
            showNotification('Failed to refresh data after adding control design', 'error');
        }
    };

    const handleEditControlDesign = (controlDesign) => {
        controlDesignToEdit.value = controlDesign;
        showEditControlDesignModal.value = true;
    };

    const handleCloseEditControlDesignModal = () => {
        showEditControlDesignModal.value = false;
        controlDesignToEdit.value = null;
    };

    const handleControlDesignUpdated = async (updatedControlDesign) => {
       try {
        const index = controlDesigns.value.findIndex(obj => obj.id === updatedControlDesign.id);
       
        if(index !== -1){
           controlDesigns.value[index] = { ...controlDesigns.value[index], ...updatedControlDesign };
        }
        emit('control-design-updated', updatedControlDesign);

        showNotification(
            `Control Design "${updatedControlDesign.description}" has been updated successfully!`,
             'success'
        );

        handleCloseEditControlDesignModal();
       }catch (error) {
            console.error('Error in handleControlDesignUpdated:', error);
            showNotification('Failed to refresh data after updating control design', 'error');
       }
    };

    const handleDeleteControlDesign = (controlDesign) => {
        itemToDelete.value = controlDesign;
        deleteDialogTitle.value = 'Delete Control Design';
        deleteDialogMessage.value = `Are you sure you want to delete the control design "${controlDesign.description}"?`;
        showDeleteDialog.value = true;
    };

    const confirmDelete = async () => {
      if (!itemToDelete.value) return;
      
        const deletedControlDesign = { ...itemToDelete.value };
        const result = await deleteControlDesign(deletedControlDesign.id);

        if (result.success) {
            const index = controlDesigns.value.findIndex(obj => obj.id === deletedControlDesign.id);
            if (index !== -1) {
                controlDesigns.value.splice(index, 1);
            }

            showNotification(
                `Control Design "${deletedControlDesign.description}" has been deleted successfully!`,
                'success'
            );

            localStorage.removeItem('riskControlSelectedControlDesignId');
            
            emit('control-design-deleted', deletedControlDesign.id);        
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
        showAddControlDesignModal,
        showEditControlDesignModal,
        controlDesignToEdit,
        showDeleteDialog,
        deleteDialogTitle,
        deleteDialogMessage,
        itemToDelete,
        handleAddControlDesign,
        handleCloseAddControlDesignModal,
        handleControlDesignAdded,
        handleEditControlDesign,
        handleCloseEditControlDesignModal,
        handleControlDesignUpdated,
        handleDeleteControlDesign,
        confirmDelete,
        cancelDelete,
    };
}
