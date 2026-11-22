import { startTransition } from 'react';
import { Link, NavLink, useNavigate } from 'react-router';
import { useAuth } from '../auth/useAuth.js';

export default function Antet() {
  const { utilizator, iesire } = useAuth();
  const navigheaza = useNavigate();

  function deconectare() {
    navigheaza('/', { replace: true });
    startTransition(iesire);
  }

  return (
    <header className="antet">
      <div className="antet-continut">
        <Link to="/" className="logo">📚 BiblioWeb</Link>

        <nav className="meniu">
          <NavLink to="/" end>Catalog</NavLink>
          {utilizator && <NavLink to="/imprumuturi">Împrumuturile mele</NavLink>}
          {utilizator?.rol === 'Administrator' && <NavLink to="/admin">Administrare</NavLink>}
        </nav>

        <div className="utilizator">
          {utilizator ? (
            <>
            <span className="nume-utilizator">
                {utilizator.numeComplet}
                {utilizator.rol === 'Administrator' && <span className="eticheta eticheta-admin">admin</span>}
              </span>
              <button className="buton buton-secundar buton-mic" onClick={deconectare}>Deconectare</button>
            </>
          ) : (
            <>
              <Link to="/login">Autentificare</Link>
              <Link to="/inregistrare" className="buton buton-secundar buton-mic">Cont nou</Link>
            </>
          )}
        </div>
      </div>
    </header>
  );
}
