import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const RECOMMENDATION_API_ENDPOINT = "/api/Recommendation";
const CREATE_RECOMMENDATION_API_ENDPOINT = "/create-recommendation";

export const createRecommendationRepository = {
  async createRecommendation(recommendationData) {
    try {
      const token = authService.getLocalToken();

      const payload = {
        title: recommendationData.title,
        description: recommendationData.description,
        observationId:
          recommendationData.observationId && typeof recommendationData.observationId === "object"
            ? recommendationData.observationId.value || recommendationData.observationId.id
            : recommendationData.observationId
      };

      const response = await api.post(`${RECOMMENDATION_API_ENDPOINT}${CREATE_RECOMMENDATION_API_ENDPOINT}`, payload, {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      });

      const { data, status } = response;
      if (status === 201 && data?.success) {
        return ApiResponseT.ok(data.result, data.successMessage || "Recommendation created successfully");
      }
      return ApiResponseT.badRequest(
        data?.errors || [{ message: data?.successMessage || "Failed to create Recommendation" }],
        data?.successMessage || "Failed to create Recommendation."
      );

    } catch (error) {

      console.error("Error creating Recommendation:", error);
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
        [{ message: error.message || "An unexpected error occurred while creating Recommendation." }],
        "An unexpected error occurred while creating Recommendation."
      );
    }
  }
};
