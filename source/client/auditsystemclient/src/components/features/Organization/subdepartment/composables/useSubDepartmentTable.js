import { useSubDepartmentData } from './useSubDepartmentData';
import { useDepartmentManagement } from './useDepartmentManagement';
import { useSearchAndPagination } from '@/composables/common/pagination/useSearchAndPagination';
import { useSubDepartmentActions } from './useSubDepartmentActions';
import { useSubDepartmentWatchers } from './useSubDepartmentWatchers';
import { useSubDepartmentLifecycle } from './useSubDepartmentLifecycle';
import { useSubDepartmentComputed } from './useSubDepartmentComputed';

export function useSubDepartmentTable(departments) {
  const dataComposable = useSubDepartmentData();
  const departmentComposable = useDepartmentManagement(departments);
  const searchComposable = useSearchAndPagination();

  const actionsComposable = useSubDepartmentActions({
    ...dataComposable,
    ...departmentComposable,
    ...searchComposable
  });

  const lifecycleComposable = useSubDepartmentLifecycle({
    ...dataComposable,
    ...departmentComposable,
    ...searchComposable,
    ...actionsComposable,
  });

  const computedComposable = useSubDepartmentComputed({
    ...dataComposable,
    ...departmentComposable,
    ...searchComposable,
    ...lifecycleComposable
  });

  useSubDepartmentWatchers({
    ...dataComposable,
    ...departmentComposable,
    ...searchComposable,
    ...actionsComposable,
    ...lifecycleComposable
  });

  return {
    subDepartments: computedComposable.subDepartments,
    isLoading: computedComposable.isLoading,
    selectedDepartmentId: departmentComposable.selectedDepartmentId,
    selectedDepartment: departmentComposable.selectedDepartment,
    currentDepartment: departmentComposable.currentDepartment,
    isDepartmentLoading: departmentComposable.isDepartmentLoading,
    searchQuery: searchComposable.searchQuery,
    currentPage: searchComposable.currentPage,
    pageSize: searchComposable.pageSize,
    totalCount: dataComposable.totalCount,
    refreshData: actionsComposable.refreshData,
    handleSubDepartmentChange: actionsComposable.handleSubDepartmentChange,
    handleSearch: searchComposable.handleSearch,
    resetToDefaultView: actionsComposable.resetToDefaultView,
  };
}
