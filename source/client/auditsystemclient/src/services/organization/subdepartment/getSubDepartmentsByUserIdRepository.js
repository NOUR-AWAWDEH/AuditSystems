import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUBDEPARTMENT_BASE_ENDPOINT = '/api/SubDepartment';
const SUBDEPARTMENT_GET_BY_USER_ID_ENDPOINT = '/get-sub-department-by-user-id';

export const getSubDepartmentsByUserIdRepository = {
  async getSubDepartmentsByUserId() {
    try {
      const token = authService.getLocalToken();
      
      const response = await api.get(
        `${SUBDEPARTMENT_BASE_ENDPOINT}${SUBDEPARTMENT_GET_BY_USER_ID_ENDPOINT}`,
        {
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
        }
      );

      if (response?.success && response?.result) {
        return ApiResponseT.ok(response.result, response.successMessage || 'SubDepartments retrieved successfully');
      }

      return ApiResponseT.badRequest(
        response?.errors || [{ message: 'Failed to retrieve subdepartments' }],
        response?.message || 'Failed to retrieve subdepartments'
      );

    } catch (error) {
      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: 'Authentication required. Please log in again.' }],
          'Authentication required'
        );
      }
      
      if (error.response?.status === 400) {
        const errors = error.response?.errors || 
          [{ message: error.response?.message || 'Bad request' }];
        return ApiResponseT.badRequest(errors, error.response?.message || 'Bad request');
      }
      
      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred while retrieving subdepartments.' }],
        'Server error'
      );
    }
  }
};