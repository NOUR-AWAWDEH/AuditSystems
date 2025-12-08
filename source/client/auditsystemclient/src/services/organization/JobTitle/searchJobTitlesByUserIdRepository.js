import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";
import api from "@/services/api/api";

const JOB_TITLE_BASE_ENDPOINT = "/api/JobTitle";
const SEARCH_JOB_TITLE_BY_USER_ID_ENDPOINT = "/search-job-titles-by-user-id";

export const searchJobTitlesByUserIdRepository = {
  async searchJobTitlesByUserId(searchParams = {}) {
    try {
      const token = authService.getLocalToken();

      if (!token) {
        throw new Error("Authentication token not found");
      }

      const companyId = authService.getLocalCompanyId();

      const queryParams = new URLSearchParams();

      if (searchParams.searchTerm) {
        queryParams.append("searchTerm", searchParams.searchTerm);
      }

      if (searchParams.pageNumber) {
        queryParams.append("pageNumber", searchParams.pageNumber);
      } else if (searchParams.page) {
        queryParams.append("pageNumber", searchParams.page);
      }

      if (searchParams.pageSize) {
        queryParams.append("pageSize", searchParams.pageSize);
      }

      // Backend SearchJobTitleQuery supports CompanyId if provided
      if (searchParams.companyId) {
        queryParams.append("companyId", searchParams.companyId);
      } else if (companyId) {
        queryParams.append("companyId", companyId);
      }

      const queryString = queryParams.toString();
      const url = `${JOB_TITLE_BASE_ENDPOINT}${SEARCH_JOB_TITLE_BY_USER_ID_ENDPOINT}${
        queryString ? `?${queryString}` : ""
      }`;

      const response = await api.get(url, {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      });

      const { data, status } = response;

      if (status === 200 && data?.success) {
        return ApiResponseT.ok(
          data.result,
          data.successMessage || "Job titles retrieved successfully."
        );
      }

      return ApiResponseT.badRequest(
        data?.errors || [{ message: "Failed to retrieve job titles." }],
        data?.message || "Failed to retrieve job titles."
      );
    } catch (error) {
      console.error("Error searching job titles:", error);

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

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred." }],
        "Server error"
      );
    }
  },
};
