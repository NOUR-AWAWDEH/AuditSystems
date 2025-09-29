import { useExitControlData } from './useExitControlData';
import { useRiskManagement } from './useRiskManagement';
import { useSearchAndPagination } from '@/composables/common/pagination/useSearchAndPagination';
import { useExitControlActions } from './useExitControlActions';
import { useExitControlWatchers } from './useExitControlWatchers';
import { useExitControlLifecycle } from './useExitControlLifecycle';
import { useExitControlComputed } from './useExitControlComputed';

export function useExitControlTable(risks) {

    const dataComposable = useExitControlData();
    const riskComposable = useRiskManagement(risks);
    const searchComposable = useSearchAndPagination();

    const actionsComposable = useExitControlActions({
        ...dataComposable,
        ...riskComposable,
        ...searchComposable
    });

    const lifecycleComposable = useExitControlLifecycle({
        ...dataComposable,
        ...riskComposable,
        ...searchComposable,
        ...actionsComposable
    });

    const computedComposable = useExitControlComputed({
        ...dataComposable,
        ...riskComposable,
        ...searchComposable,
        ...lifecycleComposable
    });

    useExitControlWatchers({
        ...dataComposable,
        ...riskComposable,
        ...searchComposable,
        ...actionsComposable,
        ...lifecycleComposable
    });

    return {
        exitControls: computedComposable.exitControls,
        isLoading: computedComposable.isLoading,
        
        selectedRiskId: riskComposable.selectedRiskId,
        selectedRisk: riskComposable.selectedRisk,
        currentRisk: riskComposable.currentRisk,
        isRiskLoading: riskComposable.isRiskLoading,
        
        searchQuery: searchComposable.searchQuery,
        currentPage: searchComposable.currentPage,
        pageSize: searchComposable.pageSize,
        totalCount: dataComposable.totalCount,

        handleExitControlAdded: actionsComposable.handleExitControlAdded,
        handleExitControlChange: actionsComposable.handleExitControlChange,
        handleSearch: searchComposable.handleSearch,
        resetToDefaultView: actionsComposable.resetToDefaultView,
    };
}