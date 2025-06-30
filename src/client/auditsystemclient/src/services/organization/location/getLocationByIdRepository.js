import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const LOCATION_BASE_ENDPOINT = 'api/Location';

export const getLocationByIdRepository = {

  async getLocationById(locationId) {
    try {
      const token = authService.getLocalToken();
      
      const response = await api.get(`${LOCATION_BASE_ENDPOINT}/get-location-by-id`, {
        params: { id: locationId },
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (response.data && response.status === 200) {
        if (response.data.success && response.data.result) {
          return ApiResponseT.ok(response.data.result, response.data.message || 'Location fetched successfully.');
        } else if (response.data.success === false) {
          return ApiResponseT.badRequest(
            response.data.errors || [{ message: response.data.message || 'Failed to fetch location.' }], 
            response.data.message || 'Failed to fetch location.'
          );
        }
      }

      return ApiResponseT.badRequest(
        [{ message: 'Unexpected response format' }],
        'Failed to fetch location'
      );

    } catch (error) {
      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: 'Authentication required. Please log in again.' }],
          'Authentication required'
        );
      }
      
      if (error.response?.status === 404) {
        return ApiResponseT.notFound(
          [{ message: 'Location not found' }],
          'Location not found'
        );
      }
      
      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred while fetching location.' }],
        'Server error'
      );
    }
  }
};