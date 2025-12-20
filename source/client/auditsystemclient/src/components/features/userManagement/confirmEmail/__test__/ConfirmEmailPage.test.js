import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import { mount } from "@vue/test-utils";
import ConfirmEmailPage from "../ConfirmEmailPage.vue";
import { useRoute, useRouter } from "vue-router";
import { useConfirmEmail } from "@/composables/userManagement/confirmEmail/useConfirmEmail";
import { ref } from "vue";

// Mock vue-router
vi.mock("vue-router", () => ({
  useRoute: vi.fn(),
  useRouter: vi.fn(),
}));

// Mock composable
vi.mock("@/composables/userManagement/confirmEmail/useConfirmEmail", () => ({
  useConfirmEmail: vi.fn(),
}));

describe("ConfirmEmailPage.vue", () => {
  let mockPush;
  let mockConfirmEmail;

  beforeEach(() => {
    vi.clearAllMocks();
    vi.useFakeTimers();

    mockPush = vi.fn();
    useRouter.mockReturnValue({ push: mockPush });

    useRoute.mockReturnValue({
      query: { uid: "user-123", token: "token-456" },
    });

    mockConfirmEmail = vi.fn();
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  const setupComposableMock = (overrides = {}) => {
    const defaults = {
      isLoading: ref(false),
      error: ref(null),
      success: ref(null),
      confirmEmail: mockConfirmEmail,
    };
    useConfirmEmail.mockReturnValue({ ...defaults, ...overrides });
  };

  it("should call confirmEmail on mount", () => {
    setupComposableMock({ isLoading: ref(true) });
    mockConfirmEmail.mockResolvedValue({ success: true });

    mount(ConfirmEmailPage);

    expect(mockConfirmEmail).toHaveBeenCalledWith("user-123", "token-456");
  });

  it("should display loading state", () => {
    setupComposableMock({ isLoading: ref(true) });
    mockConfirmEmail.mockResolvedValue({ success: true });

    const wrapper = mount(ConfirmEmailPage);

    expect(wrapper.find(".loading-state").exists()).toBe(true);
    expect(wrapper.findComponent({ name: "AuditLogo" }).exists()).toBe(true);
    expect(wrapper.text()).toContain("Verifying your email...");
  });

  it("should display success state and redirect", async () => {
    setupComposableMock({
      success: ref({ message: "Success Message" }),
    });
    mockConfirmEmail.mockResolvedValue({ success: true });

    const wrapper = mount(ConfirmEmailPage);
    // Wait for mounted hook
    await wrapper.vm.$nextTick();

    expect(wrapper.find(".success-state").exists()).toBe(true);
    expect(wrapper.text()).toContain("Email Confirmed!");
    expect(wrapper.text()).toContain("Success Message");

    // Fast-forward time
    vi.advanceTimersByTime(3000);

    expect(mockPush).toHaveBeenCalledWith("/login");
  });

  it("should display error state and handle manual redirect", async () => {
    setupComposableMock({
      error: ref("Error Message"),
    });
    mockConfirmEmail.mockResolvedValue({ success: false });

    const wrapper = mount(ConfirmEmailPage);
    await wrapper.vm.$nextTick();

    expect(wrapper.find(".error-state").exists()).toBe(true);
    expect(wrapper.text()).toContain("Verification Failed");
    expect(wrapper.text()).toContain("Error Message");

    // Click login button
    await wrapper.find("button").trigger("click");

    expect(mockPush).toHaveBeenCalledWith("/login");
  });
});
