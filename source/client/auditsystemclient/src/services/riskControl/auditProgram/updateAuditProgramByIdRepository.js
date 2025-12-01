import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const AUDIT_PROGRAM_API_ENDPOINT = '/api/AuditProgram';
const AUDIT_PROGRAM_API_ENDPOINT_ID = '/update-audit-program-by-id';

export const updateAuditProgramByIdRepository = {
    async updateAuditProgramById(auditProgramData) {
        let responseData = null;
        try {
            const token = authService.getLocalToken();

            const payload = {
                id: auditProgramData.id,
                description: auditProgramData.description,
                exitControlId: auditProgramData.exitControlId,
                actionOwnerId: auditProgramData.actionOwnerId,
                controlDesignId: auditProgramData.controlDesignId ?? null,
            };

            const response = await api.put(
                `${AUDIT_PROGRAM_API_ENDPOINT}${AUDIT_PROGRAM_API_ENDPOINT_ID}`,
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
                return ApiResponseT.ok(null, data?.successMessage || 'Audit Program updated successfully.');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: data?.successMessage || 'Update failed' }];
            console.error('Update audit program failed with errors:', errors);
            return ApiResponseT.badRequest(errors, data?.successMessage || 'Failed to update audit program.');

        } catch (error) {
            responseData = error.response?.data;
            console.error('Error updating audit program:', error);

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
                    [{ message: 'Audit Program not found' }],
                    'Audit Program not found'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while updating audit program.' }],
                'An unexpected error occurred while updating audit program.'
            );
        }
    }
};