/**
 * Composable to manage the lifecycle of risk data loading.
 *
 * Handles initialization and fetching of risks when the component mounts.
 * Ensures that data is only loaded when necessary and prevents duplicate calls.
 *
 * @param {Object} params - Object containing reactive refs and functions:
 * @param {Ref<string|null>} params.selectedObjectiveId - Currently selected objective ID.
 * @param {Ref<number>} params.currentPage - Current pagination page.
 * @param {Ref<number>} params.pageSize - Number of items per page.
 * @param {Function} params.searchRisks - Function to search risks by objective and pagination.
 * @param {Ref<Array>} params.objectivesRef - Array of all objectives.
 * @param {Ref<string>} params.searchQuery - Current search query.
 * @param {Function} params.getRisksByUserId - Function to fetch all risks for the user.
 * @param {Function} params.resetToDefaultView - Function to reset the UI/data to default state.
 *
 * Returns reactive flags and an initialization function:
 * - `isInitialized`  → true if initial data has been loaded for the selected objective.
 * - `isInitializing` → true while the initialization process is running.
 * - `initializeData` → function to manually trigger initialization for the selected objective.
 *
 * Behavior:
 * 1. On mount, waits for the next DOM tick, then:
 *    - If an objective is selected and exists in `objectivesRef`, calls `initializeData`.
 *    - If the selected objective does not exist, calls `resetToDefaultView`.
 *    - If no objective is selected and there is no search query, fetches all risks via `getRisksByUserId`.
 * 2. `initializeData` fetches risks only if the data is not already initialized or initializing.
 * 3. Handles errors gracefully and ensures loading flags are properly reset.
 *
 * Notes:
 * - Reactive and safe: uses Vue refs to track state.
 * - Prevents multiple simultaneous API calls during initialization.
 */

import { ref, onMounted, nextTick } from 'vue';

export function useRiskLifecycle({
    selectedObjectiveId,
    currentPage,
    pageSize,
    searchRisks,
    objectivesRef,
    searchQuery,
    getRisksByUserId,
    resetToDefaultView
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const initializeData = async () => {
        if (!selectedObjectiveId.value || isInitialized.value || isInitializing.value) {
            return;
        }

        try {
            isInitializing.value = true;
            if (searchRisks) {
                await searchRisks({
                    observationId: selectedObjectiveId.value,
                    page: currentPage.value,
                    pageSize: pageSize.value,
                });
            }
            isInitialized.value = true;
        } catch (error) {
            console.error('Error initializing data:', error);
        } finally {
            isInitializing.value = false;
        }
    };

    onMounted(async () => {
        await nextTick();

        if (selectedObjectiveId.value && !isInitialized.value && !isInitializing.value) {
            if (objectivesRef.value?.length > 0) {
                const objectiveExists = objectivesRef.value.some(
                    objective => objective.id === selectedObjectiveId.value);
                if (objectiveExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedObjectiveId.value 
            && !searchQuery.value.trim() 
            && getRisksByUserId) {
                await getRisksByUserId();
        }
    });

    return {
        isInitialized,
        isInitializing,
        initializeData
    };
}