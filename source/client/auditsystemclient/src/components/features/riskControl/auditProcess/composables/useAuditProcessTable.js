import { useAuditProcessData } from './useAuditProcessData';
import { useSubDepartmentManagement } from './useSubDepartmentManagement';
import { useSearchAndPagination } from '@/composables/common/pagination/useSearchAndPagination';
import { useAuditProcessActions } from './useAuditProcessActions';
import { useAuditProcessWatchers } from './useAuditProcessWatchers';
import { useAuditProcessLifecycle } from './useAuditProcessLifecycle';
import { useAuditProcessComputed } from './useAuditProcessComputed';

export function useAuditProcessTable(subDepartments) {

    const dataComposable = useAuditProcessData();
    const subDepartmentComposable = useSubDepartmentManagement(subDepartments);
    const searchComposable = useSearchAndPagination();

    const actionsComposable = useAuditProcessActions({
        ...dataComposable,
        ...subDepartmentComposable,
        ...searchComposable
    });

    const lifecycleComposable = useAuditProcessLifecycle({
        ...dataComposable,
        ...subDepartmentComposable,
        ...searchComposable,
        ...actionsComposable
    });

    const computedComposable = useAuditProcessComputed({
        ...dataComposable,
        ...subDepartmentComposable,
        ...searchComposable,
        ...lifecycleComposable
    });

    useAuditProcessWatchers({
        ...dataComposable,
        ...subDepartmentComposable,
        ...searchComposable,
        ...actionsComposable,
        ...lifecycleComposable
    });

    return {
        auditProcesses: computedComposable.auditProcesses,
        isLoading: computedComposable.isLoading,
        selectedSubDepartmentId: subDepartmentComposable.selectedSubDepartmentId,
        selectedSubDepartment: subDepartmentComposable.selectedSubDepartment,
        currentSubDepartment: subDepartmentComposable.currentSubDepartment,
        isSubDepartmentLoading: subDepartmentComposable.isSubDepartmentLoading,
        searchQuery: searchComposable.searchQuery,
        currentPage: searchComposable.currentPage,
        pageSize: searchComposable.pageSize,
        totalCount: dataComposable.totalCount,
        refreshData: actionsComposable.refreshData,
        handleAuditProcessChange: actionsComposable.handleAuditProcessChange,
        handleSearch: searchComposable.handleSearch,
        resetToDefaultView: actionsComposable.resetToDefaultView,
    };
}
