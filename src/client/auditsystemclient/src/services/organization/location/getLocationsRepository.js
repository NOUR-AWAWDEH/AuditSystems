import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const LOCATION_BASE_ENDPOINT = 'api/Location';

export const getLocationsRepository = {
  async getLocationsByUserId(userId) {  // ← Receives userId parameter
    try {
      const token = authService.getLocalToken();
      
      // ← Uses correct API endpoint (fixed from previous issue)
      const response = await api.get(`${LOCATION_BASE_ENDPOINT}/get-locations-by-user/${userId}`, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
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
      console.error('Get locations error:', error);
      
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