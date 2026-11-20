import { useCallback, useEffect, useState } from 'react';
import { Link, useLocation } from 'react-router';
import { api } from '../../api.js';
import Paginare from '../../componente/Paginare.jsx';
import { textStatus } from '../../format.js';

export default function AdminCarti() {
  const locatie = useLocation();
  const [cautare, setCautare] = useState('');
  const [filtre, setFiltre] = useState({ cautare: '', pagina: 1 });
  const [rezultat, setRezultat] = useState(null);
  // Mesajul poate veni si din formular, dupa salvare
  const [mesaj, setMesaj] = useState(locatie.state?.mesaj ?? null);

  const incarca = useCallback(() => {
    api
      .carti({ ...filtre, limita: 20 })
      .then(setRezultat)
      .catch((err) => setMesaj({ tip: 'eroare', text: err.message }));
  }, [filtre]);

  useEffect(incarca, [incarca]);

  async function schimbaStatus(carte) {
    const nou = carte.status === 'Disponibil' ? 'Indisponibil' : 'Disponibil';
    try {
      await api.admin.schimbaStatus(carte.id, nou);
      setMesaj({ tip: 'succes', text: `„${carte.titlu}” este acum ${textStatus[nou].toLowerCase()}.` });
      incarca();
    } catch (err) {
      setMesaj({ tip: 'eroare', text: err.message });
    }
  }

  async function sterge(carte) {
    if (!window.confirm(`Ștergi definitiv „${carte.titlu}”?`)) return;
    try {
      await api.admin.stergeCarte(carte.id);
      setMesaj({ tip: 'succes', text: `„${carte.titlu}” a fost ștearsă.` });
      incarca();
    } catch (err) {
      // De exemplu: cartea are imprumuturi, deci serverul refuza stergerea (409)
      setMesaj({ tip: 'eroare', text: err.message });
    }
  }

  return (
    <>
      <div className="bara-actiuni">
        <form
          className="filtre"
          onSubmit={(e) => {
            e.preventDefault();
            setFiltre({ cautare: cautare.trim(), pagina: 1 });
          }}
        >
          <input type="search" placeholder="Caută după titlu…" value={cautare} onChange={(e) => setCautare(e.target.value)} />
          <button className="buton buton-secundar">Caută</button>
        </form>
        <Link to="/admin/carti/noua" className="buton">+ Adaugă carte</Link>
      </div>

      {mesaj && <div className={`mesaj mesaj-${mesaj.tip}`}>{mesaj.text}</div>}
      {!rezultat && <p className="secundar">Se încarcă…</p>}

      {rezultat && (
        <>
          <table className="tabel">
            <thead>
              <tr>
                <th>Titlu</th>
                <th>Autor</th>
                <th>Categorie</th>
                <th>An</th>
                <th>Status</th>
                <th aria-label="Acțiuni" />
              </tr>
            </thead>
            <tbody>
              {rezultat.elemente.map((c) => (
                <tr key={c.id}>
                  <td><Link to={`/carti/${c.id}`}>{c.titlu}</Link></td>
                  <td>{c.autor.numeAutor}</td>
                  <td>{c.categorie.numeCategorie}</td>
                  <td>{c.anPublicare}</td>
                  <td>
                    <button
                      className={`eticheta eticheta-${c.status} eticheta-buton`}
                      title="Click pentru a schimba statusul"
                      onClick={() => schimbaStatus(c)}
                    >
                      {textStatus[c.status]} ⇄
                    </button>
                  </td>
                  <td>
                    <div className="actiuni-rand">
                    <Link to={`/admin/carti/${c.id}`} className="buton buton-secundar buton-mic">Editează</Link>
                    <button className="buton buton-pericol buton-mic" onClick={() => sterge(c)}>Șterge</button>
                  </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          {rezultat.total === 0 && <p className="secundar">Nicio carte găsită.</p>}
          <Paginare
            pagina={rezultat.pagina}
            totalPagini={rezultat.totalPagini}
            schimbaPagina={(pagina) => setFiltre({ ...filtre, pagina })}
          />
        </>
      )}
    </>
  );
}
