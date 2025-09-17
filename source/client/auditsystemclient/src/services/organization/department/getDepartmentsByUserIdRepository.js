import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const DEPARTMENT_BSE_ENDPOINT = '/api/department';
const GET_DEPARTMENT_BY_USER_ID_ENDPOINT = '/get-departments-by-user-id';

export const getDepartmentByUserIdRepository = {
    async getDepartmentByUserId() {
        try {
            const token = authService.getLocalToken();

            const response = await api.get(
                `${DEPARTMENT_BSE_ENDPOINT}${GET_DEPARTMENT_BY_USER_ID_ENDPOINT}`, {

                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            // Fix: Access data through response.data
            if (response.data?.success && response.data?.result) {
                return ApiResponseT.ok(response.data.result, response.data.successMessage || 'Department retrieved successfully.');
            }

            return ApiResponseT.badRequest(
                response.data?.errors || [{ message: 'Failed to retrieve department.' }],
                response.data?.successMessage || 'Failed to retrieve department.'
            );
            
        } catch (error) {
            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = error.response?.data?.errors ||
                    [{
                        message: error.response?.data?.successMessage ||
                            'Bad request'
                    }];
                return ApiResponseT.badRequest(
                    errors,
                    error.response?.data?.successMessage || 'Bad request'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while retrieving department.' }],
                'Server error'
            );
        }
    }
};