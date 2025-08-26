import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const LOCATION_BASE_ENDPOINT = '/api/Location';
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

      const response = await api.post(`${LOCATION_BASE_ENDPOINT}${CREATE_LOCATION_ENDPOINT}`,
        payload, {
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
      });

      const {data, status} = response;


      if (status === 201 && data?.success) {
        return ApiResponseT.ok(data.result, data.successMessage || 'Location created successfully');
      }


      return ApiResponseT.badRequest(
          data?.errors || [{ message: data?.successMessage || 'Failed to create location' }],
          data?.successMessage || 'Failed to create location.');

    } catch (error) {
      console.error('Create location error:', error);
      const responseData = error.response?.data;

      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: 'Authentication required. Please log in again.' }],
          'Authentication required'
        );
      }

       if (error.response?.status === 400) {
           const errors = responseData?.errors || [{ message: responseData?.successMessage || 'Bad request' }];
           return ApiResponseT.badRequest(errors, responseData?.successMessage || 'Bad request');
      }

      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred while creating location.' }],
        'Server error'
      );
    }
  }
};
