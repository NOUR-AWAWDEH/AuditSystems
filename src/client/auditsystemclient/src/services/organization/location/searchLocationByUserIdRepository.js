import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const LOCATION_BASE_ENDPOINT = 'api/Location';

export const searchLocationByUserIdRepository = {

  async searchLocationsByUserId(searchTerm = '', pageNumber = 1, pageSize = 10) {
    try {
      const token = authService.getLocalToken();
      
      const params = {
        searchTerm: searchTerm || '',
        pageNumber,
        pageSize
      };
      
      const response = await api.get(`${LOCATION_BASE_ENDPOINT}/search-location-by-user-id`, {
        params,
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (response.data && response.status === 200) {
        if (response.data.success && response.data.result) {
          return ApiResponseT.ok(response.data.result, response.data.message || 'Locations searched successfully.');
        } else if (response.data.success === false) {
          return ApiResponseT.badRequest(
            response.data.errors || [{ message: response.data.message || 'Failed to search locations.' }], 
            response.data.message || 'Failed to search locations.'
          );
        }
      }

      return ApiResponseT.badRequest(
        [{ message: 'Unexpected response format' }],
        'Failed to search locations'
      );

    } catch (error) {
      console.error('Search locations error:', error);
      
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
        [{ message: error.message || 'An unexpected error occurred while searching locations.' }],
        'Server error'
      );
    }
  }
};