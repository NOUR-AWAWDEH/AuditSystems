import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from '@/services/auth/authRepos/authService';

const DELETE_AUDIT_PROGRAM_API_ENDPOINT = '/api/AuditProgram';
const DELETE_AUDIT_PROGRAM_BY_ID_API_ENDPOINT = '/delete-audit-program-by-id';

export const deleteAuditProgramByIdRepository = {
    async deleteAuditProgramById(auditProgramId) {
        try {
            const token = authService.getLocalToken();

            const response = await api.delete(
                `${DELETE_AUDIT_PROGRAM_API_ENDPOINT}${DELETE_AUDIT_PROGRAM_BY_ID_API_ENDPOINT}`, {
                    data: { id: auditProgramId },
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
            });

            
            const { data, status } = response;

            if (status === 204 || (status === 200 && data?.success !== false)) {
                return ApiResponseT.ok(null, 'Audit Program deleted successfully');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: data?.successMessage || 'Delete Audit Program failed' }];
            return ApiResponseT.badRequest(errors, data?.successMessage);

        } catch (error) {
            console.error('Delete Audit Program error:', error);
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
                    [{ message: 'Audit Program not found' }], 'Audit Program not found'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while deleting Audit Program.' }],
                'An unexpected error occurred while deleting Audit Program.'
            );
        }
    }
};
