import { describe, it, expect, vi, beforeEach } from "vitest";
import { mount } from "@vue/test-utils";
import LoginForm from "../LoginForm.vue";
import { useLogin } from "@/composables/auth/useLogin";
import { useRouter } from "vue-router";
import { describe, it, expect, vi, beforeEach } from "vitest";
import { mount } from "@vue/test-utils";
import LoginForm from "../LoginForm.vue";
import { useLogin } from "@/composables/auth/useLogin";
import { useRouter } from "vue-router";
import { ref } from "vue";
import { validateUsername } from "@/services/validations/validateUsername";
import validatePassword from "@/services/validations/validatePassword";

// Mock dependencies
vi.mock("@/composables/auth/useLogin", () => ({
  useLogin: vi.fn(),
}));

vi.mock("@/services/validations/validateUsername", () => ({
  validateUsername: vi.fn(),
}));
vi.mock("@/services/validations/validatePassword", () => ({
  default: vi.fn(),
}));

vi.mock("vue-router", () => ({
  useRouter: vi.fn(),
}));

// Mock child components
vi.mock("@/components/base/BaseButton.vue", () => ({
  default: {
    template: "<button><slot name='loading' v-if='disabled'></slot><slot v-else></slot></button>",
    props: ["disabled"],
  },
}));
vi.mock("@/components/base/BaseInput.vue", () => ({
  default: {
    template:
      "<input :value='modelValue' @input='$emit(\"update:modelValue\", $event.target.value)' />",
    props: ["modelValue"],
    emits: ["update:modelValue"],
  },
}));
vi.mock("@/components/base/PasswordInput.vue", () => ({
  default: {
    template:
      "<input :value='modelValue' @input='$emit(\"update:modelValue\", $event.target.value)' />",
    props: ["modelValue"],
    emits: ["update:modelValue"],
  },
}));
vi.mock("@/components/common/AuditLogo.vue", () => ({ default: { template: "<div>Logo</div>" } }));
vi.mock("@/components/common/ErrorDisplay.vue", () => ({
  default: { template: "<div></div>", props: ["errors", "isVisible"] },
}));
vi.mock("@/components/base/BaseLink.vue", () => ({
  default: { template: "<a><slot /></a>", props: ["to"] },
}));

describe("LoginForm.vue", () => {
  let mockLogin;
  let mockPush;
  let mockReplace;

  beforeEach(() => {
    vi.clearAllMocks();

    mockLogin = vi.fn();
    useLogin.mockReturnValue({
      login: mockLogin,
      error: ref(null),
    });

    mockPush = vi.fn();
    mockReplace = vi.fn();
    useRouter.mockReturnValue({
      push: mockPush,
      replace: mockReplace,
    });

    validateUsername.mockReturnValue(null);
    validatePassword.mockReturnValue(null);
  });

  it("should render correctly", () => {
    const wrapper = mount(LoginForm);
    expect(wrapper.find("h2").text()).toBe("Login");
    expect(wrapper.findAll("input")).toHaveLength(2); // Username and Password
    expect(wrapper.find("button").text()).toBe("Login");
  });

  it("should render forgot password and resend verification links", () => {
    const wrapper = mount(LoginForm);
    const links = wrapper.findAllComponents({ name: "BaseLink" });

    // We expect 2 links now
    expect(links).toHaveLength(2);

    // Check texts
    const linkTexts = links.map((link) => link.text());
    expect(linkTexts).toContain("Forgot Password?");
    expect(linkTexts).toContain("Resend Verification Email");

    // Check 'to' props if possible, or we assume BaseLink passes 'to' correctly
    // Since we mocked BaseLink as <a>, we can check props
    expect(links[0].props("to")).toBe("/forgot-password");
    expect(links[1].props("to")).toBe("/resend-verification-email");
  });

  it("should call login on form submit", async () => {
    const wrapper = mount(LoginForm);

    const inputs = wrapper.findAll("input");
    await inputs[0].setValue("testuser");
    await inputs[1].setValue("password123");

    mockLogin.mockResolvedValue({ success: true });

    await wrapper.find("form").trigger("submit.prevent");

    expect(mockLogin).toHaveBeenCalledWith("testuser", "password123");
    expect(mockReplace).toHaveBeenCalledWith("/dashboard");
  });
});
