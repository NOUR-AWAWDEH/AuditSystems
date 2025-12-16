// resetPasswordRepository.test.js
import { describe, it, expect, vi, beforeEach } from "vitest";
import { resetPasswordRepository } from "@/services/auth/passwordRepos/resetPasswordRepository";
import api from "@/services/api/api";

// Mock the api module
vi.mock("@/services/api/api", () => ({
  default: {
    post: vi.fn(),
  },
}));

describe("resetPasswordRepository", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it("should successfully reset password", async () => {
    const mockResponse = {
      data: {
        success: true,
        successMessage: "Password has been reset successfully",
      },
      status: 200,
    };

    api.post.mockResolvedValue(mockResponse);

    const result = await resetPasswordRepository.resetPasswordRequest({
      email: "test@example.com",
      token: "valid-token",
      newPassword: "NewPass123!",
      confirmNewPassword: "NewPass123!",
    });

    expect(result.success).toBe(true);
    expect(result.successMessage).toBe("Password has been reset successfully");
    expect(api.post).toHaveBeenCalledWith("/api/password/reset-password", {
      email: "test@example.com",
      token: "valid-token",
      newPassword: "NewPass123!",
      confirmNewPassword: "NewPass123!",
    });
  });

  it("should handle invalid token error", async () => {
    const mockResponse = {
      data: {
        success: false,
        errors: [{ message: "Invalid or expired token" }],
        successMessage: "Reset failed",
      },
      status: 200,
    };

    api.post.mockResolvedValue(mockResponse);

    const result = await resetPasswordRepository.resetPasswordRequest({
      email: "test@example.com",
      token: "invalid-token",
      newPassword: "NewPass123!",
      confirmNewPassword: "NewPass123!",
    });

    expect(result.success).toBe(false);
    expect(result.errors).toEqual([{ message: "Invalid or expired token" }]);
  });

  it("should handle password mismatch error", async () => {
    const mockResponse = {
      data: {
        success: false,
        errors: [{ message: "Passwords do not match" }],
      },
      status: 200,
    };

    api.post.mockResolvedValue(mockResponse);

    const result = await resetPasswordRepository.resetPasswordRequest({
      email: "test@example.com",
      token: "token",
      newPassword: "Pass1",
      confirmNewPassword: "Pass2",
    });

    expect(result.success).toBe(false);
    expect(result.errors[0].message).toBe("Passwords do not match");
  });

  it("should handle weak password errors", async () => {
    const mockResponse = {
      data: {
        success: false,
        errors: [{ message: "Password does not meet requirements" }],
      },
      status: 200,
    };

    api.post.mockResolvedValue(mockResponse);

    const result = await resetPasswordRepository.resetPasswordRequest({
      email: "test@example.com",
      token: "token",
      newPassword: "weak",
      confirmNewPassword: "weak",
    });

    expect(result.success).toBe(false);
    expect(result.errors[0].message).toBe("Password does not meet requirements");
  });

  it("should use default error message when no message provided", async () => {
    const mockResponse = {
      data: {
        success: false,
      },
      status: 200,
    };

    api.post.mockResolvedValue(mockResponse);

    const result = await resetPasswordRepository.resetPasswordRequest({
      email: "test@example.com",
      token: "token",
      newPassword: "Pass123!",
      confirmNewPassword: "Pass123!",
    });

    expect(result.success).toBe(false);
    expect(result.errors).toEqual([{ message: "Failed to reset Password." }]);
  });

  it("should handle network errors with response", async () => {
    const mockError = {
      response: {
        status: 400,
        message: "Server error",
        errors: [{ message: "Internal server error" }],
      },
    };

    api.post.mockRejectedValue(mockError);

    const result = await resetPasswordRepository.resetPasswordRequest({
      email: "test@example.com",
      token: "token",
      newPassword: "Pass123!",
      confirmNewPassword: "Pass123!",
    });

    expect(result.success).toBe(false);
    expect(result.errors).toEqual([{ message: "Internal server error" }]);
  });

  it("should handle network errors without response", async () => {
    const mockError = new Error("Connection refused");

    api.post.mockRejectedValue(mockError);

    const result = await resetPasswordRepository.resetPasswordRequest({
      email: "test@example.com",
      token: "token",
      newPassword: "Pass123!",
      confirmNewPassword: "Pass123!",
    });

    expect(result.success).toBe(false);
    expect(result.errors).toEqual([{ message: "Connection refused" }]);
  });

  it("should handle empty error object", async () => {
    const mockError = {};

    api.post.mockRejectedValue(mockError);

    const result = await resetPasswordRepository.resetPasswordRequest({
      email: "test@example.com",
      token: "token",
      newPassword: "Pass123!",
      confirmNewPassword: "Pass123!",
    });

    expect(result.success).toBe(false);
    expect(result.errors).toEqual([
      { message: "An unexpected error occurred while resetting Password." },
    ]);
  });

  it("should handle non-array errors in response", async () => {
    const mockResponse = {
      data: {
        success: false,
        errors: "Not an array",
        successMessage: "Something went wrong",
      },
      status: 200,
    };

    api.post.mockResolvedValue(mockResponse);

    const result = await resetPasswordRepository.resetPasswordRequest({
      email: "test@example.com",
      token: "token",
      newPassword: "Pass123!",
      confirmNewPassword: "Pass123!",
    });

    expect(result.errors).toEqual([{ message: "Not an array" }]);
  });

  it("should handle non-array errors in error response", async () => {
    const mockError = {
      response: {
        status: 400,
        errors: "Still not an array",
        message: "Bad request",
      },
      status: 400,
    };

    api.post.mockRejectedValue(mockError);

    const result = await resetPasswordRepository.resetPasswordRequest({
      email: "test@example.com",
      token: "token",
      newPassword: "Pass123!",
      confirmNewPassword: "Pass123!",
    });

    expect(result.errors).toEqual([{ message: "Still not an array" }]);
  });

  it("should return null result on successful reset", async () => {
    const mockResponse = {
      data: {
        success: true,
        successMessage: "Password reset",
      },
      status: 200,
    };

    api.post.mockResolvedValue(mockResponse);

    const result = await resetPasswordRepository.resetPasswordRequest({
      email: "test@example.com",
      token: "token",
      newPassword: "Pass123!",
      confirmNewPassword: "Pass123!",
    });

    expect(result.result).toBeNull();
  });
});
