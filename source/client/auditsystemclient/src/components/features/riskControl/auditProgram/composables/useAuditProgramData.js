import {ref } from 'vue';
import { useSearchAuditProgramsByUserId } from '@/composables/riskControl/auditProgram/useSearchAuditProgramsByUserId';
import { useGetAuditProgramsByUserId } from '@/composables/riskControl/auditProgram/useGetAuditProgramsByUserId';

export function useAuditProgramData() {
    const {
        searchResults = ref([]),
        isSearching = ref(false),
        totalCount = ref(0),
        searchAuditPrograms,
        clearSearch
    } = useSearchAuditProgramsByUserId() || {};

    const {
        auditPrograms: allAuditPrograms = ref([]),
        isLoading: isLoadingAll = ref(false),
        getAuditProgramsByUserId
    } = useGetAuditProgramsByUserId() || {};

    return {
        searchResults,
        allAuditPrograms,
        isSearching,
        isLoadingAll,
        totalCount,
        searchAuditPrograms,
        clearSearch,
        getAuditProgramsByUserId
    };
}