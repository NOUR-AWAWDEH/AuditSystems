<script setup>
import ReusableTable from '@/components/common/ReusableTable.vue';
import { useNotification } from "@/composables/notification/useNotification";
import { useGetSupportingDocSignedUrl } from "@/composables/supportingDoc/useGetSupportingDocSignedUrl";

defineProps({
  documents: {
    type: Array,
    required: true
  }
});

const emit = defineEmits(['delete-document']);

const { showNotification } = useNotification();
const { 
  isLoading: isDownloadLoading, 
  downloadSupportingDocument,  // Changed from getSignedUrl
  resetAllStates 
} = useGetSupportingDocSignedUrl();

const documentTableColumns = [
  { key: 'fileName', label: 'File Name' },
  { key: 'fileType', label: 'File Type' },
  { key: 'createdAt', label: 'Upload Date', type: 'date' }
];

const documentTableActions = [
  { label: 'Download', event: 'download-item', class: 'font-medium text-blue-600 dark:text-blue-500 hover:underline mr-4' },
  { label: 'Delete', event: 'delete-item', class: 'font-medium text-red-600 dark:text-red-500 hover:underline' },
];

// Just emit the delete event to parent - let parent handle confirmation and actual deletion
function handleDeleteDocument(document) {
  emit('delete-document', document);
}

async function handleDownloadDocument(document) {
  try {
    resetAllStates();
    
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
        :is-loading="isDownloadLoading"
        no-data-message="No documents uploaded yet."
        :actions="documentTableActions"
        @delete-item="handleDeleteDocument"
        @download-item="handleDownloadDocument"
        class="w-full"
      >
        <template #cell-fileName="{ item }">
          <div class="max-w-xs md:max-w-none">
            <span class="block truncate md:inline" :title="getCleanFileName(item.fileName)">
              {{ getCleanFileName(item.fileName) }}
            </span>
          </div>
        </template>
        
        <template #cell-fileType="{ item }">
          <span class="inline-flex items-center px-2 py-1 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
            {{ getFileType(item.fileName) }}
          </span>
        </template>
        
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
function getCleanFileName(fileName) {
  if (!fileName) return 'Unknown';
  
  const regex = /^(.+)_[a-f0-9]{32}(\.[^.]+)$/i;
  const match = fileName.match(regex);
  
  if (match) {
    return match[1] + match[2];
  }
  
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
@media (max-width: 768px) {
  .max-w-xs {
    max-width: 20rem;
  }
}
</style>