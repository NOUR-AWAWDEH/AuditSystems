import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const RISK_CONTROL_MATRIX_API_ENDPOINT = '/api/RiskControlMatrix';
const CREATE_RISK_CONTROL_MATRIX_API_ENDPOINT = '/create-risk-control-matrix';

export const createRiskControlMatrixRepository = { 
    async createRiskControlMatrix(riskControlMatrixData) {
        try{
            const token = authService.getLocalToken();

            const payload = {
                Description: riskControlMatrixData.description,
                AuditProcessId: typeof riskControlMatrixData.auditProcessId === 'object' 
                    ? riskControlMatrixData.auditProcessId.value || riskControlMatrixData.auditProcessId.id
                    : riskControlMatrixData.auditProcessId
            };

            const response = await api.post(
                `${RISK_CONTROL_MATRIX_API_ENDPOINT}${CREATE_RISK_CONTROL_MATRIX_API_ENDPOINT}`,
                payload,
                {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
            });

            const { data, status } = response;

            if (status === 201 && data?.success) {
                return ApiResponseT.ok(data.result, data.successMessage || 'Risk Control Matrix created successfully');
            }

            return ApiResponseT.badRequest(
                data?.errors || [{ message: data?.successMessage || 'Failed to create Risk Control Matrix' }],
                data?.successMessage || 'Failed to create Risk Control Matrix.'
            );

        }catch(error){
            console.error('Error creating RiskControlMatrix:', error);
            const responseData = error.response?.data;

            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = responseData?.errors || [{ message: responseData?.successMessage || 'Bad request' }];
                return ApiResponseT.badRequest(errors, responseData?.successMessage || 'Bad request');
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while creating Risk Control Matrix.' }], 
                'An unexpected error occurred while creating Risk Control Matrix.'
            );
        }
    }
};