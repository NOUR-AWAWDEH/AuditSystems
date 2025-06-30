import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUBLOCATION_BASE_ENDPOINT = 'api/SubLocation';

export const getSubLocationsByUserIdRepository = {
  async getSubLocationsByUserId() {
    try {
      const token = authService.getLocalToken();
      
      const response = await api.get(`${SUBLOCATION_BASE_ENDPOINT}/get-sub-locations-by-user-id`, {

        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (response.data?.success && response.data?.result) {
        return ApiResponseT.ok(response.data.result, response.data.message || 'SubLocations retrieved successfully');
      }

      return ApiResponseT.badRequest(
        response.data?.errors || [{ message: 'Failed to retrieve sublocations' }],
        response.data?.message || 'Failed to retrieve sublocations'
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
        [{ message: error.message || 'An unexpected error occurred while retrieving sublocations.' }],
        'Server error'
      );
    }
  }
};