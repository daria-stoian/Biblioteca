// Rute publice: catalogul se poate consulta fara cont (Vizitator, RF-01, RF-02).
import { Router } from 'express';
import { lista, detalii } from '../controllers/carte.controller.js';

const router = Router();

/**
 * @openapi
 * /carti:
 *   get:
 *     summary: Catalogul de cărți, cu căutare, filtrare și paginare (RF-01, RN-03)
 *     tags: [Catalog]
 *     parameters:
 *       - { in: query, name: cautare, schema: { type: string }, description: Parte din titlu, example: harry }
 *       - { in: query, name: categorieId, schema: { type: integer }, description: Filtrare după categorie }
 *       - { in: query, name: pagina, schema: { type: integer, default: 1 } }
 *       - { in: query, name: limita, schema: { type: integer, default: 12, maximum: 50 } }
 *     responses:
 *       200:
 *         description: O pagină din catalog
 *         content:
 *           application/json:
 *             example:
 *               elemente:
 *                 - id: 1
 *                   titlu: Dune
 *                   anPublicare: 1965
 *                   imagineCoperta: /coperti/dune.png
 *                   status: Disponibil
 *                   autor: { id: 7, numeAutor: Frank Herbert }
 *                   categorie: { id: 6, numeCategorie: Științifico-fantastic }
 *               total: 17
 *               pagina: 1
 *               limita: 12
 *               totalPagini: 2
 *       400: { description: categorieId invalid }
 */
router.get('/', lista);

/**
 * @openapi
 * /carti/{id}:
 *   get:
 *     summary: Detaliile unei cărți, cu recenzii și disponibilitate (RF-02)
 *     tags: [Catalog]
 *     parameters:
 *       - { in: path, name: id, required: true, schema: { type: integer } }
 *     responses:
 *       200: { description: 'Cartea, cu autor, categorie, recenzii, notaMedie, disponibila și arePdf' }
 *       400: { description: Id invalid }
 *       404: { description: Cartea nu există }
 */
router.get('/:id', detalii);

export default router;
