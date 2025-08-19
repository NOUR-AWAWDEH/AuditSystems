import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const RISK_API_ENDPOINT = '/api/Risk';
const RISK_API_ENDPOINT_ID = '/get-risk-by-id';

export const getRiskByIdRepository = {
    async getRiskById(riskId) {
        let responseData = null;
        try {
            const token = authService.getLocalToken();

            const response = await api.get(
                `${RISK_API_ENDPOINT}${RISK_API_ENDPOINT_ID}`, {
                    data: { id: riskId },
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                });
            
            console.log("Get Risk Response:", response);

            const { data, status } = response;
            responseData = data;

            if (status === 200 && data?.success !== false) {
                return ApiResponseT.ok(data?.result, data?.successMessage || 'Risk retrieved successfully');

            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: 'Failed to retrieve Risk.' }];
            console.error('Retrieve Risk failed with errors:', errors);

            return ApiResponseT.badRequest(
                errors,
                data?.successMessage || 'Failed to retrieve Risk.'
            );

        } catch (error) {
            responseData = error.response?.data;
            console.error('Error retrieving an Risk:', error);

            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = responseData?.errors ||
                    [{ message: responseData?.successMessage || 'Bad request' }];
                return ApiResponseT.badRequest(
                    errors,
                    responseData?.successMessage || 'Bad request'
                );
            }

            if (error.response?.status === 404) {
                return ApiResponseT.notFound(
                    [{ message: 'Risk not found.' }],
                    'Risk not found.'
                );
            }
            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while retrieving Risk.' }],
                'An unexpected error occurred while retrieving Risk.'
            );
        }
    }
};
