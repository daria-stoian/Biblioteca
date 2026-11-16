// Transforma un parametru de ruta (de exemplu /carti/:id) in numar intreg pozitiv.
// Pentru "abc", "-3" sau "1.5" raspunde cu 400, in loc sa ajunga o valoare gresita la baza de date.
import { AppError } from './AppError.js';

export function citesteId(valoare, entitate = 'resursei') {
  const id = Number(valoare);
  if (!Number.isInteger(id) || id < 1) throw new AppError(400, `Id-ul ${entitate} nu este valid.`);
  return id;
}
