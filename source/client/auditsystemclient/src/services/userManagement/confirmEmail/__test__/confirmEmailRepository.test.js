import { describe, it, expect, vi, beforeEach } from "vitest";
import { confirmEmailRepository } from "../confirmEmailRepository";
import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";

// Mock the api module
vi.mock("@/services/api/api", () => ({
  default: {
    post: vi.fn(),
    get: vi.fn(),
  },
}));

describe("confirmEmailRepository", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  const uid = "user-id-123";
  const token = "token-abc-456";

  it("should confirm email successfully", async () => {
    const mockResponse = {
      status: 200,
      data: {
        success: true,
        successMessage: "Email confirmed successfully",
        result: {},
      },
    };

    api.get.mockResolvedValue(mockResponse);

    const result = await confirmEmailRepository.confirmEmailRepository(uid, token);

    expect(api.get).toHaveBeenCalledWith("/api/Registration/confirm-email", {
      params: { uid, token },
    });
    expect(result).toEqual(ApiResponseT.ok({}, "Email confirmed successfully"));
  });

  it("should handle failed confirmation (200 OK but success: false - not expected with current backend but handling safe fallback)", async () => {
    const mockResponse = {
      status: 200,
      data: {
        success: false,
        successMessage: "Failed to confirm email",
        errors: [{ message: "Invalid Token" }],
      },
    };

    api.get.mockResolvedValue(mockResponse);

    const result = await confirmEmailRepository.confirmEmailRepository(uid, token);
    expect(result).toEqual(ApiResponseT.ok({}, "Failed to confirm email"));
  });

  it("should handle bad request (400) string response", async () => {
    const mockError = {
      response: {
        status: 400,
        data: "Invalid token", // Backend behavior
      },
    };

    api.get.mockRejectedValue(mockError);

    const result = await confirmEmailRepository.confirmEmailRepository(uid, token);

    expect(result).toEqual(
      ApiResponseT.badRequest([{ message: "Invalid token" }], "Invalid token")
    );
  });

  it("should handle bad request (400) object response", async () => {
    const mockError = {
      response: {
        status: 400,
        data: { message: "Missing uid" }, // Object behavior fallback
      },
    };

    api.get.mockRejectedValue(mockError);

    const result = await confirmEmailRepository.confirmEmailRepository(uid, token);

    expect(result).toEqual(ApiResponseT.badRequest([{ message: "Missing uid" }], "Missing uid"));
  });

  it("should handle server error (500) string response", async () => {
    const mockError = {
      response: {
        status: 500,
        data: "An error occurred",
      },
    };

    api.get.mockRejectedValue(mockError);

    const result = await confirmEmailRepository.confirmEmailRepository(uid, token);

    expect(result).toEqual(
      ApiResponseT.serverError([{ message: "An error occurred" }], "An error occurred")
    );
  });

  it("should handle unexpected error without response (e.g. network failure)", async () => {
    const mockError = {
      message: "Network Error",
    };

    api.get.mockRejectedValue(mockError);

    const result = await confirmEmailRepository.confirmEmailRepository(uid, token);

    expect(result).toEqual(
      ApiResponseT.serverError([{ message: "Network Error" }], "Server error")
    );
  });

  it("should handle unexpected error without message", async () => {
    const mockError = {};

    api.get.mockRejectedValue(mockError);

    const result = await confirmEmailRepository.confirmEmailRepository(uid, token);

    expect(result).toEqual(
      ApiResponseT.serverError(
        [{ message: "An unexpected error occurred while confirming email." }],
        "Server error"
      )
    );
  });
});
