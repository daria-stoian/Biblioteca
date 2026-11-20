// Pagini doar pentru utilizatori autentificati (echivalent [Authorize] din v1.0).
// Cu prop-ul "rol", doar pentru un anumit rol (echivalent [Authorize(Roles = "Administrator")]).
//
// Atentie: aceasta verificare doar ascunde paginile in interfata.
// Protectia reala este pe server, care verifica rolul din token la fiecare cerere.
import { Navigate, useLocation } from 'react-router';
import { useAuth } from './useAuth.js';

export default function RutaProtejata({ children, rol }) {
  const { utilizator, seIncarca } = useAuth();
  const locatie = useLocation();

  if (seIncarca) return <p className="secundar">Se încarcă…</p>;
  if (!utilizator) return <Navigate to="/login" replace state={{ dinPagina: locatie.pathname }} />;
  if (rol && utilizator.rol !== rol) {
    return <div className="mesaj mesaj-eroare">Nu ai drepturi pentru această pagină.</div>;
  }
  return children;
}
