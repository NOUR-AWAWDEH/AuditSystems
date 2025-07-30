import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const AUDIT_PROCESS_API_ENDPOINT = '/api/AuditProcess';
const AUDIT_PROCESS_API_ENDPOINT_ID = '/get-audit-process-by-id';

export const getAuditProcessByIdRepository = {
    async getAuditProcessById(auditProcessId) {
        let responseData = null; // Declare responseData here
        try {
            const token = authService.getLocalToken();

            const response = await api.get(
                `${AUDIT_PROCESS_API_ENDPOINT}${AUDIT_PROCESS_API_ENDPOINT_ID}`, {
                    data: { id: auditProcessId },
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                });
            
            const { data, status } = response; // Destructure data and status
            responseData = data; // Assign data to responseData
            
            if (status === 200 && data?.success !== false) { // Check data.success
                console.log('Audit process retrieved successfully:', data);
                return ApiResponseT.ok(data?.result, data?.successMessage || 'Audit process retrieved successfully');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: 'Failed to retrieve process.' }];
            console.error('Retrieve process failed with errors:', errors);
            return ApiResponseT.badRequest(
                errors,
                data?.successMessage || 'Failed to retrieve process.'
            );

        } catch (error) {
            responseData = error.response?.data; // Assign error response data to responseData
            console.error('Error retrieving audit process:', error);

            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = responseData?.errors || 
                    [{ message: responseData?.successMessage || 'Bad request' }];
                return ApiResponseT.badRequest(
                    errors,
                    responseData?.successMessage || 'Bad request'
                );
            }

            if (error.response?.status === 404) {
                return ApiResponseT.notFound(
                    [{ message: 'Process not found.' }],
                    'Process not found.'
                );
            }
            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while retrieving process.' }],
                'Retrieve process failed'
            );
        }
    }
};
