/**
 * Composable to manage the lifecycle of risk data loading.
 *
 * Handles initialization and fetching of risks when the component mounts.
 * Ensures that data is only loaded when necessary and prevents duplicate calls.
 *
 * @param {Object} params - Object containing reactive refs and functions:
 * @param {Ref<string|null>} params.selectedObservationId - Currently selected observation ID.
 * @param {Ref<number>} params.currentPage - Current pagination page.
 * @param {Ref<number>} params.pageSize - Number of items per page.
 * @param {Function} params.searchRisks - Function to search risks by observation and pagination.
 * @param {Ref<Array>} params.observationsRef - Array of all observations.
 * @param {Ref<string>} params.searchQuery - Current search query.
 * @param {Function} params.getRisksByUserId - Function to fetch all risks for the user.
 * @param {Function} params.resetToDefaultView - Function to reset the UI/data to default state.
 *
 * Returns reactive flags and an initialization function:
 * - `isInitialized`  → true if initial data has been loaded for the selected observation.
 * - `isInitializing` → true while the initialization process is running.
 * - `initializeData` → function to manually trigger initialization for the selected observation.
 *
 * Behavior:
 * 1. On mount, waits for the next DOM tick, then:
 *    - If an observation is selected and exists in `observationsRef`, calls `initializeData`.
 *    - If the selected observation does not exist, calls `resetToDefaultView`.
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
    selectedObservationId,
    currentPage,
    pageSize,
    searchRisks,
    observationsRef,
    searchQuery,
    getRisksByUserId,
    resetToDefaultView
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const initializeData = async () => {
        if (!selectedObservationId.value || isInitialized.value || isInitializing.value) {
            return;
        }

        try {
            isInitializing.value = true;
            if (searchRisks) {
                await searchRisks({
                    observationId: selectedObservationId.value,
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

        if (selectedObservationId.value && !isInitialized.value && !isInitializing.value) {
            if (observationsRef.value?.length > 0) {
                const observationExists = observationsRef.value.some(
                    observation => observation.id === selectedObservationId.value);
                if (observationExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedObservationId.value 
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