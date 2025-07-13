import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const LOCATION_BASE_ENDPOINT = 'api/Location';
const GET_LOCATIONS_BY_USER_ID_ENDPOINT = '/get-locations-by-user-id';

export const getLocationsByUserIdRepository = {
  // Get locations by user ID (UserAware - no userId parameter needed)
  async getLocationsByUserId() { 
    try {
      const token = authService.getLocalToken();
      
      const response = await api.get(
        `${LOCATION_BASE_ENDPOINT}${GET_LOCATIONS_BY_USER_ID_ENDPOINT}`, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
          'Cache-Control': 'no-cache, no-store, must-revalidate',
          'Pragma': 'no-cache',
          'Expires': '0'
        }
      });

      if (response.data && response.status === 200) {
        if (response.data.success && response.data.result) {
          return ApiResponseT.ok(response.data.result, response.data.message || 'Locations fetched successfully.');
        } else if (response.data.success === false) {
          return ApiResponseT.badRequest(
            response.data.errors || [{ message: response.data.message || 'Failed to fetch locations.' }], 
            response.data.message || 'Failed to fetch locations.'
          );
        }
      }

      return ApiResponseT.badRequest(
        [{ message: 'Unexpected response format' }],
        'Failed to fetch locations'
      );

    } catch (error) {
      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: 'Authentication required. Please log in again.' }],
          'Authentication required'
        );
      }
      
      if (error.response?.status === 404) {
        return ApiResponseT.ok([], 'No locations found');
      }
      
      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred while fetching locations.' }],
        'Server error'
      );
    }
  }
};