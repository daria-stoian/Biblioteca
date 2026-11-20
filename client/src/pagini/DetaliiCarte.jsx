// Detaliile unei carti si butonul "Împrumută" (RF-02, RF-09)
// Echivalent v1.0: Views/Carte/Detalii.cshtml + Views/Imprumut/Imprumuta.cshtml
import { useEffect, useState } from 'react';
import { Link, useParams } from 'react-router';
import { api } from '../api.js';
import { useAuth } from '../auth/useAuth.js';
import Coperta from '../componente/Coperta.jsx';
import { formatData, textStatus } from '../format.js';

const stele = (nota) => '★'.repeat(nota) + '☆'.repeat(5 - nota);

export default function DetaliiCarte() {
  const { id } = useParams();
  const { utilizator } = useAuth();
  // Raspunsul tine minte pentru ce id a venit: daca id-ul din adresa s-a schimbat,
  // raspunsul vechi este ignorat si pagina arata "Se încarcă…"
  const [incarcat, setIncarcat] = useState({ id: null, carte: null, eroare: '' });
  const [mesaj, setMesaj] = useState(null); // rezultatul ultimului imprumut: { tip, text }
  const [seTrimite, setSeTrimite] = useState(false);

  useEffect(() => {
    api
      .carte(id)
      .then((carte) => setIncarcat({ id, carte, eroare: '' }))
      .catch((err) => setIncarcat({ id, carte: null, eroare: err.message }));
  }, [id]);

  const esteCurent = incarcat.id === id;
  const carte = esteCurent ? incarcat.carte : null;
  const eroare = esteCurent ? incarcat.eroare : '';

  async function laImprumut() {
    setSeTrimite(true);
    setMesaj(null);
    try {
      const imprumut = await api.imprumuta(carte.id);
      setMesaj({ tip: 'succes', text: `Carte împrumutată! Termen de returnare: ${formatData(imprumut.dataScadenta)}.` });
    } catch (err) {
      // Regulile sunt verificate pe server; aici doar afisam mesajul primit
      setMesaj({ tip: 'eroare', text: err.message });
    } finally {
      setSeTrimite(false);
    }
  }

  if (eroare) return <div className="mesaj mesaj-eroare">{eroare}</div>;
  if (!carte) return <p className="secundar">Se încarcă…</p>;

  return (
    <>
      <p><Link to="/">← Înapoi la catalog</Link></p>

      <div className="detalii">
        <Coperta src={carte.imagineCoperta} titlu={carte.titlu} />

        <div>
          <h1>{carte.titlu}</h1>
          <p className="secundar">{carte.autor.numeAutor}</p>
          <span className={`eticheta eticheta-${carte.status}`}>{textStatus[carte.status]}</span>
          {utilizator?.rol === 'Administrator' && (
            <Link to={`/admin/carti/${carte.id}`} className="buton buton-secundar buton-mic" style={{ marginLeft: '0.75rem' }}>
              Editează
            </Link>
          )}

          <dl className="date-carte">
            <dt>Categorie</dt><dd>{carte.categorie.numeCategorie}</dd>
            <dt>An publicare</dt><dd>{carte.anPublicare}</dd>
            <dt>Pagini</dt><dd>{carte.numarPagini}</dd>
            {carte.isbn && (<><dt>ISBN</dt><dd>{carte.isbn}</dd></>)}
            <dt>Recenzii</dt>
            <dd>
              {carte.numarRecenzii === 0
                ? 'încă nu există'
                : `${carte.notaMedie} / 5 (${carte.numarRecenzii})`}
            </dd>
          </dl>

          {carte.descriere && <p>{carte.descriere}</p>}

          {mesaj && (
            <div className={`mesaj mesaj-${mesaj.tip}`}>
              {mesaj.text}{' '}
              {mesaj.tip === 'succes' && <Link to="/imprumuturi">Vezi împrumuturile tale</Link>}
            </div>
          )}

          {utilizator ? (
            <button className="buton" onClick={laImprumut} disabled={!carte.disponibila || seTrimite}>
              {seTrimite ? 'Se împrumută…' : 'Împrumută pentru 14 zile'}
            </button>
          ) : (
            <p>
              <Link to="/login" state={{ dinPagina: `/carti/${carte.id}` }}>Autentifică-te</Link> pentru a împrumuta
              această carte.
            </p>
          )}
        </div>
      </div>

      <section className="card" style={{ marginTop: '2rem' }}>
        <h2>Recenzii</h2>
        {carte.recenzii.length === 0 && <p className="secundar">Nicio recenzie încă.</p>}
        {carte.recenzii.map((r) => (
          <div className="recenzie" key={r.id}>
            <span className="stele" aria-label={`${r.nota} din 5`}>{stele(r.nota)}</span>{' '}
            <strong>{r.utilizator.numeComplet}</strong>{' '}
            <span className="secundar">· {formatData(r.dataPostare)}</span>
            {r.comentariu && <p>{r.comentariu}</p>}
          </div>
        ))}
      </section>
    </>
  );
}
