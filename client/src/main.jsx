import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router'
import { AuthProvider } from './auth/AuthContext.jsx'
import App from './App.jsx'
import './index.css'

// BrowserRouter: navigarea intre pagini fara reincarcare
// AuthProvider: utilizatorul autentificat, disponibil in toata aplicatia
createRoot(document.getElementById('root')).render(
  <StrictMode>
    <BrowserRouter>
      <AuthProvider>
        <App />
      </AuthProvider>
    </BrowserRouter>
  </StrictMode>,
)
