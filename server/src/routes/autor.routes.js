// Rute publice: nu cer autentificare (Vizitator, RF-03).
import { Router } from 'express';
import { lista, detalii } from '../controllers/autor.controller.js';

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

export default router;
