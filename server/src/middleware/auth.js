// Protejarea rutelor cu JWT
import jwt from 'jsonwebtoken';
import { config } from '../config/env.js';
import { AppError } from '../utils/AppError.js';

export function autentificare(req, res, next) {
  const antet = req.headers.authorization || '';
  const [tip, token] = antet.split(' ');

  if (tip !== 'Bearer' || !token) {
    throw new AppError(401, 'Trebuie să fii autentificat.');
  }

  try {
    const date = jwt.verify(token, config.jwt.secret);
    req.utilizator = { id: Number(date.sub), rol: date.rol };
    next();
  } catch {
    throw new AppError(401, 'Sesiunea a expirat sau token-ul este invalid. Autentifică-te din nou.');
  }
}

export function autorizare(...roluriPermise) {
  return (req, res, next) => {
    if (!req.utilizator || !roluriPermise.includes(req.utilizator.rol)) {
      throw new AppError(403, 'Nu ai drepturi pentru această operație.');
    }
    next();
  };
}

// Scurtatura pentru rutele de administrare
export const doarAdministrator = [autentificare, autorizare('Administrator')];
