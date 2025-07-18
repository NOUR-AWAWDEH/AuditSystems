import { ref } from 'vue';
import { useDeleteSubLocationById } from '@/composables/organization/sublocation/useDeleteSubLocationById';

export function useSubLocationActions(refreshData, showNotification) {
    const { deleteSubLocation } = useDeleteSubLocationById();
    
    // Modal states
    const showAddSubLocationModal = ref(false);

    const showEditSubLocationModal = ref(false);
    const subLocationToEdit = ref(null);
    const showDeleteDialog = ref(false);
    const deleteDialogTitle = ref('');
    const deleteDialogMessage = ref('');
    const itemToDelete = ref(null);
    
    // Action handlers
    const handleAddSubLocation = () => {
        showAddSubLocationModal.value = true;
    };
    
    const handleCloseAddSubLocationModal = () => {
        showAddSubLocationModal.value = false;
    };
    
    const handleSubLocationAdded = async (newSubLocation) => {
        await refreshData();
        showNotification(`SubLocation "${newSubLocation.name}" has been added successfully!`, 'success');
        handleCloseAddSubLocationModal();
    };
    
    const handleEditSubLocation = (subLocation) => {
        subLocationToEdit.value = subLocation;
        showEditSubLocationModal.value = true;
    };
    
    const handleCloseEditSubLocationModal = () => {
        showEditSubLocationModal.value = false;
        subLocationToEdit.value = null;
    };
    
    const handleSubLocationUpdated = async () => {
        await refreshData();
        handleCloseEditSubLocationModal();
    };
    
    const handleDeleteSubLocation = (subLocation) => {
        itemToDelete.value = subLocation;
        deleteDialogTitle.value = 'Delete SubLocation';
        deleteDialogMessage.value = `Are you sure you want to delete "${subLocation.name}"? This action cannot be undone.`;     
        showDeleteDialog.value = true;
    };
    
    const confirmDelete = async () => {
        if (itemToDelete.value) {
            const result = await deleteSubLocation(itemToDelete.value.id);
            
            if (result.success) {
                showNotification(`SubLocation "${itemToDelete.value.name}" has been deleted successfully!`, 'success');
                
                setTimeout(async () => {
                    await refreshData();
                }, 100);
            }
            
            cancelDelete();
        }
    };
    
    const cancelDelete = () => {
        itemToDelete.value = null;
        showDeleteDialog.value = false;
        deleteDialogTitle.value = '';
        deleteDialogMessage.value = '';
    };
    
    return {
        showAddSubLocationModal,
        showEditSubLocationModal,
        subLocationToEdit,
        showDeleteDialog,
        deleteDialogTitle,
        deleteDialogMessage,
        itemToDelete,
        handleAddSubLocation,
        handleCloseAddSubLocationModal,
        handleSubLocationAdded,
        handleEditSubLocation,
        handleCloseEditSubLocationModal,
        handleSubLocationUpdated,
        handleDeleteSubLocation,
        confirmDelete,
        cancelDelete
    };
}