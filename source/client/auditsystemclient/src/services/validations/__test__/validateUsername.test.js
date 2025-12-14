import { describe, it, expect } from "vitest";
import { validateUsername } from "@/services/validations/validateUsername";

describe("validateUsername", () => {
    it("returns error when username is empty", () => {
        expect(validateUsername("")).toBe("Username is required");
        expect(validateUsername("   ")).toBe("Username cannot start with a space");
    });

    it("sanitizes HTML and detects empty result", () => {
        expect(validateUsername("<script></script>")).toBe("Username is required");
        expect(validateUsername("<b></b>")).toBe("Username is required");
    });

    it("returns error when username is shorter than 4 characters", () => {
        expect(validateUsername("abc")).toBe(
            "Username must be at least 4 characters"
        );
    });

    it("returns error when username is longer than 50 characters", () => {
        const longUsername = "a".repeat(51);
        expect(validateUsername(longUsername)).toBe(
            "Username must be less than 50 characters"
        );
    });

    it("returns error when username contains invalid characters", () => {
        expect(validateUsername("user@name")).toBe(
            "Username can only contain letters, numbers, underscores, dots, and hyphens"
        );
        expect(validateUsername("user name")).toBe(
            "Username can only contain letters, numbers, underscores, dots, and hyphens"
        );
    });

    it("accepts valid usernames", () => {
        expect(validateUsername("user_name")).toBe(null);
        expect(validateUsername("user.name")).toBe(null);
        expect(validateUsername("user-name")).toBe(null);
        expect(validateUsername("User123")).toBe(null);
    });

    it("sanitizes HTML but allows valid content", () => {
        expect(validateUsername("<b>user123</b>")).toBe(null);
    });
});
