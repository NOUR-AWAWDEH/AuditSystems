import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import tailwindcss from '@tailwindcss/vite'
import path from 'path';
import VitePluginVueDevTools from 'vite-plugin-vue-devtools';

// https://vitejs.dev/config/
export default defineConfig({
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
        port: 5173, // Match the actual port Vue is running on
        proxy: {
            '/api': {
                target: 'https://localhost:7218', // Use HTTPS
                changeOrigin: true,
                secure: false, // Ignore SSL certificate validation in development
            }
        }
    }
})
