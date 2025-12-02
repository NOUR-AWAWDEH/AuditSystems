import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const JOB_TITLE_API_ENDPOINT = "/api/JobTitle";
const SEARCH_JOB_TITLE_ENDPOINT = "/search-jobtitle";

export const getJobTitlesRepository = {
  async getJobTitles(companyId = null, searchTerm = "", pageNumber = 1, pageSize = 100) {
    try {
      const token = authService.getLocalToken();

      // Build query parameters
      const params = new URLSearchParams({
        pageNumber: pageNumber.toString(),
        pageSize: pageSize.toString(),
      });

      if (searchTerm) {
        params.append("searchTerm", searchTerm);
      }

      if (companyId) {
        params.append("companyId", companyId);
      }

      const response = await api.get(
        `${JOB_TITLE_API_ENDPOINT}${SEARCH_JOB_TITLE_ENDPOINT}?${params.toString()}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );

      const { data, status } = response;

      if (status === 200 && data?.success) {
        return ApiResponseT.ok(
          data.result,
          data.successMessage || "Job titles fetched successfully"
        );
      }

      return ApiResponseT.badRequest(
        data?.errors || [{ message: "Failed to fetch job titles" }],
        data?.successMessage || "Failed to fetch job titles."
      );
    } catch (error) {
      console.error("Get job titles error:", error);
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
        [{ message: error.message || "An unexpected error occurred while fetching job titles." }],
        "Server error"
      );
    }
  },
};
