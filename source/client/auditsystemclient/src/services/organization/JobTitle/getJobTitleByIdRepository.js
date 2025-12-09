import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const JOB_TITLE_BASE_ENDPOINT = "/api/JobTitle";
const GET_JOB_TITLE_BY_ID_ENDPOINT = "/get-job-title-by-id";

export const getJobTitleByIdRepository = {
  async getJobTitleById(jobTitleId) {
    try {
      const token = authService.getLocalToken();

      const response = await api.get(`${JOB_TITLE_BASE_ENDPOINT}${GET_JOB_TITLE_BY_ID_ENDPOINT}`, {
        params: { id: jobTitleId },
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      });

      const { data, status } = response;

      if (status === 200 && data?.success && data?.result) {
        return ApiResponseT.ok(
          data.result,
          data.successMessage || "Job title retrieved successfully."
        );
      }

      return ApiResponseT.badRequest(
        data?.errors || [{ message: data?.message || "Failed to retrieve job title." }],
        data?.message || "Failed to retrieve job title."
      );
    } catch (error) {
      console.error("Error fetching job title:", error);

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
        return ApiResponseT.badRequest(errors, responseData?.message || "Bad request");
      }

      if (error.response?.status === 404) {
        return ApiResponseT.notFound([{ message: "Job title not found." }], "Job title not found.");
      }

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while retrieving job title." }],
        "Server error"
      );
    }
  },
};