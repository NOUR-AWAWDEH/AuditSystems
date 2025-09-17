import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUBLOCATION_BASE_ENDPOINT = 'api/SubLocation';
const SUBLOCATION_CREATE_ENDPOINT = `/create-sub-location`;

export const createSubLocationRepository = {
  async createSubLocation(subLocationData) {
    try {
      const token = authService.getLocalToken();

      const payload = {
        name: subLocationData.name,
        address: subLocationData.address || '',
        locationId: typeof subLocationData.locationId === 'object' 
          ? subLocationData.locationId.value || subLocationData.locationId.id || subLocationData.locationId
          : subLocationData.locationId
      };
      
      const response = await api.post(
        `${SUBLOCATION_BASE_ENDPOINT}${SUBLOCATION_CREATE_ENDPOINT}`,
        payload,
        {
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
        }
      );


      if (response?.success !== false || response.status === 201) {
        return ApiResponseT.ok(response?.data?.result, response?.data?.successMessage || 'SubLocation created successfully.');
      }

      const errors = Array.isArray(response?.errors)
          ? response.errors
          : [{ message: response?.data?.successMessage || 'Creation failed' }];
      return ApiResponseT.badRequest(errors, response?.data?.successMessage);

    } catch (error) {
      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: 'Authentication required. Please log in again.' }],
          'Authentication required'
        );
      }
      
      if (error.response?.status === 400) {
        const errors = error.response?.errors || 
            [{ message: error.response?.message || 'Bad request' }];
        return ApiResponseT.badRequest(errors, error.response?.data?.successMessage || 'Bad request');
      }
      
      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred while creating sublocation.' }],
        'Server error'
      );
    }
  }
};