import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const LOCATION_BASE_ENDPOINT = '/api/Location';
const GET_LOCATION_BY_ID_ENDPOINT = '/get-location-by-id';

export const getLocationByIdRepository = {

  async getLocationById(locationId) {
    try {
      const token = authService.getLocalToken();
      
      const response = await api.get(
        `${LOCATION_BASE_ENDPOINT}${GET_LOCATION_BY_ID_ENDPOINT}`, {
        params: { id: locationId },
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });
      
      // Fix: Access response.data and response.status correctly
      const { data, status } = response;
      
      if (status === 200 && data?.success) {
          return ApiResponseT.ok(
              data.result, 
              data.successMessage || 'Location fetched successfully.'
          );
      }
      
      return ApiResponseT.badRequest(
          data?.errors || [{ message: 'Failed to fetch location.' }],
          data?.message || 'Failed to fetch location'
      );
      
    } catch (error) {
        console.error('Error fetching location:', error);
        
        const responseData = error.response?.data;
        
        if (error.response?.status === 401) {
            return ApiResponseT.unauthorized(
                [{ message: 'Authentication required. Please log in again.' }],
                'Authentication required'
            );
        }
        
        if (error.response?.status === 404) {
            return ApiResponseT.notFound(
                [{ message: 'Location not found' }],
                'Location not found'
            );
        }
        
        if (error.response?.status === 400) {
            const errors = responseData?.errors || 
                [{ message: responseData?.message || 'Bad request' }];
            return ApiResponseT.badRequest(errors, responseData?.message || 'Bad request');
        }
        
        return ApiResponseT.serverError(
            [{ message: error.message || 'An unexpected error occurred while fetching location.' }],
            'Server error'
        );
    }
  }
};