import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const JOB_TITLE_BASE_ENDPOINT = "/api/JobTitle";
const GET_JOB_TITLES_BY_USER_ID_ENDPOINT = "/get-job-titles-by-user-id";

export const getJobTitlesByUserIdRepository = {
  async getJobTitlesByUserId() {
    try {
      const token = authService.getLocalToken();

      const response = await api.get(
        `${JOB_TITLE_BASE_ENDPOINT}${GET_JOB_TITLES_BY_USER_ID_ENDPOINT}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          }
        }
      );

      if (response.data?.success && response.data?.result) {
        return ApiResponseT.ok(
          response.data.result,
          response.data.successMessage || "Job titles retrieved successfully."
        );
      }

      return ApiResponseT.badRequest(
        response.data?.errors || [{ message: "Failed to retrieve job titles." }],
        response.data?.successMessage || "Failed to retrieve job titles."
      );
    } catch (error) {
      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: "Authentication required. Please log in again." }],
          "Authentication required"
        );
      }

      if (error.response?.status === 400) {
        const errors = error.response?.data?.errors || [
          {
            message: error.response?.data?.successMessage || "Bad request",
          },
        ];
        return ApiResponseT.badRequest(
          errors,
          error.response?.data?.successMessage || "Bad request"
        );
      }

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while retrieving job titles." }],
        "Server error"
      );
    }
  },
};
