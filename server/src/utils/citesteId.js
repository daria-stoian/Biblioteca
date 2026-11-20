import { AppError } from './AppError.js';

export function citesteId(valoare, entitate = 'resursei') {
  const id = Number(valoare);
  if (!Number.isInteger(id) || id < 1) throw new AppError(400, `Id-ul ${entitate} nu este valid.`);
  return id;
}
