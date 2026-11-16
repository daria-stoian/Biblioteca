import * as categorieService from '../services/categorie.service.js';

export async function lista(req, res) {
  res.json(await categorieService.listaCategorii());
}
