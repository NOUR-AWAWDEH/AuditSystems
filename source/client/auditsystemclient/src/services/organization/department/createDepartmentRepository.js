import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const DEPARTMENT_BSE_ENDPOINT = '/api/department';
const CREATE_DEPARTMENT_ENDPOINT = '/create-department';

export const createDepartmentRepository = {
    async createDepartment(departmentData) {
        try {
            const token = authService.getLocalAccessToken();

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

            if (response.data && response.status === 201) {
                if (response.data.success && response.data.result) {
                    return ApiResponseT.ok(response.data.result, response.data.message || 'Department created successfully.');
                } else if (response.data.success === false) {
                    return ApiResponseT.badRequest(
                        response.data.errors || [{ message: response.data.message || 'Failed to create department.' }],
                        response.data.message || 'Failed to create department.'
                    );
                }
            }

            return ApiResponseT.badRequest(
                [{ message: 'Unexpected response format' }],
                'Failed to create department.'
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
                return ApiResponseT.badRequest(errors, error.response.data?.message || 'Bad request');
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while creating department.' }], 'Server error'
            );
        }
    }
};