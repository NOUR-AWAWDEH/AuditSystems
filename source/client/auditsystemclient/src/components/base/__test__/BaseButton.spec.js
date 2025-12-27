import { mount } from "@vue/test-utils";
import { describe, it, expect } from "vitest";
import BaseButton from "../BaseButton.vue";

describe("BaseButton", () => {
  it("renders slot content", () => {
    const wrapper = mount(BaseButton, {
      slots: {
        default: "Click Me",
      },
    });
    expect(wrapper.text()).toContain("Click Me");
  });

  it("emits click event when clicked", async () => {
    const wrapper = mount(BaseButton);

    await wrapper.find("button").trigger("click");
    expect(wrapper.emitted()).toHaveProperty("click");
    expect(wrapper.emitted("click")).toHaveLength(1);
  });

  it("renders loading slot when isLoading is true", () => {
    const wrapper = mount(BaseButton, {
      props: {
        isLoading: true,
      },
      slots: {
        loading: "Processing...",
      },
    });
    expect(wrapper.text()).toContain("Processing...");
  });

  it("has disabled attribute when disabled prop is true", async () => {
    const wrapper = mount(BaseButton, {
      props: {
        disabled: true,
      },
    });

    const button = wrapper.find("button");

    // In Vue test utils, boolean attributes usually appear as key exists or value is ''
    // Let's check attributes object
    expect(button.attributes()).toHaveProperty("disabled");
  });
});
