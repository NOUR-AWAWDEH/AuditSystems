import { ref } from 'vue';
import { useDeleteSubDepartmentById } from '@/composables/organization/subdepartment/useDeleteSubDepartmentById';

export function useSubDepartmentActions(refreshData, showNotification) {
    const { deleteSubDepartment } = useDeleteSubDepartmentById();
    
    // Modal states
    const showAddSubDepartmentModal = ref(false);
    const showEditSubDepartmentModal = ref(false);
    const subDepartmentToEdit = ref(null);
    const showDeleteDialog = ref(false);
    const deleteDialogTitle = ref('');
    const deleteDialogMessage = ref('');
    const itemToDelete = ref(null);
    
    // Action handlers
    const handleAddSubDepartment = () => {
        showAddSubDepartmentModal.value = true;
    };
    
    const handleCloseAddSubDepartmentModal = () => {
        showAddSubDepartmentModal.value = false;
    };
    
    const handleSubDepartment = async (newSubDepartment) => {
        await refreshData();
        showNotification(`Sub Department "${newSubDepartment.name}" has been added successfully!`, 'success');
        handleCloseAddSubDepartmentModal();
    };
    
    const handleEditSubDepartment = (subDepartment) => {
        subDepartmentToEdit.value = subDepartment;
        showEditSubDepartmentModal.value = true;
    };
    
    const handleCloseEditSubDepartmentModal = () => {
        showEditSubDepartmentModal.value = false;
        subDepartmentToEdit.value = null;
    };
    
    const handleSubDepartmentUpdated = async () => {
        await refreshData();
        handleCloseEditSubDepartmentModal();
    };
    
    const handleDeleteSubDepartment = (subDepartment) => {
        itemToDelete.value = subDepartment;
        deleteDialogTitle.value = 'Delete SubDepartment';
        deleteDialogMessage.value = `Are you sure you want to delete "${subDepartment.name}"? This action cannot be undone.`;
        showDeleteDialog.value = true;
    };
    
    const confirmDelete = async () => {
        if (itemToDelete.value) {
            const result = await deleteSubDepartment(itemToDelete.value.id);
            
            if (result.success) {
                showNotification(`SubDepartment "${itemToDelete.value.name}" has been deleted successfully!`, 'success');
                
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
        showAddSubDepartmentModal,
        showEditSubDepartmentModal,
        subDepartmentToEdit,
        showDeleteDialog,
        deleteDialogTitle,
        deleteDialogMessage,
        itemToDelete,
        handleAddSubDepartment,
        handleCloseAddSubDepartmentModal,
        handleSubDepartment,
        handleEditSubDepartment,
        handleCloseEditSubDepartmentModal,
        handleSubDepartmentUpdated,
        handleDeleteSubDepartment,
        confirmDelete,
        cancelDelete
    };
}