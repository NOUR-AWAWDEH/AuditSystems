import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';
import tailwindcss from '@tailwindcss/vite'

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [
        plugin(),
        tailwindcss(),
    ],
    
    server: {
        port: 60480,
        proxy: {
            '/api': {
                target: 'http://localhost:5164',
                changeOrigin: true,
                secure: false,
            }
        }
    }
})