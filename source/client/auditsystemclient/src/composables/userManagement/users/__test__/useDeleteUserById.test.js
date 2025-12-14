import { describe, it, expect, vi, beforeEach } from "vitest";
import { deleteUserByIdRepository } from "@/services/userManagement/users/deleteUserByIdRepository";
import { useDeleteUserById } from "@/composables/userManagement/users/useDeleteUserById";

// Mock repository
vi.mock("@/services/userManagement/users/deleteUserByIdRepository", () => ({
    deleteUserByIdRepository: {
        deleteUserById: vi.fn(),
    },
}));

describe("useDeleteUserById", () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    it("deletes user successfully", async () => {
        deleteUserByIdRepository.deleteUserById.mockResolvedValue({
            success: true,
            message: "User deleted",
        });

        const { deleteUser, isDeleting, error, deletedUser } =
            useDeleteUserById();

        const result = await deleteUser("123");

        expect(isDeleting.value).toBe(false);
        expect(error.value).toBe(null);
        expect(deletedUser.value).toBe(true);
        expect(result).toEqual({
            success: true,
            message: "User deleted",
        });

        expect(deleteUserByIdRepository.deleteUserById).toHaveBeenCalledWith("123");
    });

    it("fails validation when userId is missing", async () => {
        const { deleteUser, error, deletedUser } = useDeleteUserById();

        const result = await deleteUser();

        expect(result.success).toBe(false);
        expect(result.message).toBe("Failed to delete user");
        expect(error.value).toBe("Failed to delete user");
        expect(deletedUser.value).toBe(null);
    });

    it("handles API failure response", async () => {
        deleteUserByIdRepository.deleteUserById.mockResolvedValue({
            success: false,
            errors: [{ message: "User not found" }],
        });

        const { deleteUser, error, deletedUser } = useDeleteUserById();

        const result = await deleteUser("123");

        expect(result.success).toBe(false);
        expect(result.message).toBe("User not found");
        expect(error.value).toBe("User not found");
        expect(deletedUser.value).toBe(null);
    });

    it("handles thrown exception", async () => {
        deleteUserByIdRepository.deleteUserById.mockRejectedValue(
            new Error("Network error")
        );

        const { deleteUser, error } = useDeleteUserById();

        const result = await deleteUser("123");

        expect(result.success).toBe(false);
        expect(result.message).toBe("Network error");
        expect(error.value).toBe("Network error");
    });

    it("resets state correctly", () => {
        const { isDeleting, error, deletedUser, resetState } =
            useDeleteUserById();

        isDeleting.value = true;
        error.value = "Some error";
        deletedUser.value = true;

        resetState();

        expect(isDeleting.value).toBe(false);
        expect(error.value).toBe(null);
        expect(deletedUser.value).toBe(null);
    });
});
