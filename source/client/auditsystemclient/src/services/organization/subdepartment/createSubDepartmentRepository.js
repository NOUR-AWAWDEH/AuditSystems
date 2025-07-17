import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';


const SUBDEPARTMENT_BASE_ENDPOINT = '/api/SubDepartment';
const CREATE_SUBDEPARTMENT_ENDPOINT = '/create-sub-department';


export const createSubDepartmentRepository = {
  async createSubDepartment(subDepartmentData) {
    try {
      const token = authService.getLocalToken();

      // Ensure all required fields are present
      const payload = {
        name: subDepartmentData.name,
        departmentId: typeof subDepartmentData.departmentId === 'object' 
          ? subDepartmentData.departmentId.value || subDepartmentData.departmentId.id || subDepartmentData.departmentId
          : subDepartmentData.departmentId
      };
      
      const response = await api.post(
        `${SUBDEPARTMENT_BASE_ENDPOINT}${CREATE_SUBDEPARTMENT_ENDPOINT}`,
        payload,
        {
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
        }
      );

      if (response.data && response.status === 201) {
        if (response.data.success && response.data.result) {
          return ApiResponseT.ok(response.data.result, response.data.message || 'SubDepartment created successfully.');
        } else if (response.data.success === false) {
          return ApiResponseT.badRequest(
            response.data.errors || [{ message: response.data.message || 'Failed to create subdepartment.' }], 
            response.data.message || 'Failed to create subdepartment.'
          );
        }
      }

      return ApiResponseT.badRequest(
        [{ message: 'Unexpected response format' }],
        'Failed to create subdepartment'
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
        [{ message: error.message || 'An unexpected error occurred while creating subdepartment.' }],
        'Server error'
      );
    }
  }
};