import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";

const REGISTRATION_API_ENDPOINT = "/api/Registration";
const CONFIRM_EMAIL_API_ENDPOINT = "/confirm-email";

export const confirmEmailRepository = {
  async confirmEmailRepository(uid, token) {
    try {
      const response = await api.get(
        `${REGISTRATION_API_ENDPOINT}${CONFIRM_EMAIL_API_ENDPOINT}?uid=${uid}&token=${token}`
      );

      const { data, status } = response;

      if (status === 200 && data?.success) {
        return ApiResponseT.ok(data.result, data.successMessage || "Email confirmed successfully");
      }

      return ApiResponseT.badRequest(
        data?.errors || [{ message: data?.successMessage || "Failed to confirm email" }],
        data?.successMessage || "Failed to confirm email."
      );
        
    } catch (error) {
      console.error("Confirm email error:", error);
      const responseData = error.response?.data;

      if (error.response?.status === 400) {
        const errors = responseData?.errors || [
          { message: responseData?.successMessage || "Bad request" },
        ];
        return ApiResponseT.badRequest(errors, responseData?.successMessage || "Bad request");
      }

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while confirming email." }],
        "Server error"
      );
    }
  },
};