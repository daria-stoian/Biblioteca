import { useCallback, useEffect, useState } from 'react';

export default function ListaEditabila({ eticheta, textNou, camp, incarcaLista, adauga, modifica, sterge }) {
  const [elemente, setElemente] = useState(null);
  const [numeNou, setNumeNou] = useState('');
  const [editat, setEditat] = useState(null);
  const [mesaj, setMesaj] = useState(null);

  const incarca = useCallback(() => {
    incarcaLista()
      .then(setElemente)
      .catch((err) => setMesaj({ tip: 'eroare', text: err.message }));
  }, [incarcaLista]);

  useEffect(incarca, [incarca]);

  // Apel la API, mesaj, reincarcarea listei
  async function executa(operatie, textSucces) {
    try {
      await operatie();
      setMesaj({ tip: 'succes', text: textSucces });
      incarca();
      return true;
    } catch (err) {
      setMesaj({ tip: 'eroare', text: err.message });
      return false;
    }
  }

  async function adaugaElement(e) {
    e.preventDefault();
    const nume = numeNou.trim();
    if (await executa(() => adauga({ [camp]: nume }), `Am adăugat „${nume}”.`)) setNumeNou('');
  }

  async function salveaza(e) {
    e.preventDefault();
    if (await executa(() => modifica(editat.id, { [camp]: editat.nume.trim() }), 'Modificare salvată.')) setEditat(null);
  }

  function stergeElement(el) {
    if (!window.confirm(`Ștergi „${el[camp]}”?`)) return;
    executa(() => sterge(el.id), `Am șters „${el[camp]}”.`);
  }

  return (
    <>
      <form className="filtre" onSubmit={adaugaElement}>
        <input placeholder={textNou} value={numeNou} onChange={(e) => setNumeNou(e.target.value)} required />
        <button className="buton">+ Adaugă</button>
      </form>

      {mesaj && <div className={`mesaj mesaj-${mesaj.tip}`}>{mesaj.text}</div>}
      {!elemente && <p className="secundar">Se încarcă…</p>}

      {elemente && (
        <table className="tabel">
          <thead>
            <tr>
              <th>{eticheta}</th>
              <th>Cărți</th>
              <th aria-label="Acțiuni" />
            </tr>
          </thead>
          <tbody>
            {elemente.map((el) =>
              editat?.id === el.id ? (
                <tr key={el.id}>
                  <td colSpan={3}>
                    <form className="actiuni-rand" onSubmit={salveaza}>
                      <input
                        value={editat.nume}
                        onChange={(e) => setEditat({ ...editat, nume: e.target.value })}
                        required
                        autoFocus
                      />
                      <button className="buton buton-mic">Salvează</button>
                      <button type="button" className="buton buton-secundar buton-mic" onClick={() => setEditat(null)}>
                        Renunță
                      </button>
                    </form>
                  </td>
                </tr>
              ) : (
                <tr key={el.id}>
                  <td>{el[camp]}</td>
                  <td>{el._count.carti}</td>
                  <td>
                    <div className="actiuni-rand">
                    <button
                      className="buton buton-secundar buton-mic"
                      onClick={() => setEditat({ id: el.id, nume: el[camp] })}
                    >
                      Redenumește
                    </button>
                    <button
                      className="buton buton-pericol buton-mic"
                      onClick={() => stergeElement(el)}
                      disabled={el._count.carti > 0}
                      title={el._count.carti > 0 ? 'Are cărți în catalog, nu poate fi șters' : undefined}
                    >
                      Șterge
                    </button>
                  </div>
                  </td>
                </tr>
              ),
            )}
          </tbody>
        </table>
      )}
    </>
  );
}
