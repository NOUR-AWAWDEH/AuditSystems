import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const API_ENDPOINT = "/api/Users/get-user-by-id";

export const getUserByIdRepository = {
  async getUserById(userId) {
    try {
      const token = authService.getLocalToken();

      const response = await api.get(API_ENDPOINT, {
        params: { id: userId },
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        }
      });

      const { data, status } = response;

      if (status === 200 && data?.success && data?.result) {
        return ApiResponseT.ok(
          data.result,
          data.successMessage || "User retrieved successfully"
        );
      }

      return ApiResponseT.badRequest(
        data?.errors || [{ message: data?.message || "Failed to retrieve user" }],
        data?.message || "Failed to retrieve user."
      );

    } catch (error) {
      console.error("Get user by id error:", error);
      
      const responseData = error.response?.data;

      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: "Authentication required. Please log in again." }],
          "Authentication required"
        );
      }

      if (error.response?.status === 400) {
        const errors = responseData?.errors || [
          { message: responseData?.message || "Bad request" },
        ];
        return ApiResponseT.badRequest(
          errors,
          responseData?.message || "Bad request"
        );
      }

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while retrieving user." }],
        "Server error"
      );
    }
  },
};