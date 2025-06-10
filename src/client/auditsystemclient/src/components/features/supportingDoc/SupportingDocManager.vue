// src/components/SupportingDocManager.vue

<script setup>
import { ref, reactive } from "vue";
import { useSupportingDoc } from "@/composables/supportingDoc/useSupportingDoc";
import SideBar from "@/components/common/SideBar.vue";

const { uploadSupportingDoc, isUploading } = useSupportingDoc();
const uploadDialogOpen = ref(false);
const selectedFile = ref(null);
const fileInput = ref(null);
const notification = reactive({
  show: false,
  message: "",
  type: "error",
  timeout: null,
});

function openUploadDialog() {
  uploadDialogOpen.value = true;
}

function closeUploadDialog() {
  uploadDialogOpen.value = false;
  selectedFile.value = null;
}

function triggerFileInput() {
  fileInput.value.click();
}

function handleFileChange(e) {
  const file = e.target.files[0];
  if (file) {
    selectedFile.value = file;
  }
}

function handleFileDrop(e) {
  const file = e.dataTransfer.files[0];
  if (file) {
    selectedFile.value = file;
  }
}

function clearSelectedFile() {
  selectedFile.value = null;
  if (fileInput.value) {
    fileInput.value.value = "";
  }
}

async function uploadFile() {
  if (!selectedFile.value) return;

  try {
    console.log("Uploading file:", selectedFile.value.name);

    const result = await uploadSupportingDoc({
      file: selectedFile.value,
      fileName: selectedFile.value.name,
    });

    if (result.success) {
      selectedFile.value = null;
      uploadDialogOpen.value = false;
      showNotification("Document uploaded successfully", "success");
    } else {
      showNotification(result.message || "Upload failed", "error");
    }
  } catch (err) {
    console.error("Error in uploadFile function:", err);
    showNotification(err.message || "Upload failed", "error");
  }
}

function showNotification(message, type = "error") {
  if (notification.timeout) {
    clearTimeout(notification.timeout);
  }
  notification.show = true;
  notification.message = message;
  notification.type = type;
  notification.timeout = setTimeout(() => {
    notification.show = false;
  }, 4000);
}

function closeNotification() {
  notification.show = false;
  if (notification.timeout) {
    clearTimeout(notification.timeout);
  }
}
</script>

<template>
  <div class="flex h-screen">
    <SideBar />
    <div class="flex-1 p-6">
      <h2 class="text-2xl font-bold mb-4">Supporting Documents</h2>
      <!-- Toast notifications -->
      <Transition name="fade">
        <div
          v-if="notification.show"
          class="fixed top-4 right-4 z-50 p-4 rounded-md shadow-md"
          :class="
            notification.type === 'error'
              ? 'bg-red-100 text-red-700 border border-red-300'
              : 'bg-green-100 text-green-700 border border-green-300'
          "
        >
          <div class="flex items-center">
            <span>{{ notification.message }}</span>
            <button @click="closeNotification" class="ml-3 font-bold">×</button>
          </div>
        </div>
      </Transition>
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
            <div class="flex items-center justify-between bg-gray-100 p-2 rounded">
              <span class="font-medium truncate flex-1">{{ selectedFile.name }}</span>
              <button @click="clearSelectedFile" class="text-red-500 ml-2">×</button>
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
