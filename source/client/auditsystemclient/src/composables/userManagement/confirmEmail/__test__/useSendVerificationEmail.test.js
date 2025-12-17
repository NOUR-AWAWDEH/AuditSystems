import { describe, it, expect, vi, beforeEach } from "vitest";
import { useVerificationEmail } from "../useSendVerificationEmail";
import { sendVerificationEmailRepository } from "@/services/userManagement/confirmEmail/sendVerificationEmailRepository";
import { validateEmail } from "@/services/validations/validateEmail";

// Mock dependencies
vi.mock("@/services/userManagement/confirmEmail/sendVerificationEmailRepository", () => ({
  sendVerificationEmailRepository: {
    sendVerificationEmailRepository: vi.fn(),
  },
}));

vi.mock("@/services/validations/validateEmail", () => ({
  validateEmail: vi.fn(),
}));

describe("useVerificationEmail", () => {
  let composable;

  beforeEach(() => {
    vi.clearAllMocks();
    composable = useVerificationEmail();
  });

  it("should have initial state", () => {
    expect(composable.isLoading.value).toBe(false);
    expect(composable.error.value).toBe(null);
    expect(composable.success.value).toBe(null);
  });

  it("should validate email successfully", async () => {
    const email = "test@example.com";
    validateEmail.mockReturnValue(true);
    sendVerificationEmailRepository.sendVerificationEmailRepository.mockResolvedValue({
      success: true,
      successMessage: "Email sent",
    });

    await composable.sendVerificationEmail(email);

    expect(validateEmail).toHaveBeenCalledWith(email);
    expect(composable.error.value).toBe(null);
  });

  it("should handle email validation failure", async () => {
    const email = "invalid-email";
    validateEmail.mockReturnValue(false);

    await composable.sendVerificationEmail(email);

    expect(composable.error.value).toBe("Invalid email format");
  });

  it("should handle successful email sending", async () => {
    const email = "test@example.com";
    validateEmail.mockReturnValue(true);
    const mockResponse = {
      success: true,
      successMessage: "Verification link sent.",
    };
    sendVerificationEmailRepository.sendVerificationEmailRepository.mockResolvedValue(mockResponse);

    await composable.sendVerificationEmail(email);

    expect(composable.isLoading.value).toBe(false);
    expect(composable.success.value).toEqual({
      success: true,
      message: "Verification link sent.",
    });
    expect(composable.error.value).toBe(null);
  });

  it("should handle repository failure", async () => {
    const email = "test@example.com";
    validateEmail.mockReturnValue(true);
    const mockResponse = {
      success: false,
      message: "Repo failed",
      errors: [{ message: "Repo failed" }],
    };
    sendVerificationEmailRepository.sendVerificationEmailRepository.mockResolvedValue(mockResponse);

    await composable.sendVerificationEmail(email);

    expect(composable.isLoading.value).toBe(false);
    expect(composable.success.value).toBe(null);
    expect(composable.error.value).toBe("Repo failed");
  });

  it("should handle exception during sending", async () => {
    const email = "test@example.com";
    validateEmail.mockReturnValue(true);
    const mockError = new Error("Network error");
    sendVerificationEmailRepository.sendVerificationEmailRepository.mockRejectedValue(mockError);

    await composable.sendVerificationEmail(email);

    expect(composable.isLoading.value).toBe(false);
    expect(composable.error.value).toBe("Network error");
  });
});