// src/services/auth/passwordRepos/forgetPasswordRepository.js
import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";

const FORGOT_PASSWORD_ENDPOINT = "api/password/forgot-password";

export const forgetPasswordRepository = {
  async forgetPasswordRequest(email) {
    try {
      const response = await api.post(FORGOT_PASSWORD_ENDPOINT, { email });

      const { data, status } = response;

      if (status === 200 && data?.success !== false) {
        return ApiResponseT.ok(
          data?.result,
          data?.successMessage || "Reset code sent successfully"
        );
      }

      return ApiResponseT.badRequest(
        data?.errors || [{ message: data?.message || "Failed to send reset code" }],
        data?.message || "Failed to send reset code"
      );

    } catch (error) {
      console.error("Forgot password error:", error);

      const responseData = error.response?.data;

      if (error.response?.status === 400) {
        const errors = responseData?.errors || [
          { message: responseData?.message || "Bad request" },
        ];
        return ApiResponseT.badRequest(errors, responseData?.message || "Bad request");
      }

      if (error.response?.status === 404) {
        return ApiResponseT.notFound([{ message: "Email not found" }], "Email not found");
      }

      return ApiResponseT.serverError(
        [
          {
            message:
              error.message || "An unexpected error occurred while requesting password reset.",
          },
        ],
        "Server error"
      );
    }
  },
};