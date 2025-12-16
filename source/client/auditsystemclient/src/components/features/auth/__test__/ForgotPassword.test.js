import { describe, it, expect, vi, beforeEach } from "vitest";
import { mount } from "@vue/test-utils";
import ForgotPassword from "../ForgotPassword.vue";
import { useForgetPassword } from "@/composables/password/useForgetPassword";
import { useNotification } from "@/composables/notification/useNotification";

// Mock composables
vi.mock("@/composables/password/useForgetPassword");
vi.mock("@/composables/notification/useNotification");

describe("ForgotPassword Component Tests", () => {
  let mockForgetPassword;
  let mockShowNotification;

  beforeEach(() => {
    vi.clearAllMocks();

    mockForgetPassword = vi.fn();
    mockShowNotification = vi.fn();

    useForgetPassword.mockReturnValue({
      forgetPassword: mockForgetPassword,
    });

    useNotification.mockReturnValue({
      showNotification: mockShowNotification,
    });
  });

  it("should show error notification for invalid email on blur", async () => {
    const wrapper = mount(ForgotPassword, {
      global: {
        stubs: {
          BaseLink: true,
          "router-link": true,
        },
      },
    });
    const emailInput = wrapper.find('input[type="text"]');
    await emailInput.setValue("invalid-email");
    await emailInput.trigger("blur");

    expect(mockShowNotification).toHaveBeenCalledWith(
      "Please enter a valid email address.",
      "error"
    );
    expect(mockForgetPassword).not.toHaveBeenCalled();
  });

  it("should have disabled submit button for invalid email", async () => {
    const wrapper = mount(ForgotPassword, {
      global: {
        stubs: {
          BaseLink: true,
          "router-link": true,
        },
      },
    });
    const emailInput = wrapper.find('input[type="text"]');
    await emailInput.setValue("invalid-email");

    const button = wrapper.find("button");
    expect(button.element.disabled).toBe(true);
  });

  it("should call forgetPassword and show success notification", async () => {
    mockForgetPassword.mockResolvedValue({
      success: true,
      message: "Reset link sent",
    });

    const wrapper = mount(ForgotPassword, {
      global: {
        stubs: {
          BaseLink: true,
          "router-link": true,
        },
      },
    });
    const emailInput = wrapper.find('input[type="text"]'); // Verify input selector if needed, usually BaseInput passes attrs
    await emailInput.setValue("test@example.com");

    await wrapper.find("button").trigger("click");

    expect(mockForgetPassword).toHaveBeenCalledWith("test@example.com");
    expect(mockShowNotification).toHaveBeenCalledWith("Reset link sent", "success");
  });

  it("should show error notification on API failure", async () => {
    mockForgetPassword.mockResolvedValue({
      success: false,
      message: "Email not found",
    });

    const wrapper = mount(ForgotPassword, {
      global: {
        stubs: {
          BaseLink: true,
          "router-link": true,
        },
      },
    });
    const emailInput = wrapper.find('input[type="text"]');
    await emailInput.setValue("notfound@example.com");

    await wrapper.find("button").trigger("click");

    expect(mockForgetPassword).toHaveBeenCalledWith("notfound@example.com");
    expect(mockShowNotification).toHaveBeenCalledWith("Email not found", "error");
  });

  it("should handle exceptions and show error notification", async () => {
    mockForgetPassword.mockRejectedValue(new Error("Network Error"));

    const wrapper = mount(ForgotPassword, {
      global: {
        stubs: {
          BaseLink: true,
          "router-link": true,
        },
      },
    });
    const emailInput = wrapper.find('input[type="text"]');
    await emailInput.setValue("error@example.com");

    await wrapper.find("button").trigger("click");

    expect(mockShowNotification).toHaveBeenCalledWith("Network Error", "error");
  });
});
