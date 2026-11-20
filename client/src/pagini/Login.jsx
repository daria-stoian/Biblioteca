import { useState } from 'react';
import { Link, useLocation, useNavigate } from 'react-router';
import { useAuth } from '../auth/useAuth.js';

export default function Login() {
  const { autentificare } = useAuth();
  const navigheaza = useNavigate();
  const locatie = useLocation();
  const [email, setEmail] = useState('');
  const [parola, setParola] = useState('');
  const [eroare, setEroare] = useState('');
  const [seTrimite, setSeTrimite] = useState(false);

  async function autentifica(e) {
    e.preventDefault();
    setEroare('');
    setSeTrimite(true);
    try {
      await autentificare(email, parola);
      navigheaza(locatie.state?.dinPagina || '/', { replace: true });
    } catch (err) {
      setEroare(err.message);
    } finally {
      setSeTrimite(false);
    }
  }

  return (
    <form className="card formular" onSubmit={autentifica}>
      <h1>Autentificare</h1>
      {eroare && <div className="mesaj mesaj-eroare">{eroare}</div>}

      <label className="camp">
        Email
        <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required autoFocus />
      </label>
      <label className="camp">
        Parolă
        <input type="password" value={parola} onChange={(e) => setParola(e.target.value)} required />
      </label>

      <button className="buton" disabled={seTrimite}>{seTrimite ? 'Se verifică…' : 'Intră în cont'}</button>
      <p className="secundar">
        Nu ai cont? <Link to="/inregistrare" state={locatie.state}>Creează unul</Link>
      </p>
    </form>
  );
}
