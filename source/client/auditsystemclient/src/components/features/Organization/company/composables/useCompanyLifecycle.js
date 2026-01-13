import { ref, onMounted, nextTick } from 'vue';

export function useCompanyLifecycle({
    getCompanysByUserId,
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const initializeData = async () => {
        if (isInitialized.value || isInitializing.value) {
            return;
        }

        try {
            isInitializing.value = true;
            if (getCompanysByUserId) {
                await getCompanysByUserId();
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

        if (!isInitialized.value && !isInitializing.value) {
           await initializeData();
        }
    });

    return {
        isInitialized,
        isInitializing,
        initializeData
    };
}