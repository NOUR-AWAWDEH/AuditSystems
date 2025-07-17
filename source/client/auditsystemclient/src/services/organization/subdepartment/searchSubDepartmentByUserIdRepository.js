import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";
import api from "@/services/api/api";

const SUBDEPARTMENT_BASE_ENDPOINT = "api/SubDepartment";
const SUBDEPARTMENT_SEARCH_BY_USER_ID_ENDPOINT = "/search-sub-department-by-user-id";

export const searchSubDepartmentByUserIdRepository = {
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

      if (response.data && response.status === 200) {
        return ApiResponseT.ok(response.data, "Subdepartments retrieved successfully");
      }

      return ApiResponseT.badRequest(
        response.data?.errors || [{ message: "Failed to retrieve subdepartments" }],
        response.data?.message || "Failed to retrieve subdepartments"
      );

    } catch (error) {
      console.error("Error searching subdepartments:", error);

      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized("Unauthorized: Please login again");
      }

      return ApiResponseT.internalServerError(
        error.response?.data?.message || error.message || "Failed to search subdepartments"
      );
    }
  }
};
