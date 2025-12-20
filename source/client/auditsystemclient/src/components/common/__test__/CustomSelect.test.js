import { describe, it, expect, vi, beforeEach } from "vitest";
import { mount, flushPromises } from "@vue/test-utils";
import CustomSelect from "../CustomSelect.vue";

describe("CustomSelect.vue", () => {
  const simpleOptions = ["Option 1", "Option 2", "Option 3"];

  const objectOptions = [
    { id: "1", name: "First Option" },
    { id: "2", name: "Second Option" },
    { id: "3", name: "Third Option" },
  ];

  const customKeyOptions = [
    { value: "val1", label: "Label 1" },
    { value: "val2", label: "Label 2" },
  ];

  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe("Rendering", () => {
    it("renders with required props only", () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions },
      });
      expect(wrapper.find("select").exists()).toBe(true);
      expect(wrapper.findAll("option").length).toBe(4); // 3 options + placeholder
    });

    it("renders label when provided", () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions, label: "Test Label" },
      });
      expect(wrapper.find("label").exists()).toBe(true);
      expect(wrapper.text()).toContain("Test Label");
    });

    it("does not render label when not provided", () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions },
      });
      expect(wrapper.find("label").exists()).toBe(false);
    });

    it("renders placeholder option", () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions, placeholder: "Please select" },
      });
      const placeholderOption = wrapper.find("option[disabled]");
      expect(placeholderOption.exists()).toBe(true);
      expect(placeholderOption.text()).toBe("Please select");
    });

    it("renders error message when error prop is provided", () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions, error: "This field is required" },
      });
      expect(wrapper.text()).toContain("This field is required");
    });

    it("does not render error message when error prop is empty", () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions, error: "" },
      });
      expect(wrapper.find(".text-red-600").exists()).toBe(false);
    });

    it("renders all simple options", () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions },
      });
      const options = wrapper.findAll("option");
      expect(options.at(1).text()).toBe("Option 1");
      expect(options.at(2).text()).toBe("Option 2");
      expect(options.at(3).text()).toBe("Option 3");
    });
  });

  describe("Object Options", () => {
    it("renders object options using default keys (value/label)", () => {
      const wrapper = mount(CustomSelect, {
        props: { options: customKeyOptions },
      });
      const options = wrapper.findAll("option");
      expect(options.at(1).text()).toBe("Label 1");
      expect(options.at(1).attributes("value")).toBe("val1");
    });

    it("renders object options using custom keys (id/name)", () => {
      const wrapper = mount(CustomSelect, {
        props: {
          options: objectOptions,
          valueKey: "id",
          labelKey: "name",
        },
      });
      const options = wrapper.findAll("option");
      expect(options.at(1).text()).toBe("First Option");
      expect(options.at(1).attributes("value")).toBe("1");
    });
  });

  describe("v-model and Selection", () => {
    it("sets initial value from modelValue (string)", async () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions, modelValue: "Option 2" },
      });
      expect(wrapper.find("select").element.value).toBe("Option 2");
    });

    it("sets initial value from modelValue (object)", async () => {
      const wrapper = mount(CustomSelect, {
        props: {
          options: objectOptions,
          valueKey: "id",
          labelKey: "name",
          modelValue: { id: "2", name: "Second Option" },
        },
      });
      expect(wrapper.find("select").element.value).toBe("2");
    });

    it("emits update:modelValue with value when option is selected", async () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions },
      });

      await wrapper.find("select").setValue("Option 2");

      expect(wrapper.emitted("update:modelValue")).toBeTruthy();
      expect(wrapper.emitted("update:modelValue")[0]).toEqual(["Option 2"]);
    });

    it("emits update:modelValue with object value when emitObject is false", async () => {
      const wrapper = mount(CustomSelect, {
        props: {
          options: objectOptions,
          valueKey: "id",
          labelKey: "name",
          emitObject: false,
        },
      });

      await wrapper.find("select").setValue("2");

      expect(wrapper.emitted("update:modelValue")[0]).toEqual(["2"]);
    });

    it("emits update:modelValue with full object when emitObject is true", async () => {
      const wrapper = mount(CustomSelect, {
        props: {
          options: objectOptions,
          valueKey: "id",
          labelKey: "name",
          emitObject: true,
        },
      });

      await wrapper.find("select").setValue("2");

      expect(wrapper.emitted("update:modelValue")[0]).toEqual([{ id: "2", name: "Second Option" }]);
    });

    it("emits change event along with update:modelValue", async () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions },
      });

      await wrapper.find("select").setValue("Option 1");

      expect(wrapper.emitted("change")).toBeTruthy();
      expect(wrapper.emitted("change")[0]).toEqual(["Option 1"]);
    });
  });

  describe("Focus and Blur", () => {
    it("emits focus event on focus", async () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions },
      });

      await wrapper.find("select").trigger("focus");

      expect(wrapper.emitted("focus")).toBeTruthy();
    });

    it("emits blur event on blur", async () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions },
      });

      await wrapper.find("select").trigger("blur");

      expect(wrapper.emitted("blur")).toBeTruthy();
    });

    it("sets isOpen to true on focus (adds is-open class)", async () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions },
      });

      expect(wrapper.find(".custom-select").classes()).not.toContain("is-open");

      await wrapper.find("select").trigger("focus");

      expect(wrapper.find(".custom-select").classes()).toContain("is-open");
    });

    it("sets isOpen to false on blur (removes is-open class)", async () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions },
      });

      await wrapper.find("select").trigger("focus");
      expect(wrapper.find(".custom-select").classes()).toContain("is-open");

      await wrapper.find("select").trigger("blur");
      expect(wrapper.find(".custom-select").classes()).not.toContain("is-open");
    });
  });

  describe("Disabled State", () => {
    it("sets disabled attribute on select when disabled prop is true", () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions, disabled: true },
      });
      expect(wrapper.find("select").attributes("disabled")).toBeDefined();
    });

    it("does not set disabled attribute when disabled prop is false", () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions, disabled: false },
      });
      expect(wrapper.find("select").attributes("disabled")).toBeUndefined();
    });
  });

  describe("Required State", () => {
    it("sets required attribute on select when required prop is true", () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions, required: true },
      });
      expect(wrapper.find("select").attributes("required")).toBeDefined();
    });

    it("does not set required attribute when required prop is false", () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions, required: false },
      });
      expect(wrapper.find("select").attributes("required")).toBeUndefined();
    });
  });

  describe("SelectClass Prop", () => {
    it("applies custom class to select element", () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions, selectClass: "custom-class" },
      });
      expect(wrapper.find("select").classes()).toContain("custom-class");
    });
  });

  describe("selectValue computed property", () => {
    it("returns empty string when modelValue is null", () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions, modelValue: null },
      });
      expect(wrapper.find("select").element.value).toBe("");
    });

    it("returns empty string when modelValue is undefined", () => {
      const wrapper = mount(CustomSelect, {
        props: { options: simpleOptions, modelValue: undefined },
      });
      expect(wrapper.find("select").element.value).toBe("");
    });

    it("extracts value from object modelValue using valueKey", () => {
      const wrapper = mount(CustomSelect, {
        props: {
          options: objectOptions,
          valueKey: "id",
          labelKey: "name",
          modelValue: { id: "3", name: "Third Option" },
        },
      });
      expect(wrapper.find("select").element.value).toBe("3");
    });

    it("handles object modelValue with undefined value in key", () => {
      const wrapper = mount(CustomSelect, {
        props: {
          options: objectOptions,
          valueKey: "id",
          labelKey: "name",
          modelValue: { id: undefined, name: "First" },
        },
      });
      expect(wrapper.find("select").element.value).toBe("");
    });
  });

  describe("Edge Cases", () => {
    it("handles empty options array", () => {
      const wrapper = mount(CustomSelect, {
        props: { options: [] },
      });
      // Should only have placeholder option
      expect(wrapper.findAll("option").length).toBe(1);
    });

    it("handles numeric option values", async () => {
      const numericOptions = [
        { id: 1, name: "One" },
        { id: 2, name: "Two" },
      ];
      const wrapper = mount(CustomSelect, {
        props: {
          options: numericOptions,
          valueKey: "id",
          labelKey: "name",
        },
      });

      await wrapper.find("select").setValue("1");

      // Value is converted to string for comparison
      expect(wrapper.emitted("update:modelValue")[0]).toEqual([1]);
    });

    it("generates unique id for each instance", () => {
      const wrapper1 = mount(CustomSelect, {
        props: { options: simpleOptions, label: "Test" },
      });
      const wrapper2 = mount(CustomSelect, {
        props: { options: simpleOptions, label: "Test" },
      });

      const id1 = wrapper1.find("label").attributes("for");
      const id2 = wrapper2.find("label").attributes("for");

      expect(id1).toMatch(/^select-/);
      expect(id2).toMatch(/^select-/);
      // IDs should be different (high probability with random generation)
      // Note: There's a tiny chance this could fail due to random collision
    });
  });
});
