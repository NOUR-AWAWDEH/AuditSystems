import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const JOB_TITLE_BASE_ENDPOINT = "/api/JobTitle";
const DELETE_JOB_TITLE_ENDPOINT = "/delete-jobtitle-by-id";

export const deleteJobTitleByIdRepository = {
  async deleteJobTitleById(jobTitleId) {
    try {
      const token = authService.getLocalToken();

      const response = await api.delete(`${JOB_TITLE_BASE_ENDPOINT}${DELETE_JOB_TITLE_ENDPOINT}`, {
        data: { id: jobTitleId },
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      });

      if (response.status === 204 || response?.success !== false) {
        return ApiResponseT.ok(null, "Job title deleted successfully");
      }

      const errors = Array.isArray(response.errors)
        ? response.errors
        : [{ message: response.successMessage || "Delete failed" }];
      return ApiResponseT.badRequest(errors, response.successMessage);
    } catch (error) {
      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: "Authentication required. Please log in again." }],
          "Authentication required"
        );
      }

      if (error.response?.status === 400) {
        // Extract server error message properly
        const responseData = error.response?.data;
        const serverMessage =
          responseData?.message || responseData?.title || responseData?.detail || "Bad request";
        const errors = responseData?.errors || [{ message: serverMessage }];
        return ApiResponseT.badRequest(errors, serverMessage);
      }

      if (error.response?.status === 404) {
        return ApiResponseT.notFound([{ message: "Job title not found" }], "Job title not found");
      }

      // Handle other server errors
      const responseData = error.response?.data;
      const serverMessage =
        responseData?.message ||
        responseData?.title ||
        error.message ||
        "An unexpected error occurred while deleting job title.";

      return ApiResponseT.serverError([{ message: serverMessage }], "Server error");
    }
  },
};
