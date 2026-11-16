import { Router } from 'express';
import { register, login, me } from '../controllers/auth.controller.js';
import { autentificare } from '../middleware/auth.js';

const router = Router();

/**
 * @openapi
 * components:
 *   securitySchemes:
 *     bearerAuth:
 *       type: http
 *       scheme: bearer
 *       bearerFormat: JWT
 *   schemas:
 *     Utilizator:
 *       type: object
 *       properties:
 *         id: { type: integer, example: 2 }
 *         numeComplet: { type: string, example: Ana Popescu }
 *         email: { type: string, example: ana@exemplu.ro }
 *         rol: { type: string, enum: [Administrator, Cititor] }
 *         telefon: { type: string, nullable: true }
 *         adresa: { type: string, nullable: true }
 *         pozaProfil: { type: string, nullable: true }
 *     RaspunsAutentificare:
 *       type: object
 *       properties:
 *         token: { type: string, description: 'Se trimite apoi în antetul Authorization: Bearer <token>' }
 *         utilizator: { $ref: '#/components/schemas/Utilizator' }
 *     Eroare:
 *       type: object
 *       properties:
 *         eroare: { type: string }
 */

/**
 * @openapi
 * /auth/register:
 *   post:
 *     summary: Înregistrarea unui cont nou de cititor (RF-06)
 *     tags: [Autentificare]
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           example: { numeComplet: Ana Popescu, email: ana@exemplu.ro, parola: parola123 }
 *     responses:
 *       201:
 *         description: Cont creat; utilizatorul este deja autentificat
 *         content:
 *           application/json:
 *             schema: { $ref: '#/components/schemas/RaspunsAutentificare' }
 *       400: { description: Date invalide, content: { application/json: { schema: { $ref: '#/components/schemas/Eroare' } } } }
 *       409: { description: Emailul este deja folosit, content: { application/json: { schema: { $ref: '#/components/schemas/Eroare' } } } }
 */
router.post('/register', register);

/**
 * @openapi
 * /auth/login:
 *   post:
 *     summary: Autentificare cu email și parolă (RF-07)
 *     tags: [Autentificare]
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           example: { email: ana@exemplu.ro, parola: parola123 }
 *     responses:
 *       200:
 *         description: Autentificare reușită
 *         content:
 *           application/json:
 *             schema: { $ref: '#/components/schemas/RaspunsAutentificare' }
 *       401: { description: Email sau parolă incorectă, content: { application/json: { schema: { $ref: '#/components/schemas/Eroare' } } } }
 */
router.post('/login', login);

/**
 * @openapi
 * /auth/me:
 *   get:
 *     summary: Datele utilizatorului autentificat
 *     tags: [Autentificare]
 *     security: [{ bearerAuth: [] }]
 *     responses:
 *       200:
 *         description: Utilizatorul curent
 *         content:
 *           application/json:
 *             schema: { $ref: '#/components/schemas/Utilizator' }
 *       401: { description: Lipsește token-ul sau este invalid, content: { application/json: { schema: { $ref: '#/components/schemas/Eroare' } } } }
 */
router.get('/me', autentificare, me);

export default router;
