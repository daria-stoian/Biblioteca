// Contextul de autentificare prin care il citesc componentele
import { createContext, useContext } from 'react';

export const AuthContext = createContext(null);

export function useAuth() {
  return useContext(AuthContext);
}
