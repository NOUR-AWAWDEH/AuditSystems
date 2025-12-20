import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const USER_ENDPOINT = "/api/Users";
const UPDATE_USER_ENDPOINT = "/update-user";

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

      const response = await api.put(`${USER_ENDPOINT}${UPDATE_USER_ENDPOINT}`, payload, {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      });

      if (response.data && response.status === 200) {
        if (response.data.success) {
          return ApiResponseT.ok(
            null,
            response.data.successMessage || "User updated successfully."
          );
        } else {
          return ApiResponseT.badRequest(
            response.data.errors || [
              { message: response.data.successMessage || "Failed to update User." },
            ],
            response.data.successMessage || "Failed to update User."
          );
        }
      }

      return ApiResponseT.badRequest(
        [{ message: "Unexpected response format" }],
        "Failed to update User"
      );
    } catch (error) {
      console.error("Update User error:", error);

      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: "Authentication required. Please log in again." }],
          "Authentication required"
        );
      }

      if (error.response?.status === 400) {
        const errors = error.response?.errors || [
          { message: error.response?.message || "Bad request" },
        ];
        return ApiResponseT.badRequest(errors, error.response?.message || "Bad request");
      }

      if (error.response?.status === 404) {
        return ApiResponseT.notFound([{ message: "User not found" }], "User not found");
      }

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while updating User." }],
        "Server error"
      );
    }
  },
};
