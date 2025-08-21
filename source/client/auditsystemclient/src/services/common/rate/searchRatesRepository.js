import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const RATE_BASE_ENDPOINT = '/api/Rate';
const SEARCH_RATES_ENDPOINT = '/search-rates';

export const searchRatesRepository = {
  async searchRates(searchParams = {}) {
    try {
      const token = authService.getLocalToken();

      if (!token) {
        throw new Error('Authentication token not found');
      }

      const queryParams = new URLSearchParams();

      if (searchParams.searchTerm !== undefined) {
        queryParams.append('searchTerm', searchParams.searchTerm);
      }

      if (searchParams.pageNumber !== undefined) {
        queryParams.append('pageNumber', searchParams.pageNumber);
      }

      if (searchParams.pageSize !== undefined) {
        queryParams.append('pageSize', searchParams.pageSize);
      }

      const queryString = queryParams.toString();
      const url = `${RATE_BASE_ENDPOINT}${SEARCH_RATES_ENDPOINT}${queryString ? `?${queryString}` : ''}`;

      const response = await api.get(url, {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      });

      console.log("searchRatesRepository: " , response);
      const { data, status } = response;

      if (status === 200 && data?.success) {
        return ApiResponseT.ok(
          data.result,
          data.successMessage || 'Rates retrieved successfully.'
        );
      }

      return ApiResponseT.badRequest(
        data?.errors || [{ message: 'Failed to retrieve rates.' }],
        data?.message || 'Failed to retrieve rates.'
      );
    } catch (error) {
      console.error('Error searching rates:', error);

      const responseData = error.response?.data;

      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: 'Authentication required. Please log in again.' }],
          'Authentication required'
        );
      }

      if (error.response?.status === 400) {
        const errors = responseData?.errors || [
          { message: responseData?.message || 'Bad request' },
        ];
        return ApiResponseT.badRequest(
          errors,
          responseData?.message || 'Bad request'
        );
      }

      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred.' }],
        'Server error'
      );
    }
  },
};
