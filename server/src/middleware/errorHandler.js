// Middleware-ul de erori: Express il recunoaste dupa cei 4 parametri (err, req, res, next).
// In Express 5, o eroare aruncata intr-un controller async ajunge automat aici,
// deci controllerele nu au nevoie de try/catch.
import { AppError } from '../utils/AppError.js';
import { config } from '../config/env.js';

// eslint-disable-next-line no-unused-vars
export function errorHandler(err, req, res, next) {
  // Erori asteptate (reguli de business, validare)
  if (err instanceof AppError) {
    return res.status(err.status).json({ eroare: err.message });
  }

  // JSON invalid in corpul cererii
  if (err.type === 'entity.parse.failed') {
    return res.status(400).json({ eroare: 'Corpul cererii nu este un JSON valid.' });
  }

  // Erori neasteptate: se scriu in consola, utilizatorul primeste un mesaj generic
  console.error(err);
  res.status(500).json({
    eroare: 'A apărut o eroare internă.',
    ...(config.nodeEnv === 'development' && { detalii: err.message }),
  });
}
