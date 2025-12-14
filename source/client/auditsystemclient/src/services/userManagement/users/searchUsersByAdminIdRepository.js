import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";
import api from "@/services/api/api";

const USER_BASE_ENDPOINT = "/api/Users";
const SEARCH_USERS_BY_ADMIN_ID_ENDPOINT = "/search-users-by-admin-id";

export const searchUsersByAdminIdRepository = {
    async searchUsersByAdminId(searchParams = {}) {
        try {
            const token = authService.getLocalToken();

            if (!token) {
                throw new Error("Authentication token not found");
            }

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

            const queryString = queryParams.toString();

            const url = `${USER_BASE_ENDPOINT}${SEARCH_USERS_BY_ADMIN_ID_ENDPOINT}
                ${queryString ? `?${queryString}` : ""}`;

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
                    data.successMessage || "Users retrieved successfully."
                );
            }

            return ApiResponseT.badRequest(
                data?.errors || [{ message: "Failed to retrieve users." }],
                data?.message || "Failed to retrieve users."
            );
            
        } catch (error) {
            console.error("Error searching users:", error);

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