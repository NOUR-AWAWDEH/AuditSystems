import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const LOCATION_BASE_ENDPOINT = '/api/Location';
const SEARCH_LOCATION_BY_USER_ID_ENDPOINT = '/search-location-by-user-id';

export const searchLocationsByUserIdRepository = {
  async searchLocationsByUserId(searchTerm = '', pageNumber = 1, pageSize = 10) {
    try {
      const token = authService.getLocalToken();

      const params = {
        searchTerm,
        page: pageNumber,
        pageSize,
        sortBy: 'name'
      };

      const response = await api.get(
        `${LOCATION_BASE_ENDPOINT}${SEARCH_LOCATION_BY_USER_ID_ENDPOINT}`,
        {
          params,
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
        }
      );

      const { data, status } = response;
      
      if (status === 200 && data?.success) {
        const result = data.result;

        // Defensive check in case the result structure changes
        if (result?.items && typeof result.totalCount === 'number') {
          return ApiResponseT.ok(
            result,
            data.successMessage || 'Locations fetched successfully.'
          );
        }

        return ApiResponseT.badRequest(
          [{ message: 'Missing or invalid result structure.' }],
          'Invalid response format'
        );
      }

      return ApiResponseT.badRequest(
        [{ message: 'Unexpected response format.' }],
        'Failed to search locations'
      );

    } catch (error) {
      console.error('Search locations error:', error);

      const responseData = error.response?.data;

      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: 'Authentication required. Please log in again.' }],
          'Unauthorized'
        );
      }

      if (error.response?.status === 400) {
        const errors = responseData?.errors || [{ message: responseData?.message || 'Bad request' }];
        return ApiResponseT.badRequest(errors, responseData?.message || 'Bad request');
      }

      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred.' }],
        'Server error'
      );
    }
  }
};