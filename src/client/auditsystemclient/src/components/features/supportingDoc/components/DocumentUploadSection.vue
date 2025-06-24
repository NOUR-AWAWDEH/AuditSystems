<script setup>
import { ref } from "vue";
import { useUploadSupportingDocument } from "@/composables/supportingDoc/useUploadSupportingDocuments";
import { useNotification } from "@/composables/notification/useNotification";
import FileUploadDialog from "./FileUploadDialog.vue";

const emit = defineEmits(['document-uploaded', 'documents-refreshed']);

const { uploadSupportingDocument, isUploading, uploadProgress, error: uploadError } = useUploadSupportingDocument();
const { showNotification, hideNotification } = useNotification();

// Changed to true to show dialog automatically
const uploadDialogOpen = ref(true);

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

    // Hide the uploading notification
    hideNotification();

    if (result.success) {
      emit('documents-refreshed', result.documents);
      showNotification("Document uploaded successfully", "success");
    } else {
      showNotification(result.message || uploadError.value || "Upload failed", "error");
    }
  } catch (err) {
    // Hide the uploading notification in case of error
    hideNotification();
    console.error("Error in uploadFile function:", err);
    showNotification(err.message || "Upload failed", "error");
  }
}
</script>

<template>
  <!-- Removed the upload button div -->
  
  <FileUploadDialog
    v-if="uploadDialogOpen"
    :is-uploading="isUploading"
    :upload-progress="uploadProgress"
    @close="closeUploadDialog"
    @upload="handleFileUpload"
  />
</template>