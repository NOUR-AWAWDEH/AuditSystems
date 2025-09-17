import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const AUDIT_PROCESS_API_ENDPOINT = '/api/AuditProcess';
const CREATE_AUDIT_PROCESS_API_ENDPOINT = '/create-audit-process';

export const createAuditProcessRepository = {
    async createAuditProcess(AuditProcessData) {
        try {
            const token = authService.getLocalToken();

            const payload = {
                processName: AuditProcessData.processName,
                SubDepartmentId: typeof AuditProcessData.subDepartmentId === 'object' 
                    ? AuditProcessData.subDepartmentId.value || AuditProcessData.subDepartmentId.id
                    : AuditProcessData.subDepartmentId
            };

            const response = await api.post(
                `${AUDIT_PROCESS_API_ENDPOINT}${CREATE_AUDIT_PROCESS_API_ENDPOINT}`,
                payload, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json',
                },
            });

            const { data, status } = response;

            if (status === 201 && data?.success) {
                return ApiResponseT.ok(data.result, data.successMessage || 'Process created successfully');
            }

            return ApiResponseT.badRequest(
                data?.errors || [{ message: data?.successMessage || 'Failed to create process' }],
                data?.successMessage || 'Failed to create process.'
            );
            
        }
        catch (error) {
            console.error('Create audit process error:', error);
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
                [{ message: error.message || 'An unexpected error occurred while creating process.' }], 'Server error'
            );
        }
    }
};