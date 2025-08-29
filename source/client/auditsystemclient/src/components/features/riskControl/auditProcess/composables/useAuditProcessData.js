import { ref } from 'vue';
import { useSearchAuditProcessesByUserId } from '@/composables/riskControl/auditProcess/useSearchAuditProcessesByUserId';
import { useGetAuditProcessesByUserId } from '@/composables/riskControl/auditProcess/useGetAuditProcessesByUserId';

export function useAuditProcessData() {
    const {
        searchResults = ref([]),
        isSearching = ref(false),
        totalCount = ref(0),
        searchAuditProcesses,
        clearSearch
    } = useSearchAuditProcessesByUserId() || {};

    const {
        auditProcesses: allAuditProcesses = ref([]),
        isLoading: isLoadingAll = ref(false),
        getAuditProcessesByUserId
    } = useGetAuditProcessesByUserId() || {};

    console.log("useAuditProcessData:  allAuditProcesses: ", allAuditProcesses.value);
    return {
        searchResults,
        allAuditProcesses,
        isSearching,
        isLoadingAll,
        totalCount,
        searchAuditProcesses,
        clearSearch,
        getAuditProcessesByUserId
    };
}