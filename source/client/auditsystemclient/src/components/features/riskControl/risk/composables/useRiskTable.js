import { useRiskData } from './useRiskData';
import { useObservationManagement } from './useObservationManagement';
import { useSearchAndPagination } from '@/composables/common/pagination/useSearchAndPagination';
import { useRiskActions } from './useRiskActions';
import { useRiskLifecycle } from './useRiskLifecycle';
import { useRiskComputed } from './useRiskComputed';
import { useRiskWatchers } from './useRiskWatchers';

export function useRiskTable(observations) {
    const dataComposable = useRiskData();
    const observationComposable = useObservationManagement(observations);
    const searchComposable = useSearchAndPagination();

    const actionsComposable = useRiskActions({
        ...dataComposable,
        ...observationComposable,
        ...searchComposable
    });

    const lifecycleComposable = useRiskLifecycle({
        ...dataComposable,
        ...observationComposable,
        ...searchComposable,
        ...actionsComposable
    });

    const computedComposable = useRiskComputed({
        ...dataComposable,
        ...observationComposable,
        ...searchComposable,
        ...lifecycleComposable
    });

    useRiskWatchers({
        ...dataComposable,
        ...observationComposable,
        ...searchComposable,
        ...actionsComposable,
        ...lifecycleComposable
    });

    return {
        risks: computedComposable.risks,
        isLoading: computedComposable.isLoading,
        selectedObservationId: observationComposable.selectedObservationId,
        selectedObservation: observationComposable.selectedObservation,
        currentObservation: observationComposable.currentObservation,
        isObservationLoading: observationComposable.isObservationLoading,
        searchQuery: searchComposable.searchQuery,
        currentPage: searchComposable.currentPage,
        pageSize: searchComposable.pageSize,
        totalCount: dataComposable.totalCount,
        handleRiskAdded: actionsComposable.handleRiskAdded,
        handleRiskChange: actionsComposable.handleRiskChange,
        handleSearch: searchComposable.handleSearch,
        resetToDefaultView: actionsComposable.resetToDefaultView,
    };
}
