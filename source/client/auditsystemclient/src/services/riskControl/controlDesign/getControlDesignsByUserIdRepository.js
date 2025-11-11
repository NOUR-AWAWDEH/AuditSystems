import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const CONTROL_DESIGN_API_ENDPOINT = '/api/ControlDesign';
const CONTROL_DESIGN_API_ENDPOINT_ID = '/get-control-designs-by-user-id';

export const getControlDesignsByUserIdRepository = {
    async getControlDesignsByUserId() {
        let responseData = null;
        try {
            const token = authService.getLocalToken();

            const response = await api.get(
                `${CONTROL_DESIGN_API_ENDPOINT}${CONTROL_DESIGN_API_ENDPOINT_ID}`, {    
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                }
            );
            
            const { data, status } = response;
            responseData = data;
            if (status === 200 && data?.success !== false) {
                return ApiResponseT.ok(data?.result?.controlDesignModels, data?.successMessage || 'Control Designs retrieved successfully');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: 'Failed to retrieve Control Designs.' }];
            console.error('Retrieve Control Designs failed with errors:', errors);
            return ApiResponseT.badRequest(
                errors,
                data?.successMessage || 'Failed to retrieve Control Designs.'
            );

        } catch (error) {
            responseData = error.response?.data;
            console.error('Error retrieving Control Designs:', error);

            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = responseData?.errors ||
                    [{
                        message: responseData?.successMessage ||
                            'Bad request'
                    }];
                return ApiResponseT.badRequest(
                    errors,
                    responseData?.successMessage || 'Bad request'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while retrieving Control Designs.' }],
                'An unexpected error occurred while retrieving Control Designs.'
            );
        }
    }
};