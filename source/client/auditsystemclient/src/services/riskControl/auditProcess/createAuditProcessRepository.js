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

        if (response && response.status === 201) {
            if(response.success && response.result) {
                return ApiResponseT.ok(response.result, response.successMessage || 'Process created successfully');
            } else if (response.success === false) {
                return ApiResponseT.badRequest(
                    response.errors || [{ message: response.successMessage || 'Failed to create process' }],
                    response.successMessage || 'Failed to create process.'
                );
            }
        }

        return ApiResponseT.badRequest(
            [{ message: response.successMessage || 'Unexpected response format from server' }],
            response.successMessage || 'Failed to create process.'
        );
    }
    catch (error) {
        if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = error.response?.errors || [{ message: error.response?.data?.successMessage || 'Bad request' }];
                return ApiResponseT.badRequest(errors, error.response?.data?.successMessage || 'Bad request');
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while creating process.' }], 'Server error'
            );
    }
  }
};