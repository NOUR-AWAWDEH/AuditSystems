import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const REGISTRATION_API_ENDPOINT = "/api/Users";
const CREATE_USER_API_ENDPOINT = "/create-user";

export const createUserRepository = {
  async createUser(userData) {
    try {
      const token = authService.getLocalToken();

      const payload = {
        userName: userData.userName,
        email: userData.email,
        password: userData.password,
        confirmPassword: userData.confirmPassword,
        firstName: userData.firstName,
        lastName: userData.lastName,
        roleName: userData.roleName,
        companyId: userData.companyId,
        jobTitleId: userData.jobTitleId || null,
        subDepartmentId: userData.subDepartmentId || null,
      };

      const response = await api.post(
        `${REGISTRATION_API_ENDPOINT}${CREATE_USER_API_ENDPOINT}`,
        payload,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );

      const { data, status } = response;

      if (status === 201 && data?.success) {
        return ApiResponseT.ok(data.result, data.successMessage || "User created successfully");
      }

      return ApiResponseT.badRequest(
        data?.errors || [{ message: data?.successMessage || "Failed to create user" }],
        data?.successMessage || "Failed to create user."
      );
    } catch (error) {
      console.error("Create user error:", error);
      const responseData = error.response?.data;

      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: "Authentication required. Please log in again." }],
          "Authentication required"
        );
      }

      if (error.response?.status === 400) {
        const errors = responseData?.errors || [
          { message: responseData?.successMessage || "Bad request" },
        ];
        return ApiResponseT.badRequest(errors, responseData?.successMessage || "Bad request");
      }

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while creating user." }],
        "Server error"
      );
    }
  },
};
