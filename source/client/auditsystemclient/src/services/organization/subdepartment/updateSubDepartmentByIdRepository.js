import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUBDEPARTMENT_BASE_ENDPOINT = 'api/SubDepartment';
const SUBDEPARTMENT_UPDATE_ENDPOINT = '/update-sub-department';

export const updateSubDepartmentByIdRepository = {

  async updateSubDepartmentById(subDepartmentData) {
    try {
      const token = authService.getLocalToken();
      
      const payload = {
        id: subDepartmentData.id,
        name: subDepartmentData.name,
        departmentId: typeof subDepartmentData.departmentId === 'object' 
          ? subDepartmentData.departmentId.value || subDepartmentData.departmentId.id || subDepartmentData.departmentId
          : subDepartmentData.departmentId
      };
            
      const response = await api.put(
        `${SUBDEPARTMENT_BASE_ENDPOINT}${SUBDEPARTMENT_UPDATE_ENDPOINT}`,
        payload,
        {
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
        }
      );


      if (response.status === 200 || response?.data?.success !== false) {
        return ApiResponseT.ok(null, response.data?.message || 'SubDepartment updated successfully');
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
        console.error(' Not Found - subdepartment may not exist');
        return ApiResponseT.notFound(
          [{ message: 'SubDepartment not found' }],
          'SubDepartment not found'
        );
      }
      
      console.error(' Server Error or Network Issue');
      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred while updating subdepartment.' }],
        'Server error'
      );
    }
  }
};