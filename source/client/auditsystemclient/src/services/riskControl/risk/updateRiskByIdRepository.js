import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";
import api from "@/services/api/api";

const RISK_API_ENDPOINT = "/api/Risk";
const RISK_API_ENDPOINT_ID = "/update-risk-by-id";

export const updateRiskByIdRepository = {
  async updateRiskById(riskData) {
    let responseData = null;
    try {
      const token = authService.getLocalToken();

      const payload = {
        id: riskData.id,
        factor: riskData.factor,
        impact: riskData.impact,
        observationId:
          riskData.observationId && typeof riskData.observationId === "object"
            ? riskData.observationId.value || riskData.observationId.id
            : riskData.observationId,
      };

      const response = await api.put(`${RISK_API_ENDPOINT}${RISK_API_ENDPOINT_ID}`, payload, {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      });

      const { data, status } = response;
      responseData = data;

      if (status === 200 && data?.success !== false) {
        return ApiResponseT.ok(data?.result, data?.successMessage || "Risk updated successfully.");
      }

      const errors = Array.isArray(data?.errors)
        ? data.errors
        : [{ message: data?.successMessage || "Update failed" }];
      console.error("Update risk failed with errors:", errors);
      return ApiResponseT.badRequest(errors, data?.successMessage || "Failed to update risk.");
    } catch (error) {
      responseData = error.response?.data;
      console.error("Error updating risk:", error);

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
        return ApiResponseT.notFound([{ message: "Risk not found" }], "Risk not found");
      }

      return ApiResponseT.serverError(
        [{ message: error.message || "An unexpected error occurred while updating risk." }],
        "An unexpected error occurred while updating risk."
      );
    }
  },
};
