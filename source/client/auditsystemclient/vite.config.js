import { defineConfig, loadEnv } from "vite";
import vue from "@vitejs/plugin-vue";
import tailwindcss from "@tailwindcss/vite";
import VitePluginVueDevTools from "vite-plugin-vue-devtools";
import path from "path";
// ... all your other imports

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, ".", "");

  return {
    // ... keep all your existing config
    plugins: [vue(), tailwindcss(), VitePluginVueDevTools()],
    resolve: {
      alias: { "@": path.resolve(__dirname, "src") },
    },
    server: {
      port: parseInt(env.VITE_PORT),
    },
    define: {
      __VITE_API_URL__: JSON.stringify(env.VITE_API_URL),
      __VITE_PORT__: JSON.stringify(env.VITE_PORT),
    },
    preview: {
      host: true,
      allowedHosts: ["auditsystem.online", "www.auditsystem.online"],
    },
    test: {
      environment: "happy-dom",
      globals: true,
    },
  };
});
