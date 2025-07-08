import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUBLOCATION_BASE_ENDPOINT = 'api/SubLocation';

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
            
      const response = await api.put(`${SUBLOCATION_BASE_ENDPOINT}/update-sub-location`, payload, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });


      if (response.status === 200 || response?.data?.success !== false) {
        return ApiResponseT.ok(null, response.data?.message || 'SubLocation updated successfully');
      }

      console.error('❌ API call failed - bad response');
      const errors = Array.isArray(response?.data?.errors)
        ? response.data.errors
        : [{ message: response?.data?.message || 'Update failed' }];
      return ApiResponseT.badRequest(errors, response?.data?.message);

    } catch (error) {
      console.error('❌ API Exception:', error);
      console.error('Exception details:', {
        message: error.message,
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        headers: error.response?.headers
      });
      
      if (error.response?.status === 401) {
        console.error('🚫 Unauthorized - token may be invalid');
        return ApiResponseT.unauthorized(
          [{ message: 'Authentication required. Please log in again.' }],
          'Authentication required'
        );
      }
      
      if (error.response?.status === 400) {
        console.error('❌ Bad Request:', error.response.data);
        const errors = error.response.data?.errors || [{ message: error.response.data?.message || 'Bad request' }];
        return ApiResponseT.badRequest(errors, error.response.data?.message || 'Bad request');
      }
      
      if (error.response?.status === 404) {
        console.error('🔍 Not Found - sublocation may not exist');
        return ApiResponseT.notFound(
          [{ message: 'SubLocation not found' }],
          'SubLocation not found'
        );
      }
      
      console.error('🔥 Server Error or Network Issue');
      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred while updating sublocation.' }],
        'Server error'
      );
    }
  }
};