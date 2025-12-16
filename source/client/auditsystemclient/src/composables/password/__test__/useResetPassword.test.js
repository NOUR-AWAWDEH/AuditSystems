import { describe, it, expect, vi, beforeEach } from "vitest";
import { useResetPassword } from "@/composables/password/useResetPassword";
import { resetPasswordRepository } from "@/services/auth/passwordRepos/resetPasswordRepository";
import { validateResetPasswordData } from "@/services/validations/validateResetPasswordData";

// Mock repository
vi.mock("@/services/auth/passwordRepos/resetPasswordRepository", () => ({
  resetPasswordRepository: {
    resetPasswordRequest: vi.fn(),
  },
}));

// Mock validation
vi.mock("@/services/validations/validateResetPasswordData", () => ({
  validateResetPasswordData: vi.fn(),
}));

describe("useResetPassword", () => {
  beforeEach(() => {
    vi.clearAllMocks();
    validateResetPasswordData.mockReturnValue(null); // valid payload
  });

  it("should export correct properties", () => {
    const { error, resetPassword, resetState } = useResetPassword();

    expect(error).toBeDefined();
    expect(typeof resetPassword).toBe("function");
    expect(typeof resetState).toBe("function");
  });

  it("should successfully reset password", async () => {
    resetPasswordRepository.resetPasswordRequest.mockResolvedValue({
      success: true,
      successMessage: "Password reset successfully",
    });

    const { resetPassword } = useResetPassword();

    const payload = {
      email: "test@example.com",
      token: "token",
      newPassword: "NewPass123!",
      confirmNewPassword: "NewPass123!",
    };

    const result = await resetPassword(payload);

    expect(result.success).toBe(true);
    expect(result.message).toBe("Password reset successfully");
    expect(resetPasswordRepository.resetPasswordRequest).toHaveBeenCalledWith(payload);
  });

  it("should handle reset failure with errors array", async () => {
    resetPasswordRepository.resetPasswordRequest.mockResolvedValue({
      success: false,
      errors: [{ message: "Invalid token" }],
    });

    const { resetPassword, error } = useResetPassword();

    const result = await resetPassword({
      email: "test@example.com",
      token: "bad-token",
      newPassword: "Pass123!",
      confirmNewPassword: "Pass123!",
    });

    expect(result.success).toBe(false);
    expect(result.message).toBe("Invalid token");
    expect(error.value).toBe("Invalid token");
    expect(result.errors).toHaveLength(1);
  });

  it("should handle validation failure", async () => {
    validateResetPasswordData.mockReturnValue("Passwords do not match");

    const { resetPassword } = useResetPassword();

    const result = await resetPassword({
      email: "test@example.com",
      token: "token",
      newPassword: "Pass1",
      confirmNewPassword: "Pass2",
    });

    expect(result.success).toBe(false);
    expect(result.message).toBe("Passwords do not match");
  });

  it("should handle network errors", async () => {
    resetPasswordRepository.resetPasswordRequest.mockRejectedValue(new Error("Connection timeout"));

    const { resetPassword } = useResetPassword();

    const result = await resetPassword({
      email: "test@example.com",
      token: "token",
      newPassword: "Pass123!",
      confirmNewPassword: "Pass123!",
    });

    expect(result.success).toBe(false);
    expect(result.message).toBe("Connection timeout");
  });

  it("should use default error message when none provided", async () => {
    resetPasswordRepository.resetPasswordRequest.mockResolvedValue({ success: false });

    const { resetPassword } = useResetPassword();

    const result = await resetPassword({
      email: "test@example.com",
      token: "token",
      newPassword: "Pass123!",
      confirmNewPassword: "Pass123!",
    });

    expect(result.message).toBe("Password reset failed");
  });

  it("should reset error before request", async () => {
    resetPasswordRepository.resetPasswordRequest.mockResolvedValue({ success: true });

    const { error, resetPassword } = useResetPassword();
    error.value = "Old error";

    await resetPassword({
      email: "test@example.com",
      token: "token",
      newPassword: "Pass123!",
      confirmNewPassword: "Pass123!",
    });

    expect(error.value).toBeNull();
  });

  it("should prioritize errors[0].message over successMessage", async () => {
    resetPasswordRepository.resetPasswordRequest.mockResolvedValue({
      success: false,
      errors: [{ message: "From errors array" }],
      successMessage: "Ignored",
    });

    const { resetPassword } = useResetPassword();

    const result = await resetPassword({
      email: "test@example.com",
      token: "token",
      newPassword: "Pass123!",
      confirmNewPassword: "Pass123!",
    });

    expect(result.message).toBe("From errors array");
  });
});
