import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";
import api from "@/services/api/api";

const SUBDEPARTMENT_BASE_ENDPOINT = "api/SubDepartment";
const SUBDEPARTMENT_SEARCH_BY_USER_ID_ENDPOINT = "/search-sub-department-by-user-id";

export const searchSubDepartmentsByUserIdRepository = {
  async searchSubDepartmentsByUserId(searchParams = {}) {
    try {
      const token = authService.getLocalToken();

      if (!token) {
        throw new Error("Authentication token not found");
      }

      const queryParams = new URLSearchParams();

      // Add search parameters
      if (searchParams.searchTerm) {
        queryParams.append("searchTerm", searchParams.searchTerm);
      }

      // Add departmentId parameter for server-side filtering
      if (searchParams.departmentId) {
        queryParams.append("departmentId", searchParams.departmentId);
      }

      // Pagination parameters
      if (searchParams.page) {
        queryParams.append("page", searchParams.page);
      }
      if (searchParams.pageSize) {
        queryParams.append("pageSize", searchParams.pageSize);
      }

      // Sorting parameters
      if (searchParams.sortBy) {
        queryParams.append("sortBy", searchParams.sortBy);
      }
      if (searchParams.sortDirection) {
        queryParams.append("sortDirection", searchParams.sortDirection);
      }

      const queryString = queryParams.toString();
      const url = `${SUBDEPARTMENT_BASE_ENDPOINT}${SUBDEPARTMENT_SEARCH_BY_USER_ID_ENDPOINT}${
        queryString ? `?${queryString}` : ""
      }`;

      const response = await api.get(url, {
          headers: {
              Authorization: `Bearer ${token}`,
              "Content-Type": "application/json",
          },
      });

      // Fix: Access response.data and response.status correctly
      const { data, status } = response;

      if (status === 200 && data?.success) {
          return ApiResponseT.ok(
              data.result,
              data.successMessage || "Subdepartments retrieved successfully"
          );
      }

      return ApiResponseT.badRequest(
          data?.errors || [{ message: data?.message || "Failed to retrieve subdepartments" }],
          data?.message || "Failed to retrieve subdepartments"
      );

    } catch (error) {
        console.error("Error searching subdepartments:", error);

        const responseData = error.response?.data;

        if (error.response?.status === 401) {
            return ApiResponseT.unauthorized(
                [{ message: "Authentication required. Please log in again." }],
                "Authentication required"
            );
        }

        if (error.response?.status === 400) {
            const errors = responseData?.errors ||
                [{ message: responseData?.message || "Bad request" }];
            return ApiResponseT.badRequest(errors, responseData?.message || "Bad request");
        }

        return ApiResponseT.serverError(
            [{ message: error.message || "An unexpected error occurred while searching subdepartments." }],
            "Server error"
        );
    }
  }
};
