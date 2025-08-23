/**
 * Composable for managing risk-related modals and dialogs (Add, Edit, Delete).
 *
 * Provides reactive state and handlers for showing, hiding, and confirming actions
 * related to risks in the UI.
 *
 * @param {Function} refreshData - Callback to refresh the risk data after changes (e.g., API refetch).
 * @param {Function} showNotification - Callback to show user notifications (message, type).
 *
 * State Refs:
 * - `showAddRiskModal` → Controls visibility of the Add Risk modal.
 * - `showEditRiskModal` → Controls visibility of the Edit Risk modal.
 * - `riskToEdit` → Stores the risk currently being edited.
 * - `showDeleteDialog` → Controls visibility of the delete confirmation dialog.
 * - `deleteDialogTitle` → Title text for the delete confirmation dialog.
 * - `deleteDialogMessage` → Message text for the delete confirmation dialog.
 * - `itemToDelete` → Stores the risk item scheduled for deletion.
 *
 * Handlers:
 * - `handleAddRisk()` → Opens the Add Risk modal.
 * - `handleCloseAddRiskModal()` → Closes the Add Risk modal.
 * - `handleRiskAdded()` → Refreshes data and closes modal after adding a risk.
 * - `handleEditRisk(risk)` → Opens the Edit Risk modal for the given `risk`.
 * - `handleCloseEditRiskModal()` → Closes the Edit Risk modal and clears state.
 * - `handleRiskUpdated()` → Refreshes data, notifies user, and closes the Edit modal.
 * - `handleDeleteRisk(risk)` → Prepares and opens the delete confirmation dialog for the given `risk`.
 * - `confirmDelete()` → Deletes the selected risk via API, refreshes data, shows success notification.
 * - `cancelDelete()` → Cancels delete operation, resets dialog state.
 *
 * Behavior:
 * - Uses `useDeleteRiskById` composable for deletion logic.
 * - Automatically clears localStorage key `riskControlSelectedRiskId` after deletion.
 * - Wraps all async operations in error handling to ensure UI stability.
 *
 * Returns:
 * Reactive refs and handlers for integration into risk management UI.
 */

import { ref } from 'vue';
import { useDeleteRiskById } from '@/composables/riskControl/risk/useDeleteRiskById';
import { useNotification } from '@/composables/notification/useNotification';

export function useRiskModals(refreshData, emit) {
    const { deleteRisk } = useDeleteRiskById();
    const { showNotification } = useNotification();

    const showAddRiskModal = ref(false);
    const showEditRiskModal = ref(false);
    const riskToEdit = ref(null);
    const showDeleteDialog = ref(false);
    const deleteDialogTitle = ref('');
    const deleteDialogMessage = ref('');
    const itemToDelete = ref(null);

    const handleAddRisk = () => {
        showAddRiskModal.value = true;
    };

    const handleCloseAddRiskModal = () => {
        showAddRiskModal.value = false;
    };

    const handleRiskAdded = async () => {
        try {
            await refreshData();
            emit('risk-added');
            handleCloseAddRiskModal();
        } catch (error) {
            console.error('Error in handleRiskAdded:', error);
            showNotification('Failed to refresh data after adding risk', 'error');
        }
    };

    const handleEditRisk = (risk) => {
        riskToEdit.value = risk;
        showEditRiskModal.value = true;
    };

    const handleCloseEditRiskModal = () => {
        showEditRiskModal.value = false;
        riskToEdit.value = null;
    };

    const handleRiskUpdated = async () => {
        const riskName = riskToEdit.value?.riskName || 'Unknown risk';
        await refreshData();
        emit('risk-updated');
        showNotification(`Risk "${riskName}" has been updated successfully!`, 'success');
        handleCloseEditRiskModal();
    };

    const handleDeleteRisk = (risk) => {
        itemToDelete.value = risk;
        deleteDialogTitle.value = 'Delete Risk';
        deleteDialogMessage.value = `Are you sure you want to delete the risk "${risk.riskName}"?`;
        showDeleteDialog.value = true;
    };

    const confirmDelete = async () => {
        if (itemToDelete.value) {
            const deletedRisk = { ...itemToDelete.value };
            const result = await deleteRisk(deletedRisk.id);

            if (result.success) {
                showNotification(
                    `Risk "${deletedRisk.riskName}" has been deleted successfully!`,
                    'success'
                );
                localStorage.removeItem('riskControlSelectedRiskId');

            setTimeout(async () => {
                await refreshData();
                emit('risk-deleted', deletedRisk.id);
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
        showAddRiskModal,
        showEditRiskModal,
        riskToEdit,
        showDeleteDialog,
        deleteDialogTitle,
        deleteDialogMessage,
        itemToDelete,
        handleAddRisk,
        handleCloseAddRiskModal,
        handleRiskAdded,
        handleEditRisk,
        handleCloseEditRiskModal,
        handleRiskUpdated,
        handleDeleteRisk,
        confirmDelete,
        cancelDelete,
    };
}
