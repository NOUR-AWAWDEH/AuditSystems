import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUBLOCATION_BASE_ENDPOINT = 'api/SubLocation';
const SUBLOCATION_UPDATE_ENDPOINT = '/update-sub-location';

export const updateSubLocationByIdRepository = {

  async updateSubLocationById(subLocationData) {
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
            
      const response = await api.put(
        `${SUBLOCATION_BASE_ENDPOINT}${SUBLOCATION_UPDATE_ENDPOINT}`,
        payload,
        {
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
        }
      );


      if (response.status === 200 || response?.success !== false) {
        return ApiResponseT.ok(null, response?.data?.successMessage || 'SubLocation updated successfully');
      }

      console.error('API call failed - bad response');
      const errors = Array.isArray(response?.errors)
        ? response.errors
        : [{ message: response?.data?.successMessage || 'Update failed' }];
      return ApiResponseT.badRequest(errors, response?.data?.successMessage);

    } catch (error) {
      
      if (error.response?.status === 401) {
        console.error('Unauthorized - token may be invalid');
        return ApiResponseT.unauthorized(
          [{ message: 'Authentication required. Please log in again.' }],
          'Authentication required'
        );
      }
      
      if (error.response?.status === 400) {
        console.error(' Bad Request:', error.response);
        const errors = error.response?.errors || [{ message: error.response?.data?.successMessage || 'Bad request' }];
        return ApiResponseT.badRequest(errors, error.response?.data?.successMessage || 'Bad request');
      }
      
      if (error.response?.status === 404) {
        console.error(' Not Found - sublocation may not exist');
        return ApiResponseT.notFound(
          [{ message: 'SubLocation not found' }],
          'SubLocation not found'
        );
      }
      
      console.error(' Server Error or Network Issue');
      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred while updating sublocation.' }],
        'Server error'
      );
    }
  }
};