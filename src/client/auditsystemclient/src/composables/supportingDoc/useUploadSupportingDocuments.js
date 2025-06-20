// src/composables/supportingDoc/useSupportingDoc.js

import { ref } from 'vue';
import { uploadSupportingDocumentRepository } from '@/services/supportingDocs/uploadSupportingDocumentRepository';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';
import authService from '@/services/auth/authRepos/authService';
import { supportingDocsRepository } from '@/services/supportingDocs/fetchSupportingDocumentsRepository';

export function useUploadSupportingDocument() {
  const uploadStatus = ref(null);
  const isUploading = ref(false);
  const error = ref(null);
  const { authStatus, checkAuthStatus } = useAuthStatus();

  const uploadSupportingDocument = async ({ file, fileName }) => {
    isUploading.value = true;
    error.value = null;
    uploadStatus.value = null;

    try {
      const token = authService.getLocalToken();
      if (!token) {
        throw new Error('No authentication token found');
      }

      if (!authStatus.value || !authStatus.value.userId) {
        const statusResult = await checkAuthStatus(token);
        if (!statusResult || !statusResult.userId) {
          throw new Error('Failed to get user ID. Please log in again.');
        }
      }

      const userId = authStatus.value.userId;
      if (!userId) {
        throw new Error('User ID not found');
      }

      const response = await uploadSupportingDocumentRepository.uploadSupportingDocument({
        file,
        userId,
        fileName,
        token,
      });

      if (response?.success) {
        // After successful upload, fetch updated documents list
        const documentsResponse = await supportingDocsRepository.getSupportingDocumentsByUserId(userId);
        
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
    error,
  };
}
