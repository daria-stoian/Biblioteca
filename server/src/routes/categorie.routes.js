import { Router } from 'express';
import { lista, adauga, modifica, sterge } from '../controllers/categorie.controller.js';
import { doarAdministrator } from '../middleware/auth.js';

const router = Router();

/**
 * @openapi
 * /categorii:
 *   get:
 *     summary: Lista categoriilor (RF-03)
 *     tags: [Catalog]
 *     responses:
 *       200:
 *         description: Categoriile, în ordine alfabetică, cu numărul de cărți
 *         content:
 *           application/json:
 *             example: [{ id: 1, numeCategorie: Fantasy, _count: { carti: 7 } }]
 */
router.get('/', lista);

/**
 * @openapi
 * /categorii:
 *   post:
 *     summary: Adaugă (RF-20, administrator)
 *     tags: [Administrare]
 *     security: [{ bearerAuth: [] }]
 *     requestBody: { required: true, content: { application/json: { example: { numeCategorie: Literatură română } } } }
 *     responses:
 *       201: { description: Adăugat }
 *       400: { description: Date invalide }
 *       403: { description: Utilizatorul nu este administrator }
 * /categorii/{id}:
 *   put:
 *     summary: Modifică (RF-20, administrator)
 *     tags: [Administrare]
 *     security: [{ bearerAuth: [] }]
 *     parameters: [{ in: path, name: id, required: true, schema: { type: integer } }]
 *     requestBody: { required: true, content: { application/json: { example: { numeCategorie: Literatură română } } } }
 *     responses:
 *       200: { description: Modificat }
 *       404: { description: Nu există }
 *   delete:
 *     summary: Șterge, doar dacă nu are cărți (RF-20, administrator)
 *     tags: [Administrare]
 *     security: [{ bearerAuth: [] }]
 *     parameters: [{ in: path, name: id, required: true, schema: { type: integer } }]
 *     responses:
 *       204: { description: Șters }
 *       409: { description: Are cărți în catalog }
 */
router.post('/', doarAdministrator, adauga);
router.put('/:id', doarAdministrator, modifica);
router.delete('/:id', doarAdministrator, sterge);

export default router;
