import { ref } from 'vue';
import { logoutRepository } from '@/services/auth/logoutRepository';

// Constants for localStorage keys
const TOKEN_KEY = 'token';
const REFRESH_TOKEN_KEY = 'refreshToken';

export function useLogout() {
  const isAuthenticated = ref(!!localStorage.getItem(TOKEN_KEY)); // Initialize based on existing token
  const user = ref(null);
  const error = ref(null);
  const token = ref(localStorage.getItem(TOKEN_KEY) || null);
  const refreshToken = ref(localStorage.getItem(REFRESH_TOKEN_KEY) || null);

  const clearAuthData = () => {
    token.value = null;
    refreshToken.value = null;
    user.value = null;
    isAuthenticated.value = false;
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
  };

  const handleLogoutSuccess = (response) => {
    clearAuthData();
    return { success: true, message: response?.message || 'Successfully logged out' };
  };

  const handleLogoutFailure = (response, defaultErrorMessage = 'Logout failed') => {
    const errorMessage = response?.errors?.length
      ? response.errors[0].message
      : defaultErrorMessage;
    error.value = errorMessage;
    clearAuthData(); // Clear auth data even on failure to ensure clean state
    return {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };
  };

  const logout = async () => {
    try {
      error.value = null;
      const response = await logoutRepository.logoutRequest(refreshToken.value);

      if (response?.success) {
        return handleLogoutSuccess(response);
      } else {
        return handleLogoutFailure(response);
      }
    } catch (err) {
      console.error('Logout error:', err);
      return handleLogoutFailure(err, err.message || 'Logout failed');
    }
  };

  return {
    isAuthenticated,
    user,
    error,
    token,
    refreshToken,
    logout,
  };
}
