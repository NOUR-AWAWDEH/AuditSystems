import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const ACTION_OWNER_BASE_ENDPOINT = "/api/Users";
const GET_ACTION_OWNERS_BY_ADMIN_ID_ENDPOINT = "/get-action-owners-by-admin-id";

export const getActionOwnersByAdminIdRepository = {
  async getActionOwnersByAdminId() {
    try {
      const token = authService.getLocalToken();

      const response = await api.get(
        `${ACTION_OWNER_BASE_ENDPOINT}${GET_ACTION_OWNERS_BY_ADMIN_ID_ENDPOINT}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );

      if (response.status === 200) {
        const actionOwners = response.data?.result?.actionOwners || [];

        return ApiResponseT.ok(
          actionOwners,
          response.data?.successMessage || "Action owners fetched successfully."
        );
      }

      // Handle non-200 responses
      return ApiResponseT.badRequest(
        response?.errors || [
          { message: response?.data?.successMessage || "Failed to fetch action-owners." },
        ],
        response?.data?.successMessage || "Failed to fetch action-owners."
      );
    } catch (error) {
      if (error.response?.status === 400) {
        return ApiResponseT.badRequest(
          error.response?.data?.errors || [
            { message: error.response?.data?.successMessage || "Bad request" },
          ],
          error.response?.data?.successMessage || "Bad request"
        );
      }

      if (error.response?.status === 401) {
        return ApiResponseT.unauthorized(
          [{ message: "Authentication required. Please log in again." }],
          "Authentication required"
        );
      }

      if (error.response?.status === 404) {
        return ApiResponseT.ok([], "No action-owners found");
      }

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while fetching action-owners." }],
        "Server error"
      );
    }
  },
};
