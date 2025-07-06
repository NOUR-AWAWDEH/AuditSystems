import { ref } from 'vue';
import { useDeleteLocationById } from '@/composables/organization/location/useDeleteLocationById';

export function useLocationActions(refreshData, showNotification) {
    const { deleteLocation } = useDeleteLocationById();

    // Modal states
    const showAddLocationModal = ref(false);
    const showEditModal = ref(false);
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
        await refreshData(); // Refresh the data to show the new location immediately
        showNotification(`Location "${newLocation.name}" has been added successfully!`, 'success');
        handleCloseAddLocationModal();
    };

    // Location actions
    const handleEditLocation = (location) => {
        locationToEdit.value = location;
        showEditModal.value = true;
    };

    const handleCloseEditModal = () => {
        showEditModal.value = false;
        locationToEdit.value = null;
    };

    const handleLocationUpdated = async (updatedLocation) => {
        await refreshData();
        showNotification(`Location "${updatedLocation.name}" has been updated successfully!`, 'success');
        handleCloseEditModal();
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
            const result = await deleteLocation(itemToDelete.value.id, itemToDelete.value.name);
            
            if (result.success) {
                showNotification(`Location "${itemToDelete.value.name}" has been deleted successfully!`, 'success');
                await refreshData();
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
        showEditModal,
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
        handleCloseEditModal,
        handleLocationUpdated,
        handleDeleteLocation,
        confirmDelete,
        cancelDelete
    };
}