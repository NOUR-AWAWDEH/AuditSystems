import api from '@/utils/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authService';

const DEPARTMENT_BSE_ENDPOINT = '/api/department';
const GET_DEPARTMENT_BY_USER_ID_ENDPOINT = '/get-department-by-user-id';

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

            if (response.data?.success && response.data?.result) {
                return ApiResponseT.ok(response.data.result, response.data.message || 'Department retrieved successfully.');
            }

            return ApiResponseT.badRequest(
                [{ message: 'Unexpected response format' }],
                'Failed to retrieve department.'
            );
        } catch (error) {
            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = error.response.data?.errors ||
                    [{
                        message: error.response.data?.message ||
                            'Bad request'
                    }];
                return ApiResponseT.badRequest(
                    errors,
                    error.response.data?.message || 'Bad request'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while retrieving department.' }],
                'Server error'
            );
        }
    }
};