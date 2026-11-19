// Imprumuturile proprii: status, termen, taxa, returnare si prelungire (RF-10, RF-11, RF-12)
// Echivalent v1.0: Views/Imprumut/Index.cshtml
import { useCallback, useEffect, useState } from 'react';
import { Link } from 'react-router';
import { api } from '../api.js';
import Coperta from '../componente/Coperta.jsx';
import { formatData, formatLei, textStatus } from '../format.js';

export default function ImprumuturileMele() {
  const [imprumuturi, setImprumuturi] = useState(null);
  const [mesaj, setMesaj] = useState(null);
  const [inLucru, setInLucru] = useState(null); // id-ul imprumutului la care se lucreaza acum

  const incarca = useCallback(() => {
    api
      .imprumuturileMele()
      .then(setImprumuturi)
      .catch((err) => setMesaj({ tip: 'eroare', text: err.message }));
  }, []);

  useEffect(incarca, [incarca]);

  // Returnare si prelungire au aceeasi forma: apel la API, mesaj, reincarcarea listei
  async function actiune(imprumut, tip) {
    if (tip === 'returnare' && imprumut.taxaEstimata > 0) {
      const confirmat = window.confirm(
        `Returnarea are o taxă de întârziere de ${formatLei(imprumut.taxaEstimata)}. Continui?`,
      );
      if (!confirmat) return;
    }

    setInLucru(imprumut.id);
    setMesaj(null);
    try {
      if (tip === 'returnare') {
        const rezultat = await api.returneaza(imprumut.id);
        const taxa = rezultat.taxaPenalizare > 0 ? ` Taxă de penalizare: ${formatLei(rezultat.taxaPenalizare)}.` : '';
        setMesaj({ tip: 'succes', text: `„${imprumut.carte.titlu}” a fost returnată.${taxa}` });
      } else {
        const rezultat = await api.prelungeste(imprumut.id);
        setMesaj({ tip: 'succes', text: `Termen prelungit până la ${formatData(rezultat.dataScadenta)}.` });
      }
      incarca();
    } catch (err) {
      setMesaj({ tip: 'eroare', text: err.message });
    } finally {
      setInLucru(null);
    }
  }

  if (!imprumuturi) {
    return mesaj ? <div className="mesaj mesaj-eroare">{mesaj.text}</div> : <p className="secundar">Se încarcă…</p>;
  }

  const active = imprumuturi.filter((i) => i.status !== 'Returnat').length;

  return (
    <>
      <h1>Împrumuturile mele</h1>
      <p className="secundar">{active} din 5 împrumuturi active.</p>

      {mesaj && <div className={`mesaj mesaj-${mesaj.tip}`}>{mesaj.text}</div>}

      {imprumuturi.length === 0 && (
        <p>
          Nu ai împrumutat încă nicio carte. <Link to="/">Vezi catalogul</Link>
        </p>
      )}

      <div className="lista-imprumuturi">
        {imprumuturi.map((i) => (
          <article className="card imprumut" key={i.id}>
            <Coperta src={i.carte.imagineCoperta} titlu={i.carte.titlu} />

            <div>
              <Link to={`/carti/${i.carte.id}`}><strong>{i.carte.titlu}</strong></Link>
              <div className="secundar">{i.carte.autor.numeAutor}</div>
              <span className={`eticheta eticheta-${i.status}`}>{textStatus[i.status]}</span>
              <div className="secundar">
                Împrumutată pe {formatData(i.dataImprumut)} ·{' '}
                {i.dataReturnare
                  ? `returnată pe ${formatData(i.dataReturnare)}`
                  : `termen: ${formatData(i.dataScadenta)}`}
              </div>
              {i.status === 'Intarziat' && (
                <div className="taxa">
                  Întârziere: {i.zileIntarziere} {i.zileIntarziere === 1 ? 'zi' : 'zile'} · taxă{' '}
                  {formatLei(i.taxaEstimata)}
                </div>
              )}
              {i.status === 'Returnat' && i.taxaPenalizare > 0 && (
                <div className="taxa">Taxă plătită: {formatLei(i.taxaPenalizare)}</div>
              )}
            </div>

            {i.status !== 'Returnat' && (
              <div className="actiuni">
                {i.poatePrelungi && (
                  <button
                    className="buton buton-secundar buton-mic"
                    disabled={inLucru === i.id}
                    onClick={() => actiune(i, 'prelungire')}
                  >
                    Prelungește +14 zile
                  </button>
                )}
                <button className="buton buton-mic" disabled={inLucru === i.id} onClick={() => actiune(i, 'returnare')}>
                  Returnează
                </button>
              </div>
            )}
          </article>
        ))}
      </div>
    </>
  );
}
