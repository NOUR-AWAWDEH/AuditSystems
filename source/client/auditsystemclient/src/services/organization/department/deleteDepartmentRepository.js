import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authService";

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

            if (response.status === 204 || response?.data?.success !== false) {
                return ApiResponseT.ok(null, 'Department deleted successfully');
            }

            const errors = Array.isArray(response?.data?.errors)
                ? response.data.errors
                : [{ message: response?.data?.message || 'Delete failed' }];
            return ApiResponseT.badRequest(errors, response?.data?.message);

        } catch (error) {
            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = error.response.data?.errors ||
                    [{ message: error.response.data?.message || 'Bad request' }];
                return ApiResponseT.badRequest(errors, error.response.data?.message || 'Bad request');
            }

            if (error.response?.status === 404) {
                return ApiResponseT.notFound(
                    [{ message: 'Department not found' }], 'Department not found'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while deleting department.' }],
                'Server error'
            );
        }
    }
};