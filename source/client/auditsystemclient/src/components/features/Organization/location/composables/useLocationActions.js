import { ref } from 'vue';
import { useDeleteLocationById } from '@/composables/organization/location/useDeleteLocationById';

export function useLocationActions(refreshData, showNotification, onLocationDeleted = null) {
    const { deleteLocation } = useDeleteLocationById();

    // Modal states
    const showAddLocationModal = ref(false);
    const showEditLocationModal = ref(false);
    const locationToEdit = ref(null);
    const showDeleteDialog = ref(false);
    const deleteDialogTitle = ref('');
    const deleteDialogMessage = ref('');
    const itemToDelete = ref(null);

    // Add location actions
    const handleAddLocation = () => {
        showAddLocationModal.value = true;
    };

    const handleCloseAddLocationModal = () => {
        showAddLocationModal.value = false;
    };

    const handleLocationAdded = async (newLocation) => {
        await refreshData(); 
        showNotification(`Location "${newLocation.name}" has been added successfully!`, 'success');
        handleCloseAddLocationModal();
    };

    // Location actions
    const handleEditLocation = (location) => {
        locationToEdit.value = location;
        showEditLocationModal.value = true; // Changed from showEditModal
    };

    const handleCloseEditLocationModal = () => { // Changed function name
        showEditLocationModal.value = false; // Changed from showEditModal
        locationToEdit.value = null;
    };

    const handleLocationUpdated = async (updatedLocation) => {
        await refreshData();
        showNotification(`Location "${updatedLocation.name}" has been updated successfully!`, 'success');
        handleCloseEditLocationModal();
    };

    const handleDeleteLocation = (location) => {
        itemToDelete.value = location;
        deleteDialogTitle.value = 'Delete Location';
        deleteDialogMessage.value = `Are you sure you want to delete "${location.name}"? This action cannot be undone.`;
        showDeleteDialog.value = true;
    };

    // Delete confirmation
    const confirmDelete = async () => {
        if (itemToDelete.value) {
            const deletedLocationId = itemToDelete.value.id;
            const result = await deleteLocation(itemToDelete.value.id, itemToDelete.value.name);
            
            if (result.success) {
                showNotification(`Location "${itemToDelete.value.name}" has been deleted successfully!`, 'success');
                await refreshData();
                
                // Notify parent component about location deletion
                if (onLocationDeleted) {
                    onLocationDeleted(deletedLocationId);
                }
            }
            
            itemToDelete.value = null;
            showDeleteDialog.value = false;
        }
    };

    const cancelDelete = () => {
        itemToDelete.value = null;
        showDeleteDialog.value = false;
    };

    return {
        // Modal states
        showAddLocationModal,
        showEditLocationModal,
        locationToEdit,
        showDeleteDialog,
        deleteDialogTitle,
        deleteDialogMessage,
        itemToDelete,
        
        // Action handlers
        handleAddLocation,
        handleCloseAddLocationModal,
        handleLocationAdded,
        handleEditLocation,
        handleCloseEditLocationModal,
        handleLocationUpdated,
        handleDeleteLocation,
        confirmDelete,
        cancelDelete
    };
}