import { useObjectiveData } from './useObjectiveData';
import { useRiskControlMatrixManagement } from './useRiskControlMatrixManagement';
import { useSearchAndPagination } from '@/composables/common/pagination/useSearchAndPagination';
import { useObjectiveActions } from './useObjectiveActions';
import { useObjectiveWatchers } from './useObjectiveWatchers';
import { useObjectiveLifecycle } from './useObjectiveLifecycle';
import { useObjectiveComputed } from './useObjectiveComputed';

export function useObjectiveTable(riskControlMatrixes) {

    const dataComposable = useObjectiveData();
    const riskControlMatrixComposable = useRiskControlMatrixManagement(riskControlMatrixes);
    const searchComposable = useSearchAndPagination();

    const actionsComposable = useObjectiveActions({
        ...dataComposable,
        ...riskControlMatrixComposable,
        ...searchComposable
    });

    const lifecycleComposable = useObjectiveLifecycle({
        ...dataComposable,
        ...riskControlMatrixComposable,
        ...searchComposable,
        ...actionsComposable
    });

    const computedComposable = useObjectiveComputed({
        ...dataComposable,
        ...riskControlMatrixComposable,
        ...searchComposable,
        ...lifecycleComposable
    });

    useObjectiveWatchers({
        ...dataComposable,
        ...riskControlMatrixComposable,
        ...searchComposable,
        ...actionsComposable,
        ...lifecycleComposable
    });

    return {
        objectives: computedComposable.objectives,
        isLoading: computedComposable.isLoading,
        selectedRiskControlMatrixId: riskControlMatrixComposable.selectedRiskControlMatrixId,
        selectedRiskControlMatrix: riskControlMatrixComposable.selectedRiskControlMatrix,
        currentRiskControlMatrix: riskControlMatrixComposable.currentRiskControlMatrix,
        isRiskControlMatrixLoading: riskControlMatrixComposable.isRiskControlMatrixLoading,
        searchQuery: searchComposable.searchQuery,
        currentPage: searchComposable.currentPage,
        pageSize: searchComposable.pageSize,
        totalCount: dataComposable.totalCount,
        handleObjectiveAdded: actionsComposable.handleObjectiveAdded,
        handleObjectiveChange: actionsComposable.handleObjectiveChange,
        handleSearch: searchComposable.handleSearch,
        resetToDefaultView: actionsComposable.resetToDefaultView,
    };
}
