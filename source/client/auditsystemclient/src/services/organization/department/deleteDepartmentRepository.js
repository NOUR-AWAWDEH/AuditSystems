import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from '@/services/auth/authRepos/authService';

const DEPARTMENT_BSE_ENDPOINT = '/api/department';
const DELETE_DEPARTMENT_ENDPOINT = '/delete-department-by-id';

export const deleteDepartmentByIdRepository = {
    async deleteDepartmentById(departmentId) {
        try {
            const token = authService.getLocalToken();

            const response = await api.delete(
                `${DEPARTMENT_BSE_ENDPOINT}${DELETE_DEPARTMENT_ENDPOINT}`, {

                data: { id: departmentId },
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.status === 204 || response?.success !== false) {
                return ApiResponseT.ok(null, 'Department deleted successfully');
            }

            const errors = Array.isArray(response.errors)
                ? response.errors
                : [{ message: response.successMessage || 'Delete failed' }];
            return ApiResponseT.badRequest(errors, response.successMessage);

        } catch (error) {
            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                // Extract server error message properly
                const responseData = error.response?.data;
                const serverMessage = responseData?.message || responseData?.title || responseData?.detail || 'Bad request';
                const errors = responseData?.errors || [{ message: serverMessage }];
                return ApiResponseT.badRequest(errors, serverMessage);
            }

            if (error.response?.status === 404) {
                return ApiResponseT.notFound(
                    [{ message: 'Department not found' }], 'Department not found'
                );
            }

            // Handle other server errors
            const responseData = error.response?.data;
            const serverMessage = responseData?.message || responseData?.title || error.message || 'An unexpected error occurred while deleting department.';
            return ApiResponseT.serverError(
                [{ message: serverMessage }],
                'Server error'
            );
        }
    }
};