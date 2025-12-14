// validateUpdateUserData.test.js
import { describe, it, expect, vi, beforeEach } from "vitest";
import { validateUpdateUserData } from "@/services/validations/validateUpdateUserData";
import { validateEmail } from "@/services/validations/validateEmail";
import { validateUsername } from "@/services/validations/validateUsername";
import { isNotAdmin } from "@/services/validations/validateRole";

// Make sure mocks match the actual import paths exactly
vi.mock("@/services/validations/validateEmail", () => ({
    validateEmail: vi.fn(),
}));

vi.mock("@/services/validations/validateUsername", () => ({
    validateUsername: vi.fn(),
}));

vi.mock("@/services/validations/validateRole", () => ({
    isNotAdmin: vi.fn(),
}));

const validUser = {
    id: "123e4567-e89b-12d3-a456-426614174000",
    username: "john_doe",
    email: "john@test.com",
    firstName: "John",
    lastName: "Doe",
    roleName: "User",
};

describe("validateUpdateUserData", () => {
    beforeEach(() => {
        vi.clearAllMocks();
        validateUsername.mockReturnValue(null);
        validateEmail.mockReturnValue(true);
        isNotAdmin.mockReturnValue(true);
    });

    it("returns error when userData is missing", () => {
        expect(validateUpdateUserData(null)).toBe("User data is required");
    });

    it("returns error when id is missing", () => {
        expect(validateUpdateUserData({})).toBe("User ID is required");
    });

    it("returns error when id is not a valid GUID", () => {
        expect(
            validateUpdateUserData({ ...validUser, id: "123" })
        ).toBe("User ID must be a valid GUID");
    });

    it("returns username validation error", () => {
        validateUsername.mockReturnValue("Invalid username");

        expect(
            validateUpdateUserData(validUser)
        ).toBe("Invalid username");
    });

    it("returns error when email is missing", () => {
        expect(
            validateUpdateUserData({ ...validUser, email: " " })
        ).toBe("Email is required");
    });

    it("returns error when email format is invalid", () => {
        validateEmail.mockReturnValue(false);

        expect(
            validateUpdateUserData(validUser)
        ).toBe("Invalid email format");
    });

    it("returns error when first name is missing", () => {
        expect(
            validateUpdateUserData({ ...validUser, firstName: "" })
        ).toBe("First name is required");
    });

    it("returns error when last name is missing", () => {
        expect(
            validateUpdateUserData({ ...validUser, lastName: "" })
        ).toBe("Last name is required");
    });

    it("returns error when role name is missing", () => {
        expect(
            validateUpdateUserData({ ...validUser, roleName: "" })
        ).toBe("Role name is required");
    });

    it("returns error when role is Admin", () => {
        isNotAdmin.mockReturnValue(false);

        expect(
            validateUpdateUserData(validUser)
        ).toBe("Cannot assign Admin role");
    });

    it("returns null when data is valid", () => {
        expect(
            validateUpdateUserData(validUser)
        ).toBe(null);
    });
});