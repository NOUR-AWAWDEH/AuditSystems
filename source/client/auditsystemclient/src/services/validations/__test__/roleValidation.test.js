import { describe, it, expect } from "vitest";
import { isNotAdmin, requireNotAdmin } from "../validateRole";

describe("isNotAdmin", () => {
    it("returns false when user is null", () => {
        expect(isNotAdmin(null)).toBe(false);
    });

    it("returns false when role is missing", () => {
        expect(isNotAdmin({})).toBe(false);
    });

    it("returns false when role is admin (case-insensitive)", () => {
        expect(isNotAdmin({ role: "Admin" })).toBe(false);
        expect(isNotAdmin({ role: "ADMIN" })).toBe(false);
    });

    it("returns true when role is not admin", () => {
        expect(isNotAdmin({ role: "User" })).toBe(true);
        expect(isNotAdmin({ role: "Manager" })).toBe(true);
    });
});

describe("requireNotAdmin", () => {
    it("throws error when user is admin", () => {
        expect(() =>
            requireNotAdmin({ role: "Admin" })
        ).toThrow("Admin users are not allowed");
    });

    it("throws error when user is invalid", () => {
        expect(() =>
            requireNotAdmin({})
        ).toThrow("Admin users are not allowed");
    });

    it("does not throw when user is not admin", () => {
        expect(() => requireNotAdmin({ role: "User" })).not.toThrow();
    });
});