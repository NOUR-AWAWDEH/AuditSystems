// apiResponse.test.js
import { describe, it, expect } from "vitest";
import { ApiResponse, ApiResponseT, ApiErrorResponse } from "../ApiResponse";

describe("ApiResponse - Normal Cases", () => {
    it("should create ok response", () => {
        const res = ApiResponse.ok("Success");
        expect(res.success).toBe(true);
        expect(res.statusCode).toBe(200);
        expect(res.successMessage).toBe("Success");
        expect(res.errors).toEqual([]);
    });

    it("should create badRequest with string", () => {
        const res = ApiResponse.badRequest("Invalid input");
        expect(res.success).toBe(false);
        expect(res.statusCode).toBe(400);
        expect(res.errors).toEqual([{ message: "Invalid input" }]);
    });

    it("should create badRequest with array of errors", () => {
        const errors = [{ message: "Error 1" }, { message: "Error 2" }];
        const res = ApiResponse.badRequest(errors);
        expect(res.success).toBe(false);
        expect(res.errors).toEqual(errors);
    });

    it("should handle unauthorized, forbidden, notFound, internalServerError", () => {
        const methods = [
            { fn: ApiResponse.unauthorized, code: 401 },
            { fn: ApiResponse.forbidden, code: 403 },
            { fn: ApiResponse.notFound, code: 404 },
            { fn: ApiResponse.internalServerError, code: 500 },
        ];

        methods.forEach(({ fn, code }) => {
            const res = fn("Error");
            expect(res.success).toBe(false);
            expect(res.statusCode).toBe(code);
            expect(res.errors).toEqual([{ message: "Error" }]);
        });
    });
});

describe("ApiResponseT - Normal Cases", () => {
    it("should create ok response with result", () => {
        const result = { id: 1, name: "Test" };
        const res = ApiResponseT.ok(result, "Loaded");
        expect(res.success).toBe(true);
        expect(res.statusCode).toBe(200);
        expect(res.result).toEqual(result);
        expect(res.successMessage).toBe("Loaded");
        expect(res.errors).toEqual([]);
    });

    it("should create created response", () => {
        const result = { id: 2 };
        const res = ApiResponseT.created(result, "Created");
        expect(res.success).toBe(true);
        expect(res.statusCode).toBe(201);
        expect(res.result).toEqual(result);
        expect(res.successMessage).toBe("Created");
        expect(res.errors).toEqual([]);
    });

    it("should handle all error methods", () => {
        const methods = [
            { fn: ApiResponseT.unauthorized, code: 401 },
            { fn: ApiResponseT.forbidden, code: 403 },
            { fn: ApiResponseT.notFound, code: 404 },
            { fn: ApiResponseT.internalServerError, code: 500 },
            { fn: ApiResponseT.serverError, code: 500 },
        ];

        methods.forEach(({ fn, code }) => {
            const res = fn("Error");
            expect(res.success).toBe(false);
            expect(res.statusCode).toBe(code);
            expect(res.errors).toEqual([{ message: "Error" }]);
        });
    });
});

describe("ApiResponse - Edge Cases", () => {
    it("should handle undefined error message", () => {
        const res = ApiResponse.badRequest(undefined);
        expect(res.errors).toEqual([{ message: undefined }]);
    });

    it("should handle null error message", () => {
        const res = ApiResponse.badRequest(null);
        expect(res.errors).toEqual([{ message: null }]);
    });

    it("should handle empty array", () => {
        const res = ApiResponse.badRequest([]);
        expect(res.errors).toEqual([]);
    });
});

describe("ApiResponseT - Edge Cases", () => {
    it("should handle undefined result", () => {
        const res = ApiResponseT.ok(undefined);
        expect(res.result).toBeUndefined();
        expect(res.errors).toEqual([]);
    });

    it("should handle null result", () => {
        const res = ApiResponseT.ok(null);
        expect(res.result).toBeNull();
        expect(res.errors).toEqual([]);
    });

    it("should handle empty array result", () => {
        const res = ApiResponseT.ok([], "Empty list");
        expect(res.result).toEqual([]);
        expect(res.successMessage).toBe("Empty list");
    });

    it("should handle undefined error message in badRequest", () => {
        const res = ApiResponseT.badRequest(undefined);
        expect(res.errors).toEqual([{ message: undefined }]);
    });
});

describe("ApiErrorResponse", () => {
    it("should have empty message by default", () => {
        expect(ApiErrorResponse.message).toBe("");
    });

    it("should allow changing message dynamically", () => {
        ApiErrorResponse.message = "New error";
        expect(ApiErrorResponse.message).toBe("New error");
        ApiErrorResponse.message = "";
    });
});
