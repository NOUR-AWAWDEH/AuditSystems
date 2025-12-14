import { describe, it, expect, vi, beforeEach } from "vitest";
import { useCreateUser } from "@/composables/userManagement/users/useCreateUser";
import { createUserRepository } from "@/services/userManagement/users/createUserRepository";
import { validateUserData } from "@/services/validations/userValidators";

// Mock dependencies
vi.mock("@/services/userManagement/users/createUserRepository", () => ({
    createUserRepository: {
        createUser: vi.fn(),
    },
}));

vi.mock("@/services/validations/userValidators", () => ({
    validateUserData: vi.fn(),
}));

describe("useCreateUser", () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    it("creates user successfully", async () => {
        validateUserData.mockReturnValue(null);
        createUserRepository.createUser.mockResolvedValue({
            success: true,
            result: { id: "1", name: "John" },
            message: "User created successfully.",
        });

        const { createUser, isCreating, error, createdUser } = useCreateUser();

        const result = await createUser({ id: "1", name: "John" });

        expect(result).toEqual({
            success: true,
            data: { id: "1", name: "John" },
            message: "User created successfully.",
        });

        expect(isCreating.value).toBe(false);
        expect(error.value).toBe(null);
        expect(createdUser.value).toEqual({
            success: true,
            result: { id: "1", name: "John" },
            message: "User created successfully.",
        });
    });

    it("fails when validation fails", async () => {
        validateUserData.mockReturnValue("Name is required");

        const { createUser, error, createdUser } = useCreateUser();

        const result = await createUser({});

        expect(result.success).toBe(false);
        expect(result.message).toBe("Name is required");
        expect(error.value).toBe("Name is required");
        expect(createdUser.value).toBe(null);

        expect(createUserRepository.createUser).not.toHaveBeenCalled();
    });

    it("handles API failure response", async () => {
        validateUserData.mockReturnValue(null);
        createUserRepository.createUser.mockResolvedValue({
            success: false,
            errors: [{ message: "Email already exists" }],
        });

        const { createUser, error } = useCreateUser();

        const result = await createUser({ email: "test@test.com" });

        expect(result.success).toBe(false);
        expect(result.message).toBe("Email already exists");
        expect(error.value).toBe("Email already exists");
    });

    it("handles thrown exception", async () => {
        validateUserData.mockReturnValue(null);
        createUserRepository.createUser.mockRejectedValue(
            new Error("Network error")
        );

        const { createUser, error } = useCreateUser();

        const result = await createUser({ name: "John" });

        expect(result.success).toBe(false);
        expect(result.message).toBe("Network error");
        expect(error.value).toBe("Network error");
    });

    it("clears state correctly", () => {
        const { isCreating, error, createdUser, clearState } = useCreateUser();

        isCreating.value = true;
        error.value = "Some error";
        createdUser.value = { id: 1 };

        clearState();

        expect(isCreating.value).toBe(false);
        expect(error.value).toBe(null);
        expect(createdUser.value).toBe(null);
    });
});