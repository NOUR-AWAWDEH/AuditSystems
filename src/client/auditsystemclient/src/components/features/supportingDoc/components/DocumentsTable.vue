<script setup>
import ReusableTable from '@/components/common/ReusableTable.vue';
import { useNotification } from "@/composables/notification/useNotification";
import { useDeleteSupportingDocument } from "@/composables/supportingDoc/useDeleteSupportingDocument";
import { useSupportingDocDownload } from "@/composables/supportingDoc/useSupportingDocDownload";

defineProps({
  documents: {
    type: Array,
    required: true
  }
});

const emit = defineEmits(['delete-document']);

const { showNotification } = useNotification();
const { isDeleting, deleteSupportingDocument } = useDeleteSupportingDocument();
const { 
  isLoading: isDownloadLoading, 
  downloadSupportingDocument,
  resetAllStates 
} = useSupportingDocDownload();

const documentTableColumns = [
  { key: 'fileName', label: 'File Name' },
  { key: 'fileType', label: 'File Type' },
  { key: 'createdAt', label: 'Upload Date', type: 'date' }
];

const documentTableActions = [
  { label: 'Download', event: 'download-item', class: 'font-medium text-blue-600 dark:text-blue-500 hover:underline mr-4' },
  { label: 'Delete', event: 'delete-item', class: 'font-medium text-red-600 dark:text-red-500 hover:underline' },
];

async function handleDeleteDocument(document) {
  try {
    const result = await deleteSupportingDocument(document.id);
    
    if (result.success) {
      // Remove from local state
      emit('delete-document', document.id);
      showNotification(`Document "${getCleanFileName(document.fileName)}" deleted successfully`, 'success');
    } else {
      showNotification(result.message || 'Delete failed', 'error');
    }
  } catch {
    showNotification('Delete failed', 'error');
  }
}

async function handleDownloadDocument(document) {
  try {
    resetAllStates();
    
    // Don't pass clean filename - let backend handle the original filename
    const result = await downloadSupportingDocument(document.id);
    
    if (result?.success) {
      showNotification(`Document "${getCleanFileName(document.fileName)}" downloaded successfully`, 'success');
    } else {
      const errorMessage = result?.errors?.[0]?.message || result?.message || 'Download failed';
      showNotification(errorMessage, 'error');
    }
  } catch {
    showNotification('Download failed', 'error');
  }
}
</script>

<template>
  <div class="w-full">
    <h3 class="text-lg md:text-xl font-semibold mb-3 md:mb-4 text-secondary-base px-1">Uploaded Documents</h3>
    <div class="w-full overflow-hidden">
      <ReusableTable
        :columns="documentTableColumns"
        :items="documents"
        item-key="id"
        :is-loading="isDeleting || isDownloadLoading"
        no-data-message="No documents uploaded yet."
        :actions="documentTableActions"
        @delete-item="handleDeleteDocument"
        @download-item="handleDownloadDocument"
        class="w-full"
      >
        <!-- Custom slot for clean file name display -->
        <template #cell-fileName="{ item }">
          <div class="max-w-xs md:max-w-none">
            <span class="block truncate md:inline" :title="getCleanFileName(item.fileName)">
              {{ getCleanFileName(item.fileName) }}
            </span>
          </div>
        </template>
        
        <!-- Custom slot for file type display -->
        <template #cell-fileType="{ item }">
          <span class="inline-flex items-center px-2 py-1 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
            {{ getFileType(item.fileName) }}
          </span>
        </template>
        
        <!-- Custom slot for upload date display -->
        <template #cell-createdAt="{ value }">
          <span class="text-sm text-gray-600">
            {{ formatDate(value) }}
          </span>
        </template>
      </ReusableTable>
    </div>
  </div>
</template>

<script>
// Helper functions for formatting
function getCleanFileName(fileName) {
  if (!fileName) return 'Unknown';
  
  // Remove the unique ID pattern (underscore followed by 32 characters before the file extension)
  // Pattern: filename_32characterID.extension -> filename.extension
  const regex = /^(.+)_[a-f0-9]{32}(\.[^.]+)$/i;
  const match = fileName.match(regex);
  
  if (match) {
    return match[1] + match[2]; // Return original name + extension
  }
  
  // If pattern doesn't match, return original filename
  return fileName;
}

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

<style scoped>
/* Ensure proper mobile layout */
@media (max-width: 768px) {
  .max-w-xs {
    max-width: 20rem;
  }
}
</style>