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


      if (!responseData?.success) {
        console.warn('❌ [LOGIN REPOSITORY] Login failed - response.success is false', {
          success: responseData?.success,
          message: responseData?.successMessage,
          errors: responseData?.errors,
          fullResponse: responseData
        });
        return ApiResponseT.badRequest(
            responseData?.errors || [{ message: responseData?.successMessage || 'Login failed' }],
            responseData?.successMessage || 'Login failed'
        );
      }

      const result = {
        jwtToken: responseData.result.jwtToken,
        refreshToken: responseData.result.refreshToken,
        isEmailVerified: responseData.result.isEmailVerified,
      };
      
      this.setAuthHeader(result.jwtToken);
      // Return only the actual properties from the server
      return ApiResponseT.ok(result, responseData?.successMessage || 'Login successful');

    } catch (error) {
      console.error('💥 [LOGIN REPOSITORY] Login error caught:', {
        error,
        errorMessage: error?.message,
        errorResponse: error?.response,
        errorResponseData: error?.response?.data,
        stack: error?.stack
      });
      
      let errorMessage = 'Login failed';
      let errors = [{ message: errorMessage }];

      if (error?.response?.data) {
        // Handle server error response - FIX: Access error.response.data
        const errorData = error.response.data;

        errorMessage = errorData.successMessage || 'Login failed';
        errors = Array.isArray(errorData.errors)
          ? errorData.errors
          : [{ message: errorMessage }];
      } else if (error?.message) {
        // Handle network or other errors
        errorMessage = error.message;
        errors = [{ message: errorMessage }];
      }

      return ApiResponseT.badRequest(errors, errorMessage);
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
