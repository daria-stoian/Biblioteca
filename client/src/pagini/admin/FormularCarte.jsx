import { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router';
import { api } from '../../api.js';

const gol = {
  titlu: '',
  autorId: '',
  categorieId: '',
  anPublicare: '',
  numarPagini: '',
  isbn: '',
  descriere: '',
  imagineCoperta: '',
  status: 'Disponibil',
};

export default function FormularCarte() {
  const { id } = useParams();
  const esteEditare = Boolean(id);
  const navigheaza = useNavigate();

  const [date, setDate] = useState(gol);
  const [optiuni, setOptiuni] = useState(null); // { autori, categorii, coperti }
  const [eroare, setEroare] = useState('');
  const [seSalveaza, setSeSalveaza] = useState(false);

  // Listele pentru campurile de selectie si, la editare, datele cartii
  useEffect(() => {
    Promise.all([api.autori(), api.categorii(), api.admin.coperti(), esteEditare ? api.carte(id) : null])
      .then(([autori, categorii, coperti, carte]) => {
        setOptiuni({ autori, categorii, coperti });
        if (carte) {
          setDate({
            titlu: carte.titlu,
            autorId: carte.autor.id,
            categorieId: carte.categorie.id,
            anPublicare: carte.anPublicare,
            numarPagini: carte.numarPagini,
            isbn: carte.isbn ?? '',
            descriere: carte.descriere ?? '',
            imagineCoperta: carte.imagineCoperta ?? '',
            status: carte.status,
          });
        }
      })
      .catch((err) => setEroare(err.message));
  }, [id, esteEditare]);

  const schimba = (camp) => (e) => setDate({ ...date, [camp]: e.target.value });

  async function salveaza(e) {
    e.preventDefault();
    setEroare('');
    setSeSalveaza(true);
    try {
      // Validarea completa se face pe server
      const salvata = esteEditare ? await api.admin.modificaCarte(id, date) : await api.admin.adaugaCarte(date);
      navigheaza('/admin', {
        state: { mesaj: { tip: 'succes', text: `„${salvata.titlu}” a fost ${esteEditare ? 'modificată' : 'adăugată'}.` } },
      });
    } catch (err) {
      setEroare(err.message);
      window.scrollTo({ top: 0, behavior: 'smooth' });
    } finally {
      setSeSalveaza(false);
    }
  }

  if (!optiuni) return eroare ? <div className="mesaj mesaj-eroare">{eroare}</div> : <p className="secundar">Se încarcă…</p>;

  return (
    <form className="card formular-lat" onSubmit={salveaza}>
      <h2>{esteEditare ? 'Editează cartea' : 'Carte nouă'}</h2>
      {eroare && <div className="mesaj mesaj-eroare">{eroare}</div>}

      <div className="formular-coloane">
        <div>
          <label className="camp">
            Titlu *
            <input value={date.titlu} onChange={schimba('titlu')} required maxLength={200} />
          </label>
          <div className="rand-campuri">
            <label className="camp">
              Autor *
              <select value={date.autorId} onChange={schimba('autorId')} required>
                <option value="">Alege…</option>
                {optiuni.autori.map((a) => <option key={a.id} value={a.id}>{a.numeAutor}</option>)}
              </select>
            </label>
            <label className="camp">
              Categorie *
              <select value={date.categorieId} onChange={schimba('categorieId')} required>
                <option value="">Alege…</option>
                {optiuni.categorii.map((c) => <option key={c.id} value={c.id}>{c.numeCategorie}</option>)}
              </select>
            </label>
          </div>
          <div className="rand-campuri">
            <label className="camp">
              An publicare *
              <input type="number" value={date.anPublicare} onChange={schimba('anPublicare')} required />
            </label>
            <label className="camp">
              Număr pagini *
              <input type="number" min={1} value={date.numarPagini} onChange={schimba('numarPagini')} required />
            </label>
            <label className="camp">
              ISBN
              <input value={date.isbn} onChange={schimba('isbn')} maxLength={20} />
            </label>
          </div>
          <label className="camp">
            Descriere
            <textarea rows={4} value={date.descriere} onChange={schimba('descriere')} maxLength={2000} />
          </label>
          <div className="rand-campuri">
            <label className="camp">
              Copertă
              <select value={date.imagineCoperta} onChange={schimba('imagineCoperta')}>
                <option value="">Fără copertă</option>
                {optiuni.coperti.map((c) => <option key={c} value={c}>{c.replace('/coperti/', '')}</option>)}
              </select>
            </label>
            <label className="camp">
              Status
              <select value={date.status} onChange={schimba('status')}>
                <option value="Disponibil">Disponibilă</option>
                <option value="Indisponibil">Indisponibilă</option>
              </select>
            </label>
          </div>
        </div>

        <div className="previzualizare">
          {date.imagineCoperta ? (
            <img className="coperta" src={date.imagineCoperta} alt="Previzualizare copertă" />
          ) : (
            <div className="coperta" />
          )}
        </div>
      </div>

      <div className="actiuni-rand">
        <button className="buton" disabled={seSalveaza}>{seSalveaza ? 'Se salvează…' : 'Salvează'}</button>
        <Link to="/admin" className="buton buton-secundar">Renunță</Link>
      </div>
    </form>
  );
}
