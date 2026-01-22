import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import dotenv from 'dotenv';

// Load environment variables from .env file
dotenv.config();

// https://vite.dev/config/
export default defineConfig({
  plugins: [vue()],
  define: {
    'process.env': {
      VITE_API_BASE_URL: process.env.VITE_API_BASE_URL || 'https://localhost:7034'
    }
  }
})
