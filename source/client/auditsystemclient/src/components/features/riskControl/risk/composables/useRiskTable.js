/**
 * Composable for orchestrating risk table management by combining multiple
 * smaller composables into one unified interface for the risk module.
 *
 * Integrates data handling, objectives, search/pagination, actions,
 * lifecycle management, computed state, watchers, and modal handling.
 *
 * @param {Array} objectives - List of available objectives for filtering risks.
 * @param {Function} showNotification - Callback to show user notifications (message, type).
 *
 * Internally uses:
 * - `useRiskData()` → Handles API data fetching and state for risks.
 * - `useObjectiveManagement(objectives)` → Manages selected objective and related state.
 * - `useSearchAndPagination()` → Provides search query and pagination controls.
 * - `useRiskActions()` → Handles refreshing, adding, and risk-related actions.
 * - `useRiskLifecycle()` → Manages lifecycle hooks for loading and state transitions.
 * - `useRiskComputed()` → Provides computed values (risks list, loading state).
 * - `useRiskWatchers()` → Sets up watchers to auto-refresh and respond to state changes.
 * - `useRiskModals(refreshData, showNotification)` → Manages Add/Edit/Delete modals for risks.
 *
 * Returns:
 * - Risks Data:
 *   - `risks` → List of risks (computed, filtered, and paginated).
 *   - `isLoading` → Whether risk data is currently loading.
 *   - `totalCount` → Total count of risks (for pagination).
 *
 * - Objectives:
 *   - `selectedObjectiveId` → Currently selected objective ID.
 *   - `selectedObjective` → Full selected objective object.
 *   - `currentObjective` → Current working objective state.
 *   - `isObjectiveLoading` → Loading state for objective data.
 *
 * - Search & Pagination:
 *   - `searchQuery` → Current search query string.
 *   - `currentPage` → Current pagination page.
 *   - `pageSize` → Number of items per page.
 *
 * - Actions:
 *   - `refreshData()` → Reloads risk data from API.
 *   - `handleRiskAdded()` → Refresh and handle UI updates after adding a risk.
 *   - `handleRiskChange()` → Handles updates when risk data changes.
 *   - `handleSearch(query)` → Triggers a search with the provided query.
 *   - `resetToDefaultView()` → Resets filters and pagination to defaults.
 *
 * - Modals (from `useRiskModals`):
 *   - `showAddRiskModal`, `showEditRiskModal`, `showDeleteDialog` → Reactive modal states.
 *   - `riskToEdit`, `itemToDelete` → Stores the currently selected risk for edit/delete.
 *   - `deleteDialogTitle`, `deleteDialogMessage` → Text for delete confirmation dialog.
 *   - `handleAddRisk()`, `handleCloseAddRiskModal()` → Add Risk modal handlers.
 *   - `handleEditRisk(risk)`, `handleCloseEditRiskModal()` → Edit Risk modal handlers.
 *   - `handleRiskUpdated()` → Refresh and notify after editing a risk.
 *   - `handleDeleteRisk(risk)`, `confirmDelete()`, `cancelDelete()` → Delete Risk dialog handlers.
 *
 * This acts as the single entry point for managing risk tables,
 * providing a clean API for use in components.
 */

import { useRiskData } from './useRiskData';
import { useObjectiveManagement } from './useObjectiveManagement';
import { useSearchAndPagination } from '@/composables/common/pagination/useSearchAndPagination';
import { useRiskActions } from './useRiskActions';
import { useRiskLifecycle } from './useRiskLifecycle';
import { useRiskComputed } from './useRiskComputed';
import { useRiskWatchers } from './useRiskWatchers';

export function useRiskTable(objectives) {
    const dataComposable = useRiskData();
    const objectiveComposable = useObjectiveManagement(objectives);
    const searchComposable = useSearchAndPagination();

    const actionsComposable = useRiskActions({
        ...dataComposable,
        ...objectiveComposable,
        ...searchComposable
    });

    const lifecycleComposable = useRiskLifecycle({
        ...dataComposable,
        ...objectiveComposable,
        ...searchComposable,
        ...actionsComposable
    });

    const computedComposable = useRiskComputed({
        ...dataComposable,
        ...objectiveComposable,
        ...searchComposable,
        ...lifecycleComposable
    });

    useRiskWatchers({
        ...dataComposable,
        ...objectiveComposable,
        ...searchComposable,
        ...actionsComposable,
        ...lifecycleComposable
    });

    return {
        risks: computedComposable.risks,
        isLoading: computedComposable.isLoading,
        selectedObjectiveId: objectiveComposable.selectedObjectiveId,
        selectedObjective: objectiveComposable.selectedObjective,
        currentObjective: objectiveComposable.currentObjective,
        isObjectiveLoading: objectiveComposable.isObjectiveLoading,
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
