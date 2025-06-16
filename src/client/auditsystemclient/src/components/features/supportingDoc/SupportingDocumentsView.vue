// src/components/SupportingDocManager.vue

<script setup>
import { ref, onMounted } from "vue";
import { useSupportingDoc } from "@/composables/supportingDoc/useUploadSupportingDocuments";
import { useFetchSupportingDocuments } from "@/composables/supportingDoc/useFetchSupportingDocuments"; // Import the fetch composable
import { useNotification } from "@/composables/notification/useNotification"; // Import the composable
import SideBar from "@/components/common/SideBar.vue";
import { useAuthStatus } from '@/composables/auth/useAuthStatus'; // To get userId
import ReusableTable from '@/components/common/ReusableTable.vue';

const { uploadSupportingDoc, isUploading, error: uploadError } = useSupportingDoc();
const { documents, isLoading: isLoadingDocuments, error: fetchError, fetchDocumentsByUserId } = useFetchSupportingDocuments();
const { showNotification } = useNotification(); // Use the composable
const { authStatus, checkAuthStatus } = useAuthStatus();

const uploadDialogOpen = ref(false);
const selectedFile = ref(null);
const fileInput = ref(null);
const customFileName = ref(''); // Add custom file name input

function openUploadDialog() {
  uploadDialogOpen.value = true;
}

function closeUploadDialog() {
  uploadDialogOpen.value = false;
  selectedFile.value = null;
  customFileName.value = ''; // Clear custom file name
}

function triggerFileInput() {
  fileInput.value.click();
}

function handleFileChange(e) {
  const file = e.target.files[0];
  if (file) {
    selectedFile.value = file;
    // Auto-populate custom file name with original name (without extension)
    const nameWithoutExtension = file.name.substring(0, file.name.lastIndexOf('.'));
    customFileName.value = nameWithoutExtension;
  }
}

function handleFileDrop(e) {
  const file = e.dataTransfer.files[0];
  if (file) {
    selectedFile.value = file;
    // Auto-populate custom file name with original name (without extension)
    const nameWithoutExtension = file.name.substring(0, file.name.lastIndexOf('.'));
    customFileName.value = nameWithoutExtension;
  }
}

function clearSelectedFile() {
  selectedFile.value = null;
  customFileName.value = ''; // Clear custom file name
  if (fileInput.value) {
    fileInput.value.value = "";
  }
}

async function uploadFile() {
  if (!selectedFile.value) return;

  try {
    // Use custom file name if provided, otherwise use original file name
    const fileExtension = selectedFile.value.name.substring(selectedFile.value.name.lastIndexOf('.'));
    const finalFileName = customFileName.value.trim() 
      ? `${customFileName.value.trim()}${fileExtension}`
      : selectedFile.value.name;

    const result = await uploadSupportingDoc({
      file: selectedFile.value,
      fileName: finalFileName,
    });

    if (result.success) {
      selectedFile.value = null;
      customFileName.value = '';
      uploadDialogOpen.value = false;
      showNotification("Document uploaded successfully", "success");
      // Refresh the document list after successful upload
      if (authStatus.value?.userId) {
        fetchDocumentsByUserId(authStatus.value.userId);
        console.log(authStatus.value.userId);
      }
    } else {
      showNotification(result.message || uploadError.value || "Upload failed", "error");
    }
  } catch (err) {
    console.error("Error in uploadFile function:", err);
    showNotification(err.message || "Upload failed", "error");
  }
}

onMounted(async () => {
  await checkAuthStatus(); // Ensure auth status is checked and userId is available
  if (authStatus.value?.userId) {
    fetchDocumentsByUserId(authStatus.value.userId);
  } else {
    showNotification("User ID not found. Cannot fetch documents.", "error");
    console.error('User ID not available in onMounted');
  }
});

const documentTableColumns = [
  { key: 'fileName', label: 'File Name' },
  { key: 'uploadDate', label: 'Upload Date' },
];

const documentTableActions = [
  { label: 'View', event: 'view-item', class: 'font-medium text-blue-600 dark:text-blue-500 hover:underline mr-2' },
  { label: 'Delete', event: 'delete-item', class: 'font-medium text-red-600 dark:text-red-500 hover:underline' },
];

const handleViewDocument = (document) => {
  showNotification(`Viewing document: ${document.fileName}`, 'info');
  // Implement actual view logic here, e.g., open in new tab or modal
};

const handleDeleteDocument = (document) => {
  showNotification(`Deleting document: ${document.fileName}`, 'warning');
  // Implement actual delete logic here
};
</script>

<template>
  <div class="flex h-screen">
    <SideBar />
    <div class="flex-1 p-6">
      <h2 class="text-2xl font-bold mb-4">Supporting Documents</h2>
      <!-- Upload Controls -->
      <div class="mb-4 flex flex-col md:flex-row md:items-center md:justify-between gap-3">
        <button
          @click="openUploadDialog"
          class="bg-primary-base text-white px-4 py-2 rounded hover:bg-primary-dark transition duration-200 flex items-center justify-center gap-2"
        >
          <span>Upload Document</span>
        </button>
      </div>
      <!-- Upload Dialog -->
      <!-- Display Documents Section -->
      <div class="mt-6">
        <h3 class="text-xl font-semibold mb-3">Uploaded Documents</h3>
        <div v-if="isLoadingDocuments" class="text-center py-4">
          <p class="text-gray-500">Loading documents...</p>
        </div>
        <div v-else-if="fetchError" class="text-center py-4 bg-red-100 border border-red-400 text-red-700 px-4 rounded">
          <p>Error fetching documents: {{ fetchError }}</p>
        </div>
        <div v-else-if="documents.length === 0" class="text-center py-4">
          <p class="text-gray-500">No documents found.</p>
        </div>
        <ReusableTable
          :columns="documentTableColumns"
          :items="documents"
          item-key="id"
          :is-loading="isLoadingDocuments"
          no-data-message="No documents found."
          :actions="documentTableActions"
          @view-item="handleViewDocument"
          @delete-item="handleDeleteDocument"
        >
          <template #cell-uploadDate="{ value }">
            {{ value ? new Date(value).toLocaleDateString() : 'N/A' }}
          </template>
        </ReusableTable>
      </div>

      <!-- Upload Dialog -->
      <div
        v-if="uploadDialogOpen"
        class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
      >
        <div class="bg-white rounded-lg shadow-xl p-6 w-full max-w-md mx-4">
          <div class="flex justify-between items-center mb-4">
            <h3 class="text-xl font-semibold">Upload Document</h3>
            <button @click="closeUploadDialog" class="text-gray-500 hover:text-gray-700">×</button>
          </div>
          <div
            @click="triggerFileInput"
            @dragover.prevent
            @drop.prevent="handleFileDrop"
            class="border-2 border-dashed border-gray-400 rounded-lg p-8 flex flex-col items-center justify-center cursor-pointer bg-gray-50 hover:bg-gray-100 transition duration-200"
          >
            <div class="text-center">
              <span class="text-lg text-gray-500 block mb-1">Drag and drop your file here</span>
              <span class="text-sm text-gray-400 block mb-2">or</span>
              <span class="text-primary-base underline">Browse files</span>
            </div>
          </div>
          <input ref="fileInput" type="file" class="hidden" @change="handleFileChange" />
          <div v-if="selectedFile" class="mt-4">
            <div class="flex items-center justify-between bg-gray-100 p-2 rounded mb-3">
              <span class="font-medium truncate flex-1">{{ selectedFile.name }}</span>
              <button @click="clearSelectedFile" class="text-red-500 ml-2">×</button>
            </div>
            
            <!-- Custom File Name Input -->
            <div class="mb-4">
              <label for="customFileName" class="block text-sm font-medium text-gray-700 mb-2">
                Custom File Name (optional)
              </label>
              <input
                id="customFileName"
                v-model="customFileName"
                type="text"
                placeholder="Enter custom file name"
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary-base focus:border-transparent"
              />
              <p class="text-xs text-gray-500 mt-1">
                File extension will be added automatically. Leave empty to use original name.
              </p>
            </div>
            
            <div class="flex justify-end mt-4 gap-3">
              <button
                @click="closeUploadDialog"
                class="border border-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-100"
              >
                Cancel
              </button>
              <button
                @click="uploadFile"
                :disabled="isUploading || !selectedFile"
                class="bg-primary-base text-white px-4 py-2 rounded hover:bg-primary-dark disabled:opacity-50 disabled:cursor-not-allowed"
              >
                <span v-if="isUploading">Uploading...</span>
                <span v-else>Upload</span>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-leave-to,
.fade-enter-from {
  opacity: 0;
}
</style>
