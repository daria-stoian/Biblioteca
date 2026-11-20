// Starea de autentificare, disponibila in orice componenta prin useAuth() (din useAuth.js).
// Echivalentul din v1.0: cookie-ul de autentificare Identity + User.Identity in Razor.
import { useCallback, useEffect, useMemo, useState } from 'react';
import { api, setLaSesiuneExpirata, tokenStocat } from '../api.js';
import { AuthContext } from './useAuth.js';

export function AuthProvider({ children }) {
  const [utilizator, setUtilizator] = useState(null);
  // La pornire nu stim inca daca token-ul salvat mai e valid; pana aflam, "seIncarca" e true
  const [seIncarca, setSeIncarca] = useState(() => Boolean(tokenStocat.citeste()));

  const iesire = useCallback(() => {
    tokenStocat.sterge();
    setUtilizator(null);
  }, []);

  // La deschiderea aplicatiei: daca exista un token salvat, cerem datele utilizatorului
  useEffect(() => {
    setLaSesiuneExpirata(iesire);
    if (!tokenStocat.citeste()) return;
    api
      .eu()
      .then(setUtilizator)
      .catch(iesire)
      .finally(() => setSeIncarca(false));
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
