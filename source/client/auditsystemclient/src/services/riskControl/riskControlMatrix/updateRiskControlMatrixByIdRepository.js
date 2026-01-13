import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const RISK_CONTROL_MATRIX_API_ENDPOINT = '/api/RiskControlMatrix';
const RISK_CONTROL_MATRIX_API_ENDPOINT_ID = '/update-risk-control-matrix';

export const updateRiskControlMatrixByIdRepository = {
    async updateRiskControlMatrixById(riskControlMatrixData) {
        let responseData = null;
        try {
            const token = authService.getLocalToken();

            const payload = {
                id: riskControlMatrixData.id,
                description: riskControlMatrixData.description,
                auditProcessId:  (riskControlMatrixData.auditProcessId && typeof riskControlMatrixData.auditProcessId === 'object')
                    ? riskControlMatrixData.auditProcessId.value || riskControlMatrixData.auditProcessId.id
                    : riskControlMatrixData.auditProcessId,
            };

            const response = await api.put(
                `${RISK_CONTROL_MATRIX_API_ENDPOINT}${RISK_CONTROL_MATRIX_API_ENDPOINT_ID}`,
                payload,
                {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                }
            );

            const { data, status } = response;
            responseData = data;

            if (status === 200 && data?.success !== false) {
                return ApiResponseT.ok(null, data?.successMessage || 'Risk Control Matrix updated successfully.');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: data?.successMessage || 'Update failed' }];
            console.error('Update risk control matrix failed with errors:', errors);
            return ApiResponseT.badRequest(errors, data?.successMessage || 'Failed to update risk control matrix.');

        } catch (error) {
            responseData = error.response?.data;
            console.error('Error updating risk control matrix:', error);

            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = responseData?.errors || [{ message: responseData?.message || 'Bad request' }];
                return ApiResponseT.badRequest(errors, responseData?.message || 'Bad request');
            }

            if (error.response?.status === 404) {
                return ApiResponseT.notFound(
                    [{ message: 'Risk Control Matrix not found' }],
                    'Risk Control Matrix not found'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'Failed to update risk control matrix.' }],
                'An unexpected error occurred while updating risk control matrix.'
            );
        }
    }
};
