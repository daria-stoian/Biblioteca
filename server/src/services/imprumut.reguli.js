// Regulile de imprumut (RF-09 – RF-12), ca functii simple, fara baza de date.
// Fiind "pure", se pot testa usor si se pot compara direct cu ImprumutService.cs din v1.0.

export const ZILE_TERMEN = 14;
export const ZILE_PRELUNGIRE = 14;
export const MAX_IMPRUMUTURI_ACTIVE = 5;
export const TAXA_PE_ZI = 1.0; // RON

const MS_PE_ZI = 24 * 60 * 60 * 1000;

export function adaugaZile(data, zile) {
  return new Date(data.getTime() + zile * MS_PE_ZI);
}

// Diferenta fata de v1.0: acolo zilele se rotunjeau in jos ((int)TotalDays),
// deci o intarziere de 23 de ore era gratuita. Aici orice zi inceputa se plateste.
export function zileIntarziere(dataScadenta, dataReferinta) {
  const diferenta = dataReferinta.getTime() - dataScadenta.getTime();
  return diferenta > 0 ? Math.ceil(diferenta / MS_PE_ZI) : 0;
}

export function calculeazaPenalizare(dataScadenta, dataReturnare) {
  return zileIntarziere(dataScadenta, dataReturnare) * TAXA_PE_ZI;
}

// In v1.0, statusul "Intarziat" exista in enum, dar nu era setat nicaieri:
// un imprumut depasit ramanea "Activ". Aici statusul se calculeaza la fiecare citire,
// deci este mereu corect, fara un proces care sa actualizeze baza de date periodic.
export function statusEfectiv(imprumut, acum = new Date()) {
  if (imprumut.status === 'Returnat') return 'Returnat';
  if (acum > imprumut.dataScadenta) return 'Intarziat';
  return imprumut.status; // Activ sau Prelungit
}

// Informatii utile pentru afisare, adaugate la fiecare imprumut trimis clientului.
export function descrieImprumut(imprumut, acum = new Date()) {
  const status = statusEfectiv(imprumut, acum);
  const deschis = status !== 'Returnat';
  return {
    ...imprumut,
    taxaPenalizare: Number(imprumut.taxaPenalizare), // Decimal din Prisma -> numar
    status,
    zileIntarziere: deschis ? zileIntarziere(imprumut.dataScadenta, acum) : 0,
    taxaEstimata: deschis ? calculeazaPenalizare(imprumut.dataScadenta, acum) : Number(imprumut.taxaPenalizare),
    poatePrelungi: imprumut.status === 'Activ' && acum <= imprumut.dataScadenta,
  };
}
