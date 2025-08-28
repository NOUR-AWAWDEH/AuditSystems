import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";
import api from "@/services/api/api";

const SUBLOCATION_BASE_ENDPOINT = "api/SubLocation";
const SUBLOCATION_SEARCH_BY_USER_ID_ENDPOINT = "/search-sub-location-by-user-id";

export const searchSubLocationsByUserIdRepository = {
  async searchSubLocationsByUserId(searchParams = {}) {
    try {
      const {
        searchTerm = '',
        locationId = null,
        page = 1,
        pageSize = 10
      } = searchParams;
      
      const token = authService.getLocalToken();
      const params = {
        searchTerm,
        page,
        pageSize,
        sortBy: 'name'
      };
      
      // Add locationId to params if provided
      if (locationId) {
        params.locationId = locationId;
      }
      
      const response = await api.get(
        `${SUBLOCATION_BASE_ENDPOINT}${SUBLOCATION_SEARCH_BY_USER_ID_ENDPOINT}`,
        {
          params,
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );

      const { data, status } = response;
      
      if (status === 200 && data?.success) {
        const result = data.result;

        // Defensive check in case the result structure changes
        if (result?.items && typeof result.totalCount === 'number') {
          return ApiResponseT.ok(
            result, // includes items and totalCount
            data.successMessage || 'Sublocations fetched successfully.'
          );
        }

        return ApiResponseT.badRequest(
          [{ message: 'Missing or invalid result structure.' }],
          'Invalid response format'
        );
      }

      return ApiResponseT.badRequest(
        [{ message: 'Unexpected response format.' }],
        'Failed to search sublocations'
      );

    } catch(error) { 
      console.error('Search sublocations error:', error);

      const responseData = error.response?.data;

      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: 'Authentication required. Please log in again.' }],
          'Unauthorized'
        );
      }

      if (error.response?.status === 400) {
        const errors = responseData?.errors || [{ message: responseData?.message || 'Bad request' }];
        return ApiResponseT.badRequest(errors, responseData?.message || 'Bad request');
      }

      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred.' }],
        'Server error'
      );
    }
  }
};