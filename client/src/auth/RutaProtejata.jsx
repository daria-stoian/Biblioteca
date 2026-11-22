// Pagini doar pentru utilizatori autentificati
import { Navigate, useLocation } from 'react-router';
import { tokenStocat } from '../api.js';
import { useAuth } from './useAuth.js';

export default function RutaProtejata({ children, rol }) {
  const { utilizator, seIncarca } = useAuth();
  const locatie = useLocation();

  if (seIncarca) return <p className="secundar">Se încarcă…</p>;
  if (!utilizator && tokenStocat.citeste()) {
    return <div className="mesaj mesaj-eroare">Nu am putut verifica sesiunea. Verifică dacă serverul este pornit, apoi reîncarcă pagina.</div>;
  }
  if (!utilizator) return <Navigate to="/login" replace state={{ dinPagina: locatie.pathname }} />;
  if (rol && utilizator.rol !== rol) {
    return <div className="mesaj mesaj-eroare">Nu ai drepturi pentru această pagină.</div>;
  }
  return children;
}
