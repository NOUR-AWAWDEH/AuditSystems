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

      // Fix: Access response.data and response.status correctly
      const { data, status } = response;

      if (status === 201 && data?.success) {
          return ApiResponseT.ok(
              data.result, 
              data.successMessage || 'SubDepartment created successfully.'
          );
      }

      return ApiResponseT.badRequest(
          data?.errors || [{ message: data?.message || 'Failed to create subdepartment.' }],
          data?.message || 'Failed to create subdepartment.'
      );

    } catch (error) {
      console.error('Error creating subdepartment:', error);
      
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
          return ApiResponseT.badRequest(errors, responseData?.message || 'Bad request');
      }

      return ApiResponseT.serverError(
          [{ message: error.message || 'An unexpected error occurred while creating subdepartment.' }],
          'Server error'
      );
    }
  },
};