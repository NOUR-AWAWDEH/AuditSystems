// src/composables/useLogin.js
import { ref } from "vue";
import { loginRepository } from "@/services/auth/authRepos/loginRepository";
import authService from "@/services/auth/authRepos/authService";

// Persistent refs
const isAuthenticated = ref(!!authService.getLocalToken());
const user = ref(null);
const error = ref(null);
const token = ref(authService.getLocalToken() || null);
const refreshToken = ref(authService.getLocalRefreshToken ? authService.getLocalRefreshToken() : null);
const companyId = ref(authService.getLocalCompanyId ? authService.getLocalCompanyId() : null);

export function useLogin() {
  const setAuthData = (newToken, newRefreshToken, username, newCompanyId) => {
    token.value = newToken;
    refreshToken.value = newRefreshToken;
    user.value = { username };
    companyId.value = newCompanyId;
    isAuthenticated.value = true;

    authService.setLocalToken(token.value);
    authService.setLocalRefreshToken(refreshToken.value);

    if (newCompanyId) {
      authService.setLocalCompanyId(newCompanyId);
    }
  };

  const extractBackendError = (response, statusCode, axiosError) => {
    const fallback = "An unexpected error occurred";

    if (statusCode === 429) {
      return "Too many login attempts. Please try again later.";
    }

    if (response && typeof response === "object") {
      
      if (response.errors && Array.isArray(response.errors) && response.errors.length > 0) {
        const firstError = response.errors[0];
        if (firstError && firstError.message) {
          return firstError.message;
        }
        if (firstError && typeof firstError === "string") {
          return firstError;
        }
      }

      
      if (response.successMessage) {
        return response.successMessage;
      }

      
      if (response.message) {
        return response.message;
      }
    }

    
    if (axiosError?.response?.data) {
      const errorData = axiosError.response.data;

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

    if (axiosError && axiosError.message) {
      if (axiosError.message.includes("Network Error")) {
        return "Network error. Please check your connection.";
      }
      if (axiosError.message.includes("timeout")) {
        return "Request timeout. Please try again.";
      }
    }

    if (typeof response === "string") {
      return response;
    }

    return fallback;
  };

  const handleLoginSuccess = (response, username) => {
    
    setAuthData(
      response.result.jwtToken,
      response.result.refreshToken,
      username,
      response.result.companyId
    );
    error.value = null; 
    return { success: true };
  };

  const handleLoginFailure = (response, statusCode = 400, axiosError = null) => {
    const errorMessage = extractBackendError(response, statusCode, axiosError);
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
      
      if (response?.success === true) {
        return handleLoginSuccess(response, username);
      } else {
        return handleLoginFailure(response, response.statusCode);
      }

    } catch (err) {
      console.error("💥 [USE LOGIN] Caught error during login:", {
        err,
        message: err?.message,
        statusCode: err?.response?.status,
        responseData: err?.response?.data,
        stack: err?.stack,
      });

      const statusCode = err?.response?.status;
      return handleLoginFailure(
        err.response?.data || err,
        statusCode,
        err
      );
    }
  };

  const logout = () => {
    isAuthenticated.value = false;
    user.value = null;
    token.value = null;
    refreshToken.value = null;
    companyId.value = null;
    error.value = null;
    authService.removeLocalTokens();
  };

  return {
    isAuthenticated,
    user,
    error,
    token,
    refreshToken,
    companyId,
    login,
    logout,
  };
}
