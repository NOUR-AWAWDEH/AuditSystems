import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const JOB_TITLE_BASE_ENDPOINT = "/api/JobTitle";
const CREATE_JOB_TITLE_ENDPOINT = "/create-job-title";

export const createJobTitleRepository = {
  async createJobTitle(jobTitleData) {
    try {
      const token = authService.getLocalToken();
      const companyId = authService.getLocalCompanyId();

      const payload = {
        name: jobTitleData.name,
        companyId: companyId,
      };

      const response = await api.post(
        `${JOB_TITLE_BASE_ENDPOINT}${CREATE_JOB_TITLE_ENDPOINT}`,
        payload,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );

      const { data, status } = response;

      if (status === 201 || data?.success) {
        return ApiResponseT.ok(
          data.result,
          data.successMessage || "Job title created successfully"
        );
      }

      return ApiResponseT.badRequest(
        data?.errors || [{ message: data?.successMessage || "Failed to create location" }],
        data?.successMessage || "Failed to create job title."
      );

    } catch (error) {
      console.error("Create job-title error:", error);
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
        [{ message: error.message || "An unexpected error occurred while creating job-title." }],
        "Server error"
      );
    }
  },
};
