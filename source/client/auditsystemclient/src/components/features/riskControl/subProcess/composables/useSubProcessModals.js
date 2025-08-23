import { ref } from 'vue';
import { useDeleteSubProcessById } from '@/composables/riskControl/subProcess/useDeleteSubProcessById';
import { useNotification } from '@/composables/notification/useNotification';

export function useSubProcessModals(refreshData, emit) {
    const { deleteSubProcess } = useDeleteSubProcessById();
    const { showNotification } = useNotification();

    const showAddSubProcessModal = ref(false);
    const showEditSubProcessModal = ref(false);
    const subProcessToEdit = ref(null);
    const showDeleteDialog = ref(false);
    const deleteDialogTitle = ref('');
    const deleteDialogMessage = ref('');
    const itemToDelete = ref(null);

    const handleAddSubProcess = () => {
        showAddSubProcessModal.value = true;
    };

    const handleCloseAddSubProcessModal = () => {
        showAddSubProcessModal.value = false;
    };

    const handleSubProcessAdded = async () => {
        try {
            await refreshData();
            emit('sub-process-added');
            handleCloseAddSubProcessModal();
        } catch (error) {
            console.error('Error in handleSubProcessAdded:', error);
            showNotification('Failed to refresh data after adding subprocess', 'error');
        }
    };

    const handleEditSubProcess = (subProcess) => {
        subProcessToEdit.value = subProcess;
        showEditSubProcessModal.value = true;
    };

    const handleCloseEditSubProcessModal = () => {
        showEditSubProcessModal.value = false;
        subProcessToEdit.value = null;
    };

    const handleSubProcessUpdated = async () => {
        // Store the particular subprocess name before clearing the reference
        const particular = subProcessToEdit.value?.particular || 'Unknown Process';
        await refreshData();
        emit('sub-process-updated');
        showNotification(`SubProcess "${particular}" has been updated successfully!`, 'success');
        handleCloseEditSubProcessModal();
    };

    const handleDeleteSubProcess = (subProcess) => {
        itemToDelete.value = subProcess;
        deleteDialogTitle.value = 'Delete SubProcess';
        deleteDialogMessage.value = `Are you sure you want to delete the subprocess "${subProcess.particular}"?`;
        showDeleteDialog.value = true;
    };

    const confirmDelete = async () => {
        if (itemToDelete.value) {
            const deletedSubProcess = { ...itemToDelete.value };
            const result = await deleteSubProcess(deletedSubProcess.id);

            if (result.success) {
            showNotification(
                `SubProcess "${deletedSubProcess.particular}" has been deleted successfully!`,
                'success'
            );
            localStorage.removeItem("riskControlSelectedSubProcessId");

            setTimeout(async () => {
                await refreshData();
                emit('sub-process-deleted', deletedSubProcess.id);
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
        showAddSubProcessModal,
        showEditSubProcessModal,
        subProcessToEdit,
        showDeleteDialog,
        deleteDialogTitle,
        deleteDialogMessage,
        itemToDelete,
        handleAddSubProcess,
        handleCloseAddSubProcessModal,
        handleSubProcessAdded,
        handleEditSubProcess,
        handleCloseEditSubProcessModal,
        handleSubProcessUpdated,
        handleDeleteSubProcess,
        confirmDelete,
        cancelDelete,
    };
}
