// src/composables/auth/useAuthStatus.js

import { ref } from 'vue';
import { authStatuseRepository } from '@/services/auth/authRepos/authStatuseRepository';

// Token key for consistency
const TOKEN_KEY = 'token';

// Create the authStorage object to manage token operations
export const authStorage = {
  // Store the token in localStorage
  setToken(token) {
    if (token) {
      localStorage.setItem(TOKEN_KEY, token);
      return true;
    }
    return false;
  },

  // Get the token from localStorage
  getToken() {
    return localStorage.getItem(TOKEN_KEY);
  },

  // Remove the token from localStorage
  removeToken() {
    localStorage.removeItem(TOKEN_KEY);
    return true;
  },

  // Check if a token exists
  hasToken() {
    return !!this.getToken();
  }
};

export function useAuthStatus() {
  const authStatus = ref(null);
  const isChecking = ref(false);
  const error = ref(null);

  const checkAuthStatus = async (token = null) => {
    isChecking.value = true;
    error.value = null;

    try {
      const authToken = token || authStorage.getToken();

      if (!authToken) {
        throw new Error('No authentication token found');
      }

      const response = await authStatuseRepository.getAuthStatus(authToken);

      if (response?.success && response?.result?.isAuthenticated) {
        // Extract data from the ApiResponseT result field
        authStatus.value = {
          isAuthenticated: response.result.isAuthenticated,
          username: response.result.username,
          email: response.result.email,
          userId: response.result.userId,
          roles: response.result.roles,
          expiresAt: response.result.expiresAt,
          isEmailVerified: response.result.isEmailVerified
        };
        return authStatus.value;
      } else {
        // Token is invalid or expired
        authStorage.removeToken();
        authStatus.value = null;
        throw new Error(response?.errors?.[0]?.message || 'Authentication failed');
      }
    } catch (err) {
      console.error('Auth status check failed:', err);
      error.value = err.message || 'Authentication check failed';
      authStatus.value = null;
      return null;
    } finally {
      isChecking.value = false;
    }
  };

  // Check auth status on initialization if token exists
  const initCheck = async () => {
    if (authStorage.hasToken()) {
      await checkAuthStatus();
    }
  };

  return {
    authStatus,
    isChecking,
    error,
    checkAuthStatus,
    initCheck
  };
}
