import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUBLOCATION_BASE_ENDPOINT = 'api/SubLocation';
const SUBLOCATION_GET_BY_ID_ENDPOINT = '/get-sub-location-by-id';

export const getSubLocationByIdRepository = {
  async getSubLocationById(subLocationId) {
    try {
      const token = authService.getLocalToken();
      
      const response = await api.get(
        `${SUBLOCATION_BASE_ENDPOINT}${SUBLOCATION_GET_BY_ID_ENDPOINT}`,
        {
          params: { id: subLocationId },
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
        }
      );

      if (response.data?.success && response.data?.result) {
        return ApiResponseT.ok(response.data.result, response.data.message || 'SubLocation retrieved successfully');
      }

      return ApiResponseT.badRequest(
        response.data?.errors || [{ message: 'Failed to retrieve sublocation' }],
        response.data?.message || 'Failed to retrieve sublocation'
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
          [{ message: 'SubLocation not found' }],
          'SubLocation not found'
        );
      }
      
      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred while retrieving sublocation.' }],
        'Server error'
      );
    }
  }
};