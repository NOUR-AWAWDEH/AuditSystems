import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUBLOCATION_BASE_ENDPOINT = 'api/SubLocation';
const SUBLOCATION_GET_BY_USER_ID_ENDPOINT = '/get-sub-locations-by-user-id';

export const getSubLocationsByUserIdRepository = {
  async getSubLocationsByUserId() {
    try {
      const token = authService.getLocalToken();
      
      const response = await api.get(
        `${SUBLOCATION_BASE_ENDPOINT}${SUBLOCATION_GET_BY_USER_ID_ENDPOINT}`,
        {
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
        }
      );

      if (response.status === 200) {
        const subLocations = response.data?.result?.subLocations || [];
        return ApiResponseT.ok(subLocations, response?.data?.successMessage || 'SubLocations retrieved successfully');
      }

      return ApiResponseT.badRequest(
        response?.errors || [{ message: response?.data?.successMessage || 'Failed to retrieve sublocations' }],
        response?.data?.successMessage || 'Failed to retrieve sublocations'
      );

    } catch (error) {
      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: 'Authentication required. Please log in again.' }],
          'Authentication required'
        );
      }
      
      if (error.response?.status === 400) {
        const errors = error.response?.errors || 
          [{ message: error.response?.data?.successMessage || 'Bad request' }];
        return ApiResponseT.badRequest(errors, error.response?.data?.successMessage || 'Bad request');
      }
      
      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred while retrieving sublocations.' }],
        'Server error'
      );
    }
  }
};