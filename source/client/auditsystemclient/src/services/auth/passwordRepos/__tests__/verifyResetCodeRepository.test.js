import { describe, it, expect, vi, beforeEach } from "vitest";
import verifyResetCode from "../verifyResetCodeRepository";
import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";

// Mock the api module
vi.mock("@/services/api/api", () => ({
  default: {
    post: vi.fn(),
  },
}));

describe("verifyResetCodeRepository", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it("should return success response when API call is successful", async () => {
    // Arrange
    const email = "test@example.com";
    const code = "123456";
    const mockResponse = {
      status: 200,
      data: {
        success: true,
        result: true, // Assuming the result is boolean purely based on "verify" nature, but adapting to whatever the structure is.
        successMessage: "Code verified successfully",
      },
    };

    // The current implementation expects response.success directly, which matches how some axios interceptors might work
    // IF they unwrap the response. However, standard axios returns { data: ... }.
    // Based on searchRatesRepository, the codebase seems to expect `response.data`.
    // We will simulate a standard axios response here.
    api.post.mockResolvedValue(mockResponse);

    // Act
    const result = await verifyResetCode(email, code);

    // Assert
    expect(api.post).toHaveBeenCalledWith("/api/password/verify-reset-code", { email, code });

    // IF the code was correct, it would access mockResponse.data.success.
    // IF the code is buggy (as we suspect), it will try mockResponse.success which is undefined/falsey.
    expect(result).toEqual(ApiResponseT.ok(true, "Code verified successfully"));
  });

  it("should return badRequest when API call returns success: false", async () => {
    // Arrange
    const email = "test@example.com";
    const code = "wrong-code";
    const mockResponse = {
      status: 200,
      data: {
        success: false,
        message: "Invalid code",
        errors: [{ message: "Invalid code" }],
      },
    };
    api.post.mockResolvedValue(mockResponse);

    // Act
    const result = await verifyResetCode(email, code);

    // Assert
    expect(result).toEqual(ApiResponseT.badRequest([{ message: "Invalid code" }], "Invalid code"));
  });

  it("should handle API errors (e.g. 400 Bad Request)", async () => {
    // Arrange
    const email = "test@example.com";
    const code = "invalid";
    const errorResponse = {
      response: {
        status: 400,
        data: {
          success: false,
          message: "Validation failed",
          errors: [{ message: "Code is required" }],
        },
      },
    };
    api.post.mockRejectedValue(errorResponse);

    // Act
    const result = await verifyResetCode(email, code);

    // Assert
    expect(result).toEqual(
      ApiResponseT.badRequest([{ message: "Code is required" }], "Validation failed")
    );
  });
});
