import { vi } from "vitest";

// Mock localStorage
Object.defineProperty(window, "localStorage", {
  value: {
    getItem: vi.fn(),
    setItem: vi.fn(),
    removeItem: vi.fn(),
    clear: vi.fn(),
  },
  writable: true,
});

// Mock environment variables
window.__VITE_API_URL__ = "http://localhost:7218";
window.__VITE_PORT__ = "80";

// Mock router to prevent errors when importing api.js or other modules that use router
vi.mock("@/router/router", () => ({
  default: {
    push: vi.fn(),
    replace: vi.fn(),
    beforeEach: vi.fn(),
    afterEach: vi.fn(),
  },
}));
