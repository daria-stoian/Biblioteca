import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    // In dezvoltare, React ruleaza pe 5173 si API-ul pe 3000.
    // Proxy-ul trimite mai departe cererile catre /api si /coperti, asa ca in cod
    // scriem doar fetch('/api/carti'), fara adresa completa a serverului.
    proxy: {
      '/api': 'http://localhost:3000',
      '/coperti': 'http://localhost:3000',
    },
  },
})
