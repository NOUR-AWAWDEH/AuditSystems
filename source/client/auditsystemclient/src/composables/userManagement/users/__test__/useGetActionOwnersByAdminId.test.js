// useGetActionOwnersByAdminId.test.js
import { describe, it, expect, vi, beforeEach } from "vitest";
import { getActionOwnersByAdminIdRepository } from "@/services/userManagement/users/getActionOwnersByAdminIdRepository";
import { useGetActionOwnersByAdminId } from "../useGetActionOwnersByAdminId";

// Mock the repository
vi.mock("@/services/userManagement/users/getActionOwnersByAdminIdRepository", () => {
    return {
        getActionOwnersByAdminIdRepository: {
            getActionOwnersByAdminId: vi.fn(),
        },
    };
});

describe("useGetActionOwnersByAdminId", () => {
    let composable;

    beforeEach(() => {
        composable = useGetActionOwnersByAdminId();
        vi.clearAllMocks();
    });

    it("should initialize with default values", () => {
        expect(composable.isLoading.value).toBe(false);
        expect(composable.error.value).toBeNull();
        expect(composable.actionOwners.value).toEqual([]);
    });

    it("should handle success response", async () => {
        const mockResponse = {
            success: true,
            result: [{ id: 1, name: "Owner 1" }],
            successMessage: "Owners loaded",
        };
        getActionOwnersByAdminIdRepository.getActionOwnersByAdminId.mockResolvedValue(mockResponse);

        const result = await composable.getActionOwnersByAdminId();

        expect(getActionOwnersByAdminIdRepository.getActionOwnersByAdminId).toHaveBeenCalled();
        expect(composable.isLoading.value).toBe(false);
        expect(composable.error.value).toBeNull();
        expect(composable.actionOwners.value).toEqual(mockResponse.result);
        expect(result).toEqual({
            success: true,
            data: mockResponse.result,
            message: "Owners loaded",
        });
    });

    it("should handle failure response with errors", async () => {
        const mockResponse = {
            success: false,
            errors: [{ message: "Failed to fetch" }],
        };
        getActionOwnersByAdminIdRepository.getActionOwnersByAdminId.mockResolvedValue(mockResponse);

        const result = await composable.getActionOwnersByAdminId();

        expect(composable.isLoading.value).toBe(false);
        expect(composable.error.value).toBe("Failed to fetch");
        expect(composable.actionOwners.value).toEqual([]);
        expect(result).toEqual({
            success: false,
            message: "Failed to fetch",
            errors: mockResponse.errors,
        });
    });

    it("should handle thrown error", async () => {
        const mockError = new Error("Network error");
        getActionOwnersByAdminIdRepository.getActionOwnersByAdminId.mockRejectedValue(mockError);

        const result = await composable.getActionOwnersByAdminId();

        expect(composable.isLoading.value).toBe(false);
        expect(composable.error.value).toBe("Network error");
        expect(composable.actionOwners.value).toEqual([]);
        expect(result).toEqual({
            success: false,
            message: "Network error",
            errors: [{ message: "Network error" }],
        });
    });
});
