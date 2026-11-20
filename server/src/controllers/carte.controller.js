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

// ---- Administrare
export async function coperti(req, res) {
  res.json(await carteService.copertiDisponibile());
}

export async function adauga(req, res) {
  res.status(201).json(await carteService.adaugaCarte(req.body ?? {}));
}

export async function modifica(req, res) {
  res.json(await carteService.modificaCarte(citesteId(req.params.id, 'cărții'), req.body ?? {}));
}

export async function schimbaStatus(req, res) {
  res.json(await carteService.schimbaStatus(citesteId(req.params.id, 'cărții'), req.body?.status));
}

export async function sterge(req, res) {
  await carteService.stergeCarte(citesteId(req.params.id, 'cărții'));
  res.status(204).end();
}
