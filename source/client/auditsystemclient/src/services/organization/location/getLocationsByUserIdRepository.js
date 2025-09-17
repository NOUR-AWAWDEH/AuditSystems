import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const LOCATION_BASE_ENDPOINT = '/api/Location';
const GET_LOCATIONS_BY_USER_ID_ENDPOINT = '/get-locations-by-user-id';

export const getLocationsByUserIdRepository = {
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

      if (response.status === 200) {
        const locations = response.data?.result?.locations || [];

        return ApiResponseT.ok(
          locations,
          response.data?.successMessage || 'Locations fetched successfully.'
        );
      }


      // Handle non-200 responses
      return ApiResponseT.badRequest(
        response?.errors || [{ message: response?.data?.successMessage || 'Failed to fetch locations.' }],
        response?.data?.successMessage || 'Failed to fetch locations.'
      );

    } catch (error) {
      if (error.response?.status === 400) {
        return ApiResponseT.badRequest(
          error.response?.data?.errors || [{ message: error.response?.data?.successMessage || 'Bad request' }],
          error.response?.data?.successMessage || 'Bad request'
        );
      }
      
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