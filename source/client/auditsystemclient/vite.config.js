import { defineConfig, loadEnv } from 'vite';
import vue from '@vitejs/plugin-vue';
import tailwindcss from '@tailwindcss/vite';
import path from 'path';
import VitePluginVueDevTools from 'vite-plugin-vue-devtools';
import { fileURLToPath } from 'url';
import { dirname } from 'path';

// Establish __dirname for ES modules
const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, '.', '');

  return {
    plugins: [
      vue(),
      tailwindcss(),
      VitePluginVueDevTools(),
    ],

    resolve: {
      alias: {
        '@': path.resolve(__dirname, 'src'),
      },
    },

    server: {
      port: parseInt(env.VITE_PORT),
    },

    define: {
      __VITE_API_URL__: JSON.stringify(env.VITE_API_URL || 'https://localhost:7218'),
      __VITE_PORT__: JSON.stringify(env.VITE_PORT || '80'),
    },

    preview: {
      allowedHosts: ['auditsystem.onrender.com'],
    },
  };
});