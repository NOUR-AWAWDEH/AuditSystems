<script setup>
import { ref } from "vue";
import { validateFileType } from '../utils/fileUtils.js';

const props = defineProps({
  isUploading: {
    type: Boolean,
    default: false
  },
  uploadProgress: {
    type: Number,
    default: 0
  }
});

const emit = defineEmits(['close', 'upload']);

const selectedFile = ref(null);
const fileInput = ref(null);
const customFileName = ref('');
const errorMessage = ref('');

// Define allowed file types based on your backend
const allowedTypes = ['jpg', 'jpeg', 'png', 'gif', 'pdf', 'docx', 'doc', 'xlsx', 'ppt', 'pptx', 'mp4', 'mkv'];

function triggerFileInput() {
  fileInput.value.click();
}

function validateFile(file) {
  errorMessage.value = '';
  
  if (!validateFileType(file, allowedTypes)) {
    errorMessage.value = `Invalid file type. Allowed types: ${allowedTypes.join(', ')}`;
    return false;
  }
  
  return true;
}

function handleFileChange(e) {
  const file = e.target.files[0];
  if (file) {
    if (validateFile(file)) {
      selectedFile.value = file;
      const nameWithoutExtension = file.name.substring(0, file.name.lastIndexOf('.'));
      customFileName.value = nameWithoutExtension;
    } else {
      // Clear the input if file is invalid
      if (fileInput.value) {
        fileInput.value.value = "";
      }
    }
  }
}

function handleFileDrop(e) {
  const file = e.dataTransfer.files[0];
  if (file) {
    if (validateFile(file)) {
      selectedFile.value = file;
      const nameWithoutExtension = file.name.substring(0, file.name.lastIndexOf('.'));
      customFileName.value = nameWithoutExtension;
    }
  }
}

function clearSelectedFile() {
  selectedFile.value = null;
  customFileName.value = '';
  errorMessage.value = '';
  if (fileInput.value) {
    fileInput.value.value = "";
  }
}

function handleUpload() {
  if (!selectedFile.value) return;

  const fileExtension = selectedFile.value.name.substring(selectedFile.value.name.lastIndexOf('.'));
  const finalFileName = customFileName.value.trim() 
    ? `${customFileName.value.trim()}${fileExtension}`
    : selectedFile.value.name;

  emit('upload', {
    file: selectedFile.value,
    fileName: finalFileName
  });

  // Reset form
  selectedFile.value = null;
  customFileName.value = '';
  errorMessage.value = '';
}

function handleClose() {
  clearSelectedFile();
  emit('close');
}
</script>

<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
    <div class="bg-white rounded-lg shadow-xl p-6 w-full max-w-md mx-4">
      <div class="flex justify-between items-center mb-4">
        <h3 class="text-xl font-semibold">Upload Document</h3>
        <button @click="handleClose" class="text-gray-500 hover:text-gray-700" :disabled="isUploading">×</button>
      </div>
      
      <!-- Error Message -->
      <div v-if="errorMessage" class="mb-4 p-3 bg-red-100 border border-red-400 text-red-700 rounded">
        {{ errorMessage }}
      </div>
      
      <!-- Progress Bar (shown when uploading) -->
      <div v-if="isUploading" class="mb-4">
        <div class="flex justify-between items-center mb-2">
          <span class="text-sm font-medium text-gray-700">Uploading...</span>
          <span class="text-sm text-gray-500">{{ uploadProgress }}%</span>
        </div>
        <div class="w-full bg-gray-200 rounded-full h-2">
          <div 
            class="bg-primary-base h-2 rounded-full transition-all duration-300 ease-out"
            :style="{ width: uploadProgress + '%' }"
          ></div>
        </div>
        <div v-if="uploadProgress === 100" class="text-center mt-2">
          <span class="text-sm text-green-600 font-medium">✓ Upload completed successfully!</span>
        </div>
      </div>
      
      <!-- File Selection Area (hidden when uploading) -->
      <div v-if="!isUploading">
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
        
        <!-- Allowed file types info -->
        <div class="mt-2 text-xs text-gray-500">
          Allowed file types: {{ allowedTypes.join(', ') }}
        </div>
        
        <div v-if="selectedFile" class="mt-4">
          <div class="flex items-center justify-between bg-gray-100 p-2 rounded mb-3">
            <span class="font-medium truncate flex-1">{{ selectedFile.name }}</span>
            <button @click="clearSelectedFile" class="text-red-500 ml-2">×</button>
          </div>
          
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
              @click="handleClose"
              class="border border-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-100"
            >
              Cancel
            </button>
            <button
              @click="handleUpload"
              :disabled="!selectedFile"
              class="bg-primary-base text-white px-4 py-2 rounded hover:bg-primary-dark disabled:opacity-50 disabled:cursor-not-allowed"
            >
              Upload
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>