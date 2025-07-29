import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const LOCATION_BASE_ENDPOINT = '/api/Location';
const UPDATE_LOCATION_BY_ID_ENDPOINT = '/update-location';

export const updateLocationByIdRepository = {
  async updateLocationById(locationData) {
    try {
      const token = authService.getLocalToken();
      
      // Note: With UserAware pattern, userId is automatically injected
      const payload = {
        id: locationData.id,
        name: locationData.name,
        address: locationData.address || null,
        companyId: locationData.companyId
      };
      
      const response = await api.put(
        `${LOCATION_BASE_ENDPOINT}${UPDATE_LOCATION_BY_ID_ENDPOINT}`,
        payload, {
          
          headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (response.data && response.status === 200) {
        if (response.data.success) {
          return ApiResponseT.ok(
            null,
            response.data.successMessage || 'Location updated successfully.'
          );
        } else {
          return ApiResponseT.badRequest(
            response.data.errors || [{ message: response.data.successMessage || 'Failed to update location.' }],
            response.data.successMessage || 'Failed to update location.'
          );
        }
      }


      return ApiResponseT.badRequest(
        [{ message: 'Unexpected response format' }],
        'Failed to update location'
      );

    } catch (error) {
      console.error('Update location error:', error);
      
      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: 'Authentication required. Please log in again.' }],
          'Authentication required'
        );
      }
      
      if (error.response?.status === 400) {
        const errors = error.response?.errors || [{ message: error.response?.message || 'Bad request' }];
        return ApiResponseT.badRequest(errors, error.response?.message || 'Bad request');
      }
      
      if (error.response?.status === 404) {
        return ApiResponseT.notFound(
          [{ message: 'Location not found' }],
          'Location not found'
        );
      }
      
      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred while updating location.' }],
        'Server error'
      );
    }
  }
};