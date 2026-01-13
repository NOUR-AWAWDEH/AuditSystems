import { useCompanyData } from './useCompanyData';
import { useSearchAndPagination } from '@/composables/common/pagination/useSearchAndPagination';
import { useCompanyActions } from './useCompanyActions';
import { useCompanyWatchers } from './useCompanyWatchers';
import { useCompanyLifecycle } from './useCompanyLifecycle';
import { useCompanyComputed } from './useCompanyComputed';

export function useCompanyTable() {
  const dataComposable = useCompanyData();
  const searchComposable = useSearchAndPagination();

  const actionsComposable = useCompanyActions({
    ...dataComposable,
    ...searchComposable
  });

  const lifecycleComposable = useCompanyLifecycle({
    ...dataComposable,
    ...searchComposable,
    ...actionsComposable,
  });

  const computedComposable = useCompanyComputed({
    ...dataComposable,
    ...searchComposable,
    ...lifecycleComposable
  });

  useCompanyWatchers({
    ...dataComposable,
    ...searchComposable,
    ...actionsComposable,
    ...lifecycleComposable
  });

  return {
    companies: computedComposable.companies,
    isLoading: computedComposable.isLoading,
    searchQuery: searchComposable.searchQuery,
    currentPage: searchComposable.currentPage,
    pageSize: searchComposable.pageSize,
    totalCount: dataComposable.totalCount,
    handleCompanyChange: actionsComposable.handleCompanyChange,
    handleSearch: searchComposable.handleSearch,
    resetToDefaultView: actionsComposable.resetToDefaultView,
  };
}