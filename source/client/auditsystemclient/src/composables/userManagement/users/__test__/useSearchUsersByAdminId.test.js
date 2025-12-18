import { describe, it, expect, vi, beforeEach } from "vitest";
import { useSearchUsersByAdminId } from "../useSearchUsersByAdminId";
import { searchUsersByAdminIdRepository } from "@/services/userManagement/users/searchUsersByAdminIdRepository";

// Mock the repository
vi.mock("@/services/userManagement/users/searchUsersByAdminIdRepository");

describe("useSearchUsersByAdminId", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it("should initialize with default values", () => {
    const { searchResults, isSearching, error, totalCount, currentPage, pageSize, searchTerm } =
      useSearchUsersByAdminId();

    expect(searchResults.value).toEqual([]);
    expect(isSearching.value).toBe(false);
    expect(error.value).toBeNull();
    expect(totalCount.value).toBe(0);
    expect(currentPage.value).toBe(1);
    expect(pageSize.value).toBe(10);
    expect(searchTerm.value).toBe("");
  });

  it("should search users successfully with search params", async () => {
    const mockUsers = [
      { id: 1, name: "John Doe", email: "john@example.com" },
      { id: 2, name: "Jane Smith", email: "jane@example.com" },
    ];
    const mockResult = {
      success: true,
      result: {
        items: mockUsers,
        totalCount: 25,
      },
    };
    searchUsersByAdminIdRepository.searchUsersByAdminId = vi.fn().mockResolvedValue(mockResult);

    const { searchResults, totalCount, searchUsers } = useSearchUsersByAdminId();

    await searchUsers({
      searchTerm: "John",
      pageNumber: 2,
      pageSize: 10,
    });

    expect(searchUsersByAdminIdRepository.searchUsersByAdminId).toHaveBeenCalledWith({
      searchTerm: "John",
      pageNumber: 2,
      pageSize: 10,
    });
    expect(searchResults.value).toEqual(mockUsers);
    expect(totalCount.value).toBe(25);
  });

  it("should use default values when search params are not provided", async () => {
    const mockResult = {
      success: true,
      result: {
        items: [],
        totalCount: 0,
      },
    };
    searchUsersByAdminIdRepository.searchUsersByAdminId = vi.fn().mockResolvedValue(mockResult);

    const { searchUsers } = useSearchUsersByAdminId();

    await searchUsers();

    expect(searchUsersByAdminIdRepository.searchUsersByAdminId).toHaveBeenCalledWith({
      searchTerm: "",
      pageNumber: 1,
      pageSize: 10,
    });
  });

  it("should handle API failure response", async () => {
    const mockResult = {
      success: false,
      message: "Access denied",
    };
    searchUsersByAdminIdRepository.searchUsersByAdminId = vi.fn().mockResolvedValue(mockResult);

    const { error, searchResults, searchUsers } = useSearchUsersByAdminId();

    await searchUsers({ searchTerm: "test" });

    expect(error.value).toBe("Access denied");
    expect(searchResults.value).toEqual([]);
  });

  it("should use default error message when API failure has no message", async () => {
    const mockResult = {
      success: false,
    };
    searchUsersByAdminIdRepository.searchUsersByAdminId = vi.fn().mockResolvedValue(mockResult);

    const { error, searchUsers } = useSearchUsersByAdminId();

    await searchUsers();

    expect(error.value).toBe("Failed to search users");
  });

  it("should handle API failure with errors array", async () => {
    const mockResult = {
      success: false,
      errors: [{ message: "Validation error" }],
    };
    searchUsersByAdminIdRepository.searchUsersByAdminId = vi.fn().mockResolvedValue(mockResult);

    const { error, searchUsers } = useSearchUsersByAdminId();

    await searchUsers();

    expect(error.value).toBe("Validation error");
  });

  it("should handle exception during search", async () => {
    const errorMessage = "Network error";
    searchUsersByAdminIdRepository.searchUsersByAdminId = vi
      .fn()
      .mockRejectedValue(new Error(errorMessage));

    const consoleErrorSpy = vi.spyOn(console, "error").mockImplementation(() => {});

    const { error, searchUsers } = useSearchUsersByAdminId();

    await searchUsers();

    expect(error.value).toBe(errorMessage);
    expect(consoleErrorSpy).toHaveBeenCalled();

    consoleErrorSpy.mockRestore();
  });

  it("should handle exception without message", async () => {
    searchUsersByAdminIdRepository.searchUsersByAdminId = vi.fn().mockRejectedValue({});

    const consoleErrorSpy = vi.spyOn(console, "error").mockImplementation(() => {});

    const { error, searchUsers } = useSearchUsersByAdminId();

    await searchUsers();

    expect(error.value).toBe("Failed to search users");

    consoleErrorSpy.mockRestore();
  });

  it("should clear previous error on new search", async () => {
    searchUsersByAdminIdRepository.searchUsersByAdminId = vi
      .fn()
      .mockResolvedValueOnce({
        success: false,
        message: "First error",
      })
      .mockResolvedValueOnce({
        success: true,
        result: {
          items: [],
          totalCount: 0,
        },
      });

    const { error, searchUsers } = useSearchUsersByAdminId();

    await searchUsers();
    expect(error.value).toBe("First error");

    // Second call succeeds
    await searchUsers();
    expect(error.value).toBeNull();
  });

  it("should handle null result items gracefully", async () => {
    const mockResult = {
      success: true,
      result: {
        items: null,
        totalCount: null,
      },
    };
    searchUsersByAdminIdRepository.searchUsersByAdminId = vi.fn().mockResolvedValue(mockResult);

    const { searchResults, totalCount, searchUsers } = useSearchUsersByAdminId();

    await searchUsers();

    expect(searchResults.value).toEqual([]);
    expect(totalCount.value).toBe(0);
  });

  it("should handle undefined result gracefully", async () => {
    const mockResult = {
      success: true,
      result: undefined,
    };
    searchUsersByAdminIdRepository.searchUsersByAdminId = vi.fn().mockResolvedValue(mockResult);

    const { searchResults, totalCount, searchUsers } = useSearchUsersByAdminId();

    await searchUsers();

    expect(searchResults.value).toEqual([]);
    expect(totalCount.value).toBe(0);
  });

  it("should update searchTerm when provided in params", async () => {
    const mockResult = {
      success: true,
      result: {
        items: [],
        totalCount: 0,
      },
    };
    searchUsersByAdminIdRepository.searchUsersByAdminId = vi.fn().mockResolvedValue(mockResult);

    const { searchTerm, searchUsers } = useSearchUsersByAdminId();

    await searchUsers({ searchTerm: "test query" });

    expect(searchTerm.value).toBe("test query");
  });

  it("should clearSearch reset all values", () => {
    const { searchResults, searchTerm, currentPage, error, totalCount, pageSize, clearSearch } =
      useSearchUsersByAdminId();

    // Set some values
    searchResults.value = [{ id: 1, name: "Test" }];
    searchTerm.value = "test";
    currentPage.value = 5;
    error.value = "Some error";
    totalCount.value = 100;
    pageSize.value = 25;

    // Clear search
    clearSearch();

    expect(searchResults.value).toEqual([]);
    expect(searchTerm.value).toBe("");
    expect(currentPage.value).toBe(1);
    expect(error.value).toBeNull();
    expect(totalCount.value).toBe(0);
    expect(pageSize.value).toBe(10);
  });

  it("should resetState reset all values", () => {
    const { searchResults, isSearching, error, totalCount, currentPage, searchTerm, resetState } =
      useSearchUsersByAdminId();

    // Set some values
    searchResults.value = [{ id: 1, name: "Test" }];
    isSearching.value = true;
    error.value = "Some error";
    totalCount.value = 100;
    currentPage.value = 5;
    searchTerm.value = "test";

    // Reset state
    resetState();

    expect(searchResults.value).toEqual([]);
    expect(isSearching.value).toBe(false);
    expect(error.value).toBeNull();
    expect(totalCount.value).toBe(0);
    expect(currentPage.value).toBe(1);
    expect(searchTerm.value).toBe("");
  });
});
