import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const AUDIT_PROCESS_API_ENDPOINT = '/api/AuditProcess';
const AUDIT_PROCESS_API_ENDPOINT_ID = '/update-audit-process';

export const updateAuditProcessByIdRepository = {
    async updateAuditProcessById(auditProcessData) {
        try {
            const token = authService.getLocalToken();

            if (!token) {
                throw new Error('Authentication token not found');
            }

            const payload = {
                id: auditProcessData.id,
                processName: auditProcessData.processName,
                subDepartmentId:  typeof auditProcessData.subDepartmentId === 'object'
                    ? auditProcessData.subDepartmentId.value || auditProcessData.subDepartmentId.id 
                    : auditProcessData.subDepartmentId,
            };

            const response = await api.put(
                `${AUDIT_PROCESS_API_ENDPOINT}${AUDIT_PROCESS_API_ENDPOINT_ID}`,
                payload,
                {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                }
            );

            if (response.status === 200 || response?.success !== false) {
                return ApiResponseT.ok(null, response?.data?.successMessage || 'Process updated successfully.');
            }

            const errors = Array.isArray(response?.errors)
            ? response.errors
            : [{ message: response?.data?.successMessage || 'Update failed' }];

            return ApiResponseT.badRequest( errors, response?.data?.successMessage || 'Failed to update process.');
        } catch (error) {

            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }
        
            if (error.response?.status === 400) {
                const errors = error.response?.errors || [{ message: error.response?.message || 'Bad request' }];
                return ApiResponseT.badRequest(errors, error.response?.message || 'Bad request');
            }
        
            if (error.response?.status === 404) {
                return ApiResponseT.notFound(
                    [{ message: 'Process not found' }],
                    'Process not found'
                );
            }
        
            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while updating process.' }],
                'Server error'
            );  
        }
    }
};