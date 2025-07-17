import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUBDEPARTMENT_BASE_ENDPOINT = 'api/SubDepartment';
const SUBDEPARTMENT_GET_BY_ID_ENDPOINT = '/get-sub-department-by-id';

export const getSubDepartmentByIdRepository = {
  async getSubDepartmentById(subDepartmentId) {
    try {
      const token = authService.getLocalToken();
      
      const response = await api.get(
        `${SUBDEPARTMENT_BASE_ENDPOINT}${SUBDEPARTMENT_GET_BY_ID_ENDPOINT}`,
        {
          params: { id: subDepartmentId },
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
        }
      );

      if (response.data?.success && response.data?.result) {
        return ApiResponseT.ok(response.data.result, response.data.message || 'SubDepartment retrieved successfully');
      }

      return ApiResponseT.badRequest(
        response.data?.errors || [{ message: 'Failed to retrieve subdepartment' }],
        response.data?.message || 'Failed to retrieve subdepartment'
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
      
      if (error.response?.status === 404) {
        return ApiResponseT.notFound(
          [{ message: 'SubDepartment not found' }],
          'SubDepartment not found'
        );
      }
      
      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred while retrieving subdepartment.' }],
        'Server error'
      );
    }
  }
};
