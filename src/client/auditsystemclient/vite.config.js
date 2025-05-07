import { defineConfig, loadEnv } from 'vite';
import vue from '@vitejs/plugin-vue';
import tailwindcss from '@tailwindcss/vite'
import path from 'path';
import { fileURLToPath } from 'url';
import { dirname } from 'path';
import VitePluginVueDevTools from 'vite-plugin-vue-devtools';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
  // Load env file based on mode
  const env = loadEnv(mode, __dirname, 'VITE_');

  return {
    plugins: [
        vue(),
        tailwindcss(),
        VitePluginVueDevTools(),
    ],

    resolve: {
        alias: {
            '@': path.resolve('src')
        }
    },

    server: {
        port: parseInt(env.VITE_PORT || '5173'), // Use env variable with fallback
        proxy: {
            '/api': {
                target: 'https://auditsystem-o9sd.onrender.com',
                //|| 'https://localhost:7218', // Use env variable with fallback
                changeOrigin: true,
                secure: false, // Ignore SSL certificate validation in development
            }
        }
    }
  };
})
