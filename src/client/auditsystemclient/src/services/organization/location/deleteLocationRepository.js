import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const LOCATION_BASE_ENDPOINT = 'api/location';

export const deleteLocationRepository = {
  async deleteLocation(locationId) {
    try {
      const token = authService.getLocalToken();
      
      const response = await api.delete(`${LOCATION_BASE_ENDPOINT}/delete-location`, {
        data: { id: locationId },
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (response.status === 204 || response?.data?.success !== false) {
        return ApiResponseT.ok(null, 'Location deleted successfully');
      }

      const errors = Array.isArray(response?.data?.errors)
        ? response.data.errors
        : [{ message: response?.data?.message || 'Delete failed' }];
      return ApiResponseT.badRequest(errors, response?.data?.message);

    } catch (error) {
      let errorMessage = 'Delete failed';
      let errors = [{ message: errorMessage }];

      if (error?.response?.data) {
        errorMessage = error.response.data.message || errorMessage;
        errors = Array.isArray(error.response.data.errors)
          ? error.response.data.errors
          : [{ message: errorMessage }];
      } else if (error?.message) {
        errorMessage = error.message;
        errors = [{ message: errorMessage }];
      }

      return ApiResponseT.badRequest(errors, errorMessage);
    }
  }
};