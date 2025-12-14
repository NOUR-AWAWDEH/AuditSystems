// useGetUsersByAdminId.test.js
import { describe, it, expect, vi, beforeEach } from "vitest";
import { useGetUsersByAdminId } from "../useGetUsersByAdminId";
import { getUsersByAdminIdRepository } from "@/services/userManagement/users/getUsersByAdminIdRepository";

// Mock the repository
vi.mock("@/services/userManagement/users/getUsersByAdminIdRepository", () => ({
    getUsersByAdminIdRepository: {
        getAllUsers: vi.fn(),
    },
}));

describe("useGetUsersByAdminId", () => {
    let composable;

    beforeEach(() => {
        composable = useGetUsersByAdminId();
        vi.clearAllMocks();
    });

    it("should initialize with default values", () => {
        expect(composable.isLoading.value).toBe(false);
        expect(composable.error.value).toBeNull();
        expect(composable.users.value).toEqual([]);
    });

    it("should handle successful response", async () => {
        const mockResponse = {
            success: true,
            result: [{ id: 1, name: "User 1" }, { id: 2, name: "User 2" }],
            successMessage: "Users loaded",
        };
        getUsersByAdminIdRepository.getAllUsers.mockResolvedValue(mockResponse);

        const result = await composable.getUsers();

        expect(getUsersByAdminIdRepository.getAllUsers).toHaveBeenCalled();
        expect(result).toEqual({
            success: true,
            data: mockResponse.result,
            message: "Users loaded",
        });
        expect(composable.users.value).toEqual(mockResponse.result);
        expect(composable.error.value).toBeNull();
        expect(composable.isLoading.value).toBe(false);
    });

    it("should handle failure response with errors", async () => {
        const mockResponse = {
            success: false,
            errors: [{ message: "Failed to fetch users" }],
        };
        getUsersByAdminIdRepository.getAllUsers.mockResolvedValue(mockResponse);

        const result = await composable.getUsers();

        expect(result).toEqual({
            success: false,
            message: "Failed to fetch users",
            errors: mockResponse.errors,
        });
        expect(composable.users.value).toEqual([]);
        expect(composable.error.value).toBe("Failed to fetch users");
        expect(composable.isLoading.value).toBe(false);
    });

    it("should handle thrown errors", async () => {
        const mockError = new Error("Network error");
        getUsersByAdminIdRepository.getAllUsers.mockRejectedValue(mockError);

        const result = await composable.getUsers();

        expect(result).toEqual({
            success: false,
            message: "Network error",
            errors: [{ message: "Network error" }],
        });
        expect(composable.users.value).toEqual([]);
        expect(composable.error.value).toBe("Network error");
        expect(composable.isLoading.value).toBe(false);
    });
});
