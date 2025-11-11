import { ref } from 'vue';
import { useDeleteAuditProgramById } from '@/composables/riskControl/auditProgram/useDeleteAuditProgramById';
import { useNotification } from '@/composables/notification/useNotification';

export function useAuditProgramModals(auditPrograms, emit) {

    const { deleteAuditProgram } = useDeleteAuditProgramById();
    const { showNotification } = useNotification();

    const showAddAuditProgramModal = ref(false);
    const showEditAuditProgramModal = ref(false);
    const auditProgramToEdit = ref(null);
    const showDeleteDialog = ref(false);
    const deleteDialogTitle = ref('');
    const deleteDialogMessage = ref('');
    const itemToDelete = ref(null);

    const handleAddAuditProgram = () => {
        showAddAuditProgramModal.value = true;
    };

    const handleCloseAddAuditProgramModal = () => {
        showAddAuditProgramModal.value = false;
    };

    const handleAuditProgramAdded = async (newAuditProgram) => {
        try {
            auditPrograms.value.push(newAuditProgram);
            emit('audit-program-added', newAuditProgram);
            handleCloseAddAuditProgramModal();
        } catch (error) {
            console.error('Error in handleAuditProgramAdded:', error);
            showNotification('Failed to refresh data after adding audit program', 'error');
        }
    };

    const handleEditAuditProgram = (auditProgram) => {
        auditProgramToEdit.value = auditProgram;
        showEditAuditProgramModal.value = true;
    };

    const handleCloseEditAuditProgramModal = () => {
        showEditAuditProgramModal.value = false;
        auditProgramToEdit.value = null;
    };

    const handleAuditProgramUpdated = async (updatedAuditProgram) => {
       try {
        const index = auditPrograms.value.findIndex(obj => obj.id === updatedAuditProgram.id);
       
        if(index !== -1){
           auditPrograms.value[index] = { ...auditPrograms.value[index], ...updatedAuditProgram };
        }
        emit('audit-program-updated', updatedAuditProgram);

        showNotification(
            `Audit Program "${updatedAuditProgram.description}" has been updated successfully!`,
             'success'
        );

        handleCloseEditAuditProgramModal();
       }catch (error) {
            console.error('Error in handleAuditProgramUpdated:', error);
            showNotification('Failed to refresh data after updating audit program', 'error');
       }
    };

    const handleDeleteAuditProgram = (auditProgram) => {
        itemToDelete.value = auditProgram;
        deleteDialogTitle.value = 'Delete Audit Program';   
        deleteDialogMessage.value = `Are you sure you want to delete the audit program "${auditProgram.description}"?`;
        showDeleteDialog.value = true;
    };

    const confirmDelete = async () => {
      if (!itemToDelete.value) return;
      
        const deletedAuditProgram = { ...itemToDelete.value };
        const result = await deleteAuditProgram(deletedAuditProgram.id);

        if (result.success) {
            const index = auditPrograms.value.findIndex(obj => obj.id === deletedAuditProgram.id);
            if (index !== -1) {
                auditPrograms.value.splice(index, 1);
            }

            showNotification(
                `Audit Program "${deletedAuditProgram.description}" has been deleted successfully!`,
                'success'
            );

            localStorage.removeItem('riskControlSelectedAuditProgramId');
            
            emit('audit-program-deleted', deletedAuditProgram.id);    
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
        showAddAuditProgramModal,
        showEditAuditProgramModal,
        auditProgramToEdit,
        showDeleteDialog,
        deleteDialogTitle,
        deleteDialogMessage,
        itemToDelete,
        handleAddAuditProgram,
        handleCloseAddAuditProgramModal,
        handleAuditProgramAdded,
        handleEditAuditProgram,
        handleCloseEditAuditProgramModal,
        handleAuditProgramUpdated,
        handleDeleteAuditProgram,
        confirmDelete,
        cancelDelete
    };
}