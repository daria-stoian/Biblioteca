import { useState } from 'react';
import { Link, useLocation, useNavigate } from 'react-router';
import { useAuth } from '../auth/useAuth.js';

export default function Inregistrare() {
  const { inregistrare } = useAuth();
  const navigheaza = useNavigate();
  const locatie = useLocation();
  const [date, setDate] = useState({ numeComplet: '', email: '', parola: '' });
  const [eroare, setEroare] = useState('');
  const [seTrimite, setSeTrimite] = useState(false);

  const schimba = (camp) => (e) => setDate({ ...date, [camp]: e.target.value });

  async function creeazaCont(e) {
    e.preventDefault();
    setEroare('');
    setSeTrimite(true);
    try {
      await inregistrare(date.numeComplet, date.email, date.parola);
      navigheaza(locatie.state?.dinPagina || '/', { replace: true });
    } catch (err) {
      // email folosit, parola prea scurta etc
      setEroare(err.message);
    } finally {
      setSeTrimite(false);
    }
  }

  return (
    <form className="card formular" onSubmit={creeazaCont}>
      <h1>Cont nou</h1>
      {eroare && <div className="mesaj mesaj-eroare">{eroare}</div>}

      <label className="camp">
        Nume complet
        <input value={date.numeComplet} onChange={schimba('numeComplet')} required autoFocus />
      </label>
      <label className="camp">
        Email
        <input type="email" value={date.email} onChange={schimba('email')} required />
      </label>
      <label className="camp">
        Parolă <span className="secundar">(minim 6 caractere)</span>
        <input type="password" value={date.parola} onChange={schimba('parola')} required minLength={6} />
      </label>

      <button className="buton" disabled={seTrimite}>{seTrimite ? 'Se creează…' : 'Creează contul'}</button>
      <p className="secundar">
        Ai deja cont? <Link to="/login" state={locatie.state}>Autentifică-te</Link>
      </p>
    </form>
  );
}
