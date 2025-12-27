import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";
import api from "@/services/api/api";

const RECOMMENDATION_API_ENDPOINT = "/api/Recommendation";
const RECOMMENDATION_API_ENDPOINT_ID = "/update-Recommendation-by-id";

export const updateRecommendationByIdRepository = {
  async updateRecommendationById(recommendationData) {
    let responseData = null;
    try {
      const token = authService.getLocalToken();

      const payload = {
        id: recommendationData.id,
        title: recommendationData.title,
        description: recommendationData.description,
        observationId:
          recommendationData.observationId && typeof recommendationData.observationId === "object"
            ? recommendationData.observationId.value || recommendationData.observationId.id
            : recommendationData.observationId,
      };

      const response = await api.put(`${RECOMMENDATION_API_ENDPOINT}${RECOMMENDATION_API_ENDPOINT_ID}`, payload, {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      });

      const { data, status } = response;
      responseData = data;

      if (status === 200 && data?.success !== false) {
        return ApiResponseT.ok(data?.result, data?.successMessage || "Recommendation updated successfully.");
      }

      const errors = Array.isArray(data?.errors)
        ? data.errors
        : [{ message: data?.successMessage || "Update failed" }];

      console.error("Update Recommendation failed with errors:", errors);
      return ApiResponseT.badRequest(errors, data?.successMessage || "Failed to update Recommendation.");
    
    } catch (error) {
      
      responseData = error.response?.data;
      console.error("Error updating Recommendation:", error);

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
        return ApiResponseT.notFound([{ message: "Recommendation not found" }], "Recommendation not found");
      }

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while updating Recommendation." }],
        "An unexpected error occurred while updating Recommendation."
      );
    }
  },
};
