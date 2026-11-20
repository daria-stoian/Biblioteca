import { api } from '../../api.js';
import ListaEditabila from './ListaEditabila.jsx';

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
