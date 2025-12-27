import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const RECOMMENDATION_API_ENDPOINT = "/api/Recommendation";
const RECOMMENDATION_API_ENDPOINT_ID = "/get-recommendation-by-id";

export const getRecommendationByIdRepository = {
  async getRecommendationById(recommendationId) {
    let responseData = null;
    try {
      const token = authService.getLocalToken();

      const response = await api.get(`${RECOMMENDATION_API_ENDPOINT}${RECOMMENDATION_API_ENDPOINT_ID}`, {
        params: { id: recommendationId },
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      });

      const { data, status } = response;
      responseData = data;

      if (status === 200 && data?.success !== false) {
        return ApiResponseT.ok(
          data?.result?.recommendationModel,
          data?.successMessage || "Recommendation retrieved successfully"
        );
      }

      const errors = Array.isArray(data?.errors)
        ? data.errors
        : [{ message: "Failed to retrieve Recommendation." }];
      console.error("Retrieve Recommendation failed with errors:", errors);

      return ApiResponseT.badRequest(errors, data?.successMessage || "Failed to retrieve Recommendation.");
    } catch (error) {
      responseData = error.response?.data;
      console.error("Error retrieving an Recommendation:", error);

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

      if (error.response?.status === 404) {
        return ApiResponseT.notFound([{ message: "Recommendation not found." }], "Recommendation not found.");
      }
      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while retrieving Recommendation." }],
        "An unexpected error occurred while retrieving Recommendation."
      );
    }
  },
};
