import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const AUDIT_PROCESS_API_ENDPOINT = '/api/AuditProcess';
const AUDIT_PROCESS_API_ENDPOINT_ID = '/update-audit-process';

export const updateAuditProcessByIdRepository = {
    async updateAuditProcessById(auditProcessData) {
        let responseData = null; // Declare responseData here
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

            const { data, status } = response; // Destructure data and status
            responseData = data; // Assign data to responseData

            if (status === 200 && data?.success !== false) { // Check data.success
                console.log('Audit process updated successfully:', data);
                return ApiResponseT.ok(null, data?.successMessage || 'Process updated successfully.');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: data?.successMessage || 'Update failed' }];
            console.error('Update process failed with errors:', errors);
            return ApiResponseT.badRequest(errors, data?.successMessage || 'Failed to update process.');
            
        } catch (error) {
            responseData = error.response?.data; // Assign error response data to responseData
            console.error('Error updating audit process:', error);

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