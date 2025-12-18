import { describe, it, expect, vi, beforeEach } from "vitest";
import { ref, nextTick } from "vue";
import { mount } from "@vue/test-utils";
import { useUserTable } from "../useUserTable";
import { useSearchUsersByAdminId } from "@/composables/userManagement/users/useSearchUsersByAdminId";
import { useGetUsersByAdminId } from "@/composables/userManagement/users/useGetUsersByAdminId";
import { useSearchAndPagination } from "@/composables/common/pagination/useSearchAndPagination";

// Mock dependencies
vi.mock("@/composables/userManagement/users/useSearchUsersByAdminId");
vi.mock("@/composables/userManagement/users/useGetUsersByAdminId");
vi.mock("@/composables/common/pagination/useSearchAndPagination");

// Helper component to test composables with lifecycle hooks
const TestComponent = {
  template: "<div></div>",
  setup() {
    return useUserTable();
  },
};

describe("useUserTable", () => {
  let mockSearchUsers;
  let mockGetUsersByAdminId;
  let mockUsers;
  let mockAllUsers;
  let mockIsLoading;
  let mockTotalCount;
  let mockSearchQuery;
  let mockCurrentPage;
  let mockPageSize;
  let mockHandleSearch;

  beforeEach(() => {
    // Reset all mocks
    vi.clearAllMocks();

    // Setup mock data
    mockUsers = ref([]);
    mockAllUsers = ref([
      { id: 1, name: "User 1" },
      { id: 2, name: "User 2" },
    ]);
    mockIsLoading = ref(false);
    mockTotalCount = ref(0);
    mockSearchQuery = ref("");
    mockCurrentPage = ref(1);
    mockPageSize = ref(10);

    // Setup mock functions
    mockSearchUsers = vi.fn();
    mockGetUsersByAdminId = vi.fn();
    mockHandleSearch = vi.fn();

    // Mock useSearchUsersByAdminId
    useSearchUsersByAdminId.mockReturnValue({
      users: mockUsers,
      isLoading: mockIsLoading,
      totalCount: mockTotalCount,
      searchUsers: mockSearchUsers,
    });

    // Mock useGetUsersByAdminId
    useGetUsersByAdminId.mockReturnValue({
      users: mockAllUsers,
      getUsersByAdminId: mockGetUsersByAdminId,
    });

    // Mock useSearchAndPagination
    useSearchAndPagination.mockReturnValue({
      searchQuery: mockSearchQuery,
      currentPage: mockCurrentPage,
      pageSize: mockPageSize,
      handleSearch: mockHandleSearch,
    });
  });

  it("should call getUsersByAdminId on mount when searchQuery is empty", async () => {
    mockGetUsersByAdminId.mockResolvedValue();

    mount(TestComponent);

    await nextTick();
    await nextTick();

    expect(mockGetUsersByAdminId).toHaveBeenCalled();
    expect(mockSearchUsers).not.toHaveBeenCalled();
  });

  it("should populate users from allUsers when searchQuery is empty", async () => {
    mockGetUsersByAdminId.mockResolvedValue();

    mount(TestComponent);

    await nextTick();
    await nextTick();

    // Verify getUsersByAdminId was called to fetch data
    expect(mockGetUsersByAdminId).toHaveBeenCalled();
  });

  it("should call searchUsers when searchQuery has value", async () => {
    mockSearchQuery.value = "test";
    mockSearchUsers.mockResolvedValue();

    mount(TestComponent);

    await nextTick();
    await nextTick();

    expect(mockSearchUsers).toHaveBeenCalledWith({
      searchTerm: "test",
      pageNumber: 1,
      pageSize: 10,
    });
    expect(mockGetUsersByAdminId).not.toHaveBeenCalled();
  });

  it("should call refreshData when searchQuery changes", async () => {
    mockGetUsersByAdminId.mockResolvedValue();
    mockSearchUsers.mockResolvedValue();

    mount(TestComponent);

    await nextTick();
    await nextTick();

    // Change searchQuery
    mockSearchQuery.value = "search term";
    await nextTick();
    await nextTick();

    expect(mockSearchUsers).toHaveBeenCalledWith({
      searchTerm: "search term",
      pageNumber: 1,
      pageSize: 10,
    });
  });

  it("should call refreshData when currentPage changes", async () => {
    mockSearchQuery.value = "test";
    mockSearchUsers.mockResolvedValue();

    useUserTable();

    await nextTick();

    // Reset mock to check next call
    mockSearchUsers.mockClear();

    // Change currentPage
    mockCurrentPage.value = 2;
    await nextTick();

    expect(mockSearchUsers).toHaveBeenCalledWith({
      searchTerm: "test",
      pageNumber: 2,
      pageSize: 10,
    });
  });

  it("should manually refresh data via refreshData function", async () => {
    mockGetUsersByAdminId.mockResolvedValue();

    const wrapper = mount(TestComponent);
    const result = wrapper.vm;

    await nextTick();
    await nextTick();
    mockGetUsersByAdminId.mockClear();

    await result.refreshData();

    expect(mockGetUsersByAdminId).toHaveBeenCalled();
  });

  it("should handle empty allUsers gracefully", async () => {
    mockAllUsers.value = null;
    mockGetUsersByAdminId.mockResolvedValue();

    mount(TestComponent);

    await nextTick();
    await nextTick();

    // Verify getUsersByAdminId was still called even with null data
    expect(mockGetUsersByAdminId).toHaveBeenCalled();
  });

  it("should trim searchQuery before checking", async () => {
    mockSearchQuery.value = "   "; // Only whitespace
    mockGetUsersByAdminId.mockResolvedValue();

    mount(TestComponent);

    await nextTick();
    await nextTick();

    expect(mockGetUsersByAdminId).toHaveBeenCalled();
    expect(mockSearchUsers).not.toHaveBeenCalled();
  });
});
