import { useRiskControlMatrixData } from './useRiskControlMatrixData';
import { useSearchAndPagination } from './useSearchAndPagination';
import { useRiskControlMatrixActions } from './useRiskControlMatrixActions';
import { useRiskControlMatrixWatchers } from './useRiskControlMatrixWatchers';
import { useRiskControlMatrixLifecycle } from './useRiskControlMatrixLifecycle';
import { useRiskControlMatrixComputed } from './useRiskControlMatrixComputed';

import { ref } from 'vue';

export function useRiskControlMatrixTable(selectedAuditProcessId) {

    const dataComposable = useRiskControlMatrixData();
    const searchComposable = useSearchAndPagination();

    const actionsComposable = useRiskControlMatrixActions({
        ...dataComposable,
        selectedAuditProcessId,
        ...searchComposable
    });

    const lifecycleComposable = useRiskControlMatrixLifecycle({
        ...dataComposable,
        selectedAuditProcessId,
        ...searchComposable,
        ...actionsComposable
    });

    const computedComposable = useRiskControlMatrixComputed({
        ...dataComposable,
        selectedAuditProcessId,
        ...searchComposable,
        ...lifecycleComposable
    });

    useRiskControlMatrixWatchers({
        ...dataComposable,
        selectedAuditProcessId,
        ...searchComposable,
        ...actionsComposable,
        ...lifecycleComposable
    });

    return {
        riskControlMatrices: computedComposable.riskControlMatrices,
        isLoading: computedComposable.isLoading,
        selectedAuditProcessId,
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