import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const DEPARTMENT_BASE_ENDPOINT = '/api/department';
const UPDATE_DEPARTMENT_BY_ID_ENDPOINT = '/update-department';

export const updateDepartmentByIdRepository = {
    async updateDepartmentById(departmentData) {
        try {
            const token = authService.getLocalToken();

            if (!token) {
                throw new Error('Authentication token not found');
            }

            const payload = {
                id: departmentData.id,
                name: departmentData.name,
                subLocationId: typeof departmentData.subLocationId === 'object' 
                    ? departmentData.subLocationId.value || departmentData.subLocationId.id || departmentData.subLocationId
                    : departmentData.subLocationId
            };

            const response = await api.put(
                `${DEPARTMENT_BASE_ENDPOINT}${UPDATE_DEPARTMENT_BY_ID_ENDPOINT}`,
                payload, 
                {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                }
            );

            if (response.status === 200 || response?.success !== false) {
              return ApiResponseT.ok(null, response?.data?.successMessage || 'Department updated successfully');
            }

          const errors = Array.isArray(response?.errors)
            ? response?.errors
            : [{ message: response?.data?.successMessage || 'Update failed' }];
        
          return ApiResponseT.badRequest(errors, response?.data?.successMessage);

    } catch (error) {
      
      if (error.response?.status === 401) {
        console.error('Unauthorized - token may be invalid');
        return ApiResponseT.unauthorized(
          [{ message: 'Authentication required. Please log in again.' }],
          'Authentication required'
        );
      }
      
      if (error.response?.status === 400) {
        const errors = error.response?.errors || [{ message: error.response?.data?.successMessage || 'Bad request' }];
        return ApiResponseT.badRequest(errors, error.response?.data?.successMessage || 'Bad request');
      }
      
      if (error.response?.status === 404) {
        return ApiResponseT.notFound(
          [{ message: 'Department not found' }],
          'Department not found'
        );
      }
      
      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred while updating department.' }],
        'Server error'
      );
    }
  }
};