import { ref } from 'vue';
import { useDeleteAuditProcessById } from '@composables/riskControl/auditProcess/useDeleteAuditProcessById';

export function useAuditProcessActions(refreshData, showNotification) {
    const { deleteAuditProcess } = useDeleteAuditProcessById();

    const showAddAuditProcessModel = ref(false);
    const showEditAuditProcessModel = ref(false);
    const auditProcessToEdit = ref(null);
    const showDeleteDialog = ref(false);
    const deleteDialogTitle = ref('');
    const deleteDialogMessage = ref('');
    const itemToDelete = ref(null);

    //Add
    const handleAddAuditProcess =() => {
        showAddAuditProcessModel.value = true;
    };

    const handleCloseAddAuditProcessModel = () => {
        showAddAuditProcessModel.value = false;
    };

    const handleAuditProcessAdded = async (newAuditProcess) => {
        await refreshData();
        showNotification(`Process "${newAuditProcess.name}" has been added successfully!`, 'success');
        handleCloseAddAuditProcessModel();
    };

    //Edit
    const handleEditAuditProcess = (auditProcess) => {
        auditProcessToEdit.value = auditProcess;
        showEditAuditProcessModel.value = true;
    };

    const handleCloseEditAuditProcessModel = () => {
        showEditAuditProcessModel.value = false;
        auditProcessToEdit.value = null;
    };

    const handleAuditProcessUpdated = async () => {
        await refreshData();
        showNotification(`Process "${auditProcessToEdit.value.name}" has been updated successfully!`, 'success');
        handleCloseEditAuditProcessModel();
    }

    //Delete

    const handleDeleteAuditProcess = (auditProcess) => {
        itemToDelete.value = auditProcess;
        deleteDialogTitle.value = 'Delete Process';
        deleteDialogMessage.value = `Are you sure you want to delete the process "${auditProcess.name}"?`;
        showDeleteDialog.value = true;
    };

    const confirmDelete = async () => {
        if (itemToDelete.value) {
            const result = await deleteAuditProcess(itemToDelete.value.id);
            if (result.success) {
                showNotification(`Process "${itemToDelete.value.name}" has been deleted successfully!`, 'success');
                
                setTimeout(async () => {
                    await refreshData();
                }, 100);
            }

            CancelDelete();
        }
    };

    const CancelDelete = () => {
        showDeleteDialog.value = false;
        itemToDelete.value = null;
        deleteDialogTitle.value = '';
        deleteDialogMessage.value = '';
    };

    return {
        showAddAuditProcessModel,
        showEditAuditProcessModel,
        auditProcessToEdit,
        showDeleteDialog,
        deleteDialogTitle,
        deleteDialogMessage,
        itemToDelete,
        handleAddAuditProcess,
        handleCloseAddAuditProcessModel,
        handleAuditProcessAdded,
        handleEditAuditProcess,
        handleCloseEditAuditProcessModel,
        handleAuditProcessUpdated,
        handleDeleteAuditProcess,
        confirmDelete,
        CancelDelete,
    };
}