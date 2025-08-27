import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const DEPARTMENT_BSE_ENDPOINT = '/api/department';
const CREATE_DEPARTMENT_ENDPOINT = '/create-department';

export const createDepartmentRepository = {
    async createDepartment(departmentData) {
        try {
            const token = authService.getLocalToken();

            const payload = {
                name: departmentData.name,
                subLocationId: typeof departmentData.subLocationId === 'object'
                    ? departmentData.subLocationId.value || departmentData.subLocationId.id
                    : departmentData.subLocationId
            };

            const response = await api.post(
                `${DEPARTMENT_BSE_ENDPOINT}${CREATE_DEPARTMENT_ENDPOINT}`,
                payload, {

                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.status === 201 || response?.success !== false) {
                return ApiResponseT.ok(response.data.result, response.successMessage || 'Department created successfully');
            }

            const errors = Array.isArray(response?.errors)
                ? response.errors
                : [{ message: response?.data?.successMessage || 'Creation failed' }];
            return ApiResponseT.badRequest(errors, response?.data?.successMessage);
        } catch (error) {
            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = error.response?.errors ||
                    [{ message: error.response?.data?.successMessage || 'Bad request' }];
                return ApiResponseT.badRequest(errors, error.response?.data?.successMessage || 'Bad request');
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while creating department.' }],
                'Server error'
            );
        }
    }
};