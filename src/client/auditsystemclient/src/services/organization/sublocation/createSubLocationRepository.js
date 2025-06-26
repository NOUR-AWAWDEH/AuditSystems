import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUBLOCATION_BASE_ENDPOINT = 'api/SubLocation';

export const createSubLocationRepository = {
  async createSubLocation(subLocationData) {
    try {
      const token = authService.getLocalToken();
      
      // Ensure all required fields are present
      const payload = {
        name: subLocationData.name,
        address: subLocationData.address || '',
        locationId: subLocationData.locationId
      };
      
      const response = await api.post(`${SUBLOCATION_BASE_ENDPOINT}/create-sub-location`, payload, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (response.data && response.status === 201) {
        if (response.data.success && response.data.result) {
          return ApiResponseT.ok(response.data.result, response.data.message || 'SubLocation created successfully.');
        } else if (response.data.success === false) {
          return ApiResponseT.badRequest(
            response.data.errors || [{ message: response.data.message || 'Failed to create sublocation.' }], 
            response.data.message || 'Failed to create sublocation.'
          );
        }
      }

      return ApiResponseT.badRequest(
        [{ message: 'Unexpected response format' }],
        'Failed to create sublocation'
      );

    } catch (error) {
      console.error('Create sublocation error:', error);
      
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
        [{ message: error.message || 'An unexpected error occurred while creating sublocation.' }],
        'Server error'
      );
    }
  }
};
