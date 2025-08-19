import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const RISK_API_ENDPOINT = '/api/Risk';
const RISK_API_ENDPOINT_ID = '/get-risks-by-user-id';

export const getRisksByUserIdRepository = {
    async getRisksByUserId() {
        let responseData = null;
        try {
            const token = authService.getLocalToken();

            const response = await api.get(
                `${RISK_API_ENDPOINT}${RISK_API_ENDPOINT_ID}`, {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                }
            );
            
            console.log("GetRisksByUserIdRepository: ", response);
            
            const { data, status } = response;
            responseData = data;

            if (status === 200 && data?.success !== false) {
                return ApiResponseT.ok(data?.result, data?.successMessage || 'Risks retrieved successfully');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: 'Failed to retrieve Risks.' }];
            console.error('Retrieve Risks failed with errors:', errors);
            return ApiResponseT.badRequest(
                errors,
                data?.successMessage || 'Failed to retrieve Risks.'
            );

        } catch (error) {
            responseData = error.response?.data;
            console.error('Error retrieving Risks:', error);

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
                [{ message: error.message || 'An unexpected error occurred while retrieving Risks.' }],
                'An unexpected error occurred while retrieving Risks.'
            );
        }
    }
};