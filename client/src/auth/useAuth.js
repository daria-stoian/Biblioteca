// Contextul de autentificare si hook-ul prin care il citesc componentele.
// Stau separat de AuthProvider (AuthContext.jsx), pentru ca un fisier .jsx
// sa exporte doar componente (necesar pentru reincarcarea rapida in Vite).
import { createContext, useContext } from 'react';

export const AuthContext = createContext(null);

export function useAuth() {
  return useContext(AuthContext);
}
