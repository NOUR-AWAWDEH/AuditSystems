// forgetPasswordRepository.test.js
import { describe, it, expect, vi, beforeEach } from "vitest";
import { forgetPasswordRepository } from "@/services/auth/passwordRepos/forgetPasswordRepository";
import api from "@/services/api/api";

// Mock the api module
vi.mock("@/services/api/api", () => ({
  default: {
    post: vi.fn(),
  },
}));

describe("forgetPasswordRepository", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe("forgetPasswordRequest", () => {
    it("should successfully send password reset request", async () => {
      const mockResponse = {
        status: 200,
        data: {
          success: true,
          result: null,
          successMessage: "Reset code sent to your email",
        },
      };

      api.post.mockResolvedValue(mockResponse);

      const result = await forgetPasswordRepository.forgetPasswordRequest("test@example.com");

      expect(result.success).toBe(true);
      expect(result.successMessage).toBe("Reset code sent to your email");
      expect(api.post).toHaveBeenCalledWith("api/password/forgot-password", {
        email: "test@example.com",
      });
    });

    it("should use default success message when not provided", async () => {
      const mockResponse = {
        status: 200,
        data: {
          success: true,
          result: { data: "some data" },
        },
      };

      api.post.mockResolvedValue(mockResponse);

      const result = await forgetPasswordRepository.forgetPasswordRequest("test@example.com");

      expect(result.success).toBe(true);
      expect(result.successMessage).toBe("Reset code sent successfully");
    });

    it("should handle API error response with errors array", async () => {
      const mockResponse = {
        status: 400,
        data: {
          success: false,
          errors: [{ message: "Email not found" }],
          message: "Failed to send reset code",
        },
      };

      api.post.mockResolvedValue(mockResponse);

      const result = await forgetPasswordRepository.forgetPasswordRequest(
        "nonexistent@example.com"
      );

      expect(result.success).toBe(false);
      expect(result.errors).toEqual([{ message: "Email not found" }]);
    });

    it("should handle API error without errors array", async () => {
      const mockResponse = {
        status: 400,
        data: {
          success: false,
          message: "Bad request",
        },
      };

      api.post.mockResolvedValue(mockResponse);

      const result = await forgetPasswordRepository.forgetPasswordRequest("test@example.com");

      expect(result.success).toBe(false);
      expect(result.errors).toEqual([{ message: "Bad request" }]);
    });

    it("should handle network errors with response data", async () => {
      const mockError = {
        response: {
          status: 400,
          data: {
            message: "Server error",
            errors: [{ message: "Internal server error" }],
          },
        },
      };

      api.post.mockRejectedValue(mockError);

      const result = await forgetPasswordRepository.forgetPasswordRequest("test@example.com");

      expect(result.success).toBe(false);
      expect(result.errors).toEqual([{ message: "Internal server error" }]);
    });

    it("should handle network errors without response", async () => {
      const mockError = new Error("Network connection failed");

      api.post.mockRejectedValue(mockError);

      const result = await forgetPasswordRepository.forgetPasswordRequest("test@example.com");

      expect(result.success).toBe(false);
      expect(result.errors).toEqual([{ message: "Network connection failed" }]);
    });

    it("should handle errors without message property", async () => {
      const mockError = {};

      api.post.mockRejectedValue(mockError);

      const result = await forgetPasswordRepository.forgetPasswordRequest("test@example.com");

      expect(result.success).toBe(false);
      expect(result.errors).toEqual([
        { message: "An unexpected error occurred while requesting password reset." },
      ]);
    });

    it("should handle response with status 200 but success false", async () => {
      const mockResponse = {
        status: 200,
        data: {
          success: false,
          errors: [{ message: "Invalid request" }],
        },
      };

      api.post.mockResolvedValue(mockResponse);

      const result = await forgetPasswordRepository.forgetPasswordRequest("test@example.com");

      // With refactored code, status 200 AND success !== false logic correctly returns badRequest when success is false
      expect(result.success).toBe(false);
      expect(result.errors).toEqual([{ message: "Invalid request" }]);
    });

    it("should handle response errors that are not arrays", async () => {
      const mockError = {
        response: {
          status: 500,
          data: {
            errors: "Not an array",
          },
        },
      };

      api.post.mockRejectedValue(mockError);

      const result = await forgetPasswordRepository.forgetPasswordRequest("test@example.com");

      expect(result.success).toBe(false);
      expect(result.errors).toEqual([
        { message: "An unexpected error occurred while requesting password reset." },
      ]);
    });
  });
});