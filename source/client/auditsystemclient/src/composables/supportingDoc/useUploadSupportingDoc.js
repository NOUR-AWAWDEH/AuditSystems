import { ref } from 'vue';
import { uploadSupportingDocRepository } from '@/services/supportingDocs/uploadSupportingDocRepository';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';
import authService from '@/services/auth/authRepos/authService';
import { useNotification } from '@/composables/notification/useNotification';

export function useUploadSupportingDoc() {

  const uploadStatus = ref(null);
  const isUploading = ref(false);
  const isScanning = ref(false);
  const uploadProgress = ref(0);
  const error = ref(null);
  const { authStatus, checkAuthStatus } = useAuthStatus();
  const { showNotification } = useNotification();

  const uploadSupportingDocument = async ({ file, fileName }) => {
    isUploading.value = true;
    isScanning.value = false;
    uploadProgress.value = 0;
    error.value = null;
    uploadStatus.value = null;

    showNotification(`Uploading "${fileName}"...`, 'info', 0);

    try {
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

      uploadProgress.value = 50;

      // Start the actual network request
      const responsePromise = uploadSupportingDocRepository.uploadSupportingDoc({
        file,
        userId,
        fileName,
        token
      });

      // Simulation of progress while waiting for network/scanning
      uploadProgress.value = 70;
      
      const response = await responsePromise;

      uploadProgress.value = 90;
      isScanning.value = true; // Enter scanning simulation phase
      
      // Deliberate delay to show the "Scanning for malware" phase
      await new Promise(resolve => setTimeout(resolve, 2000));

      if (response && response.success) {
        isScanning.value = false;
        uploadProgress.value = 100;
        uploadStatus.value = { success: true, message: 'Document uploaded successfully', result: response.result };
        showNotification(`"${fileName}" uploaded and scanned successfully!`, 'success', 3000);
        
        // Show the 100% success state for a moment before closing/resetting
        await new Promise(resolve => setTimeout(resolve, 1500));
        
        return response;
      } else {
        const errorMessage = response?.message || response?.errors?.[0]?.message || 'Upload failed';
        error.value = errorMessage;
        uploadStatus.value = { success: false, message: errorMessage, errors: response?.errors };
        showNotification(`Upload failed: ${errorMessage}`, 'error', 5000);
        return response;
      }
    } catch (err) {
      const errorMessage = err.message || 'An unexpected error occurred during upload';
      error.value = errorMessage;
      uploadStatus.value = { success: false, message: errorMessage, errors: [{ message: errorMessage }] };
      showNotification(`Upload failed: ${errorMessage}`, 'error', 5000);
      return { success: false, message: errorMessage };
    } finally {
      isUploading.value = false;
      isScanning.value = false;
      uploadProgress.value = 0;
    }
  };

  const resetUploadState = () => {
    uploadStatus.value = null;
    error.value = null;
    uploadProgress.value = 0;
    isUploading.value = false;
    isScanning.value = false;
  };

  return {
    uploadStatus,
    isUploading,
    isScanning,
    uploadProgress,
    error,
    uploadSupportingDocument,
    resetUploadState,
  };
}