<template>
  <!--DocumentUploadSection.vue-->
  <FileUploadDialog
    v-if="uploadDialogOpen"
    :is-uploading="isUploading"
    :upload-progress="uploadProgress"
    @close="closeUploadDialog"
    @upload="handleFileUpload"
  />
</template>

<script setup>
import { ref } from "vue";
import { useUploadSupportingDoc } from "@/composables/supportingDoc/useUploadSupportingDoc";
import { useGetSupportingDocsByUserId } from "@/composables/supportingDoc/useGetSupportingDocsByUserId";
import { useAuthStatus } from "@/composables/auth/useAuthStatus";
import { useNotification } from "@/composables/notification/useNotification";
import FileUploadDialog from "./FileUploadDialog.vue";

const emit = defineEmits(['document-uploaded', 'documents-refreshed']);

const { uploadSupportingDocument, isUploading, uploadProgress, error: uploadError } = useUploadSupportingDoc();
const { fetchDocumentsByUserId } = useGetSupportingDocsByUserId();
const { authStatus } = useAuthStatus();
const { showNotification, hideNotification } = useNotification();

// Changed to true to show dialog automatically
const uploadDialogOpen = ref(true);

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
      // Fetch updated documents list after successful upload
      if (authStatus.value?.userId) {
        const documentsResponse = await fetchDocumentsByUserId(authStatus.value.userId);
        if (documentsResponse?.success) {
          emit('documents-refreshed', documentsResponse.result);
        }
      }
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