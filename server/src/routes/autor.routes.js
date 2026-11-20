// Citirea este publica (Vizitator, RF-03); modificarile sunt doar pentru administrator (RF-19).
import { Router } from 'express';
import { lista, detalii, adauga, modifica, sterge } from '../controllers/autor.controller.js';
import { doarAdministrator } from '../middleware/auth.js';

const router = Router();

/**
 * @openapi
 * /autori:
 *   get:
 *     summary: Lista autorilor, cu căutare după nume (RF-03)
 *     tags: [Catalog]
 *     parameters:
 *       - { in: query, name: cautare, schema: { type: string }, example: tolkien }
 *     responses:
 *       200:
 *         description: Autorii, în ordine alfabetică, cu numărul de cărți
 *         content:
 *           application/json:
 *             example: [{ id: 5, numeAutor: J.R.R. Tolkien, _count: { carti: 1 } }]
 */
router.get('/', lista);

/**
 * @openapi
 * /autori/{id}:
 *   get:
 *     summary: Detaliile unui autor și cărțile lui
 *     tags: [Catalog]
 *     parameters:
 *       - { in: path, name: id, required: true, schema: { type: integer } }
 *     responses:
 *       200: { description: Autorul și cărțile lui }
 *       400: { description: Id invalid }
 *       404: { description: Autorul nu există }
 */
router.get('/:id', detalii);

/**
 * @openapi
 * /autori:
 *   post:
 *     summary: Adaugă (RF-19, administrator)
 *     tags: [Administrare]
 *     security: [{ bearerAuth: [] }]
 *     requestBody: { required: true, content: { application/json: { example: { numeAutor: Mircea Eliade, biografie: Scriitor și istoric al religiilor. } } } }
 *     responses:
 *       201: { description: Adăugat }
 *       400: { description: Date invalide }
 *       403: { description: Utilizatorul nu este administrator }
 * /autori/{id}:
 *   put:
 *     summary: Modifică (RF-19, administrator)
 *     tags: [Administrare]
 *     security: [{ bearerAuth: [] }]
 *     parameters: [{ in: path, name: id, required: true, schema: { type: integer } }]
 *     requestBody: { required: true, content: { application/json: { example: { numeAutor: Mircea Eliade, biografie: Scriitor și istoric al religiilor. } } } }
 *     responses:
 *       200: { description: Modificat }
 *       404: { description: Nu există }
 *   delete:
 *     summary: Șterge, doar dacă nu are cărți (RF-19, administrator)
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
