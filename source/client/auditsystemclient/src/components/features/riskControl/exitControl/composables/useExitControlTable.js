import { useExitControlData } from './useExitControlData';
import { useObjectiveManagement } from './useObjectiveManagement';
import { useSearchAndPagination } from '@/composables/common/pagination/useSearchAndPagination';
import { useExitControlActions } from './useExitControlActions';
import { useExitControlWatchers } from './useExitControlWatchers';
import { useExitControlLifecycle } from './useExitControlLifecycle';
import { useExitControlComputed } from './useExitControlComputed';

export function useExitControlTable(objectives) {

    const dataComposable = useExitControlData();
    const objectiveComposable = useObjectiveManagement(objectives);
    const searchComposable = useSearchAndPagination();

    const actionsComposable = useExitControlActions({
        ...dataComposable,
        ...objectiveComposable,
        ...searchComposable
    });

    const lifecycleComposable = useExitControlLifecycle({
        ...dataComposable,
        ...objectiveComposable,
        ...searchComposable,
        ...actionsComposable
    });

    const computedComposable = useExitControlComputed({
        ...dataComposable,
        ...objectiveComposable,
        ...searchComposable,
        ...lifecycleComposable
    });

    useExitControlWatchers({
        ...dataComposable,
        ...objectiveComposable,
        ...searchComposable,
        ...actionsComposable,
        ...lifecycleComposable
    });

    return {
        exitControls: computedComposable.exitControls,
        isLoading: computedComposable.isLoading,
        
        selectedObjectiveId: objectiveComposable.selectedObjectiveId,
        selectedObjective: objectiveComposable.selectedObjective,
        currentObjective: objectiveComposable.currentObjective,
        isObjectiveLoading: objectiveComposable.isObjectiveLoading,
        
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