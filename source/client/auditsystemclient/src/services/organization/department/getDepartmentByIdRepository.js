import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const DEPARTMENT_BASE_ENDPOINT = '/api/department';
const GET_DEPARTMENT_BY_ID_ENDPOINT = '/get-department-by-id';

export const getDepartmentByIdRepository = {
    async getDepartmentById(departmentId) {
        try {
            const token = authService.getLocalToken();

            const response = await api.get(
                `${DEPARTMENT_BASE_ENDPOINT}${GET_DEPARTMENT_BY_ID_ENDPOINT}`, {

                params: { id: departmentId },
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.data && response.status === 200) {
                return ApiResponseT.ok(response.data.result, response.data.message || 'Department retrived successfully.');
            }

            return ApiResponseT.badRequest(
                response.data?.errors || [{ message: 'Failed to retrieve department.' }],
                response.data?.message || 'Failed to retrieve department.'
            );

        } catch (error) {
            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = error.response.data?.errors || [{ message: error.response.data?.message || 'Bad request' }];
                return ApiResponseT.badRequest(
                    errors,
                    error.response.data?.message || 'Bad request'
                );
            }

            if (error.response?.status === 404) {
                return ApiResponseT.notFound(
                    [{ message: 'Department not found.' }],
                    'Department not found.'
                );
            }
            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while retriving department.' }],
                'Server error'
            );
        }
    }
};
