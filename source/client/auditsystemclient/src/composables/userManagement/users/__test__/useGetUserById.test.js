// useGetUserById.test.js
import { describe, it, expect, vi, beforeEach } from "vitest";
import { useGetUserById } from "../useGetUserById";
import { getUserByIdRepository } from "@/services/userManagement/users/getUserByIdRepository";


// Mock the repository
vi.mock("@/services/userManagement/users/getUserByIdRepository", () => ({
    getUserByIdRepository: {
        getUserById: vi.fn(),
    },
}));

describe("useGetUserById", () => {
    let composable;

    beforeEach(() => {
        composable = useGetUserById();
        vi.clearAllMocks();
    });

    it("should initialize with default values", () => {
        expect(composable.isLoading.value).toBe(false);
        expect(composable.error.value).toBeNull();
        expect(composable.user.value).toBeNull();
    });

    it("should return failure if userId is missing", async () => {
        const result = await composable.getUserById(null);

        expect(result).toEqual({
            success: false,
            message: "User ID is required",
            errors: [{ message: "User ID is required" }],
        });
        expect(composable.error.value).toBe("User ID is required");
        expect(composable.user.value).toBeNull();
        expect(composable.isLoading.value).toBe(false);
    });

    it("should handle successful response", async () => {
        const mockResponse = {
            success: true,
            result: { id: 1, name: "John Doe" },
            successMessage: "User loaded",
        };
        getUserByIdRepository.getUserById.mockResolvedValue(mockResponse);

        const result = await composable.getUserById(1);

        expect(getUserByIdRepository.getUserById).toHaveBeenCalledWith(1);
        expect(result).toEqual({
            success: true,
            data: mockResponse.result,
            message: "User loaded",
        });
        expect(composable.user.value).toEqual(mockResponse.result);
        expect(composable.error.value).toBeNull();
        expect(composable.isLoading.value).toBe(false);
    });

    it("should handle failure response from API", async () => {
        const mockResponse = {
            success: false,
            errors: [{ message: "User not found" }],
        };
        getUserByIdRepository.getUserById.mockResolvedValue(mockResponse);

        const result = await composable.getUserById(1);

        expect(result).toEqual({
            success: false,
            message: "User not found",
            errors: mockResponse.errors,
        });
        expect(composable.error.value).toBe("User not found");
        expect(composable.user.value).toBeNull();
        expect(composable.isLoading.value).toBe(false);
    });

    it("should handle thrown errors", async () => {
        const mockError = new Error("Network error");
        getUserByIdRepository.getUserById.mockRejectedValue(mockError);

        const result = await composable.getUserById(1);

        expect(result).toEqual({
            success: false,
            message: "Network error",
            errors: [{ message: "Network error" }],
        });
        expect(composable.error.value).toBe("Network error");
        expect(composable.user.value).toBeNull();
        expect(composable.isLoading.value).toBe(false);
    });

    it("should reset state correctly", () => {
        composable.isLoading.value = true;
        composable.error.value = "Error";
        composable.user.value = { id: 1 };

        composable.resetState();

        expect(composable.isLoading.value).toBe(false);
        expect(composable.error.value).toBeNull();
        expect(composable.user.value).toBeNull();
    });
});
