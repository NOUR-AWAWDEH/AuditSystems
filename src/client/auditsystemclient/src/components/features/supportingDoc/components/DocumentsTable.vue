<script setup>
import { computed } from "vue";
import ReusableTable from '@/components/common/ReusableTable.vue';
import { useNotification } from "@/composables/notification/useNotification";
import { useDeleteSupportingDocument } from "@/composables/supportingDoc/useDeleteSupportingDocument";

const props = defineProps({
  documents: {
    type: Array,
    required: true
  }
});

const emit = defineEmits(['delete-document']);

const { showNotification } = useNotification();
const { isDeleting, deleteSupportingDocument } = useDeleteSupportingDocument();

const documentTableColumns = [
  { key: 'fileName', label: 'File Name' },
  { key: 'fileType', label: 'File Type' },
  { key: 'createdAt', label: 'Upload Date', type: 'date' }
];

const documentTableActions = [
  { label: 'Delete', event: 'delete-item', class: 'font-medium text-red-600 dark:text-red-500 hover:underline' },
];

async function handleDeleteDocument(document) {
  try {
    const result = await deleteSupportingDocument(document.id);
    
    if (result.success) {
      // Remove from local state
      emit('delete-document', document.id);
      showNotification(`Document "${document.fileName}" deleted successfully`, 'success');
    } else {
      showNotification(result.message || 'Delete failed', 'error');
    }
  } catch (error) {
    console.error('Delete error:', error);
    showNotification('Delete failed', 'error');
  }
}
</script>

<template>
  <div class="mt-6">
    <h3 class="text-xl font-semibold mb-3 text-secondary-base">Uploaded Documents</h3>
    <ReusableTable
      :columns="documentTableColumns"
      :items="documents"
      item-key="id"
      :is-loading="isDeleting"
      no-data-message="No documents uploaded yet."
      :actions="documentTableActions"
      @delete-item="handleDeleteDocument"
    >
      <!-- Custom slot for file type display -->
      <template #cell-fileType="{ item }">
        {{ getFileType(item.fileName) }}
      </template>
      
      <!-- Custom slot for upload date display -->
      <template #cell-createdAt="{ value }">
        {{ formatDate(value) }}
      </template>
    </ReusableTable>
  </div>
</template>

<script>
// Helper functions for formatting
function getFileType(fileName) {
  if (!fileName) return 'Unknown';
  const extension = fileName.split('.').pop();
  return extension ? extension.toUpperCase() : 'Unknown';
}

function formatDate(dateString) {
  if (!dateString) return 'N/A';
  try {
    return new Date(dateString).toLocaleDateString();
  } catch {
    return 'Invalid Date';
  }
}
</script>