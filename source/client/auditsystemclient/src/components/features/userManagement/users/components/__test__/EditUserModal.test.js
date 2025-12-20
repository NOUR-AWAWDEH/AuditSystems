import { describe, it, expect, vi, beforeEach } from "vitest";
import { mount, flushPromises } from "@vue/test-utils";

import EditUserModal from "../EditUserModal.vue";

// Mock child components to simplify testing
vi.mock("@/components/base/BaseButton.vue", () => ({
  default: {
    template:
      '<button :disabled="disabled" @click="$emit(\'click\')"><slot /><slot name="loading" v-if="isLoading" /></button>',
    props: ["isLoading", "disabled"],
    emits: ["click"],
  },
}));

vi.mock("@/components/base/BaseInput.vue", () => ({
  default: {
    template:
      '<input :id="id" :value="modelValue" @input="$emit(\'update:modelValue\', $event.target.value)" />',
    props: ["modelValue", "id", "placeholder", "type", "inputClass"],
    emits: ["update:modelValue"],
  },
}));

vi.mock("@/components/features/userManagement/users/components/RoleSelector.vue", () => ({
  default: {
    name: "RoleSelector",
    template:
      '<select :value="modelValue" @change="$emit(\'update:modelValue\', $event.target.value)"><option value="Admin">Admin</option><option value="User">User</option></select>',
    props: ["modelValue"],
    emits: ["update:modelValue"],
  },
}));

vi.mock("@/components/features/userManagement/jobTitle/components/JobTitleSelector.vue", () => ({
  default: {
    name: "JobTitleSelector",
    template:
      '<select @change="handleChange($event)"><option value="">Select</option><option value="job1">Developer</option></select>',
    props: ["modelValue", "placeholder", "emitObject"],
    emits: ["update:modelValue"],
    methods: {
      handleChange(event) {
        const value = event.target.value;
        if (this.emitObject && value) {
          this.$emit("update:modelValue", { id: value, name: "Developer" });
        } else {
          this.$emit("update:modelValue", value);
        }
      },
    },
  },
}));

vi.mock(
  "@/components/features/Organization/subdepartment/components/SubDepartmentSelector.vue",
  () => ({
    default: {
      name: "SubDepartmentSelector",
      template:
        '<select @change="handleChange($event)"><option value="">Select</option><option value="sub1">SubDep</option></select>',
      props: ["modelValue", "placeholder", "emitObject"],
      emits: ["update:modelValue"],
      methods: {
        handleChange(event) {
          const value = event.target.value;
          if (this.emitObject && value) {
            this.$emit("update:modelValue", { id: value, name: "SubDep", departmentId: "dept1" });
          } else {
            this.$emit("update:modelValue", value);
          }
        },
      },
    },
  })
);

// Mock composable
const mockUpdateUser = vi.fn();
const mockIsUpdating = { value: false };
const mockError = { value: null };

vi.mock("@/composables/userManagement/users/useUpdateUser", () => ({
  useUpdateUser: () => ({
    updateUser: mockUpdateUser,
    isUpdating: mockIsUpdating,
    error: mockError,
  }),
}));

describe("EditUserModal.vue", () => {
  const mockJobTitle = { id: "job1", name: "Developer", companyId: "company1" };
  const mockSubDepartment = { id: "sub1", name: "Engineering", departmentId: "dept1" };

  const mockUser = {
    id: "user-123",
    firstName: "John",
    lastName: "Doe",
    userName: "johndoe",
    email: "john@example.com",
    role: "Admin",
    jobTitle: mockJobTitle,
    subDepartment: mockSubDepartment,
  };

  beforeEach(() => {
    vi.clearAllMocks();
    mockIsUpdating.value = false;
    mockError.value = null;
  });

  it("does not render modal when isVisible is false", () => {
    const wrapper = mount(EditUserModal, {
      props: { isVisible: false, user: mockUser },
    });
    expect(wrapper.find(".fixed").exists()).toBe(false);
  });

  it("renders modal when isVisible is true", () => {
    const wrapper = mount(EditUserModal, {
      props: { isVisible: true, user: mockUser },
    });
    expect(wrapper.find(".fixed").exists()).toBe(true);
    expect(wrapper.text()).toContain("Edit User");
  });

  it("populates form fields with user data", async () => {
    const wrapper = mount(EditUserModal, {
      props: { isVisible: true, user: mockUser },
    });
    await flushPromises();

    expect(wrapper.find("#edit-firstName").element.value).toBe("John");
    expect(wrapper.find("#edit-lastName").element.value).toBe("Doe");
    expect(wrapper.find("#edit-userName").element.value).toBe("johndoe");
    expect(wrapper.find("#edit-email").element.value).toBe("john@example.com");
  });

  it("emits close event when cancel button is clicked", async () => {
    const wrapper = mount(EditUserModal, {
      props: { isVisible: true, user: mockUser },
    });
    await flushPromises();

    // Find all BaseButton components and get the Cancel button (first one)
    const baseButtonComponents = wrapper.findAllComponents({ name: "BaseButton" });
    const cancelButton = baseButtonComponents.find((btn) => btn.text().includes("Cancel"));

    // Emit click event from the component to trigger parent's @click handler
    cancelButton.vm.$emit("click");
    await flushPromises();

    expect(wrapper.emitted("close")).toBeTruthy();
  });

  it("calls updateUser with correct payload on form submission", async () => {
    mockUpdateUser.mockResolvedValue({ success: true });

    const wrapper = mount(EditUserModal, {
      props: { isVisible: true, user: mockUser },
    });
    await flushPromises();

    // Modify a field
    await wrapper.find("#edit-firstName").setValue("Jane");

    // Submit form
    await wrapper.find("form").trigger("submit.prevent");
    await flushPromises();

    expect(mockUpdateUser).toHaveBeenCalledWith({
      id: "user-123",
      firstName: "Jane",
      lastName: "Doe",
      userName: "johndoe",
      email: "john@example.com",
      role: "Admin",
      jobTitle: mockJobTitle,
      subDepartment: mockSubDepartment,
    });
  });

  it("emits user-updated and close events on successful update", async () => {
    mockUpdateUser.mockResolvedValue({ success: true });

    const wrapper = mount(EditUserModal, {
      props: { isVisible: true, user: mockUser },
    });
    await flushPromises();

    await wrapper.find("form").trigger("submit.prevent");
    await flushPromises();

    expect(wrapper.emitted("user-updated")).toBeTruthy();
    expect(wrapper.emitted("close")).toBeTruthy();
  });

  it("does not emit events on failed update", async () => {
    mockUpdateUser.mockResolvedValue({ success: false });

    const wrapper = mount(EditUserModal, {
      props: { isVisible: true, user: mockUser },
    });
    await flushPromises();

    await wrapper.find("form").trigger("submit.prevent");
    await flushPromises();

    expect(wrapper.emitted("user-updated")).toBeFalsy();
    expect(wrapper.emitted("close")).toBeFalsy();
  });

  it("displays error message when updateError is set", async () => {
    mockError.value = "Update failed";

    const wrapper = mount(EditUserModal, {
      props: { isVisible: true, user: mockUser },
    });

    expect(wrapper.text()).toContain("Update failed");
  });

  it("disables submit button when required fields are empty", async () => {
    const wrapper = mount(EditUserModal, {
      props: { isVisible: true, user: { ...mockUser, firstName: "" } },
    });
    await flushPromises();

    const submitButton = wrapper
      .findAll("button")
      .find((btn) => btn.text().includes("Update User"));
    expect(submitButton.attributes("disabled")).toBeDefined();
  });

  it("does not submit when user id is missing", async () => {
    const wrapper = mount(EditUserModal, {
      props: { isVisible: true, user: { ...mockUser, id: null } },
    });
    await flushPromises();

    await wrapper.find("form").trigger("submit.prevent");
    await flushPromises();

    expect(mockUpdateUser).not.toHaveBeenCalled();
  });

  it("updates form when user prop changes", async () => {
    const wrapper = mount(EditUserModal, {
      props: { isVisible: true, user: mockUser },
    });
    await flushPromises();

    expect(wrapper.find("#edit-firstName").element.value).toBe("John");

    // Update user prop
    await wrapper.setProps({
      user: { ...mockUser, firstName: "Jane" },
    });
    await flushPromises();

    expect(wrapper.find("#edit-firstName").element.value).toBe("Jane");
  });

  it("handles null jobTitle and subDepartment gracefully", async () => {
    mockUpdateUser.mockResolvedValue({ success: true });

    const userWithNulls = {
      ...mockUser,
      jobTitle: null,
      subDepartment: null,
    };

    const wrapper = mount(EditUserModal, {
      props: { isVisible: true, user: userWithNulls },
    });
    await flushPromises();

    await wrapper.find("form").trigger("submit.prevent");
    await flushPromises();

    expect(mockUpdateUser).toHaveBeenCalledWith(
      expect.objectContaining({
        jobTitle: null,
        subDepartment: null,
      })
    );
  });

  it("disables submit button when lastName is empty", async () => {
    const wrapper = mount(EditUserModal, {
      props: { isVisible: true, user: { ...mockUser, lastName: "" } },
    });
    await flushPromises();

    const submitButton = wrapper
      .findAll("button")
      .find((btn) => btn.text().includes("Update User"));
    expect(submitButton.attributes("disabled")).toBeDefined();
  });

  it("disables submit button when userName is empty", async () => {
    const wrapper = mount(EditUserModal, {
      props: { isVisible: true, user: { ...mockUser, userName: "" } },
    });
    await flushPromises();

    const submitButton = wrapper
      .findAll("button")
      .find((btn) => btn.text().includes("Update User"));
    expect(submitButton.attributes("disabled")).toBeDefined();
  });

  it("disables submit button when email is empty", async () => {
    const wrapper = mount(EditUserModal, {
      props: { isVisible: true, user: { ...mockUser, email: "" } },
    });
    await flushPromises();

    const submitButton = wrapper
      .findAll("button")
      .find((btn) => btn.text().includes("Update User"));
    expect(submitButton.attributes("disabled")).toBeDefined();
  });

  it("disables buttons while updating", async () => {
    mockIsUpdating.value = true;

    const wrapper = mount(EditUserModal, {
      props: { isVisible: true, user: mockUser },
    });
    await flushPromises();

    const cancelButton = wrapper.findAll("button").find((btn) => btn.text().includes("Cancel"));
    expect(cancelButton.attributes("disabled")).toBeDefined();
  });

  it("trims whitespace from text fields before submission", async () => {
    mockUpdateUser.mockResolvedValue({ success: true });

    const userWithWhitespace = {
      ...mockUser,
      firstName: "  John  ",
      lastName: "  Doe  ",
      userName: "  johndoe  ",
      email: "  john@example.com  ",
    };

    const wrapper = mount(EditUserModal, {
      props: { isVisible: true, user: userWithWhitespace },
    });
    await flushPromises();

    await wrapper.find("form").trigger("submit.prevent");
    await flushPromises();

    expect(mockUpdateUser).toHaveBeenCalledWith(
      expect.objectContaining({
        firstName: "John",
        lastName: "Doe",
        userName: "johndoe",
        email: "john@example.com",
      })
    );
  });
});
