// validateResetPasswordData.test.js
import { describe, it, expect, vi } from "vitest";
import { validateResetPasswordData } from "@/services/validations/validateResetPasswordData";
import validatePassword from "@/services/validations/validatePassword";

// mock validatePassword
vi.mock("@/services/validations/validatePassword", () => ({
  default: vi.fn(),
}));

describe("validateResetPasswordData", () => {
  it("returns error when payload is invalid", () => {
    expect(validateResetPasswordData(null)).toBe("Invalid reset password payload");
    expect(validateResetPasswordData("test")).toBe("Invalid reset password payload");
  });

  it("returns error when email is missing or invalid", () => {
    const result = validateResetPasswordData({
      token: "token",
      newPassword: "Password1!",
      confirmNewPassword: "Password1!",
    });

    expect(result).toBe("Email is required");
  });

  it("returns error when token is missing or invalid", () => {
    const result = validateResetPasswordData({
      email: "test@example.com",
      newPassword: "Password1!",
      confirmNewPassword: "Password1!",
    });

    expect(result).toBe("Reset token is required");
  });

  it("returns password validation error", () => {
    validatePassword.mockReturnValue("Password must be at least 8 characters");

    const result = validateResetPasswordData({
      email: "test@example.com",
      token: "token",
      newPassword: "short",
      confirmNewPassword: "short",
    });

    expect(validatePassword).toHaveBeenCalledWith("short");
    expect(result).toBe("Password must be at least 8 characters");
  });

  it("returns error when confirm password is missing", () => {
    validatePassword.mockReturnValue(null);

    const result = validateResetPasswordData({
      email: "test@example.com",
      token: "token",
      newPassword: "Password1!",
    });

    expect(result).toBe("Confirm password is required");
  });

  it("returns error when passwords do not match", () => {
    validatePassword.mockReturnValue(null);

    const result = validateResetPasswordData({
      email: "test@example.com",
      token: "token",
      newPassword: "Password1!",
      confirmNewPassword: "Password2!",
    });

    expect(result).toBe("Passwords do not match");
  });

  it("returns null when data is valid", () => {
    validatePassword.mockReturnValue(null);

    const result = validateResetPasswordData({
      email: "test@example.com",
      token: "token",
      newPassword: "Password1!",
      confirmNewPassword: "Password1!",
    });

    expect(result).toBeNull();
  });
});