import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";

const REGISTRATION_API_ENDPOINT = "/api/Registration";
const SEND_VERIFICATION_EMAIL_API_ENDPOINT = "/send-verification-email";

export const sendVerificationEmailRepository = {
  async sendVerificationEmailRepository(email) {
    try {
      const response = await api.post(
        `${REGISTRATION_API_ENDPOINT}${SEND_VERIFICATION_EMAIL_API_ENDPOINT}`,
        { email: email }
      );

      const { data, status } = response;

      if (status === 200 && data?.success) {
        return ApiResponseT.ok(
          data?.result,
          data?.successMessage || "Verification Email sent successfully"
        );
      }

      return ApiResponseT.badRequest(
        data?.errors || [{ message: "Failed to send verification email" }],
        data?.message || "Failed to send verification email."
      );
    } catch (error) {
      console.error("Confirm email error:", error);
      const responseData = error.response?.data;

      if (error.response?.status === 404) {
        return ApiResponseT.notFound([{ message: "email not found" }], "email not found");
      }

      if (error.response?.status === 400) {
        const errors = responseData?.errors || [
          { message: responseData?.message || "Bad request" },
        ];
        return ApiResponseT.badRequest(errors, responseData?.message || "Bad request");
      }

      return ApiResponseT.serverError(
        [
          {
            message:
              error.message || "An unexpected error occurred while sending verification email.",
          },
        ],
        "Server error"
      );
    }
  },
};
