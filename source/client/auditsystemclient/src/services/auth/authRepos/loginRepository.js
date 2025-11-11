// src/services/auth/loginRepository.js
import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';

const AUTH_ENDPOINT = 'api/auth/login';
const AUTHORIZATION_HEADER = 'Authorization';
const BEARER_PREFIX = 'Bearer ';

export const loginRepository = {
  async loginRequest(username, password) {
    try {
      const response = await api.post(AUTH_ENDPOINT, { username, password });
      const responseData = response.data;

      // Handle successful response from backend
      if (responseData?.success) {
        const result = {
          jwtToken: responseData.result.jwtToken,
          refreshToken: responseData.result.refreshToken,
          isEmailVerified: responseData.result.isEmailVerified,
        };
        
        this.setAuthHeader(result.jwtToken);
        
        // Return using ApiResponseT to maintain consistency
        return ApiResponseT.ok(result, responseData.successMessage);
      }

      // Handle backend response where success is false
      console.warn('❌ [LOGIN REPOSITORY] Login failed - response.success is false', {
        success: responseData?.success,
        message: responseData?.successMessage,
        errors: responseData?.errors,
        statusCode: responseData?.statusCode,
        fullResponse: responseData
      });

      // Map backend error response to ApiResponseT
      return ApiResponseT.badRequest(
        responseData?.errors || [{ message: responseData?.successMessage || 'Login failed' }]
      );

    } catch (error) {
      console.error('💥 [LOGIN REPOSITORY] Login error caught:', {
        error,
        errorMessage: error?.message,
        errorResponse: error?.response,
        errorResponseData: error?.response?.data,
        stack: error?.stack
      });
      
      // Extract error information from backend response
      if (error?.response?.data) {
        const errorData = error.response.data;
        
        // Use the backend's error structure directly
        return new ApiResponseT(
          errorData.success || false,
          errorData.statusCode || error.response.status || 400,
          errorData.result || null,
          errorData.successMessage || 'Login failed',
          errorData.errors || [{ message: errorData.successMessage || 'Login failed' }]
        );
      }
      
      // Handle network or other client-side errors
      if (error?.message) {
        return ApiResponseT.badRequest([{ message: error.message }]);
      }
      
      // Fallback for unknown errors
      return ApiResponseT.internalServerError([{ message: 'An unexpected error occurred' }]);
    }
  },

  setAuthHeader(token) {
    if (token) {
      api.defaults.headers.common[AUTHORIZATION_HEADER] = `${BEARER_PREFIX}${token}`;
    } else {
      delete api.defaults.headers.common[AUTHORIZATION_HEADER];
    }
  },

  removeAuthHeader() {
    delete api.defaults.headers.common[AUTHORIZATION_HEADER];
  }
};