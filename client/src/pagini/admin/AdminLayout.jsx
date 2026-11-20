// Cadrul comun al paginilor de administrare: titlul si filele de navigare.
// <Outlet /> este locul in care React Router afiseaza pagina aleasa (Carti, Autori etc.).
import { NavLink, Outlet } from 'react-router';

export default function AdminLayout() {
  return (
    <>
      <h1>Administrare</h1>
      <nav className="file">
        <NavLink to="/admin" end>Cărți</NavLink>
        <NavLink to="/admin/autori">Autori</NavLink>
        <NavLink to="/admin/categorii">Categorii</NavLink>
        <NavLink to="/admin/imprumuturi">Împrumuturi active</NavLink>
      </nav>
      <Outlet />
    </>
  );
}
