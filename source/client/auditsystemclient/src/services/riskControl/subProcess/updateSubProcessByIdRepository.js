import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const SUB_PROCESS_API_ENDPOINT = '/api/SubProcess';
const SUB_PROCESS_API_ENDPOINT_ID = '/update-sub-process';

export const updateSubProcessByIdRepository = {
    async updateSubProcessById(subProcessData) {
        let responseData = null;
        try {
            const token = authService.getLocalToken();

            const payload = {
                id: subProcessData.id,
                particular: subProcessData.particular,
                auditProcessId:  typeof subProcessData.auditProcessId === 'object'
                    ? subProcessData.auditProcessId.value || subProcessData.auditProcessId.id 
                    : subProcessData.auditProcessId,
            };

            const response = await api.put(
                `${SUB_PROCESS_API_ENDPOINT}${SUB_PROCESS_API_ENDPOINT_ID}`,
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
                return ApiResponseT.ok(null, data?.successMessage || 'SubProcess updated successfully.');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: data?.successMessage || 'Update failed' }];
            console.error('Update sub process failed with errors:', errors);
            return ApiResponseT.badRequest(errors, data?.successMessage || 'Failed to update sub process.');
            
        } catch (error) {
            responseData = error.response?.data;
            console.error('Error updating sub process:', error);

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
                    [{ message: 'SubProcess not found' }],
                    'SubProcess not found'
                );
            }
        
            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while updating sub process.' }],
                'Server error'
            );  
        }
    }
};