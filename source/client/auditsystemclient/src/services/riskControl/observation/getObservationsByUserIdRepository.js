import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const OBSERVATION_API_ENDPOINT = '/api/Observation';
const OBSERVATION_API_ENDPOINT_ID = '/get-observations-by-user-id';

export const getObservationsByUserIdRepository = {
    async getObservationsByUserId() {
        let responseData = null;
        try {
            const token = authService.getLocalToken();

            const response = await api.get(
                `${OBSERVATION_API_ENDPOINT}${OBSERVATION_API_ENDPOINT_ID}`, {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                }
            );
            
            const { data, status } = response;
            responseData = data;
            
            if (status === 200 && data?.success !== false) {
                const payload =
                    Array.isArray(data?.result?.observations) ? data.result.observations :
                    [];
                return ApiResponseT.ok(payload, data?.successMessage || 'Observations retrieved successfully');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: 'Failed to retrieve Observations.' }];
            console.error('Retrieve Observations failed with errors:', errors);
            return ApiResponseT.badRequest(
                errors,
                data?.successMessage || 'Failed to retrieve Observations.'
            );

        } catch (error) {
            responseData = error.response?.data;
            console.error('Error retrieving Observations:', error);

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
                [{ message: error.message || 'An unexpected error occurred while retrieving Observations.' }],
                'Server error'
            );
        }
    }
};