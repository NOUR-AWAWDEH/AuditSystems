import { ref } from 'vue';
import { useDeleteAuditProcessById } from '@/composables/riskControl/auditProcess/useDeleteAuditProcessById';
import { useNotification } from '@/composables/notification/useNotification';

export function useAuditProcessModals(auditProcesses, emit) {
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

    const handleAuditProcessAdded = async (newAuditProcess) => {
        try {
            auditProcesses.value.push(newAuditProcess);
            emit('audit-process-added',newAuditProcess);
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

    const handleAuditProcessUpdated = async (updatedAuditProcess) => {
        try {
            // Find and replace the updated process in the local list
            const index = auditProcesses.value.findIndex(
                p => p.id === updatedAuditProcess.id
            );

            if (index !== -1) {
                auditProcesses.value[index] = { ...auditProcesses.value[index], ...updatedAuditProcess };
            }

            emit('audit-process-updated', updatedAuditProcess);

            showNotification(
                `Process "${updatedAuditProcess.processName}" has been updated successfully!`,
                'success'
            );

            handleCloseEditAuditProcessModal();
        } catch (error) {
            console.error('Error in handleAuditProcessUpdated:', error);
            showNotification('Failed to update process', 'error');
        }
    };


    const handleDeleteAuditProcess = (auditProcess) => {
        itemToDelete.value = auditProcess;
        deleteDialogTitle.value = 'Delete Process';
        deleteDialogMessage.value = `Are you sure you want to delete the process "${auditProcess.processName}"?`;
        showDeleteDialog.value = true;
    };

    const confirmDelete = async () => {
        if (!itemToDelete.value) return;

        const deleteProcess = { ...itemToDelete.value };
        const result = await deleteAuditProcess(deleteProcess.id);

        if (result.success) {
            // 🔥 Remove in place
            const index = auditProcesses.value.findIndex(p => p.id === deleteProcess.id);
            if (index !== -1) {
                auditProcesses.value.splice(index, 1); // <-- reactive change
            }

            showNotification(`Process "${deleteProcess.processName}" has been deleted successfully!`, 'success');

            localStorage.removeItem('riskControlSelectedAuditProcessId');

            emit('audit-process-deleted', deleteProcess.id);
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