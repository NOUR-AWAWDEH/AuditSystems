import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const RISK_CONTROL_MATRIX_API_ENDPOINT = '/api/RiskControlMatrix';
const RISK_CONTROL_MATRIX_API_ENDPOINT_ID = '/get-risk-control-matrices-by-user-id';

export const getRiskControlMatricesByUserIdRepository = {
    async getRiskControlMatricesByUserId() {
        let responseData = null;
        try {
            const token = authService.getLocalToken();

            const response = await api.get(
                `${RISK_CONTROL_MATRIX_API_ENDPOINT}${RISK_CONTROL_MATRIX_API_ENDPOINT_ID}`, {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                }
            );

            const { data, status } = response;
            responseData = data;
            
            if (status === 200 && data?.success !== false) {
                return ApiResponseT.ok(data?.result?.riskControlMatrixModels, data?.successMessage || 'Risk Control Matrices retrieved successfully');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: 'Failed to retrieve Risk Control Matrices.' }];
            console.error('Retrieve Risk Control Matrices failed with errors:', errors);
            return ApiResponseT.badRequest(
                errors,
                data?.successMessage || 'Failed to retrieve Risk Control Matrices.'
            );

        } catch (error) {
            responseData = error.response?.data;
            console.error('Error retrieving Risk Control Matrices:', error);

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
                [{ message: error.message || 'An unexpected error occurred while retrieving Risk Control Matrices.' }],
                'An unexpected error occurred while retrieving Risk Control Matrices.'
            );
        }
    }
};
