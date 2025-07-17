import { ref } from 'vue';
import { useDeleteSubDepartmentById } from '@/composables/organization/subdepartment/useDeleteSubDepartmentById';

export function useSubDepartmentActions(refreshData, showNotification) {
    const { deleteSubDepartment } = useDeleteSubDepartmentById();
    
    // Modal states
    const showAddSubdepartmentModal = ref(false);
    const showEditSubdepartmentModal = ref(false);
    const subdepartmentToEdit = ref(null);
    const showDeleteDialog = ref(false);
    const deleteDialogTitle = ref('');
    const deleteDialogMessage = ref('');
    const itemToDelete = ref(null);
    
    // Action handlers
    const handleAddSubdepartment = () => {
        showAddSubdepartmentModal.value = true;
    };
    
    const handleCloseAddSubdepartmentModal = () => {
        showAddSubdepartmentModal.value = false;
    };
    
    const handleSubdepartment = async (newSubdepartment) => {
        await refreshData();
        showNotification(`Subdepartment "${newSubdepartment.name}" has been added successfully!`, 'success');
        handleCloseAddSubdepartmentModal();
    };
    
    const handleEditSubdepartment = (subdepartment) => {
        subdepartmentToEdit.value = subdepartment;
        showEditSubdepartmentModal.value = true;
    };
    
    const handleCloseEditSubdepartmentModal = () => {
        showEditSubdepartmentModal.value = false;
        subdepartmentToEdit.value = null;
    };
    
    const handleSubdepartmentUpdated = async () => {
        await refreshData();
        handleCloseEditSubdepartmentModal();
    };
    
    const handleDeleteSubdepartment = (subdepartment) => {
        itemToDelete.value = subdepartment;
        deleteDialogTitle.value = 'Delete Subdepartment';
        deleteDialogMessage.value = `Are you sure you want to delete "${subdepartment.name}"? This action cannot be undone.`;
        showDeleteDialog.value = true;
    };
    
    const confirmDelete = async () => {
        if (itemToDelete.value) {
            const result = await deleteSubDepartment(itemToDelete.value.id);
            
            if (result.success) {
                showNotification(`Subdepartment "${itemToDelete.value.name}" has been deleted successfully!`, 'success');
                
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
        showAddSubdepartmentModal,
        showEditSubdepartmentModal,
        subdepartmentToEdit,
        showDeleteDialog,
        deleteDialogTitle,
        deleteDialogMessage,
        itemToDelete,
        handleAddSubdepartment,
        handleCloseAddSubdepartmentModal,
        handleSubdepartment,
        handleEditSubdepartment,
        handleCloseEditSubdepartmentModal,
        handleSubdepartmentUpdated,
        handleDeleteSubdepartment,
        confirmDelete,
        cancelDelete
    };
}