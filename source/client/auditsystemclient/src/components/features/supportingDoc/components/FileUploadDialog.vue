<template>
  <!--FileUploadDialog.vue-->
    <div class="bg-white rounded-lg shadow-sm border border-gray-200 p-4 md:p-6 mb-6">
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between mb-4">
            <h2 class="text-lg font-semibold text-secondary-base mb-2 sm:mb-0">
                Upload Document
            </h2>
            <BaseButton
                @click="toggleUploadForm"
                :class="showUploadForm ? 'px-4 py-2 rounded-md text-sm font-medium bg-red-600 hover:bg-red-700 text-white' : 'px-4 py-2 rounded-md text-sm font-medium bg-primary-base'"
            >
                {{ showUploadForm ? 'Cancel' : 'Upload Document' }}
            </BaseButton>
        </div>

        <!-- Upload Document Form -->
        <Transition name="slide-down">
            <div v-if="showUploadForm" class="mt-4">
                <!-- Error Message -->
                <div v-if="errorMessage" class="mb-4 p-3 bg-red-50 border border-red-200 rounded-md">
                    <p class="text-red-600 text-sm">{{ errorMessage }}</p>
                </div>
                
                <!-- Progress Bar (shown when uploading) -->
                <div v-if="isUploading" class="mb-4">
                    <div class="flex justify-between items-center mb-2">
                        <span class="text-sm font-medium text-gray-700">
                            {{ isScanning ? 'Checking security...' : 'Uploading...' }}
                        </span>
                        <span class="text-sm text-gray-500">{{ uploadProgress }}%</span>
                    </div>
                    <div class="w-full bg-gray-200 rounded-full h-2">
                        <div 
                            class="h-2 rounded-full transition-all duration-300 ease-out"
                            :class="[
                                isScanning ? 'bg-indigo-600 animate-pulse' : 'bg-primary-base',
                                uploadProgress === 100 ? 'bg-green-500' : ''
                            ]"
                            :style="{ width: uploadProgress + '%' }"
                        ></div>
                    </div>
                    <div v-if="uploadProgress === 100 && !isScanning" class="text-center mt-2">
                        <span class="text-sm text-green-600 font-medium flex items-center justify-center">
                           <span class="mr-1">✓</span> Scan complete. Saving...
                        </span>
                    </div>
                    <div v-if="isScanning" class="text-center mt-2 flex items-center justify-center text-indigo-600">
                        <svg class="animate-spin -ml-1 mr-3 h-4 w-4 text-indigo-600" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                            <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                        </svg>
                        <span class="text-sm font-semibold italic">Scanning for malware...</span>
                    </div>
                </div>
                
                <!-- File Selection Area (hidden when uploading) -->
                <form v-if="!isUploading" @submit.prevent="handleUpload" class="space-y-4">
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
                    <div class="text-xs text-gray-500">
                        Allowed file types: {{ allowedTypes.join(', ') }}
                    </div>
                    
                    <div v-if="selectedFile">
                        <div class="flex items-center justify-between bg-gray-100 p-2 rounded mb-3">
                            <span class="font-medium truncate flex-1">{{ selectedFile.name }}</span>
                            <button type="button" @click="clearSelectedFile" class="text-red-500 ml-2">×</button>
                        </div>
                        
                        <BaseInput
                            id="custom-file-name"
                            v-model="customFileName"
                            label="Custom File Name (optional)"
                            placeholder="Enter custom file name"
                        />
                        <p class="text-xs text-gray-500 mt-1">
                            File extension will be added automatically. Leave empty to use original name.
                        </p>
                    </div>
                    
                    <div class="flex justify-end space-x-3">
                        <BaseButton
                            type="submit"
                            :loading="isUploading"
                            :disabled="!selectedFile"
                            class="px-6 py-2 bg-primary-base rounded"
                        >
                            {{ isUploading ? 'Uploading...' : 'Upload Document' }}
                        </BaseButton>
                    </div>
                </form>
            </div>
        </Transition>
    </div>
</template>

<script setup>
import { ref, watch } from 'vue';
import BaseButton from '@/components/base/BaseButton.vue';
import BaseInput from '@/components/base/BaseInput.vue';
import { validateFileType, validateFileSize, validateFileName } from '../utils/fileUtils.js';

const props = defineProps({
  isUploading: {
    type: Boolean,
    default: false
  },
  isScanning: {
    type: Boolean,
    default: false
  },
  uploadProgress: {
    type: Number,
    default: 0
  }
});

const emit = defineEmits(['close', 'upload']);

const showUploadForm = ref(false);
const selectedFile = ref(null);
const fileInput = ref(null);
const customFileName = ref('');
const errorMessage = ref('');

// Define allowed file types based on your backend
const allowedTypes = ['pdf', 'docx', 'doc', 'docm', 'dotx', 'dotm', 'rtf','xlsx', 'xls', 'xlsm', 'xltx', 'xltm','pptx', 'ppt', 'ppsm', 'ppsx', 'potx', 'potm'];
const MAX_FILE_SIZE_MB = 200; // Match backend limit

const toggleUploadForm = () => {
    showUploadForm.value = !showUploadForm.value;
    if (!showUploadForm.value) {
        resetForm();
    }
};

const resetForm = () => {
    clearSelectedFile();
};

function triggerFileInput() {
  fileInput.value.click();
}

function validateFile(file) {
  errorMessage.value = '';
  
  if (!validateFileType(file, allowedTypes)) {
    errorMessage.value = `Invalid file type. Allowed types: ${allowedTypes.join(', ')}`;
    return false;
  }

  if (!validateFileSize(file, MAX_FILE_SIZE_MB)) {
    errorMessage.value = `File too large. Maximum size allowed is ${MAX_FILE_SIZE_MB}MB.`;
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

  // Security: Validate the final filename
  const nameValidation = validateFileName(finalFileName);
  if (!nameValidation.isValid) {
    errorMessage.value = nameValidation.error;
    return;
  }

  emit('upload', {
    file: selectedFile.value,
    fileName: finalFileName
  });
}

// Watch for upload completion to reset the form automatically
watch(() => props.isUploading, (newVal, oldVal) => {
  // When uploading turns from true to false and there is no error, it means success
  if (oldVal === true && newVal === false && !errorMessage.value) {
    showUploadForm.value = false;
    resetForm();
  }
});
</script>

<style scoped>
.slide-down-enter-active,
.slide-down-leave-active {
    transition: all 0.3s ease;
    max-height: 500px;
    overflow: hidden;
}

.slide-down-enter-from,
.slide-down-leave-to {
    max-height: 0;
    opacity: 0;
    transform: translateY(-10px);
}
</style>