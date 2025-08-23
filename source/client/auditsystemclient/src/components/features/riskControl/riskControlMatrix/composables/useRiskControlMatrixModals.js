import { ref } from 'vue';
import { useDeleteRiskControlMatrixById } from '@/composables/riskControl/riskControlMatrix/useDeleteRiskControlMatrixById';
import { useNotification } from '@/composables/notification/useNotification';

export function useRiskControlMatrixModals(refreshData, emit) {
    const { deleteRiskControlMatrix } = useDeleteRiskControlMatrixById();
    const { showNotification } = useNotification();

    const showAddRiskControlMatrixModal = ref(false);
    const showEditRiskControlMatrixModal = ref(false);
    const riskControlMatrixToEdit = ref(null);
    const showDeleteDialog = ref(false);
    const deleteDialogTitle = ref('');
    const deleteDialogMessage = ref('');
    const itemToDelete = ref(null);

    const handleAddRiskControlMatrix = () => {
        showAddRiskControlMatrixModal.value = true;
    };

    const handleCloseAddRiskControlMatrixModal = () => {
        showAddRiskControlMatrixModal.value = false;
    };

    const handleRiskControlMatrixAdded = async () => {
        try {
            await refreshData();
            emit('risk-control-matrix-added');
            handleCloseAddRiskControlMatrixModal();
        } catch (error) {
            console.error('Error in handleRiskControlMatrixAdded:', error);
            showNotification('Failed to refresh data after adding risk control matrix', 'error');
        }
    };

    const handleEditRiskControlMatrix = (riskControlMatrix) => {
        riskControlMatrixToEdit.value = riskControlMatrix;
        showEditRiskControlMatrixModal.value = true;
    };

    const handleCloseEditRiskControlMatrixModal = () => {
        showEditRiskControlMatrixModal.value = false;
        riskControlMatrixToEdit.value = null;
    };

    const handleRiskControlMatrixUpdated = async () => {
        // Store the description risk control matrix name before clearing the reference
        const description = riskControlMatrixToEdit.value?.description || 'Unknown Risk Control Matrix';
        await refreshData();
        emit('risk-control-matrix-updated');
        showNotification(`Risk Control Matrix "${description}" has been updated successfully!`, 'success');
        handleCloseEditRiskControlMatrixModal();
    };

    const handleDeleteRiskControlMatrix = (riskControlMatrix) => {
        itemToDelete.value = riskControlMatrix;
        deleteDialogTitle.value = 'Delete Risk Control Matrix';
        deleteDialogMessage.value = `Are you sure you want to delete the risk control matrix "${riskControlMatrix.description}"?`;
        showDeleteDialog.value = true;
    };

    const confirmDelete = async () => {
        if (itemToDelete.value) {
            const deletedMatrix = { ...itemToDelete.value };
            const result = await deleteRiskControlMatrix(deletedMatrix.id);

            if (result.success) {
            showNotification(
                `Risk Control Matrix "${deletedMatrix.description}" has been deleted successfully!`,
                'success'
            );
            localStorage.removeItem("riskControlSelectedRiskControlMatrixId");

            setTimeout(async () => {
                await refreshData();
                emit('risk-control-matrix-deleted', deletedMatrix.id);
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
        showAddRiskControlMatrixModal,
        showEditRiskControlMatrixModal,
        riskControlMatrixToEdit,
        showDeleteDialog,
        deleteDialogTitle,
        deleteDialogMessage,
        itemToDelete,
        handleAddRiskControlMatrix,
        handleCloseAddRiskControlMatrixModal,
        handleRiskControlMatrixAdded,
        handleEditRiskControlMatrix,
        handleCloseEditRiskControlMatrixModal,
        handleRiskControlMatrixUpdated,
        handleDeleteRiskControlMatrix,
        confirmDelete,
        cancelDelete,
    };
}
