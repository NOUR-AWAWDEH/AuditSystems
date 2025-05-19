// src/composables/supportingDoc/useSupportingDoc.js

import { ref } from 'vue';
import { supportingDocRepository } from '@/services/supportingDoc/supportingDocRepository';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';
import authService from '@/services/auth/authRepos/authService';

export function useSupportingDoc() {
  const uploadStatus = ref(null);
  const isUploading = ref(false);
  const error = ref(null);
  const { authStatus, checkAuthStatus } = useAuthStatus();

  const uploadSupportingDoc = async ({ file, fileName }) => {
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

      const response = await supportingDocRepository.uploadSupportingDoc({
        file,
        userId,
        fileName,
        token,
      });

      if (response?.success) {
        uploadStatus.value = {
          success: true,
          message: response.successMessage || 'File uploaded successfully',
          result: response.result,
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
    uploadSupportingDoc,
    uploadStatus,
    isUploading,
    error,
  };
}
