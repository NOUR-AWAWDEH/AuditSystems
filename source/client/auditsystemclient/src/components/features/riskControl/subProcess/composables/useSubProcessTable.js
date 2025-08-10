import { useSubProcessData } from './useSubProcessData';
import { useAuditProcessManagement } from './useAuditProcessManagement';
import { useSearchAndPagination } from './useSearchAndPagination';
import { useSubProcessActions } from './useSubProcessActions';
import { useSubProcessWatchers } from './useSubProcessWatchers';
import { useSubProcessLifecycle } from './useSubProcessLifecycle';
import { useSubProcessComputed } from './useSubProcessComputed';
import { useSubProcessModals } from './useSubProcessModals';

export function useSubProcessTable(auditProcesses, showNotification) {

    const dataComposable = useSubProcessData();
    const auditProcessComposable = useAuditProcessManagement(auditProcesses);
    const searchComposable = useSearchAndPagination();

    const actionsComposable = useSubProcessActions({
        ...dataComposable,
        ...auditProcessComposable,
        ...searchComposable
    });

    const lifecycleComposable = useSubProcessLifecycle({
        ...dataComposable,
        ...auditProcessComposable,
        ...searchComposable,
        ...actionsComposable
    });

    const computedComposable = useSubProcessComputed({
        ...dataComposable,
        ...auditProcessComposable,
        ...searchComposable,
        ...lifecycleComposable
    });

    useSubProcessWatchers({
        ...dataComposable,
        ...auditProcessComposable,
        ...searchComposable,
        ...actionsComposable,
        ...lifecycleComposable
    });

    const modalComposable = useSubProcessModals(
        actionsComposable.refreshData,
        showNotification
    );

    return {
        subProcesses: computedComposable.subProcesses,
        isLoading: computedComposable.isLoading,
        selectedAuditProcessId: auditProcessComposable.selectedAuditProcessId,
        selectedAuditProcess: auditProcessComposable.selectedAuditProcess,
        currentAuditProcess: auditProcessComposable.currentAuditProcess,
        isAuditProcessLoading: auditProcessComposable.isAuditProcessLoading,
        searchQuery: searchComposable.searchQuery,
        currentPage: searchComposable.currentPage,
        pageSize: searchComposable.pageSize,
        totalCount: dataComposable.totalCount,
        refreshData: actionsComposable.refreshData,
        handleSubProcessAdded: actionsComposable.handleSubProcessAdded,
        handleSubProcessChange: actionsComposable.handleSubProcessChange,
        handleSearch: searchComposable.handleSearch,
        resetToDefaultView: actionsComposable.resetToDefaultView,
        ...modalComposable
    };
}