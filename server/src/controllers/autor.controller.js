import * as autorService from '../services/autor.service.js';
import { citesteId } from '../utils/citesteId.js';

export async function lista(req, res) {
  res.json(await autorService.listaAutori({ cautare: req.query.cautare }));
}

export async function detalii(req, res) {
  res.json(await autorService.detaliiAutor(citesteId(req.params.id, 'autorului')));
}

// ---- Administrare
export async function adauga(req, res) {
  res.status(201).json(await autorService.adaugaAutor(req.body ?? {}));
}

export async function modifica(req, res) {
  res.json(await autorService.modificaAutor(citesteId(req.params.id, 'autorului'), req.body ?? {}));
}

export async function sterge(req, res) {
  await autorService.stergeAutor(citesteId(req.params.id, 'autorului'));
  res.status(204).end();
}
