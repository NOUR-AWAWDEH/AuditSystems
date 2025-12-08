import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";
import api from "@/services/api/api";

const JOB_TITLE_BASE_ENDPOINT = "/api/JobTitle";
const UPDATE_JOB_TITLE_ENDPOINT = "/update-job-title";

export const updateJobTitleByIdRepository = {
  async updateJobTitle(jobTitleData) {
    try {
      const token = authService.getLocalToken();

      const payload = {
        id: jobTitleData.id,
        name: jobTitleData.name,
        companyId: jobTitleData.companyId,
      };

      const response = await api.put(
        `${JOB_TITLE_BASE_ENDPOINT}${UPDATE_JOB_TITLE_ENDPOINT}`,
        payload,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );

      if (response.data && response.status === 200) {
        if (response.data.success) {
          return ApiResponseT.ok(
            null,
            response.data.successMessage || "Job-Title updated successfully."
          );
        } else {
          return ApiResponseT.badRequest(
            response.data.errors || [
              { message: response.data.successMessage || "Failed to update Job-Title." },
            ],
            response.data.successMessage || "Failed to update Job-Title."
          );
        }
      }

      return ApiResponseT.badRequest(
        [{ message: "Unexpected response format" }],
        "Failed to update Job-Title"
      );
    } catch (error) {
      console.error("Update Job-Title error:", error);

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
        return ApiResponseT.notFound([{ message: "Job-Title not found" }], "Job-Title not found");
      }

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while updating Job-Title." }],
        "Server error"
      );
    }
  },
};
