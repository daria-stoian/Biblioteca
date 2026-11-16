import * as autorService from '../services/autor.service.js';
import { citesteId } from '../utils/citesteId.js';

export async function lista(req, res) {
  res.json(await autorService.listaAutori({ cautare: req.query.cautare }));
}

export async function detalii(req, res) {
  res.json(await autorService.detaliiAutor(citesteId(req.params.id, 'autorului')));
}
