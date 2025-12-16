// useForgetPassword.test.js
import { describe, it, expect, vi, beforeEach } from "vitest";
import { useForgetPassword } from "@/composables/password/useForgetPassword";
import { forgetPasswordRepository } from "@/services/auth/passwordRepos/forgetPasswordRepository";

// Mock repository
vi.mock("@/services/auth/passwordRepos/forgetPasswordRepository", () => ({
  forgetPasswordRepository: {
    forgetPasswordRequest: vi.fn(),
  },
}));

// Mock validateEmail
vi.mock("@/services/validations/validateEmail", () => ({
  validateEmail: vi.fn(),
}));

import { validateEmail } from "@/services/validations/validateEmail";

describe("useForgetPassword", () => {
  beforeEach(() => {
    vi.clearAllMocks();
    validateEmail.mockReturnValue(true); // valid email
  });

  it("should export correct properties", () => {
    const { error, forgetPassword, resetState } = useForgetPassword();

    expect(error).toBeDefined();
    expect(typeof forgetPassword).toBe("function");
    expect(typeof resetState).toBe("function");
  });

  it("should handle successful password reset request", async () => {
    const mockResponse = {
      success: true,
      message: "Reset email sent successfully",
    };

    forgetPasswordRepository.forgetPasswordRequest.mockResolvedValue(mockResponse);

    const { forgetPassword } = useForgetPassword();
    const result = await forgetPassword("test@example.com");

    expect(result.success).toBe(true);
    expect(result.message).toBe("Reset email sent successfully");
    expect(forgetPasswordRepository.forgetPasswordRequest).toHaveBeenCalledWith("test@example.com");
    expect(forgetPasswordRepository.forgetPasswordRequest).toHaveBeenCalledTimes(1);
  });

  it("should handle failed password reset request with error array", async () => {
    const mockResponse = {
      success: false,
      errors: [{ message: "Email not found" }],
    };

    forgetPasswordRepository.forgetPasswordRequest.mockResolvedValue(mockResponse);

    const { forgetPassword } = useForgetPassword();
    const result = await forgetPassword("nonexistent@example.com");

    expect(result.success).toBe(false);
    expect(result.message).toBe("Email not found");
    expect(result.errors[0].message).toBe("Email not found");
  });

  it("should handle failed request without error details", async () => {
    forgetPasswordRepository.forgetPasswordRequest.mockResolvedValue({ success: false });

    const { forgetPassword } = useForgetPassword();
    const result = await forgetPassword("test@example.com");

    expect(result.success).toBe(false);
    expect(result.message).toBe("Forgot password failed");
  });

  it("should handle network errors", async () => {
    forgetPasswordRepository.forgetPasswordRequest.mockRejectedValue(new Error("Network error"));

    const { forgetPassword } = useForgetPassword();
    const result = await forgetPassword("test@example.com");

    expect(result.success).toBe(false);
    expect(result.message).toBe("Network error");
  });

  it("should handle errors without message", async () => {
    forgetPasswordRepository.forgetPasswordRequest.mockRejectedValue({});

    const { forgetPassword } = useForgetPassword();
    const result = await forgetPassword("test@example.com");

    expect(result.success).toBe(false);
    expect(result.message).toBe("Forgot password failed");
  });

  it("should use default success message when response has no message", async () => {
    forgetPasswordRepository.forgetPasswordRequest.mockResolvedValue({ success: true });

    const { forgetPassword } = useForgetPassword();
    const result = await forgetPassword("test@example.com");

    expect(result.success).toBe(true);
    expect(result.message).toBe("Password reset email sent successfully");
  });

  it("should reset error before making request", async () => {
    forgetPasswordRepository.forgetPasswordRequest.mockResolvedValue({
      success: true,
      message: "Success",
    });

    const { error, forgetPassword } = useForgetPassword();
    error.value = "Previous error";

    await forgetPassword("test@example.com");

    expect(error.value).toBeNull();
  });

  it("should fail when email is invalid", async () => {
    validateEmail.mockReturnValue(false);

    const { forgetPassword } = useForgetPassword();
    const result = await forgetPassword("invalid-email");

    expect(result.success).toBe(false);
    expect(result.message).toBe("Invalid email format");
  });
});
