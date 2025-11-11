import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from '@/services/auth/authRepos/authService';

const RISK_CONTROL_MATRIX_API_ENDPOINT = '/api/RiskControlMatrix';
const DELETE_RISK_CONTROL_MATRIX_API_ENDPOINT = '/delete-risk-control-matrix-by-id';

export const deleteRiskControlMatrixByIdRepository = {
    async deleteRiskControlMatrixById(riskControlMatrixId) {
        try {
            const token = authService.getLocalToken();

            const response = await api.delete(
                `${RISK_CONTROL_MATRIX_API_ENDPOINT}${DELETE_RISK_CONTROL_MATRIX_API_ENDPOINT}`, {
                    data: { id: riskControlMatrixId },
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
            });
            
            const { data, status } = response;
            
            if (status === 204 || (status === 200 && data?.success !== false)) {
                return ApiResponseT.ok(null, 'Risk Control Matrix deleted successfully');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: data?.successMessage || 'Delete Risk Control Matrix failed' }];
            return ApiResponseT.badRequest(errors, data?.successMessage);

        } catch (error) {
            console.error('Delete Risk Control Matrix error:', error);
            const responseData = error.response?.data;
            
            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = responseData?.errors ||
                    [{ message: responseData?.successMessage || 'Bad request' }];
                return ApiResponseT.badRequest(errors, responseData?.successMessage || 'Bad request');
            }

            if (error.response?.status === 404) {
                return ApiResponseT.notFound(
                    [{ message: 'Risk Control Matrix not found' }], 'Risk Control Matrix not found'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while deleting Risk Control Matrix.' }],
                'An unexpected error occurred while deleting Risk Control Matrix.'
            );
        }
    }
};