import { ref } from 'vue';
import { useDeleteAuditProcessById } from '@/composables/riskControl/auditProcess/useDeleteAuditProcessById';
import { useNotification } from '@/composables/notification/useNotification';

export function useAuditProcessModals(refreshData) {
    const { deleteAuditProcess } = useDeleteAuditProcessById();
    const { showNotification } = useNotification();

    const showAddAuditProcessModal = ref(false);
    const showEditAuditProcessModal = ref(false);
    const auditProcessToEdit = ref(null);
    const showDeleteDialog = ref(false);
    const deleteDialogTitle = ref('');
    const deleteDialogMessage = ref('');
    const itemToDelete = ref(null);

    const handleAddAuditProcess = () => {
        showAddAuditProcessModal.value = true;
    };

    const handleCloseAddAuditProcessModal = () => {
        showAddAuditProcessModal.value = false;
    };

    const handleAuditProcessAdded = async () => {
        try {
            await refreshData();
            handleCloseAddAuditProcessModal();
        } catch (error) {
            console.error('Error in handleAuditProcessAdded:', error);
            showNotification('Failed to refresh data after adding process', 'error');
        }
    };

    const handleEditAuditProcess = (auditProcess) => {
        auditProcessToEdit.value = auditProcess;
        showEditAuditProcessModal.value = true;
    };

    const handleCloseEditAuditProcessModal = () => {
        showEditAuditProcessModal.value = false;
        auditProcessToEdit.value = null;
    };

    const handleAuditProcessUpdated = async () => {
        // Store the process name before clearing the reference
        const processName = auditProcessToEdit.value?.processName || 'Unknown Process';
        await refreshData();
        showNotification(`Process "${processName}" has been updated successfully!`, 'success');
        handleCloseEditAuditProcessModal();
    };

    const handleDeleteAuditProcess = (auditProcess) => {
        itemToDelete.value = auditProcess;
        deleteDialogTitle.value = 'Delete Process';
        deleteDialogMessage.value = `Are you sure you want to delete the process "${auditProcess.processName}"?`;
        showDeleteDialog.value = true;
    };

    const confirmDelete = async () => {
        if (itemToDelete.value) {
            const result = await deleteAuditProcess(itemToDelete.value.id);
            if (result.success) {
                showNotification(`Process "${itemToDelete.value.processName}" has been deleted successfully!`, 'success');
                setTimeout(async () => {
                    await refreshData();
                }, 100);
            }
            cancelDelete();
        }
    };

    const cancelDelete = () => {
        showDeleteDialog.value = false;
        itemToDelete.value = null;
        deleteDialogTitle.value = '';
        deleteDialogMessage.value = '';
    };

    return {
        showAddAuditProcessModal,
        showEditAuditProcessModal,
        auditProcessToEdit,
        showDeleteDialog,
        deleteDialogTitle,
        deleteDialogMessage,
        itemToDelete,
        handleAddAuditProcess,
        handleCloseAddAuditProcessModal,
        handleAuditProcessAdded,
        handleEditAuditProcess,
        handleCloseEditAuditProcessModal,
        handleAuditProcessUpdated,
        handleDeleteAuditProcess,
        confirmDelete,
        cancelDelete,
    };
}