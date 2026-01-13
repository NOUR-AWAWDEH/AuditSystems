<template>
    <!--CompanyView.vue-->
    <div class="space-y-6">
        <!-- Add Company Modal -->
        <AddCompanyModal
            :is-visible="showAddCompanyModal"
            @close="handleCloseAddCompanyModal"
            @company-added="handleCompanyAdded"
        />

        <!-- Company Management -->
        <CompanyTable
            :companies="companies"
            :is-loading="isLoading"
            :search-query="searchQuery"
            :handle-search="handleSearch"
            @add-company="handleAddCompany"
            @edit-company="handleEditCompany"
            @delete-company="handleDeleteCompany"
            @company-selected="handleCompanySelected"
        />

         <!-- No Company Exist (Optional: Can be handled inside Table or here if list is empty and not searching) -->
        <NoCompanyExist v-if="!isLoading && companies.length === 0 && !searchQuery" />

        <!-- Edit Company Modal -->
        <EditCompanyModal
            v-if="showEditCompanyModal"
            :is-visible="showEditCompanyModal"
            :company="companyToEdit"
            @close="handleCloseEditCompanyModal"
            @company-updated="handleCompanyUpdated"
        />

        <!-- Confirmation Dialog -->
        <ConfirmDialog
            v-model="showDeleteDialog"
            :title="deleteDialogTitle"
            :message="deleteDialogMessage"
            :highlight-text="itemToDelete?.name || ''"
            :confirm-text="'Delete'"
            :cancel-text="'Cancel'"
            @confirm="confirmDelete"
            @cancel="cancelDelete"
        />
    </div>
</template>

<script setup>
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import CompanyTable from './components/CompanyTable.vue';
import AddCompanyModal from './components/AddCompanyModal.vue';
import EditCompanyModal from './components/EditCompanyModal.vue';
import { useCompanyTable } from './composables/useCompanyTable';
import { useCompanyModals } from './composables/useCompanyModals';
import NoCompanyExist from './components/NoCompanyExist.vue'

// Define emit first
const emit = defineEmits([
    'company-selected',
    'company-added',
    'company-updated',
    'company-deleted',
]);

// Use composables
const {
    companies,
    isLoading,
    searchQuery,
    handleSearch,
    resetToDefaultView,
} = useCompanyTable();

const {
    showAddCompanyModal,
    showEditCompanyModal,
    companyToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddCompany,
    handleCloseAddCompanyModal,
    handleEditCompany,
    handleCloseEditCompanyModal,
    handleCompanyAdded,
    handleCompanyUpdated,
    handleDeleteCompany,
    confirmDelete,
    cancelDelete
} = useCompanyModals(companies, emit);

// Handle company selection and forward to parent
const handleCompanySelected = (companyId) => {
    emit('company-selected', companyId);
};

defineExpose({
    companies,
    resetToDefaultView
});
</script>

<style scoped>
.company-table .table-row {
    transition: all 0.2s ease;
}
</style>
