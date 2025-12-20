import { describe, it, expect, vi, beforeEach } from "vitest";
import { mount, flushPromises } from "@vue/test-utils";
import UsersTable from "../UsersTable.vue";

// Mock child components
vi.mock("./UserHeader.vue", () => ({
  default: {
    name: "UserHeader",
    template: '<div class="mock-user-header">User Header</div>',
  },
}));

vi.mock("@/components/common/ReusableTable.vue", () => ({
  default: {
    name: "ReusableTable",
    template: `
      <div class="mock-reusable-table">
        <div v-for="item in items" :key="item.id" class="table-row" :data-id="item.id">
          <slot name="cell-subDepartment" :item="item"></slot>
          <slot name="cell-jobTitle" :item="item"></slot>
          <slot name="cell-actions" :item="item"></slot>
        </div>
        <div v-if="items.length === 0" class="no-data">{{ noDataMessage }}</div>
      </div>
    `,
    props: ["items", "columns", "isLoading", "actions", "noDataMessage"],
  },
}));

vi.mock("@/components/common/ReusableMobileCardList.vue", () => ({
  default: {
    name: "ReusableMobileCardList",
    template: `
      <div class="mock-mobile-card-list lg:hidden">
        <div v-for="item in items" :key="item.id" class="card" :data-id="item.id">
          <slot name="card-content" :item="item"></slot>
          <slot name="card-actions" :item="item"></slot>
        </div>
        <div v-if="items.length === 0" class="no-data">{{ noDataMessage }}</div>
      </div>
    `,
    props: ["items", "showNumber", "isLoading", "noDataMessage"],
  },
}));

vi.mock("@/components/common/SearchFilter.vue", () => ({
  default: {
    name: "SearchFilter",
    template: `
      <div class="mock-search-filter">
        <input 
          :value="search" 
          @input="$emit('update:search', $event.target.value)"
          :placeholder="searchPlaceholder"
        />
      </div>
    `,
    props: ["search", "searchPlaceholder"],
    emits: ["update:search"],
  },
}));

vi.mock("@/components/common/icons/EditIcon.vue", () => ({
  default: {
    name: "EditIcon",
    template: '<span class="edit-icon">Edit</span>',
    props: ["size", "customClass"],
  },
}));

vi.mock("@/components/common/icons/DeleteIcon.vue", () => ({
  default: {
    name: "DeleteIcon",
    template: '<span class="delete-icon">Delete</span>',
    props: ["size", "customClass"],
  },
}));

describe("UsersTable.vue", () => {
  const mockUsers = [
    {
      id: "user-1",
      userName: "johndoe",
      firstName: "John",
      lastName: "Doe",
      email: "john@example.com",
      role: "Admin",
      subDepartment: { id: "sd1", name: "Engineering", departmentId: "d1" },
      jobTitle: { id: "jt1", name: "Developer", companyId: "c1" },
    },
    {
      id: "user-2",
      userName: "janesmith",
      firstName: "Jane",
      lastName: "Smith",
      email: "jane@example.com",
      role: "User",
      subDepartment: null,
      jobTitle: null,
    },
  ];

  const defaultProps = {
    users: mockUsers,
    isLoading: false,
    searchQuery: "",
    handleSearch: vi.fn(),
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe("Rendering", () => {
    it("renders the component", () => {
      const wrapper = mount(UsersTable, { props: defaultProps });
      expect(wrapper.find(".bg-white").exists()).toBe(true);
    });

    it("renders UserHeader component", () => {
      const wrapper = mount(UsersTable, { props: defaultProps });
      // UserHeader is rendered within the component
      const userHeader = wrapper.findComponent({ name: "UserHeader" });
      expect(userHeader.exists()).toBe(true);
    });

    it("renders SearchFilter component", () => {
      const wrapper = mount(UsersTable, { props: defaultProps });
      expect(wrapper.find(".mock-search-filter").exists()).toBe(true);
    });

    it("renders ReusableTable for desktop view", () => {
      const wrapper = mount(UsersTable, { props: defaultProps });
      expect(wrapper.find(".mock-reusable-table").exists()).toBe(true);
    });

    it("renders ReusableMobileCardList for mobile view", () => {
      const wrapper = mount(UsersTable, { props: defaultProps });
      expect(wrapper.find(".mock-mobile-card-list").exists()).toBe(true);
    });

    it("passes users to ReusableTable", () => {
      const wrapper = mount(UsersTable, { props: defaultProps });
      const tableRows = wrapper.findAll(".table-row");
      expect(tableRows.length).toBe(2);
    });
  });

  describe("SubDepartment Display", () => {
    it("displays subDepartment name when available", () => {
      const wrapper = mount(UsersTable, { props: defaultProps });
      const firstRow = wrapper.find('[data-id="user-1"]');
      expect(firstRow.text()).toContain("Engineering");
    });

    it("displays 'Not assigned' when subDepartment is null", () => {
      const wrapper = mount(UsersTable, { props: defaultProps });
      const secondRow = wrapper.find('[data-id="user-2"]');
      expect(secondRow.text()).toContain("Not assigned");
    });
  });

  describe("JobTitle Display", () => {
    it("displays jobTitle name when available", () => {
      const wrapper = mount(UsersTable, { props: defaultProps });
      const firstRow = wrapper.find('[data-id="user-1"]');
      expect(firstRow.text()).toContain("Developer");
    });

    it("displays 'Not assigned' when jobTitle is null", () => {
      const wrapper = mount(UsersTable, { props: defaultProps });
      const secondRow = wrapper.find('[data-id="user-2"]');
      // Should have "Not assigned" for both jobTitle and subDepartment
      const notAssignedCount = (secondRow.text().match(/Not assigned/g) || []).length;
      expect(notAssignedCount).toBeGreaterThanOrEqual(2);
    });
  });

  describe("Action Buttons", () => {
    it("renders edit button for each user", () => {
      const wrapper = mount(UsersTable, { props: defaultProps });
      const editButtons = wrapper.findAll("button").filter((btn) => btn.text().includes("Edit"));
      expect(editButtons.length).toBeGreaterThan(0);
    });

    it("renders delete button for each user", () => {
      const wrapper = mount(UsersTable, { props: defaultProps });
      const deleteButtons = wrapper
        .findAll("button")
        .filter((btn) => btn.text().includes("Delete"));
      expect(deleteButtons.length).toBeGreaterThan(0);
    });

    it("emits edit-user event when edit button is clicked", async () => {
      const wrapper = mount(UsersTable, { props: defaultProps });
      const editButton = wrapper.find('[data-id="user-1"] button');
      await editButton.trigger("click");

      expect(wrapper.emitted("edit-user")).toBeTruthy();
      expect(wrapper.emitted("edit-user")[0][0]).toEqual(mockUsers[0]);
    });

    it("emits delete-user event when delete button is clicked", async () => {
      const wrapper = mount(UsersTable, { props: defaultProps });
      const buttons = wrapper.findAll('[data-id="user-1"] button');
      const deleteButton = buttons.find((btn) => btn.text().includes("Delete"));
      await deleteButton.trigger("click");

      expect(wrapper.emitted("delete-user")).toBeTruthy();
      expect(wrapper.emitted("delete-user")[0][0]).toEqual(mockUsers[0]);
    });
  });

  describe("Loading State", () => {
    it("shows loading indicator when isLoading is true", () => {
      const wrapper = mount(UsersTable, {
        props: { ...defaultProps, isLoading: true },
      });
      expect(wrapper.find(".animate-spin").exists()).toBe(true);
      expect(wrapper.text()).toContain("Loading users...");
    });

    it("does not show loading indicator when isLoading is false", () => {
      const wrapper = mount(UsersTable, {
        props: { ...defaultProps, isLoading: false },
      });
      expect(wrapper.find(".animate-spin").exists()).toBe(false);
    });
  });

  describe("No Data Message", () => {
    it("shows default no data message when users array is empty", () => {
      const wrapper = mount(UsersTable, {
        props: { ...defaultProps, users: [] },
      });
      expect(wrapper.find(".no-data").text()).toContain("No users found yet. Please add a user.");
    });

    it("shows search-specific no data message when searchQuery is provided", () => {
      const wrapper = mount(UsersTable, {
        props: { ...defaultProps, users: [], searchQuery: "test" },
      });
      expect(wrapper.find(".no-data").text()).toContain('No users found for "test".');
    });
  });

  describe("Search Filter", () => {
    it("passes searchQuery to SearchFilter", () => {
      const wrapper = mount(UsersTable, {
        props: { ...defaultProps, searchQuery: "john" },
      });
      const input = wrapper.find(".mock-search-filter input");
      expect(input.element.value).toBe("john");
    });

    it("has correct placeholder text", () => {
      const wrapper = mount(UsersTable, { props: defaultProps });
      const input = wrapper.find(".mock-search-filter input");
      expect(input.attributes("placeholder")).toBe("Search users...");
    });
  });

  describe("Columns Configuration", () => {
    it("includes all expected columns", () => {
      const wrapper = mount(UsersTable, { props: defaultProps });
      const vm = wrapper.vm;

      // Access the columns through the component
      const expectedColumns = [
        "userName",
        "firstName",
        "lastName",
        "email",
        "role",
        "subDepartment",
        "jobTitle",
        "actions",
      ];

      // The ReusableTable receives enhancedColumns which includes actions
      const table = wrapper.findComponent({ name: "ReusableTable" });
      const columns = table.props("columns");

      expectedColumns.forEach((key) => {
        expect(columns.some((col) => col.key === key)).toBe(true);
      });
    });
  });

  describe("Empty Users Array", () => {
    it("handles empty users array gracefully", () => {
      const wrapper = mount(UsersTable, {
        props: { ...defaultProps, users: [] },
      });
      expect(wrapper.findAll(".table-row").length).toBe(0);
    });
  });

  describe("Props", () => {
    it("uses default values when props are not provided", () => {
      const wrapper = mount(UsersTable, {
        props: { users: [] },
      });
      expect(wrapper.exists()).toBe(true);
    });
  });
});
