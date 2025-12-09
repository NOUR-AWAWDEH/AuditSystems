import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const USER_BASE_API_ENDPOINT = "/api/Users";
const DELETE_USER_BY_ID_API_ENDPOINT = "/delete-user-by-id";

export const deleteUserRepository = {
  async deleteUserById(userId) {
    try {
      const token = authService.getLocalToken();

      const response = await api.delete(
        `${USER_BASE_API_ENDPOINT}${DELETE_USER_BY_ID_API_ENDPOINT}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
          data: {
            id: userId,
          },
        }
      );

      const { data, status } = response;

      if (status === 204 || (status === 200 && data?.success !== false)) {
        return ApiResponseT.ok(null, data?.successMessage || "User deleted successfully");
      }

      const error = data?.errors || [{ message: data?.successMessage || "Failed to delete user" }];
      return ApiResponseT.badRequest(error, data?.message || "Delete user failed.");
    } catch (error) {
      console.error("Error deleting user:", error);

      const responseData = error.response?.data;

      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: "Authentication required. Please log in again." }],
          "Authentication required"
        );
      }

      if (error.response?.status === 404) {
        return ApiResponseT.notFound([{ message: "User not found" }], "User not found");
      }

      if (error.response?.status === 400) {
        const serverMessage =
          responseData?.message || responseData?.title || responseData?.detail || "Bad request";
        const errors = responseData?.errors || [{ message: serverMessage }];
        return ApiResponseT.badRequest(errors, serverMessage);
      }

      const serverMessage =
        responseData?.message ||
        responseData?.title ||
        error.message ||
        "An unexpected error occurred while deleting user.";
      return ApiResponseT.serverError([{ message: serverMessage }], "Server error");
    }
  },
};