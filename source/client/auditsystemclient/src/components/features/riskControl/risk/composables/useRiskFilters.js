/**
 * Composable to manage filtering of a list of risks based on a search query.
 *
 * Provides a reactive search query and a computed list of filtered risks.
 *
 * @param {Ref<Array>} risks - Reactive array of risk objects to filter.
 *   Each risk object is expected to have a `riskName` property (string).
 *
 * Returns:
 * - `searchQuery` → Reactive string representing the current search term.
 * - `filteredRisks` → Computed array of risks filtered by `searchQuery`.
 *   - Case-insensitive filtering on the `riskName` property.
 *   - Returns the full array if search query is empty.
 *   - Returns an empty array if `risks.value` is not an array.
 *
 * Behavior:
 * - Automatically clears the search query if the underlying risks list changes
 *   and results in zero matches.
 *
 * Notes:
 * - Fully reactive: updates `filteredRisks` whenever `searchQuery` or `risks` change.
 * - Safe: ensures `filteredRisks` always returns an array to prevent UI errors.
 */

import { ref, computed, watch } from 'vue';

export function useRiskFilters(risks) {
    const searchQuery = ref('');

    const filteredRisks = computed(() => {
        if (!Array.isArray(risks.value)) return [];
        let filtered = risks.value;

        if (searchQuery.value.trim()) {
            const query = searchQuery.value.toLowerCase().trim();
            filtered = filtered.filter((risk) =>
                risk.riskName.toLowerCase().includes(query)
            );
        }

        return filtered;
    });

    // Optional: auto-clear search if the list changes drastically
    watch(risks, () => {
        if (searchQuery.value && filteredRisks.value.length === 0) {
            searchQuery.value = '';
        }
    });

    return {
        searchQuery,
        filteredRisks
    };
}