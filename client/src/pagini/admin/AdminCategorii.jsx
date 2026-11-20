import { api } from '../../api.js';
import ListaEditabila from './ListaEditabila.jsx';

export default function AdminCategorii() {
  return (
    <ListaEditabila
      eticheta="Categorie"
      textNou="Categorie nouă…"
      camp="numeCategorie"
      incarcaLista={api.categorii}
      adauga={api.admin.adaugaCategorie}
      modifica={api.admin.modificaCategorie}
      sterge={api.admin.stergeCategorie}
    />
  );
}
