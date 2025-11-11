// src/composables/useLogin.js
import { ref } from 'vue';
import { loginRepository } from '@/services/auth/authRepos/loginRepository';
import authService from '@/services/auth/authRepos/authService';

// Persistent refs
const isAuthenticated = ref(!!authService.getLocalToken());
const user = ref(null);
const error = ref(null);
const token = ref(authService.getLocalToken() || null);
const refreshToken = ref(authService.getLocalRefreshToken ? authService.getLocalRefreshToken() : null);

export function useLogin() {
  const setAuthData = (newToken, newRefreshToken, username) => {
    token.value = newToken;
    refreshToken.value = newRefreshToken;
    user.value = { username };
    isAuthenticated.value = true;

    authService.setLocalToken(token.value);
    authService.setLocalRefreshToken(refreshToken.value);
  };

  const extractBackendError = (response, statusCode, axiosError) => {
    const fallback = 'An unexpected error occurred';

    // Handle rate limiting (429 Too Many Requests) - HIGHEST PRIORITY
    if (statusCode === 429) {
      return 'Too many login attempts. Please try again later.';
    }

    // If response is an ApiResponseT object (from loginRepository)
    if (response && typeof response === 'object') {
      // Check for errors array in ApiResponseT
      if (response.errors && Array.isArray(response.errors) && response.errors.length > 0) {
        const firstError = response.errors[0];
        if (firstError && firstError.message) {
          return firstError.message;
        }
        if (firstError && typeof firstError === 'string') {
          return firstError;
        }
      }

      // Check for successMessage in ApiResponseT (this contains the error message when success is false)
      if (response.successMessage) {
        return response.successMessage;
      }

      // Check for message property directly on response
      if (response.message) {
        return response.message;
      }
    }

    // Handle Axios error response data structure
    if (axiosError?.response?.data) {
      const errorData = axiosError.response.data;
      
      // Check backend error structure
      if (errorData.errors && Array.isArray(errorData.errors) && errorData.errors.length > 0) {
        const firstError = errorData.errors[0];
        if (firstError && firstError.message) {
          return firstError.message;
        }
      }
      
      if (errorData.successMessage) {
        return errorData.successMessage;
      }
    }

    // Handle Axios error message directly (for network errors)
    if (axiosError && axiosError.message) {
      if (axiosError.message.includes('Network Error')) {
        return 'Network error. Please check your connection.';
      }
      if (axiosError.message.includes('timeout')) {
        return 'Request timeout. Please try again.';
      }
    }

    // Handle case where response might be a string
    if (typeof response === 'string') {
      return response;
    }

    return fallback;
  };

  const handleLoginSuccess = (response, username) => {
    // response.result now contains { jwtToken, refreshToken, isEmailVerified }
    setAuthData(response.result.jwtToken, response.result.refreshToken, username);
    error.value = null; // Clear any previous errors
    return { success: true };
  };

  const handleLoginFailure = (response, statusCode = 400, axiosError = null) => {
    const errorMessage = extractBackendError(response, statusCode, axiosError);
    error.value = errorMessage;

    return {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }]
    };
  };

  const login = async (username, password) => {
    try {
      error.value = null; // Clear previous errors

      const response = await loginRepository.loginRequest(username, password);

      // Check if response is successful based on the success property
      if (response?.success === true) {
        return handleLoginSuccess(response, username);
      } else {
        return handleLoginFailure(response, response.statusCode);
      }
    } catch (err) {
      console.error('💥 [USE LOGIN] Caught error during login:', {
        err,
        message: err?.message,
        statusCode: err?.response?.status,
        responseData: err?.response?.data,
        stack: err?.stack
      });

      // Extract status code and pass the actual error object
      const statusCode = err?.response?.status;
      return handleLoginFailure(
        err.response?.data || err,
        statusCode,
        err  // Pass the actual axios error
      );
    }
  };

  const logout = () => {
    isAuthenticated.value = false;
    user.value = null;
    token.value = null;
    refreshToken.value = null;
    error.value = null; // Clear error on logout
    authService.removeLocalTokens();
  };

  return {
    isAuthenticated,
    user,
    error,
    token,
    refreshToken,
    login,
    logout,
  };
}