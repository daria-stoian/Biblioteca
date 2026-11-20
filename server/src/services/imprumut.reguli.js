export const ZILE_TERMEN = 14;
export const ZILE_PRELUNGIRE = 14;
export const MAX_IMPRUMUTURI_ACTIVE = 5;
export const TAXA_PE_ZI = 1.0; // RON

const MS_PE_ZI = 24 * 60 * 60 * 1000;

export function adaugaZile(data, zile) {
  return new Date(data.getTime() + zile * MS_PE_ZI);
}

export function zileIntarziere(dataScadenta, dataReferinta) {
  const diferenta = dataReferinta.getTime() - dataScadenta.getTime();
  return diferenta > 0 ? Math.ceil(diferenta / MS_PE_ZI) : 0;
}

// Zilele ramase pana la termen, 0 daca termenul e azi sau a trecut
export function zileRamase(dataScadenta, acum) {
  const diferenta = dataScadenta.getTime() - acum.getTime();
  return diferenta > 0 ? Math.ceil(diferenta / MS_PE_ZI) : 0;
}

export function calculeazaPenalizare(dataScadenta, dataReturnare) {
  return zileIntarziere(dataScadenta, dataReturnare) * TAXA_PE_ZI;
}

export function statusEfectiv(imprumut, acum = new Date()) {
  if (imprumut.status === 'Returnat') return 'Returnat';
  if (acum > imprumut.dataScadenta) return 'Intarziat';
  return imprumut.status; // Activ sau Prelungit
}

// Informatii utile pentru afisare, adaugate la fiecare imprumut trimis clientului
export function descrieImprumut(imprumut, acum = new Date()) {
  const status = statusEfectiv(imprumut, acum);
  const deschis = status !== 'Returnat';
  return {
    ...imprumut,
    taxaPenalizare: Number(imprumut.taxaPenalizare),
    status,
    zileIntarziere: deschis ? zileIntarziere(imprumut.dataScadenta, acum) : 0,
    zileRamase: deschis ? zileRamase(imprumut.dataScadenta, acum) : 0,
    taxaEstimata: deschis ? calculeazaPenalizare(imprumut.dataScadenta, acum) : Number(imprumut.taxaPenalizare),
    poatePrelungi: imprumut.status === 'Activ' && acum <= imprumut.dataScadenta,
  };
}
