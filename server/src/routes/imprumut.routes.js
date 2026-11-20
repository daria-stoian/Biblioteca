import { Router } from 'express';
import { autentificare, autorizare } from '../middleware/auth.js';
import { imprumuta, aleMele, toate, returneaza, prelungeste } from '../controllers/imprumut.controller.js';

const router = Router();

router.use(autentificare);

/**
 * @openapi
 * components:
 *   schemas:
 *     Imprumut:
 *       type: object
 *       properties:
 *         id: { type: integer }
 *         dataImprumut: { type: string, format: date-time }
 *         dataScadenta: { type: string, format: date-time }
 *         dataReturnare: { type: string, format: date-time, nullable: true }
 *         status: { type: string, enum: [Activ, Prelungit, Intarziat, Returnat] }
 *         taxaPenalizare: { type: number, description: Taxa plătită la returnare (RON) }
 *         zileIntarziere: { type: integer }
 *         zileRamase: { type: integer, description: Zile până la termen (0 dacă a trecut) }
 *         taxaEstimata: { type: number, description: Taxa dacă s-ar returna acum (RON) }
 *         poatePrelungi: { type: boolean }
 *         carte:
 *           type: object
 *           properties:
 *             id: { type: integer }
 *             titlu: { type: string }
 *             imagineCoperta: { type: string }
 *             autor: { type: object, properties: { numeAutor: { type: string } } }
 */

/**
 * @openapi
 * /imprumuturi:
 *   post:
 *     summary: Împrumută o carte pentru 14 zile (RF-09)
 *     description: Refuzat dacă cartea e indisponibilă, dacă utilizatorul o are deja sau dacă are deja 5 împrumuturi active.
 *     tags: [Împrumuturi]
 *     security: [{ bearerAuth: [] }]
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           example: { carteId: 1 }
 *     responses:
 *       201: { description: Împrumut creat, content: { application/json: { schema: { $ref: '#/components/schemas/Imprumut' } } } }
 *       400: { description: carteId lipsă sau invalid }
 *       401: { description: Neautentificat }
 *       404: { description: Cartea nu există }
 *       409: { description: 'Regulă încălcată: indisponibilă, deja împrumutată sau limita de 5' }
 *   get:
 *     summary: Toate împrumuturile nereturnate (doar administrator)
 *     tags: [Împrumuturi]
 *     security: [{ bearerAuth: [] }]
 *     responses:
 *       200: { description: Lista împrumuturilor deschise, cu cititorul }
 *       403: { description: Utilizatorul nu este administrator }
 */
router.post('/', imprumuta);
router.get('/', autorizare('Administrator'), toate);

/**
 * @openapi
 * /imprumuturi/ale-mele:
 *   get:
 *     summary: Împrumuturile utilizatorului autentificat (RF-12)
 *     tags: [Împrumuturi]
 *     security: [{ bearerAuth: [] }]
 *     responses:
 *       200:
 *         description: Împrumuturile proprii; cele nereturnate primele
 *         content:
 *           application/json:
 *             schema: { type: array, items: { $ref: '#/components/schemas/Imprumut' } }
 */
router.get('/ale-mele', aleMele);

/**
 * @openapi
 * /imprumuturi/{id}/returnare:
 *   post:
 *     summary: Returnează o carte; calculează taxa de 1 RON / zi de întârziere (RF-10)
 *     tags: [Împrumuturi]
 *     security: [{ bearerAuth: [] }]
 *     parameters:
 *       - { in: path, name: id, required: true, schema: { type: integer } }
 *     responses:
 *       200: { description: Carte returnată, content: { application/json: { schema: { $ref: '#/components/schemas/Imprumut' } } } }
 *       403: { description: Împrumutul aparține altui utilizator }
 *       404: { description: Împrumutul nu există }
 *       409: { description: Deja returnat }
 */
router.post('/:id/returnare', returneaza);

/**
 * @openapi
 * /imprumuturi/{id}/prelungire:
 *   post:
 *     summary: Prelungește termenul cu 14 zile, o singură dată, înainte de scadență (RF-11)
 *     tags: [Împrumuturi]
 *     security: [{ bearerAuth: [] }]
 *     parameters:
 *       - { in: path, name: id, required: true, schema: { type: integer } }
 *     responses:
 *       200: { description: Termen prelungit, content: { application/json: { schema: { $ref: '#/components/schemas/Imprumut' } } } }
 *       403: { description: Împrumutul aparține altui utilizator }
 *       404: { description: Împrumutul nu există }
 *       409: { description: 'Deja returnat, termen depășit sau deja prelungit' }
 */
router.post('/:id/prelungire', prelungeste);

export default router;
