import { ref } from 'vue';
import { useDeleteSubLocationById } from '@/composables/organization/sublocation/useDeleteSubLocationById';

export function useSubLocationActions(refreshData, showNotification) {
    const { deleteSubLocation } = useDeleteSubLocationById();
    
    // Modal states
    const showAddSublocationModal = ref(false);
    const showEditSublocationModal = ref(false);
    const sublocationToEdit = ref(null);
    const showDeleteDialog = ref(false);
    const deleteDialogTitle = ref('');
    const deleteDialogMessage = ref('');
    const itemToDelete = ref(null);
    
    // Action handlers
    const handleAddSublocation = () => {
        showAddSublocationModal.value = true;
    };
    
    const handleCloseAddSublocationModal = () => {
        showAddSublocationModal.value = false;
    };
    
    const handleSublocationAdded = async (newSublocation) => {
        await refreshData(); // Refresh the data to show the new sublocation immediately
        showNotification(`Sublocation "${newSublocation.name}" has been added successfully!`, 'success');
        handleCloseAddSublocationModal();
    };
    
    const handleEditSublocation = (sublocation) => {
        sublocationToEdit.value = sublocation;
        showEditSublocationModal.value = true;
    };
    
    const handleCloseEditSublocationModal = () => {
        showEditSublocationModal.value = false;
        sublocationToEdit.value = null;
    };
    
    const handleSublocationUpdated = async () => {
        await refreshData();
        handleCloseEditSublocationModal();
    };
    
    const handleDeleteSublocation = (sublocation) => {
        itemToDelete.value = sublocation;
        deleteDialogTitle.value = 'Delete Sublocation';
        deleteDialogMessage.value = `Are you sure you want to delete "${sublocation.name}"? This action cannot be undone.`;
        showDeleteDialog.value = true;
    };
    
    const confirmDelete = async () => {
        if (itemToDelete.value) {
            const result = await deleteSubLocation(itemToDelete.value.id);
            
            if (result.success) {
                showNotification(`Sublocation "${itemToDelete.value.name}" has been deleted successfully!`, 'success');
                
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
        showAddSublocationModal,
        showEditSublocationModal,
        sublocationToEdit,
        showDeleteDialog,
        deleteDialogTitle,
        deleteDialogMessage,
        itemToDelete,
        handleAddSublocation,
        handleCloseAddSublocationModal,
        handleSublocationAdded,
        handleEditSublocation,
        handleCloseEditSublocationModal,
        handleSublocationUpdated,
        handleDeleteSublocation,
        confirmDelete,
        cancelDelete
    };
}