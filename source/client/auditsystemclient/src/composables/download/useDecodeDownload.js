import { ref } from 'vue';
import { decodeDownloadRepository } from '@/services/download/decodeDownloadRepository';

export function useDecodeDownload() {
  const isDownloading = ref(false);
  const downloadError = ref(null);
  const downloadSuccess = ref(false);

  const downloadFileFromSignedUrl = async (signedUrl) => {
    try {
      isDownloading.value = true;
      downloadError.value = null;
      downloadSuccess.value = false;

      const result = await decodeDownloadRepository.downloadFileFromSignedUrl(signedUrl);
      
      // Handle ApiResponseT format
      if (result.success) {
        downloadSuccess.value = true;
        return {
          success: true,
          message: result.successMessage,
          result: result.result
        };
      } else {
        downloadError.value = result.successMessage || 'Download failed';
        return {
          success: false,
          message: result.successMessage || 'Download failed',
          errors: result.errors || [{ message: result.successMessage || 'Download failed' }]
        };
      }
    } catch (error) {
      downloadError.value = error.message || 'An unexpected error occurred';
      return {
        success: false,
        message: error.message || 'An unexpected error occurred',
        errors: [{ message: error.message || 'An unexpected error occurred' }]
      };
    } finally {
      isDownloading.value = false;
    }
  };

  const resetDownloadState = () => {
    isDownloading.value = false;
    downloadError.value = null;
    downloadSuccess.value = false;
  };

  return {
    // State
    isDownloading,
    downloadError,
    downloadSuccess,
    
    // Methods
    downloadFileFromSignedUrl,
    resetDownloadState
  };
}