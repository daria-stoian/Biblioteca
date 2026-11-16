import * as carteService from '../services/carte.service.js';
import { citestePaginare } from '../utils/paginare.js';
import { citesteId } from '../utils/citesteId.js';

// GET /api/carti?cautare=harry&categorieId=1&pagina=1&limita=12
export async function lista(req, res) {
  const { cautare, categorieId } = req.query;
  const rezultat = await carteService.listaCarti({
    cautare,
    categorieId: categorieId ? citesteId(categorieId, 'categoriei') : undefined,
    paginare: citestePaginare(req.query),
  });
  res.json(rezultat);
}

export async function detalii(req, res) {
  res.json(await carteService.detaliiCarte(citesteId(req.params.id, 'cărții')));
}
