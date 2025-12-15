// verifyResetCodeRepository.js
import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";

const AUTH_ENDPOINT = "/api/password/verify-reset-code";

const verifyResetCode = async (email, code) => {
  try {
    const response = await api.post(AUTH_ENDPOINT, { email, code });

    const { data } = response;

    if (!data?.success) {
      const errors = Array.isArray(data?.errors)
        ? data.errors
        : [{ message: data?.message || "Verification failed" }];
      return ApiResponseT.badRequest(errors, data?.message);
    }

    const result = data.result;
    return ApiResponseT.ok(result, data.successMessage);
  } catch (error) {
    console.error("verifyResetCode error:", error);
    console.error("Error response:", error.response?.data);
    console.error("Error status:", error.response?.status);
    let errorMessage = "Verification failed";
    let errors = [{ message: errorMessage }];

    if (error?.response?.data) {
      const errorData = error.response.data;
      errorMessage = errorData.successMessage || errorData.message || errorMessage;
      errors = Array.isArray(errorData.errors) ? errorData.errors : [{ message: errorMessage }];
    } else if (error?.message) {
      errorMessage = error.message;
      errors = [{ message: errorMessage }];
    }

    return ApiResponseT.badRequest(errors, errorMessage);
  }
};

export default verifyResetCode;
