// validatePassword.test.js
import { describe, it, expect } from "vitest";
import validatePassword from "@/services/validations/validatePassword";

describe("validatePassword", () => {
    it("returns error for non-string input", () => {
        expect(validatePassword(null)).toBe("Password is required");
        expect(validatePassword(undefined)).toBe("Password is required");
        expect(validatePassword(12345678)).toBe("Password is required");
    });

    it("returns error for empty or whitespace-only password", () => {
        expect(validatePassword("")).toBe("Password is required");
        expect(validatePassword("    ")).toBe("Password is required");
    });

    it("returns error if password is too short", () => {
        expect(validatePassword("Ab1$")).toBe("Password must be at least 8 characters");
    });

    it("returns error if missing uppercase letter", () => {
        expect(validatePassword("abc123$%")).toBe("Password must contain an uppercase letter");
    });

    it("returns error if missing number", () => {
        expect(validatePassword("Abcdef$%")).toBe("Password must contain a number");
    });

    it("returns error if missing special character", () => {
        expect(validatePassword("Abc12345")).toBe("Password must contain a special character");
    });

    it("returns null for valid password", () => {
        expect(validatePassword("Abc123$%")).toBe(null);
    });

    it("sanitizes HTML tags and still validates", () => {
        expect(validatePassword("<b>Abc123$%</b>")).toBe(null);
    });

    it("trims leading/trailing spaces", () => {
        expect(validatePassword("  Abc123$%  ")).toBe(null);
    });
});