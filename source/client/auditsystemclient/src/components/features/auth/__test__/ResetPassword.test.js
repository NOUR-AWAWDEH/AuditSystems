import { describe, it, expect, vi, beforeEach } from "vitest";
import { mount } from "@vue/test-utils";
import ResetPassword from "../ResetPassword.vue";
import { useResetPassword } from "@/composables/password/useResetPassword";
import { useNotification } from "@/composables/notification/useNotification";
import validatePassword from "@/services/validations/validatePassword";

// --- Mocks Setup ---

// 1. Mock validatePassword (Default Export)
vi.mock("@/services/validations/validatePassword");

// 2. Mock useNotification and useResetPassword (Named Exports)
vi.mock("@/composables/notification/useNotification");
vi.mock("@/composables/password/useResetPassword");

// 3. Mock vue-router with convenient handles
const mockPush = vi.fn();
const mockUseRoute = vi.fn();

vi.mock("vue-router", () => ({
  useRouter: () => ({ push: mockPush }),
  useRoute: (...args) => mockUseRoute(...args),
}));

describe("ResetPassword Component Tests", () => {
  let mockResetPasswordAction;
  let mockShowNotification;

  beforeEach(() => {
    vi.clearAllMocks();

    mockResetPasswordAction = vi.fn();
    mockShowNotification = vi.fn();

    // Setup Mock Implementations
    vi.mocked(useResetPassword).mockReturnValue({
      resetPassword: mockResetPasswordAction,
    });

    vi.mocked(useNotification).mockReturnValue({
      showNotification: mockShowNotification,
    });

    vi.mocked(validatePassword).mockReturnValue(null); // Default: Valid

    // Default Route Mock
    mockUseRoute.mockReturnValue({
      query: { email: "test@example.com", token: "valid-token" },
    });
  });

  const globalOptions = {
    stubs: {
      BaseButton: {
        template: "<button @click='$emit(\"click\")'><slot /></button>",
      },
      PasswordInput: {
        template:
          "<input class='pwd-input' :value='modelValue' @input='$emit(\"update:modelValue\", $event.target.value)' />",
        props: ["modelValue"],
      },
      H2Heading: {
        template: "<h2><slot /></h2>",
      },
      AuditLogo: true,
      BaseParagraph: true,
    },
  };

  it("should render correctly", () => {
    const wrapper = mount(ResetPassword, { global: globalOptions });
    expect(wrapper.find("h2").text()).toBe("Reset Password");
  });

  it("should show error notification if token or email is missing", () => {
    mockUseRoute.mockReturnValue({ query: {} }); // Override route for this test

    mount(ResetPassword, { global: globalOptions });

    expect(mockShowNotification).toHaveBeenCalledWith(
      "Missing reset token or email. Please request a password reset.",
      "error"
    );
  });

  it("should show error notification if fields are empty", async () => {
    const wrapper = mount(ResetPassword, { global: globalOptions });

    await wrapper.find("button").trigger("click");

    expect(mockShowNotification).toHaveBeenCalledWith("Please fill in all fields.", "error");
  });

  it("should show error notification if passwords do not match", async () => {
    const wrapper = mount(ResetPassword, { global: globalOptions });

    const inputs = wrapper.findAll(".pwd-input");
    await inputs[0].setValue("Password123!");
    await inputs[1].setValue("DifferentPassword123!");

    await wrapper.find("button").trigger("click");

    expect(mockShowNotification).toHaveBeenCalledWith("Passwords do not match.", "error");
  });

  it("should show error notification if password validation fails", async () => {
    vi.mocked(validatePassword).mockReturnValue("Password too weak");

    const wrapper = mount(ResetPassword, { global: globalOptions });

    const inputs = wrapper.findAll(".pwd-input");
    await inputs[0].setValue("weak");
    await inputs[1].setValue("weak");

    await wrapper.find("button").trigger("click");

    expect(mockShowNotification).toHaveBeenCalledWith("Password too weak", "error");
  });

  it("should call resetPassword and show success notification", async () => {
    mockResetPasswordAction.mockResolvedValue({
      success: true,
      successMessage: "Password reset successful",
    });

    const wrapper = mount(ResetPassword, { global: globalOptions });

    const inputs = wrapper.findAll(".pwd-input");
    await inputs[0].setValue("ValidPass123!");
    await inputs[1].setValue("ValidPass123!");

    await wrapper.find("button").trigger("click");

    expect(mockResetPasswordAction).toHaveBeenCalledWith({
      email: "test@example.com",
      token: "valid-token",
      newPassword: "ValidPass123!",
      confirmNewPassword: "ValidPass123!",
    });
    expect(mockShowNotification).toHaveBeenCalledWith("Password reset successful", "success");
  });

  it("should show error notification on API failure", async () => {
    mockResetPasswordAction.mockResolvedValue({
      success: false,
      message: "Token expired",
    });

    const wrapper = mount(ResetPassword, { global: globalOptions });

    const inputs = wrapper.findAll(".pwd-input");
    await inputs[0].setValue("ValidPass123!");
    await inputs[1].setValue("ValidPass123!");

    await wrapper.find("button").trigger("click");

    expect(mockShowNotification).toHaveBeenCalledWith("Token expired", "error");
  });
});
