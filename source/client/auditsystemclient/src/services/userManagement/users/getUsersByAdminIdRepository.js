import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const USER_BASE_API_ENDPOINT = "/api/Users";
const GET_USERS_BY_ADMIN_ID_ENDPOINT = "/get-users-by-admin-id";

export const getUsersByAdminIdRepository = {
  async getUsersByAdminId() {
    try {
      const token = authService.getLocalToken();

      const response = await api.get(`${USER_BASE_API_ENDPOINT}${GET_USERS_BY_ADMIN_ID_ENDPOINT}`, {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
          "Cache-Control": "no-cache, no-store, must-revalidate",
          Pragma: "no-cache",
          Expires: "0",
        },
      });

      if (response.status === 200) {
        const users = response.data?.result?.users || [];

        return ApiResponseT.ok(
          users,
          response.data?.successMessage || "Users retrieved successfully"
        );
      }

      return ApiResponseT.badRequest(
        response?.data?.errors || [{ message: response?.data?.successMessage || "Failed to retrieve users" }],
        response?.data?.successMessage || "Failed to retrieve users."
      );

    } catch (error) {
      if (error.response?.status === 400) {
        return ApiResponseT.badRequest(
          error.response?.data?.errors || [{ message: error.response?.data?.successMessage || "Bad request" }],
          error.response?.data?.successMessage || "Bad request"
        );
      }

      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: "Authentication required. Please log in again." }],
          "Authentication required"
        );
      }

      if (error.response?.status === 404) {
        return ApiResponseT.ok(
          [],
          "No users found"
        );
      }

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while retrieving users." }],
        "Server error"
      );
    }
  },
};