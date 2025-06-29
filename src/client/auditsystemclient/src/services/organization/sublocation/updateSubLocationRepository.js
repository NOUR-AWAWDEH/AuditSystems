import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUBLOCATION_BASE_ENDPOINT = 'api/SubLocation';

export const updateSubLocationRepository = {
  async updateSubLocation(subLocationData) {
    try {
      const token = authService.getLocalToken();
      
      const payload = {
        id: subLocationData.id,
        name: subLocationData.name,
        address: subLocationData.address || '',
        locationId: typeof subLocationData.locationId === 'object' 
          ? subLocationData.locationId.value || subLocationData.locationId.id || subLocationData.locationId
          : subLocationData.locationId
      };
      
      const response = await api.put(`${SUBLOCATION_BASE_ENDPOINT}/update-sub-location`, payload, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (response.status === 200 || response?.data?.success !== false) {
        return ApiResponseT.ok(null, response.data?.message || 'SubLocation updated successfully');
      }

      const errors = Array.isArray(response?.data?.errors)
        ? response.data.errors
        : [{ message: response?.data?.message || 'Update failed' }];
      return ApiResponseT.badRequest(errors, response?.data?.message);

    } catch (error) {
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
      
      if (error.response?.status === 404) {
        return ApiResponseT.notFound(
          [{ message: 'SubLocation not found' }],
          'SubLocation not found'
        );
      }
      
      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred while updating sublocation.' }],
        'Server error'
      );
    }
  }
};