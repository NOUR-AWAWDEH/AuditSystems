import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const API_ENDPOINT = "/api/Users/update-user";

export const updateUserRepository = {
  async updateUser(userData) {
    try {
      const token = authService.getLocalToken();

      const payload = {
        id: userData.id,
        userName: userData.userName,
        firstName: userData.firstName,
        lastName: userData.lastName,
        email: userData.email,
        role: userData.role,
        subDepartment: userData.subDepartment || null,
        jobTitle: userData.jobTitle || null,
      };

      const response = await api.put(API_ENDPOINT, payload, {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      });

      const { data, status } = response;

      if (status === 204) {
        return ApiResponseT.ok(null, "User updated successfully");
      }

      return ApiResponseT.badRequest(
        data?.errors || [{ message: data?.successMessage || "Failed to update user" }],
        data?.successMessage || "Failed to update user."
      );
    } catch (error) {
      console.error("Update user error:", error);
      const responseData = error.response?.data;

      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: "Authentication required. Please log in again." }],
          "Authentication required"
        );
      }

      if (error.response?.status === 400 || error.response?.status === 404) {
        const errors = responseData?.errors || [
          { message: responseData?.successMessage || "Failed to update user" },
        ];
        return ApiResponseT.badRequest(
          errors,
          responseData?.successMessage || "Failed to update user"
        );
      }

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while updating user." }],
        "Server error"
      );
    }
  },
};