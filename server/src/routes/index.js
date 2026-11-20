import { Router } from 'express';
import healthRoutes from './health.routes.js';
import authRoutes from './auth.routes.js';
import carteRoutes from './carte.routes.js';
import autorRoutes from './autor.routes.js';
import categorieRoutes from './categorie.routes.js';
import imprumutRoutes from './imprumut.routes.js';

const router = Router();

router.use('/health', healthRoutes);
router.use('/auth', authRoutes);
router.use('/carti', carteRoutes);
router.use('/autori', autorRoutes);
router.use('/categorii', categorieRoutes);
router.use('/imprumuturi', imprumutRoutes);

export default router;
