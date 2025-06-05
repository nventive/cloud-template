import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
    const env = loadEnv(mode, process.cwd(), '');

    // Determine the base URL for the API service from Aspire's environment variables
    const apiBaseUrl =
        process.env.services__apiservice__https__0 ||
        process.env.services__apiservice__http__0;

    return {
        plugins: [react()],
        // Define global constants for client-side code
        define: {
            'process.env.API_BASE_URL': JSON.stringify(apiBaseUrl || ""),
        },
        server: {
            port: parseInt(env.VITE_PORT)
        },
        build: {
            outDir: 'dist',
            rollupOptions: {
                input: './index.html'
            }
        }
    }
})
