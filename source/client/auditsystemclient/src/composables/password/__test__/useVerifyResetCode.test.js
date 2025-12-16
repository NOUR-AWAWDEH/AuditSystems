// useVerifyResetCode.test.js
import { describe, it, expect, vi, beforeEach } from "vitest";
import { useVerifyResetCode } from "@/composables/password/useVerifyResetCode";
import verifyResetCode from "@/services/auth/passwordRepos/verifyResetCodeRepository";

// Mock the repository
vi.mock("@/services/auth/passwordRepos/verifyResetCodeRepository", () => ({
  default: vi.fn(),
}));

describe("useVerifyResetCode", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it("should export correct properties", () => {
    const { isVerified, error, verifyCode, verifyToken } = useVerifyResetCode();

    expect(isVerified).toBeDefined();
    expect(error).toBeDefined();
    expect(typeof verifyCode).toBe("function");
    expect(typeof verifyToken).toBe("function");
  });

  it("should successfully verify a valid code", async () => {
    const mockResponse = {
      success: true,
      result: {
        resetToken: "valid-token-123",
        successMessage: "Code verified",
      },
      successMessage: "Verification successful",
    };

    verifyResetCode.mockResolvedValue(mockResponse);

    const { isVerified, verifyCode: verify } = useVerifyResetCode();
    const result = await verify("test@example.com", "123456");

    expect(result.success).toBe(true);
    expect(result.message).toBe("Verification successful");
    expect(result.data).toEqual(mockResponse.result);
    expect(isVerified.value).toBe(true);
    expect(verifyResetCode).toHaveBeenCalledWith("test@example.com", "123456");
  });

  it("should handle invalid verification code", async () => {
    const mockResponse = {
      success: false,
      errors: [{ message: "Invalid or expired code" }],
    };

    verifyResetCode.mockResolvedValue(mockResponse);

    const { isVerified, error, verifyCode: verify } = useVerifyResetCode();
    const result = await verify("test@example.com", "wrong-code");

    expect(result.success).toBe(false);
    expect(result.message).toBe("Invalid or expired code");
    expect(isVerified.value).toBe(false);
    expect(error.value).toBe("Invalid or expired code");
  });

  it("should use default error message when errors array is empty", async () => {
    const mockResponse = {
      success: false,
      errors: [],
    };

    verifyResetCode.mockResolvedValue(mockResponse);

    const { verifyCode: verify } = useVerifyResetCode();
    const result = await verify("test@example.com", "code");

    expect(result.success).toBe(false);
    expect(result.message).toBe("Verification failed");
  });

  it("should handle network errors", async () => {
    const mockError = new Error("Network error");
    verifyResetCode.mockRejectedValue(mockError);

    const { verifyCode: verify } = useVerifyResetCode();
    const result = await verify("test@example.com", "123456");

    expect(result.success).toBe(false);
    expect(result.message).toBe("Network error");
  });

  it("should handle errors without message property", async () => {
    verifyResetCode.mockRejectedValue({});

    const { verifyCode: verify } = useVerifyResetCode();
    const result = await verify("test@example.com", "123456");

    expect(result.success).toBe(false);
    expect(result.message).toBe("Verification failed");
  });

  it("should reset error value before verification", async () => {
    const mockResponse = {
      success: true,
      result: { resetToken: "token" },
      successMessage: "Success",
    };

    verifyResetCode.mockResolvedValue(mockResponse);

    const { error, verifyCode: verify } = useVerifyResetCode();

    // Set error manually
    error.value = "Previous error";

    await verify("test@example.com", "123456");

    expect(error.value).toBeNull();
  });

  it("should use fallback success message when not provided", async () => {
    const mockResponse = {
      success: true,
      result: {},
    };

    verifyResetCode.mockResolvedValue(mockResponse);

    const { verifyCode: verify } = useVerifyResetCode();
    const result = await verify("test@example.com", "123456");

    expect(result.success).toBe(true);
    expect(result.message).toBe("Verification successful");
  });

  it("should prioritize successMessage over result.successMessage", async () => {
    const mockResponse = {
      success: true,
      result: {
        successMessage: "From result",
      },
      successMessage: "From response",
    };

    verifyResetCode.mockResolvedValue(mockResponse);

    const { verifyCode: verify } = useVerifyResetCode();
    const result = await verify("test@example.com", "123456");

    expect(result.message).toBe("From response");
  });

  it("should return errors array on failure", async () => {
    const mockResponse = {
      success: false,
      errors: [{ message: "Error 1" }, { message: "Error 2" }],
    };

    verifyResetCode.mockResolvedValue(mockResponse);

    const { verifyCode: verify } = useVerifyResetCode();
    const result = await verify("test@example.com", "code");

    expect(result.errors).toHaveLength(2);
    expect(result.errors[0].message).toBe("Error 1");
    expect(result.errors[1].message).toBe("Error 2");
  });

  describe("verifyToken", () => {
    it("should successfully verify a valid token", async () => {
      const mockResponse = {
        success: true,
      };

      verifyResetCode.mockResolvedValue(mockResponse);

      const { isVerified, verifyToken } = useVerifyResetCode();
      const result = await verifyToken("test@example.com", "valid-token");

      expect(result.success).toBe(true);
      expect(isVerified.value).toBe(true);
      expect(verifyResetCode).toHaveBeenCalledWith("test@example.com", "valid-token");
    });

    it("should handle invalid token", async () => {
      const mockResponse = {
        success: false,
        errors: [{ message: "Invalid token" }],
        successMessage: "Token verification failed",
      };

      verifyResetCode.mockResolvedValue(mockResponse);

      const { isVerified, error, verifyToken } = useVerifyResetCode();
      const result = await verifyToken("test@example.com", "invalid-token");

      expect(result.success).toBe(false);
      expect(result.message).toBe("Invalid token");
      expect(isVerified.value).toBe(false);
      expect(error.value).toBe("Invalid token");
    });

    it("should handle verification errors", async () => {
      const mockError = new Error("Network error");
      verifyResetCode.mockRejectedValue(mockError);

      const { verifyToken } = useVerifyResetCode();
      const result = await verifyToken("test@example.com", "token");

      expect(result.success).toBe(false);
      expect(result.message).toBe("Network error");
    });

    it("should reset error before verification", async () => {
      const mockResponse = { success: true };
      verifyResetCode.mockResolvedValue(mockResponse);

      const { error, verifyToken } = useVerifyResetCode();
      error.value = "Previous error";

      await verifyToken("test@example.com", "token");

      expect(error.value).toBeNull();
    });

    it("should fallback to successMessage when errors array is empty", async () => {
      const mockResponse = {
        success: false,
        errors: [],
        successMessage: "Token expired",
      };

      verifyResetCode.mockResolvedValue(mockResponse);

      const { verifyToken } = useVerifyResetCode();
      const result = await verifyToken("test@example.com", "token");

      expect(result.message).toBe("Token expired");
    });
  });
});
