// userValidators.test.js
import { describe, it, expect } from "vitest";
import { validateUserData } from "../userValidators";

describe("validateUserData", () => {
  it("should return error if userData is null or undefined", () => {
    expect(validateUserData(null)).toBe("User data is required");
    expect(validateUserData(undefined)).toBe("User data is required");
  });

  it("should return error for invalid username", () => {
    const userData = {
      userName: "",
      email: "test@test.com",
      password: "Password1!",
      confirmPassword: "Password1!",
      firstName: "John",
      lastName: "Doe",
      roleName: "User",
      companyId: "123e4567-e89b-12d3-a456-426614174000",
    };
    expect(validateUserData(userData)).toBe("Username is required");
  });

  it("should return error for invalid email", () => {
    const userData = {
      userName: "john_doe",
      email: "",
      password: "Password1!",
      confirmPassword: "Password1!",
      firstName: "John",
      lastName: "Doe",
      roleName: "User",
      companyId: "123e4567-e89b-12d3-a456-426614174000",
    };
    expect(validateUserData(userData)).toBe("Email is required");

    userData.email = "invalid-email";
    expect(validateUserData(userData)).toBe("Invalid email format");
  });

  it("should return error for invalid password", () => {
    const userData = {
      userName: "john_doe",
      email: "test@test.com",
      password: "",
      confirmPassword: "",
      firstName: "John",
      lastName: "Doe",
      roleName: "User",
      companyId: "123e4567-e89b-12d3-a456-426614174000",
    };
    expect(validateUserData(userData)).toBe("Password is required");

    userData.password = "short";
    userData.confirmPassword = "short";
    expect(validateUserData(userData)).toBe("Password must be at least 8 characters");

    userData.password = "Password1!";
    userData.confirmPassword = "Password2!";
    expect(validateUserData(userData)).toBe("Passwords do not match");
  });

  it("should return error for missing firstName, lastName, roleName, companyId", () => {
    const baseData = {
      userName: "john_doe",
      email: "test@test.com",
      password: "Password1!",
      confirmPassword: "Password1!",
      firstName: "",
      lastName: "",
      roleName: "",
      companyId: null,
    };

    expect(validateUserData(baseData)).toBe("First name is required");

    baseData.firstName = "John";
    expect(validateUserData(baseData)).toBe("Last name is required");

    baseData.lastName = "Doe";
    expect(validateUserData(baseData)).toBe("Role name is required");

    baseData.roleName = "Admin";
    expect(validateUserData(baseData)).toBe("Cannot assign Admin role to a new user");

    baseData.roleName = "admin";
    expect(validateUserData(baseData)).toBe("Cannot assign Admin role to a new user");

    // Set valid role and companyId to pass validation
    baseData.roleName = "User";
    baseData.companyId = "123e4567-e89b-12d3-a456-426614174000";
    expect(validateUserData(baseData)).toBeNull();

    // Invalid GUID should fail
    baseData.companyId = "invalid-guid";
    expect(validateUserData(baseData)).toBe("Company ID must be a valid GUID");
  });

  it("should return null for fully valid data", () => {
    const validData = {
      userName: "john_doe",
      email: "test@test.com",
      password: "Password1!",
      confirmPassword: "Password1!",
      firstName: "John",
      lastName: "Doe",
      roleName: "User",
      companyId: "123e4567-e89b-12d3-a456-426614174000",
    };
    expect(validateUserData(validData)).toBeNull();
  });

  it("should validate companyId as a valid GUID", () => {
    const userData = {
      userName: "john_doe",
      email: "test@test.com",
      password: "Password1!",
      confirmPassword: "Password1!",
      firstName: "John",
      lastName: "Doe",
      roleName: "User",
      companyId: "invalid-guid",
    };
    expect(validateUserData(userData)).toBe("Company ID must be a valid GUID");

    userData.companyId = "123e4567-e89b-12d3-a456-426614174000";
    expect(validateUserData(userData)).toBeNull();
  });
});
