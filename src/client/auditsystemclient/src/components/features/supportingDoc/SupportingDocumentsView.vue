<script setup>
import { ref, onMounted, watch } from "vue";
import SideBar from "@/components/common/SideBar.vue";
import DocumentUploadSection from "./components/DocumentUploadSection.vue";
import DocumentsTable from "./components/DocumentsTable.vue";
import { useDocumentManager } from "./composables/useDocumentManager";
import { useFetchSupportingDocuments } from "@/composables/supportingDoc/useFetchSupportingDocuments";
import { useAuthStatus } from "@/composables/auth/useAuthStatus";
import authService from "@/services/auth/authRepos/authService";
import { useRoute } from 'vue-router';

const {
  documents,
  setDocuments,
  removeDocument
} = useDocumentManager();

const { fetchDocumentsByUserId } = useFetchSupportingDocuments();
const { authStatus, checkAuthStatus } = useAuthStatus();
const route = useRoute();

// Add loading state for better UX
const isLoadingDocuments = ref(false);
const error = ref(null);

// Load documents when component mounts
onMounted(async () => {
  await loadDocuments();
});

// Also reload documents when route changes (if user navigates back to this page)
watch(() => route.path, (newPath) => {
  if (newPath === '/supporting-docs') {
    loadDocuments();
  }
});

async function loadDocuments() {
  try {
    isLoadingDocuments.value = true;
    error.value = null;
    
    const token = authService.getLocalToken();
    if (!token) {
      error.value = 'Please login to view documents';
      return;
    }

    // Ensure we have user authentication status
    if (!authStatus.value || !authStatus.value.userId) {
      await checkAuthStatus(token);
    }

    if (authStatus.value?.userId) {
      const response = await fetchDocumentsByUserId(authStatus.value.userId);
      
      if (response?.success) {
        setDocuments(response.result);
      } else {
        error.value = response?.message || 'Failed to load documents';
      }
    } else {
      error.value = 'Unable to identify user';
    }
  } catch (err) {
    error.value = 'An error occurred while loading documents';
  } finally {
    isLoadingDocuments.value = false;
  }
}

function handleDocumentsRefreshed(refreshedDocuments) {
  setDocuments(refreshedDocuments);
}

function handleDocumentDeleted(documentId) {
  removeDocument(documentId);
}

// Expose loadDocuments for manual refresh if needed
defineExpose({
  loadDocuments
});
</script>

<template>
  <div class="flex min-h-screen bg-gray-50">
    <!-- Sidebar - Full Height -->
    <div class="flex-shrink-0">
      <SideBar class="min-h-screen" />
    </div>
    
    <!-- Main Content with full height -->
    <div class="flex-1 min-h-screen p-3 sm:p-4 md:p-6">
      <h2 class="text-lg sm:text-xl md:text-2xl font-bold mb-4 md:mb-6 text-gray-900">Supporting Documents</h2>
      
      <!-- Loading State -->
      <div v-if="isLoadingDocuments" class="text-center py-6 md:py-8">
        <div class="inline-block animate-spin rounded-full h-6 w-6 md:h-8 md:w-8 border-b-2 border-primary-base"></div>
        <p class="mt-2 text-gray-600 text-sm md:text-base">Loading your documents...</p>
      </div>
      
      <!-- Error State -->
      <div v-else-if="error" class="bg-red-50 border border-red-200 rounded-md p-3 md:p-4 mb-4">
        <div class="flex flex-col space-y-2 sm:flex-row sm:space-y-0 sm:space-x-3">
          <svg class="h-5 w-5 text-red-400 flex-shrink-0" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clip-rule="evenodd" />
          </svg>
          <div class="flex-1">
            <h3 class="text-sm font-medium text-red-800">Error Loading Documents</h3>
            <p class="mt-1 text-sm text-red-700">{{ error }}</p>
            <button @click="loadDocuments" class="mt-2 text-sm text-red-600 hover:text-red-500 underline">
              Try Again
            </button>
          </div>
        </div>
      </div>
      
      <!-- Main Content -->
      <div v-else class="space-y-4 md:space-y-6">
        <DocumentUploadSection @documents-refreshed="handleDocumentsRefreshed" />
        <div>
          <DocumentsTable 
            :documents="documents" 
            @delete-document="handleDocumentDeleted"
          />
        </div>
        
        <!-- Debug Info (remove in production) -->
        <div v-if="documents.length === 0" class="mt-4 p-3 md:p-4 bg-primary-base rounded-md">
          <p class="text-white text-sm md:text-base">No documents found. Upload some documents to see them here.</p>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
/* Ensure full height layout */
.min-h-screen {
  min-height: 100vh;
  min-height: 100dvh; /* Dynamic viewport height for mobile browsers */
}

/* Enhanced mobile responsiveness */
@media (max-width: 640px) {
  .p-3 {
    padding: 0.75rem;
  }
}

@media (max-width: 480px) {
  .p-3 {
    padding: 0.5rem;
  }
}
</style>
