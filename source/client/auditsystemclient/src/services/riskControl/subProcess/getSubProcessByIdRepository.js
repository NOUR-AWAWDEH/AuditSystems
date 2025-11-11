import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUB_PROCESS_API_ENDPOINT = '/api/SubProcess';
const SUB_PROCESS_API_ENDPOINT_ID = '/get-sub-process-by-id';

export const getSubProcessByIdRepository = {
    async getSubProcessById(subProcessId) {
        let responseData = null;
        try {
            const token = authService.getLocalToken();

            const response = await api.get(
                `${SUB_PROCESS_API_ENDPOINT}${SUB_PROCESS_API_ENDPOINT_ID}`, {
                    params: { id: subProcessId },
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                });
            
            const { data, status } = response;
            responseData = data;
            
            if (status === 200 && data?.success !== false) {
                return ApiResponseT.ok(data?.result, data?.successMessage || 'SubProcess retrieved successfully');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: 'Failed to retrieve SubProcess.' }];
            console.error('Retrieve SubProcess failed with errors:', errors);
            
            return ApiResponseT.badRequest(
                errors,
                data?.successMessage || 'Failed to retrieve SubProcess.'
            );

        } catch (error) {
            responseData = error.response?.data;
            console.error('Error retrieving a SubProcess:', error);

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
                    [{ message: 'SubProcess not found.' }],
                    'SubProcess not found.'
                );
            }
            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while retrieving SubProcess.' }],
                'Retrieve SubProcess failed'
            );
        }
    }
};