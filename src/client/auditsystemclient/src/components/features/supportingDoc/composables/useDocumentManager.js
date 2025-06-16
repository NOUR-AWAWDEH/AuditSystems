import { ref } from 'vue';

export function useDocumentManager() {
  const documents = ref([]);

  function addDocument(document) {
    documents.value.push(document);
  }

  function removeDocument(documentId) {
    const index = documents.value.findIndex(doc => doc.id === documentId);
    if (index > -1) {
      documents.value.splice(index, 1);
    }
  }

  function getDocumentById(id) {
    return documents.value.find(doc => doc.id === id);
  }

  function clearAllDocuments() {
    documents.value = [];
  }

  return {
    documents,
    addDocument,
    removeDocument,
    getDocumentById,
    clearAllDocuments
  };
}