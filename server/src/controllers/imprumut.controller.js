import * as imprumutService from '../services/imprumut.service.js';
import { citesteId } from '../utils/citesteId.js';

export async function imprumuta(req, res) {
  const carteId = citesteId(req.body?.carteId, 'cărții');
  res.status(201).json(await imprumutService.imprumuta(req.utilizator.id, carteId));
}

export async function aleMele(req, res) {
  res.json(await imprumutService.aleMele(req.utilizator.id));
}

export async function toate(req, res) {
  res.json(await imprumutService.toateDeschise());
}

export async function returneaza(req, res) {
  res.json(await imprumutService.returneaza(citesteId(req.params.id, 'împrumutului'), req.utilizator));
}

export async function prelungeste(req, res) {
  res.json(await imprumutService.prelungeste(citesteId(req.params.id, 'împrumutului'), req.utilizator));
}
