import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";

const RESET_PASSWORD_END_POINT = "/api/password/reset-password";

export const resetPasswordRepository = {
  async resetPasswordRequest(resetPasswordData) {
    try {
  
      const paylode = {
        email: resetPasswordData.email,
        token: resetPasswordData.token,
        newPassword: resetPasswordData.newPassword,
        confirmNewPassword: resetPasswordData.confirmNewPassword,
      };

      const response = await api.post(`${RESET_PASSWORD_END_POINT}`, paylode);

      if (response.data && response.status === 200) {
        if (response.data.success) {
          return ApiResponseT.ok(
            null,
            response.data.successMessage || "Password reset successfully."
          );
          
        } else {
          return ApiResponseT.badRequest(
            response.data.errors || [
              { message: response.data.successMessage || "Failed to reset Password." },
            ],
            response.data.successMessage || "Failed to reset Password."
          );
        }
      }

      return ApiResponseT.badRequest(
        [{ message: "Unexpected response format" }],
        "Failed to reset Password"
      );
    } catch (error) {
      console.error("Reset Password error:", error);

      if (error.response?.status === 400) {
        const errors = error.response?.errors || [
          { message: error.response?.message || "Bad request" },
        ];
        return ApiResponseT.badRequest(errors, error.response?.message || "Bad request");
      }

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while resetting Password." }],
        "Server error"
      );
    }
  },
};