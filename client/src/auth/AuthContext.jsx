import { useCallback, useEffect, useMemo, useState } from 'react';
import { api, setLaExpirareaSesiunii, tokenStocat } from '../api.js';
import { AuthContext } from './useAuth.js';

export function AuthProvider({ children }) {
  const [utilizator, setUtilizator] = useState(null);
  // Daca token-ul salvat mai e valid, seIncarca e true
  const [incarcare, setIncarcare] = useState(() => Boolean(tokenStocat.citeste()));

  const iesire = useCallback(() => {
    tokenStocat.sterge();
    setUtilizator(null);
  }, []);

  // Daca exista un token salvat, cerem datele utilizatorului
  useEffect(() => {
    setLaExpirareaSesiunii(iesire);
    if (!tokenStocat.citeste()) return;
    api
      .eu()
      .then(setUtilizator)
      .catch(iesire)
      .finally(() => setIncarcare(false));
  }, [iesire]);

  const autentificare = useCallback(async (email, parola) => {
    const { token, utilizator } = await api.login(email, parola);
    tokenStocat.salveaza(token);
    setUtilizator(utilizator);
  }, []);

  const inregistrare = useCallback(async (numeComplet, email, parola) => {
    const { token, utilizator } = await api.inregistrare(numeComplet, email, parola);
    tokenStocat.salveaza(token);
    setUtilizator(utilizator);
  }, []);

  const valoare = useMemo(
    () => ({ utilizator, incarcare, autentificare, inregistrare, iesire }),
    [utilizator, incarcare, autentificare, inregistrare, iesire],
  );

  return <AuthContext.Provider value={valoare}>{children}</AuthContext.Provider>;
}
