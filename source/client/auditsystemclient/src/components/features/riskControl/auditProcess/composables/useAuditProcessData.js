import { useSearchAuditProcessesByUserId } from '@/composables/riskControl/auditProcess/useSearchAuditProcessesByUserId';
import { useGetAuditProcessesByUserId } from '@/composables/riskControl/auditProcess/useGetAuditProcessesByUserId';

export function useAuditProcessData() {
  const searchComposable = useSearchAuditProcessesByUserId();
  const getComposable = useGetAuditProcessesByUserId();

  // Return the properties directly from the composables to preserve reactivity
  return {
    searchResults: searchComposable.searchResults,
    allAuditProcesses: getComposable.auditProcesses,
    isSearching: searchComposable.isSearching,
    isLoadingAll: getComposable.isLoading,
    totalCount: searchComposable.totalCount,
    searchAuditProcesses: searchComposable.searchAuditProcesses,
    clearSearch: searchComposable.clearSearch,
    getAuditProcessesByUserId: getComposable.getAuditProcessesByUserId
  };
}