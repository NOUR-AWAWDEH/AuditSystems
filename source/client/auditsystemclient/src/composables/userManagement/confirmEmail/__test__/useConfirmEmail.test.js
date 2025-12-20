import { describe, it, expect, vi, beforeEach } from "vitest";
import { useConfirmEmail } from "../useConfirmEmail";
import { confirmEmailRepository } from "@/services/userManagement/confirmEmail/confirmEmailRepository";

// Mock the repository
vi.mock("@/services/userManagement/confirmEmail/confirmEmailRepository", () => ({
  confirmEmailRepository: {
    confirmEmailRepository: vi.fn(),
  },
}));

describe("useConfirmEmail", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  const uid = "user-id-123";
  const token = "token-abc-456";

  it("should initialize with default states", () => {
    const { isLoading, error, success } = useConfirmEmail();

    expect(isLoading.value).toBe(false);
    expect(error.value).toBeNull();
    expect(success.value).toBeNull();
  });

  it("should validate missing inputs", async () => {
    const { confirmEmail, error } = useConfirmEmail();

    await confirmEmail(null, null);

    expect(error.value).toBe("User ID and token are required");
    expect(confirmEmailRepository.confirmEmailRepository).not.toHaveBeenCalled();
  });

  it("should confirm email successfully", async () => {
    vi.useFakeTimers();
    const { confirmEmail, isLoading, success, error } = useConfirmEmail();

    const mockResponse = {
      success: true,
      successMessage: "Email confirmed successfully",
    };

    // Return a promise that resolves only after a timer advances
    confirmEmailRepository.confirmEmailRepository.mockImplementation(
      () => new Promise((resolve) => setTimeout(() => resolve(mockResponse), 100))
    );

    const promise = confirmEmail(uid, token);

    // Should be loading now
    expect(isLoading.value).toBe(true);

    // Fast-forward time to trigger resolution
    vi.runAllTimers();

    // Await the promise to ensure completion handling runs
    await promise;

    expect(isLoading.value).toBe(false);
    expect(error.value).toBeNull();
    expect(success.value).toEqual({
      success: true,
      message: "Email confirmed successfully",
    });
    expect(confirmEmailRepository.confirmEmailRepository).toHaveBeenCalledWith(uid, token);

    vi.useRealTimers();
  });

  it("should handle repository failure", async () => {
    const { confirmEmail, isLoading, error, success } = useConfirmEmail();

    const mockResponse = {
      success: false,
      errors: [{ message: "Invalid Token" }],
    };

    confirmEmailRepository.confirmEmailRepository.mockResolvedValue(mockResponse);

    await confirmEmail(uid, token);

    expect(isLoading.value).toBe(false);
    expect(success.value).toBeNull();
    expect(error.value).toBe("Invalid Token");
  });

  it("should handle repository failure with default message", async () => {
    const { confirmEmail, isLoading, error } = useConfirmEmail();

    const mockResponse = {
      success: false,
      message: "Something went wrong",
    };

    confirmEmailRepository.confirmEmailRepository.mockResolvedValue(mockResponse);

    await confirmEmail(uid, token);

    expect(isLoading.value).toBe(false);
    expect(error.value).toBe("Something went wrong");
  });

  it("should handle exceptions", async () => {
    const { confirmEmail, isLoading, error, success } = useConfirmEmail();

    const mockError = new Error("Network Error");

    confirmEmailRepository.confirmEmailRepository.mockRejectedValue(mockError);

    await confirmEmail(uid, token);

    expect(isLoading.value).toBe(false);
    expect(success.value).toBeNull();
    expect(error.value).toBe("Network Error");
  });

  it("should handle exceptions with fallback message", async () => {
    const { confirmEmail, isLoading, error } = useConfirmEmail();

    const mockError = {};

    confirmEmailRepository.confirmEmailRepository.mockRejectedValue(mockError);

    await confirmEmail(uid, token);

    expect(isLoading.value).toBe(false);
    expect(error.value).toBe("Failed to confirm email");
  });
});
