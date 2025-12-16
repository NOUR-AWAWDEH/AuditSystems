import { describe, it, expect, vi, beforeEach } from "vitest";
import { mount } from "@vue/test-utils";
import AddUserForm from "../AddUserForm.vue";
import authService from "@/services/auth/authRepos/authService";

// Mock child components to simplify testing
vi.mock("@/components/base/BaseButton.vue", () => ({
  default: {
    template: "<button><slot /></button>",
    props: ["loading", "disabled"],
  },
}));

vi.mock("@/components/base/BaseInput.vue", () => ({
  default: {
    template:
      '<input :id="id" :value="modelValue" @input="$emit(\'update:modelValue\', $event.target.value)" />',
    props: ["modelValue", "id", "placeholder", "required"],
    emits: ["update:modelValue"],
  },
}));

vi.mock("@/components/base/PasswordInput.vue", () => ({
  default: {
    template:
      '<input type="password" :id="id" :value="modelValue" @input="$emit(\'update:modelValue\', $event.target.value)" />',
    props: ["modelValue", "id", "required"],
    emits: ["update:modelValue"],
  },
}));

vi.mock("./RoleVue.vue", () => ({
  default: {
    name: "RoleVue",
    template:
      '<select :value="modelValue" @change="$emit(\'update:modelValue\', $event.target.value)"><option value="Role">Role</option></select>',
    props: ["modelValue"],
    emits: ["update:modelValue"],
  },
}));

vi.mock("@/components/features/userManagement/jobTitle/components/JobTitleSelector.vue", () => ({
  default: {
    name: "JobTitleSelector",
    template:
      '<select :value="modelValue" @change="$emit(\'update:modelValue\', $event.target.value)"><option value="1">Job</option></select>',
    props: ["modelValue", "placeholder"],
    emits: ["update:modelValue"],
  },
}));

vi.mock(
  "@/components/features/userManagement/subDepartment/components/SubDepartmentSelector.vue",
  () => ({
    default: {
      name: "SubDepartmentSelector",
      template:
        '<select :value="modelValue" @change="$emit(\'update:modelValue\', $event.target.value)"><option value="1">SubDep</option></select>',
      props: ["modelValue", "placeholder"],
      emits: ["update:modelValue"],
    },
  })
);

// Mock authService
vi.mock("@/services/auth/authRepos/authService", () => ({
  default: {
    getLocalCompanyId: vi.fn(),
  },
}));

// Mock composable
const mockCreateUser = vi.fn();
const mockClearState = vi.fn();
const mockIsCreating = { value: false };
const mockError = { value: null };

vi.mock("@/composables/userManagement/users/useCreateUser", () => ({
  useCreateUser: () => ({
    createUser: mockCreateUser,
    isCreating: mockIsCreating,
    error: mockError,
    clearState: mockClearState,
  }),
}));

describe("AddUserForm.vue", () => {
  beforeEach(() => {
    vi.clearAllMocks();
    mockIsCreating.value = false;
    mockError.value = null;
    vi.mocked(authService.getLocalCompanyId).mockReturnValue("test-company-id");
  });

  it('renders the "Add User" toggle button initially', () => {
    const wrapper = mount(AddUserForm);
    expect(wrapper.text()).toContain("Add User");
    expect(wrapper.find("form").exists()).toBe(false);
  });

  it("toggles the form visibility when button is clicked", async () => {
    const wrapper = mount(AddUserForm);
    const button = wrapper.findComponent({ name: "BaseButton" });

    await button.trigger("click");
    expect(wrapper.find("form").exists()).toBe(true);
    expect(button.text()).toContain("Cancel");

    await button.trigger("click");
    expect(wrapper.find("form").exists()).toBe(false);
  });

  it("populates companyId from authService on mount", () => {
    const wrapper = mount(AddUserForm);
    expect(authService.getLocalCompanyId).toHaveBeenCalled();
  });

  it("calls createUser with form data on submission", async () => {
    vi.mocked(authService.getLocalCompanyId).mockReturnValue("123");
    mockCreateUser.mockResolvedValue({ success: true });

    const wrapper = mount(AddUserForm);
    await wrapper.findComponent({ name: "BaseButton" }).trigger("click");

    // Fill form
    await wrapper.find("#user-first-name").setValue("John");
    await wrapper.find("#user-last-name").setValue("Doe");
    await wrapper.find("#user-name").setValue("johndoe");
    await wrapper.find("#user-email").setValue("john@example.com");
    await wrapper.find("#user-password").setValue("password123");
    await wrapper.find("#user-confirm-password").setValue("password123");

    // For mocked selectors/role, we simulate change
    await wrapper.findComponent({ name: "RoleVue" }).vm.$emit("update:modelValue", "Admin");
    await wrapper.findComponent({ name: "JobTitleSelector" }).vm.$emit("update:modelValue", "job1");
    await wrapper
      .findComponent({ name: "SubDepartmentSelector" })
      .vm.$emit("update:modelValue", "sub1");

    await wrapper.find("form").trigger("submit.prevent");

    expect(mockCreateUser).toHaveBeenCalledWith({
      firstName: "John",
      lastName: "Doe",
      userName: "johndoe",
      email: "john@example.com",
      password: "password123",
      confirmPassword: "password123",
      roleName: "Admin",
      jobTitleId: "job1",
      subDepartmentId: "sub1",
      companyId: "123",
    });
  });

  it("closes form and resets state on successful submission", async () => {
    mockCreateUser.mockResolvedValue({ success: true });
    const wrapper = mount(AddUserForm);

    // Open form
    await wrapper.findComponent({ name: "BaseButton" }).trigger("click");
    expect(wrapper.find("form").exists()).toBe(true);

    // Submit
    await wrapper.find("form").trigger("submit.prevent");

    // Should close form
    expect(wrapper.find("form").exists()).toBe(false);
    expect(mockClearState).toHaveBeenCalled();
    expect(authService.getLocalCompanyId).toHaveBeenCalledTimes(2); // Once on mount, once on reset
  });

  it("does not close form on failed submission", async () => {
    mockCreateUser.mockResolvedValue({ success: false });
    const wrapper = mount(AddUserForm);

    // Open form
    await wrapper.findComponent({ name: "BaseButton" }).trigger("click");

    // Submit
    await wrapper.find("form").trigger("submit.prevent");

    // Should stay open
    expect(wrapper.find("form").exists()).toBe(true);
  });
});