import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";
import api from "@/services/api/api";

const JOB_TITLE_BASE_ENDPOINT = "/api/JobTitle";
const UPDATE_JOB_TITLE_ENDPOINT = "/update-job-title";

export const updateJobTitleByIdRepository = {
  async updateJobTitle(jobTitleData) {
    let responseData = null;
    try {
      const token = authService.getLocalToken();
      if (!token) {
        throw new Error("Authentication token not found");
      }

      const payload = {
        id: jobTitleData.id,
        name: jobTitleData.name,
        companyId:
          typeof jobTitleData.companyId === "object"
            ? jobTitleData.companyId.value || jobTitleData.companyId.id
            : jobTitleData.companyId,
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

      const { data, status } = response;
      responseData = data;

      if (status === 200 && data?.success !== false) {
        return ApiResponseT.ok(null, data?.successMessage || "Job title updated successfully.");
      }

      const errors = Array.isArray(data?.errors)
        ? data.errors
        : [{ message: data?.successMessage || "Update failed" }];
      console.error("Update job title failed with errors:", errors);
      return ApiResponseT.badRequest(errors, data?.successMessage || "Failed to update job title.");
    } catch (error) {
      responseData = error.response?.data;
      console.error("Error updating job title:", error);

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
        return ApiResponseT.notFound([{ message: "Job title not found" }], "Job title not found");
      }

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while updating job title." }],
        "Server error"
      );
    }
  },
};
