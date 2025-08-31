import { useRiskControlMatrixData } from './useRiskControlMatrixData';
import { useSubProcessManagement } from './useSubProcessManagement';
import { useSearchAndPagination } from './useSearchAndPagination';
import { useRiskControlMatrixActions } from './useRiskControlMatrixActions';
import { useRiskControlMatrixWatchers } from './useRiskControlMatrixWatchers';
import { useRiskControlMatrixLifecycle } from './useRiskControlMatrixLifecycle';
import { useRiskControlMatrixComputed } from './useRiskControlMatrixComputed';

export function useRiskControlMatrixTable(subProcesses) {

    const dataComposable = useRiskControlMatrixData();
    const subProcessComposable = useSubProcessManagement(subProcesses);
    const searchComposable = useSearchAndPagination();

    const actionsComposable = useRiskControlMatrixActions({
        ...dataComposable,
        ...subProcessComposable,
        ...searchComposable
    });

    const lifecycleComposable = useRiskControlMatrixLifecycle({
        ...dataComposable,
        ...subProcessComposable,
        ...searchComposable,
        ...actionsComposable
    });

    const computedComposable = useRiskControlMatrixComputed({
        ...dataComposable,
        ...subProcessComposable,
        ...searchComposable,
        ...lifecycleComposable
    });

    useRiskControlMatrixWatchers({
        ...dataComposable,
        ...subProcessComposable,
        ...searchComposable,
        ...actionsComposable,
        ...lifecycleComposable
    });

    return {
        riskControlMatrices: computedComposable.riskControlMatrices,
        isLoading: computedComposable.isLoading,
        selectedSubProcessId: subProcessComposable.selectedSubProcessId,
        selectedSubProcess: subProcessComposable.selectedSubProcess,
        currentSubProcess: subProcessComposable.currentSubProcess,
        isSubProcessLoading: subProcessComposable.isSubProcessLoading,
        searchQuery: searchComposable.searchQuery,
        currentPage: searchComposable.currentPage,
        pageSize: searchComposable.pageSize,
        totalCount: dataComposable.totalCount,
        handleRiskControlMatrixAdded: actionsComposable.handleRiskControlMatrixAdded,
        handleRiskControlMatrixChange: actionsComposable.handleRiskControlMatrixChange,
        handleSearch: searchComposable.handleSearch,
        resetToDefaultView: actionsComposable.resetToDefaultView,
    };
}