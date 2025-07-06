import { ref } from 'vue';
import { useDeleteLocationById } from '@/composables/organization/location/useDeleteLocationById';
import { useDeleteSubLocationById } from '@/composables/organization/sublocation/useDeleteSubLocationById';

export function useLocationActions(refreshData, showNotification) {
    const { deleteLocation } = useDeleteLocationById();
    const { deleteSubLocation } = useDeleteSubLocationById();

    // Modal states
    const showEditModal = ref(false);
    const locationToEdit = ref(null);
    const showAddSublocationModal = ref(false);
    const selectedLocationId = ref(null);
    const showDeleteDialog = ref(false);
    const deleteDialogTitle = ref('');
    const deleteDialogMessage = ref('');
    const itemToDelete = ref(null);
    const deleteType = ref('');

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
        deleteType.value = 'location';
        deleteDialogTitle.value = 'Delete Location';
        deleteDialogMessage.value = `Are you sure you want to delete "${location.name}"? This action cannot be undone.`;
        showDeleteDialog.value = true;
    };

    // Sublocation actions
    const handleAddSublocation = (locationId) => {
        selectedLocationId.value = locationId;
        showAddSublocationModal.value = true;
    };

    const handleCloseAddSublocationModal = () => {
        showAddSublocationModal.value = false;
        selectedLocationId.value = null;
    };

    const handleEditSublocation = (sublocation) => {
        showNotification('Edit sublocation functionality coming soon!', 'info');
    };

    const handleDeleteSublocation = (sublocation) => {
        itemToDelete.value = sublocation;
        deleteType.value = 'sublocation';
        deleteDialogTitle.value = 'Delete Sublocation';
        deleteDialogMessage.value = `Are you sure you want to delete "${sublocation.name}"? This action cannot be undone.`;
        showDeleteDialog.value = true;
    };

    // Delete confirmation
    const confirmDelete = async () => {
        if (itemToDelete.value) {
            if (deleteType.value === 'location') {
                const result = await deleteLocation(itemToDelete.value.id, itemToDelete.value.name);
                
                if (result.success) {
                    showNotification(`Location "${itemToDelete.value.name}" has been deleted successfully!`, 'success');
                    await refreshData();
                }
            } else if (deleteType.value === 'sublocation') {
                const result = await deleteSubLocation(itemToDelete.value.id, itemToDelete.value.name);
                
                if (result.success) {
                    showNotification(`Sublocation "${itemToDelete.value.name}" has been deleted successfully!`, 'success');
                    await refreshData();
                }
            }
            
            itemToDelete.value = null;
            deleteType.value = '';
            showDeleteDialog.value = false;
        }
    };

    const cancelDelete = () => {
        itemToDelete.value = null;
        deleteType.value = '';
        showDeleteDialog.value = false;
    };

    return {
        // Modal states
        showEditModal,
        locationToEdit,
        showAddSublocationModal,
        selectedLocationId,
        showDeleteDialog,
        deleteDialogTitle,
        deleteDialogMessage,
        itemToDelete,
        
        // Action handlers
        handleEditLocation,
        handleCloseEditModal,
        handleLocationUpdated,
        handleDeleteLocation,
        handleAddSublocation,
        handleCloseAddSublocationModal,
        handleEditSublocation,
        handleDeleteSublocation,
        confirmDelete,
        cancelDelete
    };
}