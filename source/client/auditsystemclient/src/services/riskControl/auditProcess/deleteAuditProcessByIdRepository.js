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
            
            const { data, status } = response;
            console.log('Delete audit process response:', { data, status });
            
            if (status === 204 || (status === 200 && data?.success !== false)) {
                return ApiResponseT.ok(null, 'Audit process deleted successfully');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: data?.successMessage || 'Delete process failed' }];
            return ApiResponseT.badRequest(errors, data?.successMessage);

        } catch (error) {
            console.error('Delete audit process error:', error);
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
                    [{ message: 'Process not found' }], 'Process not found'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while deleting process.' }],
                'Delete process failed'
            );
        }
    }
};