// Autorii (RF-19). Echivalent v1.0: Views/Autor/*.cshtml
import { api } from '../../api.js';
import ListaEditabila from './ListaEditabila.jsx';

// Functiile sunt definite in afara componentei, ca sa ramana aceleasi la fiecare randare
const incarcaAutori = () => api.autori();

export default function AdminAutori() {
  return (
    <ListaEditabila
      eticheta="Autor"
      textNou="Autor nou…"
      camp="numeAutor"
      incarcaLista={incarcaAutori}
      adauga={api.admin.adaugaAutor}
      modifica={api.admin.modificaAutor}
      sterge={api.admin.stergeAutor}
    />
  );
}
