import { useControlDesignData } from './useControlDesignData';
import { useExitControlManagement } from '../../auditProgram/composables/useExitControlManagement';
import { useSearchAndPagination } from '@/composables/common/pagination/useSearchAndPagination';
import { useControlDesignActions } from './useControlDesignActions';
import { useControlDesignWatchers } from './useControlDesignWatchers';
import { useControlDesignLifecycle } from './useControlDesignLifecycle';
import { useControlDesignComputed } from './useControlDesignComputed';

export function useControlDesignTable(exitControls) {

    const dataComposable = useControlDesignData();
    const exitControlComposable = useExitControlManagement(exitControls);
    const searchComposable = useSearchAndPagination();

    const actionsComposable = useControlDesignActions({
        ...dataComposable,
        ...exitControlComposable,
        ...searchComposable
    });

    const lifecycleComposable = useControlDesignLifecycle({
        ...dataComposable,
        ...exitControlComposable,
        ...searchComposable,
        ...actionsComposable
    });

    const computedComposable = useControlDesignComputed({
        ...dataComposable,
        ...exitControlComposable,
        ...searchComposable,
        ...lifecycleComposable
    });

    useControlDesignWatchers({
        ...dataComposable,
        ...exitControlComposable,
        ...searchComposable,
        ...actionsComposable,
        ...lifecycleComposable
    });

    return {
        controlDesigns: computedComposable.controlDesigns,
        isLoading: computedComposable.isLoading,
        
        selectedExitControlId: exitControlComposable.selectedExitControlId,
        selectedExitControl: exitControlComposable.selectedExitControl,
        currentExitControl: exitControlComposable.currentExitControl,
        isExitControlLoading: exitControlComposable.isExitControlLoading,
        
        searchQuery: searchComposable.searchQuery,
        currentPage: searchComposable.currentPage,
        pageSize: searchComposable.pageSize,
        totalCount: dataComposable.totalCount,

        handleControlDesignAdded: actionsComposable.handleControlDesignAdded,
        handleControlDesignChange: actionsComposable.handleControlDesignChange,
        handleSearch: searchComposable.handleSearch,
        resetToDefaultView: actionsComposable.resetToDefaultView,
    };
}