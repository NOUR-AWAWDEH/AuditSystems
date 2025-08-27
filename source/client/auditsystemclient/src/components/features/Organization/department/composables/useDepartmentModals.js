import { ref } from 'vue';
import { useDeleteDepartmentById } from '@/composables/organization/department/useDeleteDepartmentById';
import { useNotification } from '@/composables/notification/useNotification';

export function useDepartmentModals(departments, emit) {
    const { deleteDepartment } = useDeleteDepartmentById();
    const { showNotification } = useNotification();

    const showAddDepartmentModal = ref(false);
    const showEditDepartmentModal = ref(false);
    const departmentToEdit = ref(null);
    const showDeleteDialog = ref(false);
    const deleteDialogTitle = ref('');
    const deleteDialogMessage = ref('');
    const itemToDelete = ref(null);

    const handleAddDepartment = () => {
        showAddDepartmentModal.value = true;
    };

    const handleCloseAddDepartmentModal = () => {
        showAddDepartmentModal.value = false;
    };

    const handleDepartmentAdded = async (newDepartment) => {
        try {
            departments.value.push(newDepartment);
            emit('department-added',newDepartment);
            handleCloseAddDepartmentModal();
        } catch (error) {
            console.error('Error in handleDepartmentAdded:', error);
            showNotification('Failed to refresh data after adding department', 'error');
        }
    };

    const handleEditDepartment = (department) => {
        departmentToEdit.value = department;
        showEditDepartmentModal.value = true;
    };

    const handleCloseEditDepartmentModal = () => {
        showEditDepartmentModal.value = false;
        departmentToEdit.value = null;
    };

    const handleDepartmentUpdated = async (updatedDepartment) => {
        try {
            // Find and replace the updated process in the local list
            const index = departments.value.findIndex(
                p => p.id === updatedDepartment.id
            );

            if (index !== -1) {
                departments.value[index] = { ...departments.value[index], ...updatedDepartment };
            }

            emit('department-updated', updatedDepartment);

            showNotification(
                `Department "${updatedDepartment.name}" has been updated successfully!`,
                'success'
            );

            handleCloseEditDepartmentModal();
        } catch (error) {
            console.error('Error in handleDepartmentUpdated:', error);
            showNotification('Failed to update department', 'error');
        }
    };


    const handleDeleteDepartment = (department) => {
        itemToDelete.value = department;
        deleteDialogTitle.value = 'Delete Department';
        deleteDialogMessage.value = `Are you sure you want to delete the department "${department.name}"?`;
        showDeleteDialog.value = true;
    };

    const confirmDelete = async () => {
        if (!itemToDelete.value) return;

        const deletedDepartment = { ...itemToDelete.value };
        const result = await deleteDepartment(deletedDepartment.id);

        if (result.success) {
            // Remove the deleted audit process from the local list
            const index = departments.value.findIndex(p => p.id === deletedDepartment.id);
            if (index !== -1) {
                departments.value.splice(index, 1); // <-- reactive change
            }

            showNotification(`Department "${deletedDepartment.name}" has been deleted successfully!`, 'success');

            localStorage.removeItem('organizationSelectedDepartmentId');

            emit('department-deleted', deletedDepartment.id);
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
        showAddDepartmentModal,
        showEditDepartmentModal,
        departmentToEdit,
        showDeleteDialog,
        deleteDialogTitle,
        deleteDialogMessage,
        itemToDelete,
        handleAddDepartment,
        handleCloseAddDepartmentModal,
        handleDepartmentAdded,
        handleEditDepartment,
        handleCloseEditDepartmentModal,
        handleDepartmentUpdated,
        handleDeleteDepartment,
        confirmDelete,
        cancelDelete,
    };
}