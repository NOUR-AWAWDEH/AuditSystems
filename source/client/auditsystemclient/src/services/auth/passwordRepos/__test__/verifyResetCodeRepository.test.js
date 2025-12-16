// verifyResetCodeRepository.test.js
import { describe, it, expect, vi, beforeEach } from "vitest";
import verifyResetCode from "@/services/auth/passwordRepos/verifyResetCodeRepository";
import api from "@/services/api/api";

// Mock the api module
vi.mock("@/services/api/api", () => ({
  default: {
    post: vi.fn(),
  },
}));

describe("verifyResetCodeRepository", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it("should successfully verify reset code", async () => {
    const mockResponse = {
      data: {
        success: true,
        result: {
          resetToken: "valid-token-123",
          successMessage: "Code verified",
        },
        successMessage: "Verification successful",
      },
    };

    api.post.mockResolvedValue(mockResponse);

    const result = await verifyResetCode("test@example.com", "123456");

    expect(result.success).toBe(true);
    expect(result.result).toEqual(mockResponse.data.result);
    expect(result.successMessage).toBe("Verification successful");
    expect(api.post).toHaveBeenCalledWith("/api/password/verify-reset-code", {
      email: "test@example.com",
      code: "123456",
    });
  });

  it("should handle invalid verification code", async () => {
    const mockResponse = {
      data: {
        success: false,
        errors: [{ message: "Invalid or expired code" }],
        message: "Verification failed",
      },
    };

    api.post.mockResolvedValue(mockResponse);

    const result = await verifyResetCode("test@example.com", "wrong-code");

    expect(result.success).toBe(false);
    expect(result.errors).toEqual([{ message: "Invalid or expired code" }]);
  });

  it("should use default error message when errors array not provided", async () => {
    const mockResponse = {
      data: {
        success: false,
        message: "Custom error message",
      },
    };

    api.post.mockResolvedValue(mockResponse);

    const result = await verifyResetCode("test@example.com", "code");

    expect(result.success).toBe(false);
    expect(result.errors).toEqual([{ message: "Custom error message" }]);
  });

  it("should use fallback error when no message provided", async () => {
    const mockResponse = {
      data: {
        success: false,
      },
    };

    api.post.mockResolvedValue(mockResponse);

    const result = await verifyResetCode("test@example.com", "code");

    expect(result.success).toBe(false);
    expect(result.errors).toEqual([{ message: "Verification failed" }]);
  });

  it("should handle network errors with error response data", async () => {
    const mockError = {
      response: {
        data: {
          successMessage: "Server error",
          errors: [{ message: "Internal server error" }],
        },
        status: 500,
      },
    };

    api.post.mockRejectedValue(mockError);

    const result = await verifyResetCode("test@example.com", "code");

    expect(result.success).toBe(false);
    expect(result.errors).toEqual([{ message: "Internal server error" }]);
  });

  it("should handle network errors without response data", async () => {
    const mockError = new Error("Network timeout");

    api.post.mockRejectedValue(mockError);

    const result = await verifyResetCode("test@example.com", "code");

    expect(result.success).toBe(false);
    expect(result.errors).toEqual([{ message: "Network timeout" }]);
  });

  it("should handle empty error object", async () => {
    const mockError = {};

    api.post.mockRejectedValue(mockError);

    const result = await verifyResetCode("test@example.com", "code");

    expect(result.success).toBe(false);
    expect(result.errors).toEqual([{ message: "Verification failed" }]);
  });

  it("should prioritize successMessage over message in error response", async () => {
    const mockError = {
      response: {
        data: {
          successMessage: "From successMessage",
          message: "From message",
        },
      },
    };

    api.post.mockRejectedValue(mockError);

    const result = await verifyResetCode("test@example.com", "code");

    expect(result.errors).toEqual([{ message: "From successMessage" }]);
  });

  it("should handle non-array errors in error response", async () => {
    const mockError = {
      response: {
        data: {
          errors: "Not an array",
          successMessage: "Error occurred",
        },
      },
    };

    api.post.mockRejectedValue(mockError);

    const result = await verifyResetCode("test@example.com", "code");

    expect(result.errors).toEqual([{ message: "Error occurred" }]);
  });

  it("should return result data on successful verification", async () => {
    const mockResponse = {
      data: {
        success: true,
        result: {
          resetToken: "token-abc",
          userId: "123",
          expiresAt: "2025-12-16T00:00:00Z",
        },
        successMessage: "Success",
      },
    };

    api.post.mockResolvedValue(mockResponse);

    const result = await verifyResetCode("test@example.com", "123456");

    expect(result.result).toEqual({
      resetToken: "token-abc",
      userId: "123",
      expiresAt: "2025-12-16T00:00:00Z",
    });
  });
});
