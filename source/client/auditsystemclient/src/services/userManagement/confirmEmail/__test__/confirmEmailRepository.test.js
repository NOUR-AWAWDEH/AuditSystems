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
        result: { someData: true },
      },
    };

    api.post.mockImplementation((url, data) => {
      console.log("DEBUG: Mock POST called with", url, data);
      return Promise.resolve(mockResponse);
    });
    api.get.mockImplementation((url) => {
      console.log("DEBUG: Mock GET called with", url);
      return Promise.resolve(mockResponse);
    });

    const uid = "user-id-123";
    const token = "token-abc-456";

    const result = await confirmEmailRepository.confirmEmailRepository(uid, token);

    expect(api.get).toHaveBeenCalledWith(
      "/api/Registration/confirm-email?uid=user-id-123&token=token-abc-456"
    );
    expect(result).toEqual(ApiResponseT.ok({ someData: true }, "Email confirmed successfully"));
  });

  it("should handle failed confirmation (200 OK but success: false)", async () => {
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

    expect(result).toEqual(
      ApiResponseT.badRequest([{ message: "Invalid Token" }], "Failed to confirm email")
    );
  });

  it("should handle bad request (400)", async () => {
    const mockError = {
      response: {
        status: 400,
        data: {
          successMessage: "Bad request error",
          errors: [{ message: "Validation error" }],
        },
      },
    };

    api.get.mockRejectedValue(mockError);

    const result = await confirmEmailRepository.confirmEmailRepository(uid, token);

    expect(result).toEqual(
      ApiResponseT.badRequest([{ message: "Validation error" }], "Bad request error")
    );
  });

  it("should handle bad request (400) with default messages", async () => {
    const mockError = {
      response: {
        status: 400,
        data: {},
      },
    };

    api.get.mockRejectedValue(mockError);

    const result = await confirmEmailRepository.confirmEmailRepository(uid, token);

    expect(result).toEqual(ApiResponseT.badRequest([{ message: "Bad request" }], "Bad request"));
  });

  it("should handle server error (500)", async () => {
    const mockError = {
      message: "Network Error",
      response: {
        status: 500,
      },
    };

    api.get.mockRejectedValue(mockError);

    const result = await confirmEmailRepository.confirmEmailRepository(uid, token);

    expect(result).toEqual(
      ApiResponseT.serverError([{ message: "Network Error" }], "Server error")
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
