import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const DEPARTMENT_BASE_ENDPOINT = '/api/department';
const UPDATE_DEPARTMENT_BY_ID_ENDPOINT = '/update-department'; // Fixed: removed '-by-id'

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
       if (response.status === 200 || response?.data?.success !== false) {
        return ApiResponseT.ok(null, response.data?.message || 'Department updated successfully');
      }

      console.error('API call failed - bad response');
      const errors = Array.isArray(response?.data?.errors)
        ? response.data.errors
        : [{ message: response?.data?.message || 'Update failed' }];
    
      return ApiResponseT.badRequest(errors, response?.data?.message);

    } catch (error) {
      
      if (error.response?.status === 401) {
        console.error('Unauthorized - token may be invalid');
        return ApiResponseT.unauthorized(
          [{ message: 'Authentication required. Please log in again.' }],
          'Authentication required'
        );
      }
      
      if (error.response?.status === 400) {
        console.error(' Bad Request:', error.response.data);
        const errors = error.response.data?.errors || [{ message: error.response.data?.message || 'Bad request' }];
        return ApiResponseT.badRequest(errors, error.response.data?.message || 'Bad request');
      }
      
      if (error.response?.status === 404) {
        console.error(' Not Found - department may not exist');
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