import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUBLOCATION_BASE_ENDPOINT = 'api/SubLocation';
const SUBLOCATION_DELETE_ENDPOINT = `/delete-sub-location-by-id`;

export const deleteSubLocationByIdRepository = {
  async deleteSubLocationById(subLocationId) {
    try {
      const token = authService.getLocalToken();
      
      const response = await api.delete(
        `${SUBLOCATION_BASE_ENDPOINT}${SUBLOCATION_DELETE_ENDPOINT}`,
        {
          data: { id: subLocationId },
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
        }
      );

      if (response.status === 204 || response?.success !== false) {
        return ApiResponseT.ok(null, 'SubLocation deleted successfully');
      }

      const errors = Array.isArray(response?.errors)
        ? response?.errors
        : [{ message: response?.data?.successMessage || 'Delete failed' }];
      return ApiResponseT.badRequest(errors, response?.data?.successMessage);

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
          [{ message: 'SubLocation not found' }],
          'SubLocation not found'
        );
      }
      
      // Handle other server errors
      const responseData = error.response?.data;
      const serverMessage = responseData?.message || responseData?.title || error.message || 'An unexpected error occurred while deleting sublocation.';
      return ApiResponseT.serverError(
        [{ message: serverMessage }],
        'Server error'
      );
    }
  }
};