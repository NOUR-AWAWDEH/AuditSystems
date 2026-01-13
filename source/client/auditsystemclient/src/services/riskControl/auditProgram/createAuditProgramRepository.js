import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const AUDIT_PROGRAM_API_ENDPOINT = '/api/AuditProgram';
const CREATE_AUDIT_PROGRAM_API_ENDPOINT = '/create-audit-program';


export const createAuditProgramRepository = {
    async createAuditProgram(AuditProgramData) {
        try {
            const token = authService.getLocalToken();
            const payload = {
                description: AuditProgramData.description,
                exitControlId: AuditProgramData.exitControlId,
                controlDesignId: AuditProgramData.controlDesignId ?? null,
            };

            const response = await api.post(
                `${AUDIT_PROGRAM_API_ENDPOINT}${CREATE_AUDIT_PROGRAM_API_ENDPOINT}`,
                payload,
                {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    },
                }
            );

            const { data, status } = response;
            if (status === 201 && data?.success) {
                return ApiResponseT.ok(data.result, data.successMessage || 'Audit Program created successfully');
            }

            return ApiResponseT.badRequest(
                data?.errors || [{ message: data?.successMessage || 'Failed to create Audit Program' }],
                data?.successMessage || 'Failed to create Audit Program.'
            );

        } catch (error) {
            console.error('Error creating Audit Program:', error);
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
                [{ message: error.message || 'An unexpected error occurred while creating Audit Program.' }],
                'An unexpected error occurred while creating Audit Program.'
            );
        }
    }
};
