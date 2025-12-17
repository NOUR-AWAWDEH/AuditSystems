import { describe, it, expect, vi, beforeEach } from "vitest";
import { mount } from "@vue/test-utils";
import ResendVerificationEmailPage from "../ResendVerificationEmailPage.vue";
import { useVerificationEmail } from "@/composables/userManagement/confirmEmail/useSendVerificationEmail";
import { useNotification } from "@/composables/notification/useNotification";
import { validateEmail } from "@/services/validations/validateEmail";
import { ref } from "vue";

// Mock dependencies
vi.mock("@/composables/userManagement/confirmEmail/useSendVerificationEmail", () => ({
  useVerificationEmail: vi.fn(),
}));

vi.mock("@/composables/notification/useNotification", () => ({
  useNotification: vi.fn(),
}));

vi.mock("@/services/validations/validateEmail", () => ({
  validateEmail: vi.fn(),
}));

// Mock child components
vi.mock("@/components/base/BaseButton.vue", () => ({
  default: {
    template: "<button :disabled='disabled'><slot /></button>",
    props: ["disabled"],
  },
}));
vi.mock("@/components/base/BaseInput.vue", () => ({
  default: {
    template:
      '<input :value="modelValue" @input="$emit(\'update:modelValue\', $event.target.value)" />',
    props: ["modelValue"],
    emits: ["update:modelValue"],
  },
}));
vi.mock("@/components/common/AuditLogo.vue", () => ({ default: { template: "<div>Logo</div>" } }));
vi.mock("@/components/base/BaseParagraph.vue", () => ({
  default: { template: "<p><slot /></p>" },
}));
vi.mock("@/components/base/H2Heading.vue", () => ({ default: { template: "<h2><slot /></h2>" } }));
// Mock BaseLink to avoid router-link issues
vi.mock("@/components/base/BaseLink.vue", () => ({ default: { template: "<a><slot /></a>" } }));

describe("ResendVerificationEmailPage.vue", () => {
  let mockSendVerificationEmail;
  let mockShowNotification;

  beforeEach(() => {
    vi.clearAllMocks();

    mockSendVerificationEmail = vi.fn();
    useVerificationEmail.mockReturnValue({
      sendVerificationEmail: mockSendVerificationEmail,
      isLoading: ref(false),
    });

    mockShowNotification = vi.fn();
    useNotification.mockReturnValue({
      showNotification: mockShowNotification,
    });

    // Default to valid for happy path tests
    validateEmail.mockReturnValue(true);
  });

  it("should render correctly", () => {
    const wrapper = mount(ResendVerificationEmailPage);
    expect(wrapper.find("h2").text()).toBe("Resend Verification");
    expect(wrapper.find("input").exists()).toBe(true);
    expect(wrapper.find("button").text()).toBe("Send");
  });

  it("should disable button when email is invalid", async () => {
    validateEmail.mockReturnValue(false);
    const wrapper = mount(ResendVerificationEmailPage);

    // Initial state check
    expect(wrapper.find("button").attributes("disabled")).toBeDefined();

    // Even if we click, nothing should happen if logic holds (though in mock we need to be careful)
    // Here we just verify the disabled prop is passed to our mock button
    const button = wrapper.find("button");
    expect(button.element.disabled).toBe(true);
  });

  it("should call sendVerificationEmail on valid submit", async () => {
    validateEmail.mockReturnValue(true);
    const wrapper = mount(ResendVerificationEmailPage);
    const emailInput = wrapper.find("input");

    // Set email to trigger computed property if needed, though validateEmail mock returns true regardless
    await emailInput.setValue("test@example.com");

    mockSendVerificationEmail.mockResolvedValue({ success: true, message: "Sent" });

    // Since validation returns true, button should be enabled
    expect(wrapper.find("button").element.disabled).toBe(false);

    await wrapper.find("button").trigger("click");

    expect(mockSendVerificationEmail).toHaveBeenCalledWith("test@example.com");
    expect(mockShowNotification).toHaveBeenCalledWith("Sent", "success");
  });

  it("should handle failure response", async () => {
    validateEmail.mockReturnValue(true);
    const wrapper = mount(ResendVerificationEmailPage);
    const emailInput = wrapper.find("input");
    await emailInput.setValue("test@example.com");

    mockSendVerificationEmail.mockResolvedValue({ success: false, message: "Failed" });

    await wrapper.find("button").trigger("click");

    expect(mockSendVerificationEmail).toHaveBeenCalled();
    expect(mockShowNotification).toHaveBeenCalledWith("Failed", "error");
  });
});
