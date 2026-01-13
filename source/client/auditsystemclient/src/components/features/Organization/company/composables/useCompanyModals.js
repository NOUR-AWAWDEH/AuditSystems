import { ref } from "vue";
import { useDeleteCompanyById } from "@/composables/organization/company/useDeleteCompanyById";
import { useNotification } from "@/composables/notification/useNotification";

export function useCompanyModals(companies, emit) {
  const { deleteCompany } = useDeleteCompanyById();
  const { showNotification } = useNotification();

  const showAddCompanyModal = ref(false);
  const showEditCompanyModal = ref(false);
  const companyToEdit = ref(null);
  const showDeleteDialog = ref(false);
  const deleteDialogTitle = ref("");
  const deleteDialogMessage = ref("");
  const itemToDelete = ref(null);

  const handleAddCompany = () => {
    showAddCompanyModal.value = true;
  };

  const handleCloseAddCompanyModal = () => {
    showAddCompanyModal.value = false;
  };

  const handleCompanyAdded = async (newCompany) => {
    try {
      companies.value.push(newCompany);
      emit("company-added", newCompany);
      handleCloseAddCompanyModal();
    } catch (error) {
      console.error("Error in handleCompanyAdded:", error);
      showNotification("Failed to refresh data after adding company", "error");
    }
  };

  const handleEditCompany = (company) => {
    companyToEdit.value = company;
    showEditCompanyModal.value = true;
  };

  const handleCloseEditCompanyModal = () => {
    showEditCompanyModal.value = false;
    companyToEdit.value = null;
  };

  const handleCompanyUpdated = async (updatedCompany) => {
    try {
      // Find and replace the updated company in the local list
      const index = companies.value.findIndex((p) => p.id === updatedCompany.id);

      if (index !== -1) {
        companies.value[index] = { ...companies.value[index], ...updatedCompany };
      }

      emit("company-updated", updatedCompany);

      showNotification(
        `Company "${updatedCompany.name}" has been updated successfully!`,
        "success"
      );

      handleCloseEditCompanyModal();
    } catch (error) {
      console.error("Error in handleCompanyUpdated:", error);
      showNotification("Failed to update company", "error");
    }
  };

  const handleDeleteCompany = (company) => {
    itemToDelete.value = company;
    deleteDialogTitle.value = "Delete Company";
    deleteDialogMessage.value = `Are you sure you want to delete the company "${company.name}"?`;
    showDeleteDialog.value = true;
  };

  const confirmDelete = async () => {
    if (!itemToDelete.value) return;

    const deletedCompany = { ...itemToDelete.value };
    const result = await deleteCompany(deletedCompany.id);

    if (result.success) {
      // Remove the deleted company from the local list
      const index = companies.value.findIndex((p) => p.id === deletedCompany.id);
      if (index !== -1) {
        companies.value.splice(index, 1); // <-- reactive change
      }

      showNotification(
        `Company "${deletedCompany.name}" has been deleted successfully!`,
        "success"
      );

      localStorage.removeItem("organizationSelectedCompanyId");

      emit("company-deleted", deletedCompany.id);
    } else {
      showNotification(result.message, "error");
    }
    cancelDelete();
  };

  const cancelDelete = () => {
    showDeleteDialog.value = false;
    itemToDelete.value = null;
    deleteDialogTitle.value = "";
    deleteDialogMessage.value = "";
  };

  return {
    showAddCompanyModal,
    showEditCompanyModal,
    companyToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddCompany,
    handleCloseAddCompanyModal,
    handleCompanyAdded,
    handleEditCompany,
    handleCloseEditCompanyModal,
    handleCompanyUpdated,
    handleDeleteCompany,
    confirmDelete,
    cancelDelete,
  };
}
