// Citirea catalogului este publica (Vizitator, RF-01, RF-02);
// adaugarea, modificarea si stergerea sunt doar pentru administrator (RF-18, RF-23).
import { Router } from 'express';
import { lista, detalii, coperti, adauga, modifica, schimbaStatus, sterge } from '../controllers/carte.controller.js';
import { doarAdministrator } from '../middleware/auth.js';

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
 * /carti/coperti:
 *   get:
 *     summary: Copertele disponibile pe server, pentru formularul de carte (administrator)
 *     tags: [Administrare]
 *     security: [{ bearerAuth: [] }]
 *     responses:
 *       200: { description: 'Lista căilor, de exemplu ["/coperti/dune.png"]' }
 *       403: { description: Utilizatorul nu este administrator }
 */
router.get('/coperti', doarAdministrator, coperti); // inainte de /:id, altfel "coperti" ar fi luat drept id

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

/**
 * @openapi
 * components:
 *   schemas:
 *     CarteFormular:
 *       type: object
 *       required: [titlu, anPublicare, numarPagini, autorId, categorieId]
 *       properties:
 *         titlu: { type: string, maxLength: 200 }
 *         isbn: { type: string, maxLength: 20, nullable: true }
 *         anPublicare: { type: integer }
 *         numarPagini: { type: integer }
 *         descriere: { type: string, maxLength: 2000, nullable: true }
 *         imagineCoperta: { type: string, nullable: true, description: 'Una dintre valorile din GET /carti/coperti' }
 *         status: { type: string, enum: [Disponibil, Indisponibil] }
 *         autorId: { type: integer }
 *         categorieId: { type: integer }
 *       example: { titlu: Maitreyi, anPublicare: 1933, numarPagini: 208, autorId: 1, categorieId: 6, status: Disponibil }
 * /carti:
 *   post:
 *     summary: Adaugă o carte (RF-18, administrator)
 *     tags: [Administrare]
 *     security: [{ bearerAuth: [] }]
 *     requestBody: { required: true, content: { application/json: { schema: { $ref: '#/components/schemas/CarteFormular' } } } }
 *     responses:
 *       201: { description: Carte adăugată }
 *       400: { description: Date invalide }
 *       403: { description: Utilizatorul nu este administrator }
 *       409: { description: ISBN deja folosit }
 * /carti/{id}:
 *   put:
 *     summary: Modifică o carte (RF-18, administrator)
 *     tags: [Administrare]
 *     security: [{ bearerAuth: [] }]
 *     parameters: [{ in: path, name: id, required: true, schema: { type: integer } }]
 *     requestBody: { required: true, content: { application/json: { schema: { $ref: '#/components/schemas/CarteFormular' } } } }
 *     responses:
 *       200: { description: Carte modificată }
 *       400: { description: Date invalide }
 *       404: { description: Cartea nu există }
 *   delete:
 *     summary: Șterge o carte fără istoric (RF-18, administrator)
 *     tags: [Administrare]
 *     security: [{ bearerAuth: [] }]
 *     parameters: [{ in: path, name: id, required: true, schema: { type: integer } }]
 *     responses:
 *       204: { description: Carte ștearsă }
 *       409: { description: Cartea are împrumuturi sau recenzii }
 * /carti/{id}/status:
 *   patch:
 *     summary: Marchează o carte ca disponibilă sau indisponibilă (RF-23, administrator)
 *     tags: [Administrare]
 *     security: [{ bearerAuth: [] }]
 *     parameters: [{ in: path, name: id, required: true, schema: { type: integer } }]
 *     requestBody: { required: true, content: { application/json: { example: { status: Indisponibil } } } }
 *     responses:
 *       200: { description: Status schimbat }
 */
router.post('/', doarAdministrator, adauga);
router.put('/:id', doarAdministrator, modifica);
router.patch('/:id/status', doarAdministrator, schimbaStatus);
router.delete('/:id', doarAdministrator, sterge);

export default router;
