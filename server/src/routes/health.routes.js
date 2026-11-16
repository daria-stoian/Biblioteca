// Stratul de rute: leaga o adresa (metoda + cale) de un controller.
import { Router } from 'express';
import { getHealth } from '../controllers/health.controller.js';

const router = Router();

/**
 * @openapi
 * /health:
 *   get:
 *     summary: Verifică starea API-ului și conexiunea la baza de date
 *     tags: [Sistem]
 *     responses:
 *       200:
 *         description: API-ul și baza de date funcționează
 *         content:
 *           application/json:
 *             example: { status: ok, bazaDeDate: conectata, ora: '2026-11-16T10:00:00.000Z' }
 *       503:
 *         description: API-ul rulează, dar baza de date nu răspunde
 */
router.get('/', getHealth);

export default router;
