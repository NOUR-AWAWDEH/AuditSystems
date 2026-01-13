import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const RISK_CONTROL_MATRIX_BASE_ENDPOINT = '/api/RiskControlMatrix';
const GET_BY_ID_ENDPOINT = '/get-risk-control-matrix-by-id';

export const getRiskControlMatrixByIdRepository = {
    async getRiskControlMatrixById(riskControlMatrixId) {
        try {
            const token = authService.getLocalToken();

            const response = await api.get(
                `${RISK_CONTROL_MATRIX_BASE_ENDPOINT}${GET_BY_ID_ENDPOINT}`, {
                    params: { Id: riskControlMatrixId },
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                });

            const { data, status } = response;

            if (status === 200 && data?.success !== false) {
                const result = data.result || data.Result;
                const matrixModel = result?.riskControlMatrixModel || result?.RiskControlMatrixModel || result;
                return ApiResponseT.ok(matrixModel, data.successMessage || 'Risk Control Matrix retrieved successfully');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: 'Failed to retrieve Risk Control Matrix.' }];
            console.error('Retrieve Risk Control Matrix failed with errors:', errors);

            return ApiResponseT.badRequest(
                errors,
                data?.successMessage || 'Failed to retrieve Risk Control Matrix.'
            );

        } catch (error) {
            responseData = error.response?.data;
            console.error('Error retrieving a Risk Control Matrix:', error);

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
                    [{ message: 'Risk Control Matrix not found.' }],
                    'Risk Control Matrix not found.'
                );
            }
            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while retrieving Risk Control Matrix.' }],
                'An unexpected error occurred while retrieving Risk Control Matrix.'
            );
        }
    }
};
