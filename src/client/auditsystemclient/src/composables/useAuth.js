// src/composables/useAuth.js
import { ref } from 'vue';
import { authService } from '@/services/auth/authService';

export function useAuth() {
  const isAuthenticated = ref(false);
  const user = ref(null);
  const error = ref(null);
  const token = ref(localStorage.getItem('token') || null);
  const refreshToken = ref(localStorage.getItem('refreshToken') || null);

  const login = async (username, password) => {
    try {
      error.value = null;
      const response = await authService.login(username, password);

      if (response.success) {
        token.value = response.result.jwtToken;
        refreshToken.value = response.result.refreshToken;
        user.value = { username }; // Adjust based on actual user data
        isAuthenticated.value = true;

        localStorage.setItem('token', token.value);
        localStorage.setItem('refreshToken', refreshToken.value);
        authService.setAuthHeader(token.value);

        return { success: true };
      } else {
        // Extract the first error message for display
        const errorMessage = response.errors?.length
          ? response.errors[0].message
          : 'Authentication failed';
        error.value = errorMessage;
        return {
          success: false,
          message: errorMessage,
          errors: response.errors, // Include full errors for flexibility
        };
      }
    } catch (err) {
      console.error('Login error:', err);
      const errorMessage = err.message || 'Authentication failed';
      error.value = errorMessage;
      return {
        success: false,
        message: errorMessage,
        errors: [{ message: errorMessage }],
      };
    }
  };

  const logout = async () => {
    try {
      const response = await authService.logout(token.value);
      if (response.success) {
        clearAuthData();
        return { success: true };
      } else {
        error.value = response.errors?.length
          ? response.errors[0].message
          : 'Logout failed';
        return {
          success: false,
          message: error.value,
        };
      }
    } catch (err) {
      error.value = err.message || 'Logout failed';
      return {
        success: false,
        message: error.value,
      };
    }
  };

  const clearAuthData = () => {
    token.value = null;
    refreshToken.value = null;
    isAuthenticated.value = false;
    user.value = null;
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    authService.clearAuthHeader();
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
