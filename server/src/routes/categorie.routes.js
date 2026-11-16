// Rute publice: nu cer autentificare (Vizitator, RF-03).
import { Router } from 'express';
import { lista } from '../controllers/categorie.controller.js';

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

export default router;
