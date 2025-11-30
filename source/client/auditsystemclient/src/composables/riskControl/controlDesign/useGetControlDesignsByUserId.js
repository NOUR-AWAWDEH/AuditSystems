import { ref } from 'vue';
import { getControlDesignsByUserIdRepository } from '@/services/riskControl/controlDesign/getControlDesignsByUserIdRepository';

export function useGetControlDesignsByUserId() {
    const controlDesigns = ref([]);
    const isLoading = ref(false);
    const error = ref(null);

    const handleFetchSuccess = (response) => {
        const controlDesignsData = Array.isArray(response.result) ? response.result : [];
        controlDesigns.value = controlDesignsData;
        return { success: true, data: controlDesignsData };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch control designs') => {
        const errorMessage = response?.errors?.length > 0
            ? response.errors[0]?.message
            : response?.successMessage || defaultErrorMessage;

        error.value = errorMessage;

        return {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const getControlDesignsByUserId = async () => {
        try {
            isLoading.value = true;
            error.value = null;
            controlDesigns.value = [];

            const response = await getControlDesignsByUserIdRepository.getControlDesignsByUserId();

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch control designs');

        } finally {
            isLoading.value = false;
        }
    };

    return {
        controlDesigns,
        isLoading,
        error,
        getControlDesignsByUserId,
    };
}