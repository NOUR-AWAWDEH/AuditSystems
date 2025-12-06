import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const API_ENDPOINT = "/api/Users/delete-user";

export const deleteUserRepository = {
  async deleteUser(userId) {
    try {
      const token = authService.getLocalToken();

      const response = await api.delete(API_ENDPOINT, {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
        data: {
          id: userId,
        },
      });

      const { data, status } = response;

      if (status === 204) {
        return ApiResponseT.ok(null, "User deleted successfully");
      }

      return ApiResponseT.badRequest(
        data?.errors || [{ message: data?.successMessage || "Failed to delete user" }],
        data?.successMessage || "Failed to delete user."
      );
    } catch (error) {
      console.error("Delete user error:", error);
      const responseData = error.response?.data;

      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: "Authentication required. Please log in again." }],
          "Authentication required"
        );
      }

      if (error.response?.status === 400 || error.response?.status === 404) {
        const errors = responseData?.errors || [
          { message: responseData?.successMessage || "Failed to delete user" },
        ];
        return ApiResponseT.badRequest(
          errors,
          responseData?.successMessage || "Failed to delete user"
        );
      }

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while deleting user." }],
        "Server error"
      );
    }
  },
};
