import { useAuditProgramData } from './useAuditProgramData';
import { useExitControlManagement } from './useExitControlManagement';
import { useSearchAndPagination } from '@/composables/common/pagination/useSearchAndPagination';
import { useAuditProgramActions } from './useAuditProgramActions';
import { useAuditProgramWatchers } from './useAuditProgramWatchers';
import { useAuditProgramLifecycle } from './useAuditProgramLifecycle';
import { useAuditProgramComputed } from './useAuditProgramComputed';

export function useAuditProgramTable(exitControls) {

    const dataComposable = useAuditProgramData();
    const exitControlComposable = useExitControlManagement(exitControls);
    const searchComposable = useSearchAndPagination();

    const actionsComposable = useAuditProgramActions({
        ...dataComposable,
        ...exitControlComposable,
        ...searchComposable
    });

    const lifecycleComposable = useAuditProgramLifecycle({
        ...dataComposable,
        ...exitControlComposable,
        ...searchComposable,
        ...actionsComposable
    });

    const computedComposable = useAuditProgramComputed({
        ...dataComposable,
        ...exitControlComposable,
        ...searchComposable,
        ...lifecycleComposable
    });

    useAuditProgramWatchers({
        ...dataComposable,
        ...exitControlComposable,
        ...searchComposable,
        ...actionsComposable,
        ...lifecycleComposable
    });

    return {
        auditPrograms: computedComposable.auditPrograms,
        isLoading: computedComposable.isLoading,
        
        selectedExitControlId: exitControlComposable.selectedExitControlId,
        selectedExitControl: exitControlComposable.selectedExitControl,
        currentExitControl: exitControlComposable.currentExitControl,
        isExitControlLoading: exitControlComposable.isExitControlLoading,
        
        searchQuery: searchComposable.searchQuery,
        currentPage: searchComposable.currentPage,
        pageSize: searchComposable.pageSize,
        totalCount: dataComposable.totalCount,

        handleAuditProgramAdded: actionsComposable.handleAuditProgramAdded,
        handleAuditProgramChange: actionsComposable.handleAuditProgramChange,
        handleSearch: searchComposable.handleSearch,
        resetToDefaultView: actionsComposable.resetToDefaultView,
    };
}
