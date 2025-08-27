import { ref } from 'vue';
import { useDeleteSubLocationById } from '@/composables/organization/sublocation/useDeleteSubLocationById';
import { useNotification } from '@/composables/notification/useNotification';

export function useSubLocationModals(subLocations, emit) {
    const { deleteSubLocation } = useDeleteSubLocationById();
    const { showNotification } = useNotification();

    const showAddSubLocationModal = ref(false);
    const showEditSubLocationModal = ref(false);
    const subLocationToEdit = ref(null);
    const showDeleteDialog = ref(false);
    const deleteDialogTitle = ref('');
    const deleteDialogMessage = ref('');
    const itemToDelete = ref(null);

    const handleAddSubLocation = () => {
        showAddSubLocationModal.value = true;
    };

    const handleCloseAddSubLocationModal = () => {
        showAddSubLocationModal.value = false;
    };

    const handleSubLocationAdded = async (newSubLocation) => {
        try {
            subLocations.value.push(newSubLocation);
            emit('sub-location-added',newSubLocation);
            handleCloseAddSubLocationModal();
        } catch (error) {
            console.error('Error in handleSubLocationAdded:', error);
            showNotification('Failed to refresh data after adding sub-location', 'error');
        }
    };

    const handleEditSubLocation = (subLocation) => {
        subLocationToEdit.value = subLocation;
        showEditSubLocationModal.value = true;
    };

    const handleCloseEditSubLocationModal = () => {
        showEditSubLocationModal.value = false;
        subLocationToEdit.value = null;
    };

    const handleSubLocationUpdated = async (updatedSubLocation) => {
        try {
            // Find and replace the updated process in the local list
            const index = subLocations.value.findIndex(
                p => p.id === updatedSubLocation.id
            );

            if (index !== -1) {
                subLocations.value[index] = { ...subLocations.value[index], ...updatedSubLocation };
            }

            emit('sub-location-updated', updatedSubLocation);

            showNotification(
                `Sub-Location "${updatedSubLocation.name}" has been updated successfully!`,
                'success'
            );

            handleCloseEditSubLocationModal();
        } catch (error) {
            console.error('Error in handleSubLocationUpdated:', error);
            showNotification('Failed to update sub-location', 'error');
        }
    };


    const handleDeleteSubLocation = (subLocation) => {
        itemToDelete.value = subLocation;
        deleteDialogTitle.value = 'Delete Sub-Location';
        deleteDialogMessage.value = `Are you sure you want to delete the sub-location "${subLocation.name}"?`;
        showDeleteDialog.value = true;
    };

    const confirmDelete = async () => {
        if (!itemToDelete.value) return;

        const deletedSubLocation = { ...itemToDelete.value };
        const result = await deleteSubLocation(deletedSubLocation.id);

        if (result.success) {
            // Remove the deleted audit process from the local list
            const index = subLocations.value.findIndex(p => p.id === deletedSubLocation.id);
            if (index !== -1) {
                subLocations.value.splice(index, 1); // <-- reactive change
            }

            showNotification(`Sub-Location "${deletedSubLocation.name}" has been deleted successfully!`, 'success');

            localStorage.removeItem('organizationSelectedSubLocationId');

            emit('sub-location-deleted', deletedSubLocation.id);
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
        cancelDelete,
    };
}