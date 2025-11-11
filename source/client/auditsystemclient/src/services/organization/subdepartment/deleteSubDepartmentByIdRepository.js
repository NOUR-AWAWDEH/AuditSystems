import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUBDEPART_BASE_ENDPOINT = '/api/SubDepartment';
const SUBDEPARTMENT_DELETE_ENDPOINT = `/delete-sub-department-by-id`;

export const deleteSubDepartmentByIdRepository = {
  async deleteSubDepartmentById(subDepartmentId) {
    try {
      const token = authService.getLocalToken();
      
      const response = await api.delete(
        `${SUBDEPART_BASE_ENDPOINT}${SUBDEPARTMENT_DELETE_ENDPOINT}`,
        {
          data: { id: subDepartmentId },
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
        }
      );

      if (response.status === 204 || response?.data?.success !== false) {
        return ApiResponseT.ok(null, 'SubDepartment deleted successfully');
      }

      const errors = Array.isArray(response?.data?.errors)
        ? response?.data?.errors
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
        // Extract server error message properly
        const responseData = error.response?.data;
        const serverMessage = responseData?.message || responseData?.title || responseData?.detail || 'Bad request';
        const errors = responseData?.errors || [{ message: serverMessage }];
        return ApiResponseT.badRequest(errors, serverMessage);
      }
      
      if (error.response?.status === 404) {
        return ApiResponseT.notFound(
          [{ message: 'SubDepartment not found' }],
          'SubDepartment not found'
        );
      }
      
      // Handle other server errors
      const responseData = error.response?.data;
      const serverMessage = responseData?.message || responseData?.title || error.message || 'An unexpected error occurred while deleting subdepartment.';
      return ApiResponseT.serverError(
        [{ message: serverMessage }],
        'Server error'
      );
    }
  }
};