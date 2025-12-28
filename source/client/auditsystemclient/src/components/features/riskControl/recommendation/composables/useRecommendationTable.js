import { useRecommendationData } from './useRecommendationData';
import { useObservationManagement } from './useObservationManagement';
import { useSearchAndPagination } from '@/composables/common/pagination/useSearchAndPagination';
import { useRecommendationActions } from './useRecommendationActions';
import { useRecommendationLifecycle } from './useRecommendationLifecycle';
import { useRecommendationComputed } from './useRecommendationComputed';
import { useRecommendationWatchers } from './useRecommendationWatchers';

export function useRecommendationTable(observations) {
    const dataComposable = useRecommendationData();
    const observationComposable = useObservationManagement(observations);
    const searchComposable = useSearchAndPagination();

    const actionsComposable = useRecommendationActions({
        ...dataComposable,
        ...observationComposable,
        ...searchComposable
    });

    const lifecycleComposable = useRecommendationLifecycle({
        ...dataComposable,
        ...observationComposable,
        ...searchComposable,
        ...actionsComposable
    });

    const computedComposable = useRecommendationComputed({
        ...dataComposable,
        ...observationComposable,
        ...searchComposable,
        ...lifecycleComposable
    });

    useRecommendationWatchers({
        ...dataComposable,
        ...observationComposable,
        ...searchComposable,
        ...actionsComposable,
        ...lifecycleComposable
    });

    return {
        recommendations: computedComposable.recommendations,
        isLoading: computedComposable.isLoading,
        selectedObservationId: observationComposable.selectedObservationId,
        selectedObservation: observationComposable.selectedObservation,
        currentObservation: observationComposable.currentObservation,
        isObservationLoading: observationComposable.isObservationLoading,
        searchQuery: searchComposable.searchQuery,
        currentPage: searchComposable.currentPage,
        pageSize: searchComposable.pageSize,
        totalCount: dataComposable.totalCount,
        handleRecommendationAdded: actionsComposable.handleRecommendationAdded,
        handleRecommendationChange: actionsComposable.handleRecommendationChange,
        handleSearch: searchComposable.handleSearch,
        resetToDefaultView: actionsComposable.resetToDefaultView,
    };
}