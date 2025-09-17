/**
 * Composable to manage risk data for a user.
 *
 * Combines data from two composables:
 * 1. `useSearchRisksByUserId` → handles searching/filtering risks by user.
 * 2. `useGetRisksByUserId`    → fetches all risks for the user.
 *
 * Provides reactive state and functions:
 * - `searchResults` → reactive array of risks returned by search.
 * - `allRisks`     → reactive array of all risks for the user.
 * - `isSearching`  → true while search API is running.
 * - `isLoadingAll` → true while fetching all risks.
 * - `totalCount`   → total number of risks returned by search.
 * - `searchRisks`  → function to perform risk search.
 * - `clearSearch`  → function to clear search results.
 * - `getRisksByUserId` → function to fetch all risks for the user.
 *
 * Notes:
 * - Defaults to empty arrays or false for loading flags if the underlying composables are undefined.
 * - Reactive: updates automatically when the underlying refs change.
 */

import { ref } from 'vue';
import { useSearchRisksByUserId } from '@/composables/riskControl/risk/useSearchRisksByUserId';
import { useGetRisksByUserId } from '@/composables/riskControl/risk/useGetRisksByUserId';

export function useRiskData() {
    const {
        searchResults = ref([]),
        isSearching = ref(false),
        totalCount = ref(0),
        searchRisks,
        clearSearch
    } = useSearchRisksByUserId() || {};

    const {
        risks: allRisks = ref([]),
        isLoading: isLoadingAll = ref(false),
        getRisksByUserId
    } = useGetRisksByUserId() || {};

    return {
        searchResults,
        allRisks,
        isSearching,
        isLoadingAll,
        totalCount,
        searchRisks,
        clearSearch,
        getRisksByUserId
    };
}