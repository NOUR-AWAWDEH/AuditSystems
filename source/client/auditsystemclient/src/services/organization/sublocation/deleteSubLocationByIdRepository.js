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

      if (response.status === 204 || response?.data?.success !== false) {
        return ApiResponseT.ok(null, 'SubLocation deleted successfully');
      }

      const errors = Array.isArray(response?.data?.errors)
        ? response.data.errors
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
        const errors = error.response.data?.errors || [{ message: error.response.data?.message || 'Bad request' }];
        return ApiResponseT.badRequest(errors, error.response.data?.message || 'Bad request');
      }
      
      if (error.response?.status === 404) {
        return ApiResponseT.notFound(
          [{ message: 'SubLocation not found' }],
          'SubLocation not found'
        );
      }
      
      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred while deleting sublocation.' }],
        'Server error'
      );
    }
  }
};