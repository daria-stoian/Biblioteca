// Pagini doar pentru utilizatori autentificati (echivalent [Authorize] din v1.0).
// Daca nu esti autentificat, te trimite la Login si tine minte unde voiai sa ajungi.
import { Navigate, useLocation } from 'react-router';
import { useAuth } from './useAuth.js';

export default function RutaProtejata({ children }) {
  const { utilizator, seIncarca } = useAuth();
  const locatie = useLocation();

  if (seIncarca) return <p className="secundar">Se încarcă…</p>;
  if (!utilizator) return <Navigate to="/login" replace state={{ dinPagina: locatie.pathname }} />;
  return children;
}
