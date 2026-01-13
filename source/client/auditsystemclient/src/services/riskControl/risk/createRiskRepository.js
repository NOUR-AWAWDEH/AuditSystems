import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from "@/services/auth/authRepos/authService";

const RISK_API_ENDPOINT = "/api/Risk";
const CREATE_RISK_API_ENDPOINT = "/create-risk";

export const createRiskRepository = {
  async createRisk(riskData) {
    try {
      const token = authService.getLocalToken();

      const payload = {
        factor: riskData.factor,
        impact: riskData.impact,
        observationId:
          riskData.observationId && typeof riskData.observationId === "object"
            ? riskData.observationId.value || riskData.observationId.id
            : riskData.observationId,
      };

      const response = await api.post(`${RISK_API_ENDPOINT}${CREATE_RISK_API_ENDPOINT}`, payload, {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      });

      const { data, status } = response;
      if (status === 201 && data?.success) {
        return ApiResponseT.ok(data.result, data.successMessage || "Risk created successfully");
      }
      return ApiResponseT.badRequest(
        data?.errors || [{ message: data?.successMessage || "Failed to create Risk" }],
        data?.successMessage || "Failed to create Risk."
      );
    } catch (error) {
      console.error("Error creating Risk:", error);
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
        [{ message: error.message || "An unexpected error occurred while creating Risk." }],
        "An unexpected error occurred while creating Risk."
      );
    }
  },
};
