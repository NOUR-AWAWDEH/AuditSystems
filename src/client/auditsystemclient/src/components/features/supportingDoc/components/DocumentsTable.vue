<script setup>
import { computed } from "vue";
import ReusableTable from '@/components/common/ReusableTable.vue';
import { useNotification } from "@/composables/notification/useNotification";

const props = defineProps({
  documents: {
    type: Array,
    required: true
  }
});

const emit = defineEmits(['delete-document']);

const { showNotification } = useNotification();

const documentTableColumns = [
  { key: 'fileName', label: 'File Name' },
  { key: 'fileType', label: 'File Type' },
  { key: 'uploadDate', label: 'Upload Date' },
];

const documentTableActions = [
  { label: 'Delete', event: 'delete-item', class: 'font-medium text-red-600 dark:text-red-500 hover:underline' },
];

function handleDeleteDocument(document) {
  emit('delete-document', document.id);
  showNotification(`Document "${document.fileName}" deleted successfully`, 'success');
}
</script>

<template>
  <div class="mt-6">
    <h3 class="text-xl font-semibold mb-3 text-secondary-base">Uploaded Documents</h3>
    <ReusableTable
      :columns="documentTableColumns"
      :items="documents"
      item-key="id"
      :is-loading="false"
      no-data-message="No documents found."
      :actions="documentTableActions"
      @delete-item="handleDeleteDocument"
    >
      <template #cell-uploadDate="{ value }">
        {{ value ? new Date(value).toLocaleDateString() : 'N/A' }}
      </template>
    </ReusableTable>
  </div>
</template>