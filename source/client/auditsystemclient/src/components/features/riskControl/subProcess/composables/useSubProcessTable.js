import { ref, watch, onMounted, computed } from 'vue';
import { useSubProcessWatchers } from './useSubProcessWatchers';
import { useSubProcessComputed } from './useSubProcessComputed';
import { getSubProcessesByUserIdRepository } from '@/services/riskControl/subProcess/getSubProcessesByUserIdRepository';
import { getAuditProcessByIdRepository } from '@/services/riskControl/auditProcess/getAuditProcessByIdRepository';

export function useSubProcessTable(auditProcesses, selectedAuditProcessId) {
  const subProcesses = ref([]);
  const isLoading = ref(false);
  const selectedAuditProcess = ref(null);
  const currentAuditProcess = ref(null);
  const isAuditProcessLoading = ref(false);
  const searchQuery = ref('');
  const currentPage = ref(1);
  const pageSize = ref(10);
  const isInitialized = ref(false);
  const isInitializing = ref(false);
  const pendingInitialization = ref(false); // New flag to prevent concurrent calls
  const searchResults = ref([]);
  const allSubProcesses = ref([]);
  const isSearching = ref(false);
  const isLoadingAll = ref(false);
  let initializationCount = 0; // Track initialization calls

  // Fetch sub-processes based on audit process ID
  const getSubProcessesByUserId = async () => {
    isLoading.value = true;
    try {
      const response = await getSubProcessesByUserIdRepository.getSubProcessesByUserId({
        auditProcessId: selectedAuditProcessId.value,
        page: currentPage.value,
        pageSize: pageSize.value,
      });
      if (response.success) {
        allSubProcesses.value = response.result.subProcessModels || [];
        console.log(`Fetched sub-processes (call ${initializationCount}):`, allSubProcesses.value); // Debug log
      }
    } catch (error) {
      console.error('Error fetching sub-processes:', error);
    } finally {
      isLoading.value = false;
    }
  };

  // Get audit process details by ID
  const getAuditProcessById = async (id) => {
    isAuditProcessLoading.value = true;
    try {
      const response = await getAuditProcessByIdRepository.getAuditProcessById(id);
      if (response.success) {
        selectedAuditProcess.value = response.result.auditProcessModel || response.result || {};
        console.log(`Fetched audit process (call ${initializationCount}):`, selectedAuditProcess.value); // Debug log
      }
    } catch (error) {
      console.error('Error fetching audit process:', error);
    } finally {
      isAuditProcessLoading.value = false;
    }
  };

  // Search sub-processes
  const searchSubProcesses = async ({ searchTerm, auditProcessId, page, pageSize }) => {
    isSearching.value = true;
    try {
      const response = await getSubProcessesByUserIdRepository.getSubProcessesByUserId({
        searchTerm,
        auditProcessId,
        page,
        pageSize,
      });
      if (response.success) {
        searchResults.value = response.result.subProcessModels || [];
        console.log('Search results:', searchResults.value); // Debug log
      }
    } catch (error) {
      console.error('Error searching sub-processes:', error);
    } finally {
      isSearching.value = false;
    }
  };

  // Clear search and reset
  const clearSearch = () => {
    searchResults.value = [];
    isSearching.value = false;
  };

  // Clear audit process
  const clearAuditProcess = () => {
    selectedAuditProcess.value = null;
    currentAuditProcess.value = null;
  };

  // Initialize data
  const initializeData = async () => {
    if (isInitialized.value || pendingInitialization.value) return; // Prevent concurrent or repeated calls
    pendingInitialization.value = true;
    initializationCount++; // Increment call counter
    console.log(`Initializing data (call ${initializationCount}) - isInitialized: ${isInitialized.value}, pendingInitialization: ${pendingInitialization.value}, selectedAuditProcess: ${!!selectedAuditProcess.value}, allSubProcesses: ${allSubProcesses.value.length}`); // Debug log
    isInitializing.value = true;
    try {
      if (selectedAuditProcessId.value) {
        await getAuditProcessById(selectedAuditProcessId.value);
        await getSubProcessesByUserId();
      }
    } catch (error) {
      console.error(`Error initializing data (call ${initializationCount}):`, error);
    } finally {
      isInitializing.value = false;
      isInitialized.value = true;
      pendingInitialization.value = false;
    }
  };

  // Reset to default view
  const resetToDefaultView = () => {
    selectedAuditProcessId.value = null;
    clearSearch();
    clearAuditProcess();
    allSubProcesses.value = [];
  };

  // Handle audit process change
  const handleSubProcessChange = async (newAuditProcessId) => {
    selectedAuditProcessId.value = newAuditProcessId;
    await initializeData();
  };

  // Handle search
  const handleSearch = async (query) => {
    searchQuery.value = query;
  };

  // Refresh data
  const refreshData = async () => {
    await initializeData();
  };

  // Handle sub-process added
  const handleSubProcessAdded = async () => {
    await refreshData();
  };

  // Setup watchers
  useSubProcessWatchers({
    auditProcessesRef: computed(() => auditProcesses),
    selectedAuditProcess,
    selectedAuditProcessId,
    searchQuery,
    currentPage,
    pageSize,
    isInitialized,
    isInitializing,
    searchSubProcesses,
    clearSearch,
    getSubProcessesByUserId,
    allSubProcesses,
    clearAuditProcess,
    getAuditProcessById,
    initializeData,
    resetToDefaultView,
  });

  // Setup computed properties
  const { subProcesses: computedSubProcesses, isLoading: computedIsLoading } = useSubProcessComputed({
    searchQuery,
    selectedAuditProcessId,
    searchResults,
    allSubProcesses,
    isSearching,
    isLoadingAll,
    isInitializing,
  });

  // Sync computed subProcesses with ref
  watch(computedSubProcesses, (newSubProcesses) => {
    subProcesses.value = newSubProcesses;
    console.log(`Updated subProcesses (call ${initializationCount}):`, subProcesses.value); // Debug log
  }, { immediate: true });

  // Initial setup
  onMounted(async () => {
    if (selectedAuditProcessId.value) {
      await initializeData();
    }
  });

  return {
    subProcesses,
    isLoading: computedIsLoading,
    selectedAuditProcessId,
    selectedAuditProcess,
    currentAuditProcess,
    isAuditProcessLoading,
    searchQuery,
    refreshData,
    handleSubProcessAdded,
    handleSubProcessChange,
    handleSearch,
    resetToDefaultView,
  };
}