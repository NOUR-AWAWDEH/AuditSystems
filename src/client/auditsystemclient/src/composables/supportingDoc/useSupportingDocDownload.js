import { ref, computed } from 'vue';
import { downloadSupportingDocumentRepository } from '@/services/supportingDocs/downloadSupportingDocumentRepository';
import { useDecodeDownload } from '@/composables/download/useDecodeDownload';

export function useSupportingDocDownload() {
  
  const isGettingUrl = ref(false);
  const urlError = ref(null);
  
  // Use the existing decode download composable
  const { 
    isDownloading, 
    downloadError, 
    downloadSuccess, 
    downloadFileFromSignedUrl, 
    resetDownloadState 
  } = useDecodeDownload();

  const downloadSupportingDocument = async (fileId) => {
    
    try {
      isGettingUrl.value = true;
      urlError.value = null;
      resetDownloadState(); // Reset download state

      // Step 1: Get signed URL from supporting docs repository
      const signedUrlResult = await downloadSupportingDocumentRepository.getSupportingDocSignedUrl(fileId);
      
      if (!signedUrlResult.success) {
        urlError.value = signedUrlResult.message || 'Failed to get signed URL';
        return signedUrlResult;
      }
      
      // Extract the signed URL from the response
      const signedUrl = signedUrlResult.result?.fileHttpResult?.url || signedUrlResult.result?.fileHttpResult;
      
      if (!signedUrl) {
        const errorResult = {
          success: false,
          message: 'No signed URL found in response.',
          errors: [{ message: 'No signed URL found in response.' }]
        };
        urlError.value = errorResult.message;
        return errorResult;
      }
      
      isGettingUrl.value = false;
      
      // Step 2: Use the decode download composable to download the file
      // Don't pass preferredFileName - let backend determine the filename
      const downloadResult = await downloadFileFromSignedUrl(signedUrl);      
      return downloadResult;
    } catch (error) {
      urlError.value = error.message || 'An unexpected error occurred';
      return {
        success: false,
        message: error.message || 'An unexpected error occurred',
        errors: [{ message: error.message || 'An unexpected error occurred' }]
      };
    } finally {
      isGettingUrl.value = false;
    }
  };

  const resetAllStates = () => {
    isGettingUrl.value = false;
    urlError.value = null;
    resetDownloadState();
  };

  return {
    // Combined state from both operations
    isGettingUrl,
    urlError,
    isDownloading,
    downloadError,
    downloadSuccess,
    
    // Computed state for overall loading
    isLoading: computed(() => isGettingUrl.value || isDownloading.value),
    
    // Methods
    downloadSupportingDocument,
    resetAllStates
  };
}