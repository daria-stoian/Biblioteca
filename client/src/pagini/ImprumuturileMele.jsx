import { useCallback, useEffect, useState } from 'react';
import { Link } from 'react-router';
import { api } from '../api.js';
import Coperta from '../componente/Coperta.jsx';
import { formatData, formatLei, textStatus } from '../format.js';

const MAX_IMPRUMUTURI = 5;

function TimpRamas({ imprumut }) {
  if (imprumut.status === 'Intarziat') {
    return (
      <div className="timp timp-depasit">
        Zile de întârziere: {imprumut.zileIntarziere} · taxă {formatLei(imprumut.taxaEstimata)}
      </div>
    );
  }
  const zile = imprumut.zileRamase; // calculat pe server pentru zilele de intarziere
  if (zile <= 1) return <div className="timp timp-urgent">Termenul expiră {zile === 0 ? 'astăzi' : 'în mai puțin de o zi'}</div>;
  return <div className={`timp ${zile <= 3 ? 'timp-urgent' : ''}`}>Zile rămase: {zile}</div>;
}

export default function ImprumuturileMele() {
  const [imprumuturi, setImprumuturi] = useState(null);
  const [mesaj, setMesaj] = useState(null);
  const [inLucru, setInLucru] = useState(null);

  const incarca = useCallback(() => {
    api
      .imprumuturileMele()
      .then(setImprumuturi)
      .catch((err) => setMesaj({ tip: 'eroare', text: err.message }));
  }, []);

  useEffect(incarca, [incarca]);

  // Returnare si prelungire
  async function actiune(imprumut, tip) {
    if (tip === 'returnare' && imprumut.taxaEstimata > 0) {
      const confirmat = window.confirm(
        `Returnarea are o taxă de întârziere de ${formatLei(imprumut.taxaEstimata)}. Continuati?`,
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

  const deschise = imprumuturi.filter((i) => i.status !== 'Returnat');
  const istoric = imprumuturi.filter((i) => i.status === 'Returnat');
  const intarziate = deschise.filter((i) => i.status === 'Intarziat');
  const taxaDePlata = intarziate.reduce((suma, i) => suma + i.taxaEstimata, 0);

  const rand = (i) => (
    <article className={`card imprumut ${i.status === 'Intarziat' ? 'imprumut-intarziat' : ''}`} key={i.id}>
      <Coperta src={i.carte.imagineCoperta} titlu={i.carte.titlu} />

      <div>
        <Link to={`/carti/${i.carte.id}`}><strong>{i.carte.titlu}</strong></Link>
        <div className="secundar">{i.carte.autor.numeAutor}</div>
        <span className={`eticheta eticheta-${i.status}`}>{textStatus[i.status]}</span>
        <div className="secundar mic">
          Împrumutată pe {formatData(i.dataImprumut)} ·{' '}
          {i.dataReturnare ? `returnată pe ${formatData(i.dataReturnare)}` : `termen: ${formatData(i.dataScadenta)}`}
        </div>
        {i.status !== 'Returnat' && <TimpRamas imprumut={i} />}
        {i.status === 'Returnat' && i.taxaPenalizare > 0 && (
          <div className="taxa mic">Taxă plătită: {formatLei(i.taxaPenalizare)}</div>
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
  );

  return (
    <>
      <h1>Împrumuturile mele</h1>
      <div className="rezumat">
        <div className="card rezumat-element">
          <div className="secundar mic">Împrumuturi active</div>
          <div className="rezumat-valoare">
            {deschise.length} <span className="secundar">din {MAX_IMPRUMUTURI}</span>
          </div>
          <div className="bara-progres" role="progressbar" aria-valuenow={deschise.length} aria-valuemin={0} aria-valuemax={MAX_IMPRUMUTURI}>
            <div
              className={deschise.length >= MAX_IMPRUMUTURI ? 'plina' : ''}
              style={{ width: `${(Math.min(deschise.length, MAX_IMPRUMUTURI) / MAX_IMPRUMUTURI) * 100}%` }}
            />
          </div>
          <div className="secundar mic">
            {deschise.length >= MAX_IMPRUMUTURI
              ? 'Ai atins limita. Returnează o carte ca să poți împrumuta alta.'
              : `Mai poți împrumuta ${MAX_IMPRUMUTURI - deschise.length}.`}
          </div>
        </div>
        <div className={`card rezumat-element ${intarziate.length ? 'rezumat-alerta' : ''}`}>
          <div className="secundar mic">Cu termenul depășit</div>
          <div className="rezumat-valoare">{intarziate.length}</div>
          <div className="secundar mic">{intarziate.length === 0
            ? 'Totul e în regulă.'
            : 'Returnează cât mai curând.'}</div>
        </div>
        <div className={`card rezumat-element ${taxaDePlata ? 'rezumat-alerta' : ''}`}>
          <div className="secundar mic">Taxă de întârziere acumulată</div>
          <div className="rezumat-valoare">{formatLei(taxaDePlata)}</div>
          <div className="secundar mic">1 RON pentru fiecare zi începută.</div>
        </div>
      </div>

      {mesaj && <div className={`mesaj mesaj-${mesaj.tip}`}>{mesaj.text}</div>}

      <h2>În curs</h2>
      {deschise.length === 0 ? (
        <p className="secundar">
          Nu ai nicio carte împrumutată acum. <Link to="/">Caută ceva de citit în catalog</Link>
        </p>
      ) : (
        <div className="lista-imprumuturi">{deschise.map(rand)}</div>
      )}

      {istoric.length > 0 && (
        <>
          <h2 className="titlu-sectiune">Istoric</h2>
          <div className="lista-imprumuturi istoric">{istoric.map(rand)}</div>
        </>
      )}
    </>
  );
}
