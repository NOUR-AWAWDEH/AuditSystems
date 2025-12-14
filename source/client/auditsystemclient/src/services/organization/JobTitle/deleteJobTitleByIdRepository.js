import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const JOB_TITLE_BASE_ENDPOINT = "/api/JobTitle";
const DELETE_JOB_TITLE_ENDPOINT = "/delete-job-title-by-id";

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

      const { data, status } = response;

      if (status === 204 || (status === 200 && data?.success !== false)) {
        return ApiResponseT.ok(null, data?.successMessage || "Job title deleted successfully");
      }

      const errors = data?.errors || [{ message: data?.message || "Delete failed" }];
      return ApiResponseT.badRequest(errors, data?.message || "Delete failed");
    } catch (error) {
      console.error("Error deleting location:", error);

      const responseData = error.response?.data;

      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: "Authentication required. Please log in again." }],
          "Authentication required"
        );
      }

      if (error.response?.status === 404) {
        return ApiResponseT.notFound([{ message: "Location not found" }], "Location not found");
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
        "An unexpected error occurred while deleting Job-Title.";
      return ApiResponseT.serverError([{ message: serverMessage }], "Server error");
    }
  },
};
