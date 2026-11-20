import * as categorieService from '../services/categorie.service.js';
import { citesteId } from '../utils/citesteId.js';

export async function lista(req, res) {
  res.json(await categorieService.listaCategorii());
}

// Administrare
export async function adauga(req, res) {
  res.status(201).json(await categorieService.adaugaCategorie(req.body ?? {}));
}

export async function modifica(req, res) {
  res.json(await categorieService.modificaCategorie(citesteId(req.params.id, 'categoriei'), req.body ?? {}));
}

export async function sterge(req, res) {
  await categorieService.stergeCategorie(citesteId(req.params.id, 'categoriei'));
  res.status(204).end();
}
