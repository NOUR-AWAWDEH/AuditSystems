import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const API_ENDPOINT = "/api/Users/get-users-by-admin-id";

export const getUsersByAdminIdRepository = {
  async getAllUsers() {
    try {
      const token = authService.getLocalToken();

      const response = await api.get(API_ENDPOINT, {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      });

      const { data, status } = response;

      if (status === 200 && data?.success) {
        return ApiResponseT.ok(data.result, data.successMessage || "Users retrieved successfully");
      }

      return ApiResponseT.badRequest(
        data?.errors || [{ message: data?.successMessage || "Failed to retrieve users" }],
        data?.successMessage || "Failed to retrieve users."
      );
    } catch (error) {
      console.error("Get users by admin id error:", error);

      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: "Authentication required. Please log in again." }],
          "Authentication required"
        );
      }

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while retrieving users." }],
        "Server error"
      );
    }
  },
};
