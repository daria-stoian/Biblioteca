// Catalogul public: cautare, filtrare pe categorie, paginare (RF-01, RN-03)
// Echivalent v1.0: Views/Carte/Index.cshtml
import { useEffect, useState } from 'react';
import { useSearchParams } from 'react-router';
import { api } from '../api.js';
import CarteCard from '../componente/CarteCard.jsx';
import Paginare from '../componente/Paginare.jsx';

export default function Catalog() {
  const [parametri, setParametri] = useSearchParams();
  const cautare = parametri.get('cautare') || '';
  const categorieId = parametri.get('categorieId') || '';
  const pagina = Number(parametri.get('pagina')) || 1;

  const [textCautare, setTextCautare] = useState(cautare);
  const [categorii, setCategorii] = useState([]);
  const [rezultat, setRezultat] = useState(null);
  const [eroare, setEroare] = useState('');

  // Daca adresa se schimba din butonul "Inapoi" al browserului, actualizam si campul de cautare.
  // (Tiparul recomandat de React: ajustarea starii in timpul randarii, nu intr-un useEffect.)
  const [cautareAnterioara, setCautareAnterioara] = useState(cautare);
  if (cautare !== cautareAnterioara) {
    setCautareAnterioara(cautare);
    setTextCautare(cautare);
  }

  // Categoriile se incarca o singura data, pentru lista derulanta
  useEffect(() => {
    api.categorii().then(setCategorii).catch(() => setCategorii([]));
  }, []);

  // Cartile se reincarca la fiecare schimbare de cautare, categorie sau pagina
  useEffect(() => {
    let anulat = false; // ignora raspunsurile vechi daca utilizatorul a schimbat filtrele intre timp
    api
      .carti({ cautare, categorieId, pagina })
      .then((r) => {
        if (anulat) return;
        setRezultat(r);
        setEroare('');
      })
      .catch((err) => !anulat && setEroare(err.message));
    return () => {
      anulat = true;
    };
  }, [cautare, categorieId, pagina]);

  // Actualizeaza adresa paginii; orice filtru nou te duce inapoi la pagina 1
  function seteaza(modificari) {
    const noi = { cautare, categorieId, pagina: 1, ...modificari };
    const adresa = {};
    if (noi.cautare) adresa.cautare = noi.cautare;
    if (noi.categorieId) adresa.categorieId = noi.categorieId;
    if (noi.pagina > 1) adresa.pagina = noi.pagina; // pagina 1 nu mai apare in adresa
    setParametri(adresa);
  }

  function laCautare(e) {
    e.preventDefault();
    seteaza({ cautare: textCautare.trim() });
  }

  return (
    <>
      <h1>Catalog</h1>

      <form className="filtre" onSubmit={laCautare} role="search">
        <input
          type="search"
          placeholder="Caută după titlu…"
          value={textCautare}
          onChange={(e) => setTextCautare(e.target.value)}
        />
        <select value={categorieId} onChange={(e) => seteaza({ categorieId: e.target.value })} aria-label="Categorie">
          <option value="">Toate categoriile</option>
          {categorii.map((c) => (
            <option key={c.id} value={c.id}>
              {c.numeCategorie} ({c._count.carti})
            </option>
          ))}
        </select>
        <button className="buton">Caută</button>
      </form>

      {eroare && <div className="mesaj mesaj-eroare">{eroare}</div>}
      {!rezultat && !eroare && <p className="secundar">Se încarcă…</p>}

      {rezultat && (
        <>
          <p className="secundar">
            {rezultat.total === 0 ? 'Nicio carte găsită.' : `${rezultat.total} cărți găsite`}
          </p>
          <div className="grila">
            {rezultat.elemente.map((carte) => (
              <CarteCard key={carte.id} carte={carte} />
            ))}
          </div>
          <Paginare
            pagina={rezultat.pagina}
            totalPagini={rezultat.totalPagini}
            laSchimbare={(p) => seteaza({ pagina: p })}
          />
        </>
      )}
    </>
  );
}
