import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const AUDIT_PROCESS_API_ENDPOINT = '/api/AuditProcess';
const AUDIT_PROCESS_API_ENDPOINT_ID = '/get-audit-processes-by-user-id';

export const getAuditProcessesByUserIdRepository = {
    async getAuditProcessesByUserId() {
        let responseData = null;
        try {
            const token = authService.getLocalToken();

            const response = await api.get(
                `${AUDIT_PROCESS_API_ENDPOINT}${AUDIT_PROCESS_API_ENDPOINT_ID}`, {
                    params: {
                        UserId: '00000000-0000-0000-0000-000000000000' // Placeholder GUID, will be overridden by server
                    },
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                }
            );
            
            const { data, status } = response; // Destructure data and status
            responseData = data; // Assign data to responseData
            
            if (status === 200 && data?.success !== false) { // Check data.success
                return ApiResponseT.ok(data?.result, data?.successMessage || 'Processes retrieved successfully'); 
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: 'Failed to retrieve processes.' }];
            console.error('Retrieve processes failed with errors:', errors);
            return ApiResponseT.badRequest(
                errors,
                data?.successMessage || 'Failed to retrieve processes.'
            );

        } catch (error) {
            responseData = error.response?.data; // Assign error response data to responseData
            console.error('Error retrieving audit processes:', error);

            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = responseData?.errors ||
                    [{
                        message: responseData?.successMessage ||
                            'Bad request'
                    }];
                return ApiResponseT.badRequest(
                    errors,
                    responseData?.successMessage || 'Bad request'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while retrieving processes.' }],
                'Server error'
            );
        }
    }
};