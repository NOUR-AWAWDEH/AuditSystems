<script setup>
import { ref } from "vue";
import { useUploadSupportingDocument } from "@/composables/supportingDoc/useUploadSupportingDocuments";
import { useNotification } from "@/composables/notification/useNotification";
import FileUploadDialog from "./FileUploadDialog.vue";
import { getFileType } from "../utils/fileUtils";

const emit = defineEmits(['document-uploaded']);

const { uploadSupportingDocument, isUploading, error: uploadError } = useUploadSupportingDocument();
const { showNotification } = useNotification();

const uploadDialogOpen = ref(false);

function openUploadDialog() {
  uploadDialogOpen.value = true;
}

function closeUploadDialog() {
  uploadDialogOpen.value = false;
}

async function handleFileUpload({ file, fileName }) {
  try {
    const result = await uploadSupportingDocument({
      file,
      fileName,
    });

    if (result.success) {
      const newDocument = {
        id: Date.now(),
        fileName,
        fileType: getFileType(fileName),
        uploadDate: new Date().toISOString(),
        size: file.size
      };
      
      emit('document-uploaded', newDocument);
      closeUploadDialog();
      showNotification("Document uploaded successfully", "success");
    } else {
      showNotification(result.message || uploadError.value || "Upload failed", "error");
    }
  } catch (err) {
    console.error("Error in uploadFile function:", err);
    showNotification(err.message || "Upload failed", "error");
  }
}
</script>

<template>
  <div class="mb-4 flex flex-col md:flex-row md:items-center md:justify-between gap-3">
    <button
      @click="openUploadDialog"
      class="bg-primary-base text-white px-4 py-2 rounded hover:bg-primary-dark transition duration-200 flex items-center justify-center gap-2"
    >
      <span>Upload Document</span>
    </button>
  </div>

  <FileUploadDialog
    v-if="uploadDialogOpen"
    :is-uploading="isUploading"
    @close="closeUploadDialog"
    @upload="handleFileUpload"
  />
</template>