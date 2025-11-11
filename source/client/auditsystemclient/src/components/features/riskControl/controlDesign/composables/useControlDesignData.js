import { ref } from 'vue';
import { useSearchControlDesignsByUserId } from '@/composables/riskControl/controlDesign/useSearchControlDesignsByUserId';
import { useGetControlDesignsByUserId } from '@/composables/riskControl/controlDesign/useGetControlDesignsByUserId';

export function useControlDesignData() {
    const {
        searchResults = ref([]),
        isSearching = ref(false),
        totalCount = ref(0),
        searchControlDesigns,
        clearSearch
    } = useSearchControlDesignsByUserId() || {};

    const {
        controlDesigns: allControlDesigns = ref([]),
        isLoading: isLoadingAll = ref(false),
        getControlDesignsByUserId
    } = useGetControlDesignsByUserId() || {};

    return {
        searchResults,
        allControlDesigns,
        isSearching,
        isLoadingAll,
        totalCount,
        searchControlDesigns,
        clearSearch,
        getControlDesignsByUserId
    };
}