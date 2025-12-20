import { describe, it, expect, vi, beforeEach } from "vitest";
import { mount, flushPromises } from "@vue/test-utils";
import { ref } from "vue";
import JobTitleSelector from "../JobTitleSelector.vue";

// Mock data
const mockJobTitles = [
  { id: "jt1", name: "Developer", companyId: "c1" },
  { id: "jt2", name: "Designer", companyId: "c1" },
  { id: "jt3", name: "Manager", companyId: "c1" },
];

// Create reactive refs for mock state
const jobTitlesRef = ref([...mockJobTitles]);
const isLoadingRef = ref(false);
const errorRef = ref(null);
const mockGetJobTitlesByUserId = vi.fn();

// Mock the composable
vi.mock("@/composables/organization/jobTitle/useGetJobTitlesByUserId", () => ({
  useGetJobTitlesByUserId: () => ({
    jobTitles: jobTitlesRef,
    getJobTitlesByUserId: mockGetJobTitlesByUserId,
    isLoading: isLoadingRef,
    error: errorRef,
  }),
}));

// Mock the CustomSelect component
vi.mock("@/components/common/CustomSelect.vue", () => ({
  default: {
    name: "CustomSelect",
    template: `
      <div class="mock-custom-select">
        <select
          :value="getSelectValue()"
          @change="handleChange($event)"
          :disabled="disabled"
          :required="required"
        >
          <option v-if="placeholder" value="" disabled>{{ placeholder }}</option>
          <option v-for="option in options" :key="option[valueKey]" :value="option[valueKey]">
            {{ option[labelKey] }}
          </option>
        </select>
        <span v-if="error" class="error">{{ error }}</span>
        <span v-if="label" class="label">{{ label }}</span>
      </div>
    `,
    props: [
      "modelValue",
      "options",
      "label",
      "placeholder",
      "required",
      "error",
      "disabled",
      "valueKey",
      "labelKey",
      "emitObject",
    ],
    emits: ["update:modelValue"],
    methods: {
      getSelectValue() {
        if (this.modelValue && typeof this.modelValue === "object") {
          return this.modelValue[this.valueKey];
        }
        return this.modelValue || "";
      },
      handleChange(event) {
        const selectedId = event.target.value;
        const selectedOption = this.options.find(
          (opt) => opt[this.valueKey].toString() === selectedId
        );
        if (this.emitObject && selectedOption) {
          this.$emit("update:modelValue", selectedOption);
        } else {
          this.$emit("update:modelValue", selectedId);
        }
      },
    },
  },
}));

describe("JobTitleSelector.vue", () => {
  beforeEach(() => {
    vi.clearAllMocks();
    // Reset reactive refs
    jobTitlesRef.value = [...mockJobTitles];
    isLoadingRef.value = false;
    errorRef.value = null;
  });

  describe("Rendering", () => {
    it("renders CustomSelect component", () => {
      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "" },
      });
      expect(wrapper.find(".mock-custom-select").exists()).toBe(true);
    });

    it("renders with default label", () => {
      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "" },
      });
      expect(wrapper.find(".label").text()).toBe("Job Title");
    });

    it("renders with custom label", () => {
      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "", label: "Custom Label" },
      });
      expect(wrapper.find(".label").text()).toBe("Custom Label");
    });

    it("renders with default placeholder", () => {
      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "" },
      });
      const placeholderOption = wrapper.find("option[disabled]");
      expect(placeholderOption.text()).toBe("Select Job Title");
    });

    it("renders with custom placeholder", () => {
      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "", placeholder: "Choose a job" },
      });
      const placeholderOption = wrapper.find("option[disabled]");
      expect(placeholderOption.text()).toBe("Choose a job");
    });

    it("renders all job title options", async () => {
      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "" },
      });
      await flushPromises();

      const options = wrapper.findAll("option:not([disabled])");
      expect(options.length).toBe(3);
      expect(options.at(0).text()).toBe("Developer");
      expect(options.at(1).text()).toBe("Designer");
      expect(options.at(2).text()).toBe("Manager");
    });
  });

  describe("Data Fetching", () => {
    it("calls getJobTitlesByUserId on mount", async () => {
      mount(JobTitleSelector, {
        props: { modelValue: "" },
      });
      await flushPromises();
      expect(mockGetJobTitlesByUserId).toHaveBeenCalled();
    });
  });

  describe("v-model Binding", () => {
    it("sets initial value from modelValue (string)", async () => {
      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "jt2" },
      });
      await flushPromises();
      expect(wrapper.find("select").element.value).toBe("jt2");
    });

    it("sets initial value from modelValue (object)", async () => {
      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: { id: "jt2", name: "Designer" } },
      });
      await flushPromises();
      expect(wrapper.find("select").element.value).toBe("jt2");
    });

    it("emits update:modelValue with ID when emitObject is false", async () => {
      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "", emitObject: false },
      });
      await flushPromises();

      await wrapper.find("select").setValue("jt1");

      expect(wrapper.emitted("update:modelValue")).toBeTruthy();
      expect(wrapper.emitted("update:modelValue")[0]).toEqual(["jt1"]);
    });

    it("emits update:modelValue with full object when emitObject is true", async () => {
      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "", emitObject: true },
      });
      await flushPromises();

      await wrapper.find("select").setValue("jt1");

      expect(wrapper.emitted("update:modelValue")).toBeTruthy();
      expect(wrapper.emitted("update:modelValue")[0]).toEqual([
        { id: "jt1", name: "Developer", companyId: "c1" },
      ]);
    });
  });

  describe("Loading State", () => {
    it("disables select when loading", async () => {
      isLoadingRef.value = true;

      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "" },
      });
      await flushPromises();

      expect(wrapper.find("select").attributes("disabled")).toBeDefined();
    });

    it("enables select when not loading", async () => {
      isLoadingRef.value = false;

      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "" },
      });
      await flushPromises();

      // When disabled is false, the attribute should not be present
      const select = wrapper.find("select");
      expect(select.element.disabled).toBe(false);
    });
  });

  describe("Error State", () => {
    it("displays error message when error exists", async () => {
      errorRef.value = "Failed to load job titles";

      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "" },
      });
      await flushPromises();

      expect(wrapper.find(".error").exists()).toBe(true);
      expect(wrapper.find(".error").text()).toBe("Failed to load job titles");
    });

    it("does not display error when no error", async () => {
      errorRef.value = null;

      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "" },
      });
      await flushPromises();

      expect(wrapper.find(".error").exists()).toBe(false);
    });
  });

  describe("Required Prop", () => {
    it("passes required prop to CustomSelect when true", async () => {
      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "", required: true },
      });
      await flushPromises();

      expect(wrapper.find("select").attributes("required")).toBeDefined();
    });

    it("does not set required when prop is false", async () => {
      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "", required: false },
      });
      await flushPromises();

      expect(wrapper.find("select").element.required).toBe(false);
    });
  });

  describe("Props Passthrough", () => {
    it("uses id as valueKey", async () => {
      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "" },
      });
      await flushPromises();

      // Options should use id as value
      const options = wrapper.findAll("option:not([disabled])");
      expect(options.at(0).attributes("value")).toBe("jt1");
      expect(options.at(1).attributes("value")).toBe("jt2");
    });

    it("uses name as labelKey", async () => {
      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "" },
      });
      await flushPromises();

      // Options should display name
      const options = wrapper.findAll("option:not([disabled])");
      expect(options.at(0).text()).toBe("Developer");
      expect(options.at(1).text()).toBe("Designer");
    });
  });

  describe("Empty State", () => {
    it("handles empty job titles array", async () => {
      jobTitlesRef.value = [];

      const wrapper = mount(JobTitleSelector, {
        props: { modelValue: "" },
      });
      await flushPromises();

      // Should only have placeholder option
      const options = wrapper.findAll("option");
      expect(options.length).toBe(1);
      expect(options.at(0).text()).toBe("Select Job Title");
    });
  });
});
