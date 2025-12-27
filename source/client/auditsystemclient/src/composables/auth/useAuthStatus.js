import { ref, computed } from "vue";
import { authStatuseRepository } from "@/services/auth/authRepos/authStatuseRepository";

const TOKEN_KEY = "token";

export const authStorage = {
  setToken(token) {
    if (token) {
      localStorage.setItem(TOKEN_KEY, token);
      return true;
    }
    return false;
  },
  getToken() {
    return localStorage.getItem(TOKEN_KEY);
  },
  removeToken() {
    localStorage.removeItem(TOKEN_KEY);
    return true;
  },
  hasToken() {
    return !!this.getToken();
  },
  getCompanyId() {
    return localStorage.getItem("companyId");
  },
};

const authStatus = ref(null);
const isChecking = ref(false);
const error = ref(null);

export function useAuthStatus() {
  const checkAuthStatus = async (token = null) => {
    isChecking.value = true;
    error.value = null;
    try {
      const authToken = token || authStorage.getToken();
      if (!authToken) {
        throw new Error("No authentication token found");
      }
      const response = await authStatuseRepository.getAuthStatus(authToken);
      if (response?.success && response?.result?.isAuthenticated) {
        authStatus.value = {
          isAuthenticated: response.result.isAuthenticated,
          username: response.result.username,
          email: response.result.email,
          userId: response.result.userId,
          companyId: response.result.companyId,
          roles: response.result.roles,
          expiresAt: response.result.expiresAt,
          isEmailVerified: response.result.isEmailVerified,
        };

        return authStatus.value;
      } else {
        authStorage.removeToken();
        authStatus.value = null;
        const msg = response?.errors?.[0]?.message || response?.successMessage || 'Invalid auth status response';
        console.error('Auth status repository returned unexpected response:', response);
        error.value = msg;
        return null;
      }
    } catch (err) {
      console.error("Auth status check failed:", err);
      error.value = err.message || "Authentication check failed";
      authStatus.value = null;
      return null;
    } finally {
      isChecking.value = false;
    }
  };

  const initCheck = async () => {
    if (authStorage.hasToken()) {
      await checkAuthStatus();
    }
  };

  const companyId = computed(() => authStatus.value?.companyId || authStorage.getCompanyId());

  return {
    authStatus,
    isChecking,
    error,
    checkAuthStatus,
    companyId,
    initCheck,
  };
}
