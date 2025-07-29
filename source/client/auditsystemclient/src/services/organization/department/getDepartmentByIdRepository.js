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

            // Fix: Access response.data and response.status correctly
            const { data, status } = response;

            if (status === 200 && data?.success && data?.result) {
                return ApiResponseT.ok(
                    data.result, 
                    data.successMessage || 'Department retrieved successfully.'
                );
            }

            return ApiResponseT.badRequest(
                data?.errors || [{ message: data?.message || 'Failed to retrieve department.' }],
                data?.message || 'Failed to retrieve department.'
            );

        } catch (error) {
            console.error('Error fetching department:', error);
            
            // Fix: Define responseData from error.response?.data
            const responseData = error.response?.data;

            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = responseData?.errors || 
                    [{ message: responseData?.message || 'Bad request' }];
                return ApiResponseT.badRequest(
                    errors,
                    responseData?.message || 'Bad request'
                );
            }

            if (error.response?.status === 404) {
                return ApiResponseT.notFound(
                    [{ message: 'Department not found.' }],
                    'Department not found.'
                );
            }
            
            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while retrieving department.' }],
                'Server error'
            );
        }
    }
};
