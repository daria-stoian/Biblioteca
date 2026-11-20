// Toate imprumuturile nereturnate, cu cititorul; administratorul poate inregistra returnarea.
// Folosește GET /api/imprumuturi (doar administrator) si aceeasi ruta de returnare ca cititorul;
// serverul permite returnarea imprumutului altcuiva doar pentru rolul Administrator.
import { useCallback, useEffect, useState } from 'react';
import { api } from '../../api.js';
import { formatData, formatLei, numar, textStatus } from '../../format.js';

export default function AdminImprumuturi() {
  const [imprumuturi, setImprumuturi] = useState(null);
  const [mesaj, setMesaj] = useState(null);

  const incarca = useCallback(() => {
    api.admin
      .imprumuturiDeschise()
      .then(setImprumuturi)
      .catch((err) => setMesaj({ tip: 'eroare', text: err.message }));
  }, []);

  useEffect(incarca, [incarca]);

  async function returneaza(i) {
    const taxa = i.taxaEstimata > 0 ? ` Taxa de întârziere: ${formatLei(i.taxaEstimata)}.` : '';
    if (!window.confirm(`Înregistrezi returnarea „${i.carte.titlu}” de la ${i.utilizator.numeComplet}?${taxa}`)) return;
    try {
      await api.returneaza(i.id);
      setMesaj({ tip: 'succes', text: `Returnare înregistrată: „${i.carte.titlu}”.${taxa}` });
      incarca();
    } catch (err) {
      setMesaj({ tip: 'eroare', text: err.message });
    }
  }

  if (!imprumuturi) {
    return mesaj ? <div className="mesaj mesaj-eroare">{mesaj.text}</div> : <p className="secundar">Se încarcă…</p>;
  }

  const intarziate = imprumuturi.filter((i) => i.status === 'Intarziat').length;

  return (
    <>
      <p className="secundar">
        {numar(imprumuturi.length, 'împrumut nereturnat', 'împrumuturi nereturnate')}
        {intarziate > 0 && `, dintre care ${intarziate} cu termenul depășit`}.
      </p>
      {mesaj && <div className={`mesaj mesaj-${mesaj.tip}`}>{mesaj.text}</div>}

      {imprumuturi.length > 0 && (
        <table className="tabel">
          <thead>
            <tr>
              <th>Cititor</th>
              <th>Carte</th>
              <th>Împrumutată</th>
              <th>Termen</th>
              <th>Status</th>
              <th>Taxă</th>
              <th aria-label="Acțiuni" />
            </tr>
          </thead>
          <tbody>
            {imprumuturi.map((i) => (
              <tr key={i.id}>
                <td>
                  {i.utilizator.numeComplet}
                  <div className="secundar mic">{i.utilizator.email}</div>
                </td>
                <td>{i.carte.titlu}</td>
                <td>{formatData(i.dataImprumut)}</td>
                <td>{formatData(i.dataScadenta)}</td>
                <td><span className={`eticheta eticheta-${i.status}`}>{textStatus[i.status]}</span></td>
                <td className={i.taxaEstimata > 0 ? 'taxa' : ''}>{i.taxaEstimata > 0 ? formatLei(i.taxaEstimata) : '–'}</td>
                <td>
                    <div className="actiuni-rand">
                  <button className="buton buton-mic" onClick={() => returneaza(i)}>Returnare</button>
                </div>
                  </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </>
  );
}
