import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const LOCATION_BASE_ENDPOINT = 'api/Location';
const CREATE_LOCATION_ENDPOINT = '/create-location';

export const createLocationRepository = {
  async createLocation(locationData) {
    try {
      const token = authService.getLocalToken();
      
      // Note: With UserAware pattern, userId is automatically injected
      const payload = {
        name: locationData.name,
        address: locationData.address || null,
        companyId: locationData.companyId
      };
      
      const response = await api.post(`${LOCATION_BASE_ENDPOINT}${CREATE_LOCATION_ENDPOINT}`, payload, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json' 
        }
      });

      if (response.data && response.status === 201) {
        if (response.data.success && response.data.result) {
          return ApiResponseT.ok(response.data.result, response.data.message || 'Location created successfully.');
        } else if (response.data.success === false) {
          return ApiResponseT.badRequest(
            response.data.errors || [{ message: response.data.message || 'Failed to create location.' }], 
            response.data.message || 'Failed to create location.'
          );
        }
      }

      return ApiResponseT.badRequest(
        [{ message: 'Unexpected response format' }],
        'Failed to create location'
      );

    } catch (error) {
      console.error('Create location error:', error);
      
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
        [{ message: error.message || 'An unexpected error occurred while creating location.' }],
        'Server error'
      );
    }
  }
};