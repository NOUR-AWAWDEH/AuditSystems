// src/composables/useLogin.js
import { ref } from 'vue';
import { loginRepository } from '@/services/auth/authRepos/loginRepository';
import authService from '@/services/auth/authRepos/authService';

// Move these outside the function to make them global/shared
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

  const handleLoginSuccess = (response, username) => {
    setAuthData(response.result.jwtToken, response.result.refreshToken, username);
    return { success: true };
  };

  const handleLoginFailure = (response, defaultErrorMessage = 'Authentication failed') => {
    
    const errorMessage = response?.errors?.length
      ? response.errors[0].message
      : defaultErrorMessage;
    error.value = errorMessage;

    const failureResult = {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };

    return failureResult;
  };

  const login = async (username, password) => {
    try {
      error.value = null;

      const response = await loginRepository.loginRequest(username, password);

      if (response?.success) {
        return handleLoginSuccess(response, username);
      } else {
        return handleLoginFailure(response);
      }
    } catch (err) {
      console.error('💥 [USE LOGIN] Caught error during login:', {
        err,
        message: err?.message,
        stack: err?.stack
      });

      return handleLoginFailure(err, err.message || 'Authentication failed');
    }
  };

  // Add a logout function to properly reset the global state
  const logout = () => {
    isAuthenticated.value = false;
    user.value = null;
    token.value = null;
    refreshToken.value = null;
    authService.removeLocalTokens();
    authService.removeLocalRefreshToken();
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
