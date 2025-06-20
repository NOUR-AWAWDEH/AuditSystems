// src/composables/supportingDoc/useUploadSupportingDocuments.js

import { ref } from 'vue';
import { uploadSupportingDocumentRepository } from '@/services/supportingDocs/uploadSupportingDocumentRepository';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';
import authService from '@/services/auth/authRepos/authService';
import { supportingDocsRepository } from '@/services/supportingDocs/fetchSupportingDocumentsRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useUploadSupportingDocument() {
  const uploadStatus = ref(null);
  const isUploading = ref(false);
  const uploadProgress = ref(0);
  const error = ref(null);
  const { authStatus, checkAuthStatus } = useAuthStatus();
  const { showNotification } = useNotification();

  const uploadSupportingDocument = async ({ file, fileName }) => {
    isUploading.value = true;
    uploadProgress.value = 0;
    error.value = null;
    uploadStatus.value = null;

    showNotification(`Uploading "${fileName}"...`, 'info', 0);

    try {
      // Simulate progress during authentication and setup
      uploadProgress.value = 10;
      
      const token = authService.getLocalToken();
      if (!token) {
        throw new Error('No authentication token found');
      }

      uploadProgress.value = 20;

      if (!authStatus.value || !authStatus.value.userId) {
        const statusResult = await checkAuthStatus(token);
        if (!statusResult || !statusResult.userId) {
          throw new Error('Failed to get user ID. Please log in again.');
        }
      }

      uploadProgress.value = 30;

      const userId = authStatus.value.userId;
      if (!userId) {
        throw new Error('User ID not found');
      }

      // Start actual upload
      uploadProgress.value = 40;

      const response = await uploadSupportingDocumentRepository.uploadSupportingDocument({
        file,
        userId,
        fileName,
        token,
      });

      // Upload completed
      uploadProgress.value = 80;

      if (response?.success) {
        // Fetch updated documents list
        const documentsResponse = await supportingDocsRepository.getSupportingDocumentsByUserId(userId);
        
        // Complete progress
        uploadProgress.value = 100;
        
        uploadStatus.value = {
          success: true,
          message: response.successMessage || 'File uploaded successfully',
          result: response.result,
          documents: documentsResponse?.success ? documentsResponse.result : []
        };
        return uploadStatus.value;
      } else {
        error.value = response?.errors?.[0]?.message || 'Upload failed';
        uploadStatus.value = {
          success: false,
          message: error.value,
          errors: response?.errors,
        };
        return uploadStatus.value;
      }
    } catch (err) {
      console.error('Upload error:', err);
      error.value = err.message || 'Upload failed';
      uploadStatus.value = {
        success: false,
        message: error.value,
        errors: [{ message: error.value }],
      };
      return uploadStatus.value;
    } finally {
      isUploading.value = false;
    }
  };

  return {
    uploadSupportingDocument,
    uploadStatus,
    isUploading,
    uploadProgress,
    error,
  };
}
