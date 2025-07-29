import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from '@/services/auth/authRepos/authService';

const AUDIT_PROCESS_API_ENDPOINT = '/api/AuditProcess';
const AUDIT_PROCESS_API_ENDPOINT_ID = '/delete-audit-process-by-id';

export const deleteAuditProcessByIdRepository = {
    async deleteAuditProcessById(auditProcessId) {
        try {
                const token = authService.getLocalToken();

                const response = await api.delete(
                    `${AUDIT_PROCESS_API_ENDPOINT}${AUDIT_PROCESS_API_ENDPOINT_ID}`, {
                        data: { id: auditProcessId },
                        headers: {
                            'Authorization': `Bearer ${token}`,
                            'Content-Type': 'application/json',
                        },
                });
                
                if (response.status === 204 || response?.data?.success !== false) {
                    return ApiResponseT.ok(null, 'Audit process deleted successfully');
                }

            const errors = Array.isArray(response?.errors)
                ? response.errors
                : [{ message: response?.data?.successMessage || 'Delete process failed' }];
            return ApiResponseT.badRequest(errors, response?.data?.successMessage);

        } catch (error) {
            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = error.response?.errors ||
                    [{ message: error.response?.data?.successMessage || 'Bad request' }];
                return ApiResponseT.badRequest(errors, error.response?.data?.successMessage || 'Bad request');
            }

            if (error.response?.status === 404) {
                return ApiResponseT.notFound(
                    [{ message: 'Process not found' }], 'Process not found'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while deleting process.' }],
                'Delete process failed'
            );
        }
}};