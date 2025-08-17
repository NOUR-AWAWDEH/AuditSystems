import { ref } from 'vue';
import { useDeleteDepartmentById } from '@/composables/organization/department/useDeleteDepartmentById';

export function useDepartmentActions(refreshData, showNotification) {
    const { deleteDepartment } = useDeleteDepartmentById();

    // Modal states
    const showAddDepartmentModal = ref(false);
    const showEditDepartmentModal = ref(false);
    const departmentToEdit = ref(null);
    const showDeleteDialog = ref(false);
    const deleteDialogTitle = ref('');
    const deleteDialogMessage = ref('');
    const itemToDelete = ref(null);

    // Action handlers
    const handleAddDepartment = () => {
        showAddDepartmentModal.value = true;
    };

    const handleCloseAddDepartmentModal = () => {
        showAddDepartmentModal.value = false;
    };

    const handleDepartmentAdded = async (newDepartment) => {
        await refreshData();
        showNotification(`Department "${newDepartment.name}" has been added successfully!`, 'success');
        handleCloseAddDepartmentModal();
    };

    const handleEditDepartment = (department) => {
        departmentToEdit.value = department;
        showEditDepartmentModal.value = true;
    };

    const handleCloseEditDepartmentModal = () => {
        showEditDepartmentModal.value = false;
        departmentToEdit.value = null;
    };

    const handleDepartmentUpdated = async () => {
        await refreshData();
        showNotification(`Department "${departmentToEdit.value.name}" has been updated successfully!`, 'success');
        handleCloseEditDepartmentModal();
    };

    const handleDeleteDepartment = (department) => {
        itemToDelete.value = department;
        deleteDialogTitle.value = 'Delete Department';
        deleteDialogMessage.value = `Are you sure you want to delete "${department.name}"? This action cannot be undone.`;
        showDeleteDialog.value = true;
    };

    const confirmDelete = async () => {
        if (itemToDelete.value) {
            const result = await deleteDepartment(itemToDelete.value.id);

            if (result.success) {
                showNotification(`Department "${itemToDelete.value.name}" has been deleted successfully!`, 'success');
                localStorage.removeItem("searchDepartmentId");
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
        cancelDelete
    };
}
