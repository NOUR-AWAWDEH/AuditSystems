<script setup>
import { ref } from "vue";
import SideBar from "@/components/common/SideBar.vue";
import DocumentUploadSection from "./components/DocumentUploadSection.vue";
import DocumentsTable from "./components/DocumentsTable.vue";
import { useDocumentManager } from "./composables/useDocumentManager";

const {
  documents,
  addDocument,
  removeDocument
} = useDocumentManager();
</script>

<template>
  <div class="flex h-screen">
    <SideBar />
    <div class="flex-1 p-6">
      <h2 class="text-2xl font-bold mb-4">Supporting Documents</h2>
      
      <DocumentUploadSection @document-uploaded="addDocument" />
      <DocumentsTable 
        :documents="documents" 
        @delete-document="removeDocument" 
      />
    </div>
  </div>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-leave-to,
.fade-enter-from {
  opacity: 0;
}
</style>
