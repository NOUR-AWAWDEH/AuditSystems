// validateEmail.test.js
import { describe, it, expect } from "vitest";
import { validateEmail } from "@/services/validations/validateEmail";

describe("validateEmail", () => {
    it("returns false for non-string input", () => {
        expect(validateEmail(null)).toBe(false);
        expect(validateEmail(undefined)).toBe(false);
        expect(validateEmail(123)).toBe(false);
    });

    it("returns false for empty or whitespace-only email", () => {
        expect(validateEmail("")).toBe(false);
        expect(validateEmail("   ")).toBe(false);
    });

    it("returns false for invalid email formats", () => {
        expect(validateEmail("test")).toBe(false);
        expect(validateEmail("test@")).toBe(false);
        expect(validateEmail("test@com")).toBe(false);
        expect(validateEmail("test@.com")).toBe(false);
        expect(validateEmail("test@@example.com")).toBe(false);
    });

    it("returns true for valid email formats", () => {
        expect(validateEmail("test@example.com")).toBe(true);
        expect(validateEmail("user.name+tag@example.co.uk")).toBe(true);
        expect(validateEmail("user_name@example.org")).toBe(true);
    });

    it("sanitizes HTML tags before validation", () => {
        expect(validateEmail("<b>test@example.com</b>")).toBe(true);
    });

    it("trims whitespace before validation", () => {
        expect(validateEmail("  test@example.com  ")).toBe(true);
    });

    it("returns false for malformed email inside HTML", () => {
        expect(validateEmail("<script>alert(1)</script>@test.com")).toBe(false);
    });
});
