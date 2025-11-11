/**
 * Composable to compute reactive risk data for the UI.
 *
 * Combines search results, selected objective, and all risks to provide a
 * single computed array of risks to display, along with a unified loading flag.
 *
 * @param {Object} params - Object containing reactive refs:
 * @param {Ref<string>} params.searchQuery - Current search query.
 * @param {Ref<string|null>} params.selectedObjectiveId - Currently selected objective ID.
 * @param {Ref<Array>} params.searchResults - Reactive array of risks filtered by search or objective.
 * @param {Ref<Array>} params.allRisks - Reactive array of all risks for the user.
 * @param {Ref<boolean>} params.isSearching - True if search API is running.
 * @param {Ref<boolean>} params.isLoadingAll - True if fetching all risks.
 * @param {Ref<boolean>} params.isInitializing - True if initial data loading is in progress.
 *
 * Returns:
 * - `risks` → Computed array of risks to display in the UI.
 *   - Uses `searchResults` if a search query or selected objective exists.
 *   - Falls back to `allRisks` if no query or objective is selected.
 *   - Always returns a safe array (never null/undefined).
 *
 * - `isLoading` → Computed boolean indicating if any risk-related data is currently loading.
 *   - Combines `isSearching`, `isLoadingAll`, and `isInitializing`.
 *   - Returns true if **any** of the flags is true, otherwise false.
 *   - Safe: defaults to false if any of the refs are undefined or null.
 *
 * Notes:
 * - Fully reactive: automatically updates when any of the underlying refs change.
 * - Safe defaults ensure the UI never breaks even if data is not yet loaded.
 */

import { computed } from 'vue';

export function useRiskComputed({
    searchQuery,
    selectedObjectiveId,
    searchResults,
    allRisks,
    isSearching,
    isLoadingAll,
    isInitializing
}) {
    const risks = computed(() => {
        if ((searchQuery?.value?.trim()) || selectedObjectiveId?.value) {
            return searchResults?.value || [];
        }
        return allRisks?.value || [];
    });

    const isLoading = computed(() => {
        const searching = (isSearching && isSearching.value) ? isSearching.value : false;
        const loadingAll = (isLoadingAll && isLoadingAll.value) ? isLoadingAll.value : false;
        const initializing = (isInitializing && isInitializing.value) ? isInitializing.value : false;
        
        return searching || loadingAll || initializing;
    });

    return {
        risks,
        isLoading
    };
}