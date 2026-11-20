import { AppError } from './AppError.js';

export function text(valoare, eticheta, { obligatoriu = false, max = 255 } = {}) {
  const curat = typeof valoare === 'string' ? valoare.trim() : '';
  if (!curat) {
    if (obligatoriu) throw new AppError(400, `${eticheta} este obligatoriu.`);
    return null;
  }
  if (curat.length > max) throw new AppError(400, `${eticheta} poate avea cel mult ${max} caractere.`);
  return curat;
}

export function intreg(valoare, eticheta, { min = 1, max = Number.MAX_SAFE_INTEGER } = {}) {
  const numar = Number(valoare);
  if (valoare === '' || valoare === null || valoare === undefined || !Number.isInteger(numar)) {
    throw new AppError(400, `${eticheta} trebuie să fie un număr întreg.`);
  }
  if (numar < min || numar > max) throw new AppError(400, `${eticheta} trebuie să fie între ${min} și ${max}.`);
  return numar;
}

export function dinLista(valoare, eticheta, permise) {
  if (!permise.includes(valoare)) throw new AppError(400, `${eticheta} trebuie să fie una dintre: ${permise.join(', ')}.`);
  return valoare;
}
