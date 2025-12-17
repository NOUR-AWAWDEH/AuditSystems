import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";

const REGISTRATION_API_ENDPOINT = "/api/Registration";
const CONFIRM_EMAIL_API_ENDPOINT = "/confirm-email";

export const confirmEmailRepository = {
  async confirmEmailRepository(uid, token) {
    try {
      const response = await api.get(`${REGISTRATION_API_ENDPOINT}${CONFIRM_EMAIL_API_ENDPOINT}`, {
        params: { uid, token },
      });

      const { data, status } = response;

      if (status === 200) {
        
        const successMessage =
          typeof data === "string" ? data : data?.successMessage || "Email confirmed successfully";

        return ApiResponseT.ok(data?.result || {}, successMessage);
      }

      return ApiResponseT.badRequest(
        data?.errors || [{ message: "Failed to confirm email" }],
        "Failed to confirm email."
      );
    } catch (error) {
      console.error("Confirm email error:", error);
      const responseData = error.response?.data;
      const status = error.response?.status;

      if (status === 400 || status === 500) {

        const errorMessage =
          typeof responseData === "string"
            ? responseData
            : responseData?.message || responseData?.errors?.[0]?.message || "An error occurred";

        const apiResponse =
          status === 400
            ? ApiResponseT.badRequest([{ message: errorMessage }], errorMessage)
            : ApiResponseT.serverError([{ message: errorMessage }], errorMessage);

        return apiResponse;
      }

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while confirming email." }],
        "Server error"
      );
    }
  },
};