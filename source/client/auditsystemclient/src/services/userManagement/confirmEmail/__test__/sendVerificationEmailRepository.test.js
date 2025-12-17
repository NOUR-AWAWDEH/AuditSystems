import { describe, it, expect, vi, beforeEach } from "vitest";
import { sendVerificationEmailRepository } from "../sendVerificationEmailRepository";
import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";

// Mock the api module
vi.mock("@/services/api/api", () => ({
  default: {
    post: vi.fn(),
  },
}));

describe("sendVerificationEmailRepository", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  const email = "test@example.com";

  it("should send verification email successfully", async () => {
    const mockResponse = {
      status: 200,
      data: {
        success: true,
        successMessage: "Verification Email sent successfully",
        result: { someData: true },
      },
    };

    api.post.mockResolvedValue(mockResponse);

    const result = await sendVerificationEmailRepository.sendVerificationEmailRepository(email);

    expect(api.post).toHaveBeenCalledWith("/api/Registration/send-verification-email", {
      email: email,
    });
    expect(result).toEqual(
      ApiResponseT.ok({ someData: true }, "Verification Email sent successfully")
    );
  });

  it("should handle failed response (200 OK but success: false)", async () => {
    const mockResponse = {
      status: 200,
      data: {
        success: false,
        message: "Failed to send verification email.",
        errors: [{ message: "Some error" }],
      },
    };

    api.post.mockResolvedValue(mockResponse);

    const result = await sendVerificationEmailRepository.sendVerificationEmailRepository(email);

    expect(result).toEqual(
      ApiResponseT.badRequest([{ message: "Some error" }], "Failed to send verification email.")
    );
  });

  it("should handle 404 Not Found error", async () => {
    const mockError = {
      response: {
        status: 404,
        data: {},
      },
    };

    api.post.mockRejectedValue(mockError);

    const result = await sendVerificationEmailRepository.sendVerificationEmailRepository(email);

    expect(result).toEqual(
      ApiResponseT.notFound([{ message: "email not found" }], "email not found")
    );
  });

  it("should handle 400 Bad Request error", async () => {
    const mockError = {
      response: {
        status: 400,
        data: {
          message: "Bad request",
          errors: [{ message: "Invalid email" }],
        },
      },
    };

    api.post.mockRejectedValue(mockError);

    const result = await sendVerificationEmailRepository.sendVerificationEmailRepository(email);

    expect(result).toEqual(ApiResponseT.badRequest([{ message: "Invalid email" }], "Bad request"));
  });

  it("should handle 500 Server Error", async () => {
    const mockError = {
      response: {
        status: 500,
        data: { message: "Internal Server Error" }, // The repo code doesn't explicitly check 500 response data structure, it falls to generic error
      },
      message: "An unexpected error occurred while sending verification email.",
    };

    api.post.mockRejectedValue(mockError);

    const result = await sendVerificationEmailRepository.sendVerificationEmailRepository(email);

    expect(result).toEqual(
      ApiResponseT.serverError(
        [{ message: "An unexpected error occurred while sending verification email." }],
        "Server error"
      )
    );
  });

  it("should handle network error (no response)", async () => {
    const mockError = {
      message: "Network Error",
    };

    api.post.mockRejectedValue(mockError);

    const result = await sendVerificationEmailRepository.sendVerificationEmailRepository(email);

    expect(result).toEqual(
      ApiResponseT.serverError([{ message: "Network Error" }], "Server error")
    );
  });
});