import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUB_PROCESS_API_ENDPOINT = '/api/SubProcess';
const CREATE_SUB_PROCESS_API_ENDPOINT = '/create-sub-process';

export const createSubProcessRepository = { 
    async createSubProcess(subProcessData) {
        try{
            const token = authService.getLocalToken();

            const payload = {
                particular: subProcessData.particular,
                auditProcessId: typeof subProcessData.auditProcessId === 'object' 
                    ? subProcessData.auditProcessId.value || subProcessData.auditProcessId.id
                    : subProcessData.auditProcessId
            };

            const response = await api.post(
                `${SUB_PROCESS_API_ENDPOINT}${CREATE_SUB_PROCESS_API_ENDPOINT}`,
                payload,
                {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
            });

            const { data, status } = response;

            if (status === 201 && data?.success) {
                return ApiResponseT.ok(data.result, data.successMessage || 'SubProcess created successfully');
            }

            return ApiResponseT.badRequest(
                data?.errors || [{ message: data?.successMessage || 'Failed to create SubProcess' }],
                data?.successMessage || 'Failed to create SubProcess.'
            );

        }catch(error){
            console.error('Error creating SubProcess:', error);
            const responseData = error.response?.data;

            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = responseData?.errors || [{ message: responseData?.successMessage || 'Bad request' }];
                return ApiResponseT.badRequest(errors, responseData?.successMessage || 'Bad request');
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while creating sub-process.' }], 'Server error'
            );
        }
    }
};