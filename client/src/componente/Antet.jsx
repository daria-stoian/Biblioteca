// Bara de sus, afisata pe toate paginile (echivalent _Layout.cshtml + _LoginPartial.cshtml din v1.0)
import { Link, NavLink, useNavigate } from 'react-router';
import { useAuth } from '../auth/useAuth.js';

export default function Antet() {
  const { utilizator, iesire } = useAuth();
  const navigheaza = useNavigate();

  function laIesire() {
    iesire();
    navigheaza('/');
  }

  return (
    <header className="antet">
      <div className="antet-continut">
        <Link to="/" className="logo">BiblioWeb</Link>

        <nav className="meniu">
          <NavLink to="/" end>Catalog</NavLink>
          {utilizator && <NavLink to="/imprumuturi">Împrumuturile mele</NavLink>}
        </nav>

        <div className="utilizator">
          {utilizator ? (
            <>
              <span>{utilizator.numeComplet}</span>
              <button className="buton buton-secundar buton-mic" onClick={laIesire}>Ieșire</button>
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
