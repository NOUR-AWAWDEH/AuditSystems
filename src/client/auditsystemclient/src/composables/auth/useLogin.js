// src/composables/useLogin.js
import { ref } from 'vue';
import { loginRepository } from '@/services/auth/authRepos/loginRepository';

// Constants for localStorage keys
const TOKEN_KEY = 'token';
const REFRESH_TOKEN_KEY = 'refreshToken';

export function useLogin() {
  const isAuthenticated = ref(!!localStorage.getItem(TOKEN_KEY)); // Initialize based on existing token
  const user = ref(null);
  const error = ref(null);
  const token = ref(localStorage.getItem(TOKEN_KEY) || null);
  const refreshToken = ref(localStorage.getItem(REFRESH_TOKEN_KEY) || null);

  const setAuthData = (newToken, newRefreshToken, username) => {
    token.value = newToken;
    refreshToken.value = newRefreshToken;
    user.value = { username };
    isAuthenticated.value = true;
    localStorage.setItem(TOKEN_KEY, token.value);
    localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken.value);
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
    return {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };
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
      console.error('Login error:', err);
      return handleLoginFailure(err, err.message || 'Authentication failed');
    }
  };

  return {
    isAuthenticated,
    user,
    error,
    token,
    refreshToken,
    login,
  };
}
