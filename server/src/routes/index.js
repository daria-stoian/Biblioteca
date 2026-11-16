// Punctul unic in care se monteaza toate grupurile de rute sub /api.
// La fiecare pas nou (auth, carti, imprumuturi) se adauga aici o singura linie.
import { Router } from 'express';
import healthRoutes from './health.routes.js';
import authRoutes from './auth.routes.js';

const router = Router();

router.use('/health', healthRoutes);
router.use('/auth', authRoutes);

export default router;
