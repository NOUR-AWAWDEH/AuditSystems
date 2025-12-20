import { describe, it, expect, vi, beforeEach } from "vitest";
import { useUpdateUser } from "@/composables/userManagement/users/useUpdateUser";
import { updateUserRepository } from "@/services/userManagement/users/updateUserRepository";
import { validateUpdateUserData } from "@/services/validations/validateUpdateUserData";

// ✅ MOCKS — paths MUST match composable imports exactly
vi.mock("@/services/userManagement/users/updateUserRepository", () => ({
    updateUserRepository: {
        updateUser: vi.fn(),
    },
}));

vi.mock("@/services/validations/validateUpdateUserData", () => ({
    validateUpdateUserData: vi.fn(),
}));

describe("useUpdateUser", () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    it("updates user successfully", async () => {
        validateUpdateUserData.mockReturnValue(null);

        updateUserRepository.updateUser.mockResolvedValue({
            success: true,
            result: { id: "1", name: "John" },
            message: "Updated successfully",
        });

        const { updateUser, isUpdating, error, updatedUser } = useUpdateUser();

        const result = await updateUser({ id: "1" });

        expect(result).toEqual({
            success: true,
            data: { id: "1", name: "John" },
            message: "Updated successfully",
        });

        expect(isUpdating.value).toBe(false);
        expect(error.value).toBe(null);
        expect(updatedUser.value).toBe(true);
    });

    it("fails when validation fails", async () => {
        validateUpdateUserData.mockReturnValue("Invalid data");

        const { updateUser, error, updatedUser } = useUpdateUser();

        const result = await updateUser({});

        expect(result.success).toBe(false);
        expect(result.message).toBe("Invalid data");
        expect(error.value).toBe("Invalid data");
        expect(updatedUser.value).toBe(null);

        expect(updateUserRepository.updateUser).not.toHaveBeenCalled();
    });

    it("handles API failure response", async () => {
        validateUpdateUserData.mockReturnValue(null);

        updateUserRepository.updateUser.mockResolvedValue({
            success: false,
            errors: [{ message: "User not found" }],
        });

        const { updateUser, error } = useUpdateUser();

        const result = await updateUser({ id: "1" });

        expect(result.success).toBe(false);
        expect(result.message).toBe("User not found");
        expect(error.value).toBe("User not found");
    });

    it("handles thrown exception", async () => {
        validateUpdateUserData.mockReturnValue(null);

        updateUserRepository.updateUser.mockRejectedValue(
            new Error("Network error")
        );

        const { updateUser, error } = useUpdateUser();

        const result = await updateUser({ id: "1" });

        expect(result.success).toBe(false);
        expect(result.message).toBe("Network error");
        expect(error.value).toBe("Network error");
    });

    it("clears state correctly", () => {
        const { isUpdating, error, updatedUser, clearState } = useUpdateUser();

        isUpdating.value = true;
        error.value = "Some error";
        updatedUser.value = true;

        clearState();

        expect(isUpdating.value).toBe(false);
        expect(error.value).toBe(null);
        expect(updatedUser.value).toBe(null);
    });
});