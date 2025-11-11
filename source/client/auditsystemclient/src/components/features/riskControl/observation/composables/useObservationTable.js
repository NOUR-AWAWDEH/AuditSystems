import { useObservationData } from './useObservationData';
import { useAuditProgramManagement } from './useAuditProgramManagement';
import { useSearchAndPagination } from '@/composables/common/pagination/useSearchAndPagination';
import { useObservationActions } from './useObservationActions';
import { useObservationWatchers } from './useObservationWatchers';
import { useObservationLifecycle } from './useObservationLifecycle';
import { useObservationComputed } from './useObservationComputed';

export function useObservationTable(auditPrograms) {

    const dataComposable = useObservationData();
    const auditProgramComposable = useAuditProgramManagement(auditPrograms);
    const searchComposable = useSearchAndPagination();

    const actionsComposable = useObservationActions({
        ...dataComposable,
        ...auditProgramComposable,
        ...searchComposable
    });

    const lifecycleComposable = useObservationLifecycle({
        ...dataComposable,
        ...auditProgramComposable,
        ...searchComposable,
        ...actionsComposable
    });

    const computedComposable = useObservationComputed({
        ...dataComposable,
        ...auditProgramComposable,
        ...searchComposable,
        ...lifecycleComposable
    });

    useObservationWatchers({
        ...dataComposable,
        ...auditProgramComposable,
        ...searchComposable,
        ...actionsComposable,
        ...lifecycleComposable
    });

    return {
        observations: computedComposable.observations,
        isLoading: computedComposable.isLoading,
        selectedAuditProgramId: auditProgramComposable.selectedAuditProgramId,
        selectedAuditProgram: auditProgramComposable.selectedAuditProgram,
        currentAuditProgram: auditProgramComposable.currentAuditProgram,
        isAuditProgramLoading: auditProgramComposable.isAuditProgramLoading,
        searchQuery: searchComposable.searchQuery,
        currentPage: searchComposable.currentPage,
        pageSize: searchComposable.pageSize,
        totalCount: dataComposable.totalCount,
        handleObservationAdded: actionsComposable.handleObservationAdded,
        handleObservationChange: actionsComposable.handleObservationChange,
        handleSearch: searchComposable.handleSearch,
        resetToDefaultView: actionsComposable.resetToDefaultView,
    };
}