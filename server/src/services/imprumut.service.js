// Imprumuturile (RF-09 – RF-12). Echivalent v1.0: ImprumutService.cs
import { prisma } from '../lib/prisma.js';
import { AppError } from '../utils/AppError.js';
import {
  ZILE_TERMEN,
  ZILE_PRELUNGIRE,
  MAX_IMPRUMUTURI_ACTIVE,
  adaugaZile,
  calculeazaPenalizare,
  descrieImprumut,
} from './imprumut.reguli.js';

const STATUSURI_DESCHISE = ['Activ', 'Prelungit'];

const cuCarte = {
  carte: {
    select: {
      id: true,
      titlu: true,
      imagineCoperta: true,
      autor: { select: { numeAutor: true } },
    },
  },
};

// Tranzactie "Serializable": PostgreSQL garanteaza ca doua cereri simultane nu pot trece
// amandoua de verificari si apoi sa depaseasca regulile (de exemplu, al 6-lea imprumut
// creat in aceeasi milisecunda cu al 5-lea). Daca apare un conflict, una dintre cereri esueaza.
async function inTranzactie(operatie) {
  try {
    return await prisma.$transaction(operatie, { isolationLevel: 'Serializable' });
  } catch (err) {
    if (err.code === 'P2034') {
      throw new AppError(409, 'Operația s-a suprapus cu alta. Încearcă din nou.');
    }
    throw err;
  }
}

// RF-09: aceleasi verificari si in aceeasi ordine ca in v1.0
export async function imprumuta(utilizatorId, carteId) {
  const imprumut = await inTranzactie(async (tx) => {
    const carte = await tx.carte.findUnique({ where: { id: carteId } });
    if (!carte) throw new AppError(404, 'Cartea nu a fost găsită.');
    if (carte.status === 'Indisponibil') {
      throw new AppError(409, 'Această carte nu este disponibilă momentan.');
    }

    const dejaImprumutata = await tx.imprumut.findFirst({
      where: { utilizatorId, carteId, status: { in: STATUSURI_DESCHISE } },
    });
    if (dejaImprumutata) throw new AppError(409, 'Ai deja această carte împrumutată.');

    const active = await tx.imprumut.count({
      where: { utilizatorId, status: { in: STATUSURI_DESCHISE } },
    });
    if (active >= MAX_IMPRUMUTURI_ACTIVE) {
      throw new AppError(
        409,
        `Ai atins limita de ${MAX_IMPRUMUTURI_ACTIVE} cărți împrumutate simultan. Returnează o carte înainte.`,
      );
    }

    const acum = new Date();
    return tx.imprumut.create({
      data: {
        utilizatorId,
        carteId,
        dataImprumut: acum,
        dataScadenta: adaugaZile(acum, ZILE_TERMEN),
        status: 'Activ',
      },
      include: cuCarte,
    });
  });

  return descrieImprumut(imprumut);
}

// Cauta imprumutul si verifica dreptul: cititorul doar pe ale lui, administratorul pe oricare.
async function gasesteImprumut(tx, id, utilizator) {
  const imprumut = await tx.imprumut.findUnique({ where: { id } });
  if (!imprumut) throw new AppError(404, 'Împrumutul nu a fost găsit.');
  if (imprumut.utilizatorId !== utilizator.id && utilizator.rol !== 'Administrator') {
    throw new AppError(403, 'Nu poți modifica împrumutul altui utilizator.');
  }
  return imprumut;
}

// RF-10: returnare cu taxa de 1 RON pentru fiecare zi (inceputa) de intarziere
export async function returneaza(id, utilizator) {
  const imprumut = await inTranzactie(async (tx) => {
    const existent = await gasesteImprumut(tx, id, utilizator);
    if (existent.status === 'Returnat') {
      throw new AppError(409, 'Această carte a fost deja returnată.');
    }

    const dataReturnare = new Date();
    return tx.imprumut.update({
      where: { id },
      data: {
        dataReturnare,
        status: 'Returnat',
        taxaPenalizare: calculeazaPenalizare(existent.dataScadenta, dataReturnare),
      },
      include: cuCarte,
    });
  });

  return descrieImprumut(imprumut);
}

// RF-11: o singura prelungire, cu 14 zile, doar inainte de termen
export async function prelungeste(id, utilizator) {
  const imprumut = await inTranzactie(async (tx) => {
    const existent = await gasesteImprumut(tx, id, utilizator);
    if (existent.status === 'Returnat') {
      throw new AppError(409, 'Nu poți prelungi un împrumut deja returnat.');
    }
    if (new Date() > existent.dataScadenta) {
      throw new AppError(409, 'Nu poți prelungi un împrumut cu termenul depășit. Contactează biblioteca.');
    }
    if (existent.status === 'Prelungit') {
      throw new AppError(409, 'Împrumutul a fost deja prelungit. Nu mai sunt permise prelungiri suplimentare.');
    }

    return tx.imprumut.update({
      where: { id },
      data: { dataScadenta: adaugaZile(existent.dataScadenta, ZILE_PRELUNGIRE), status: 'Prelungit' },
      include: cuCarte,
    });
  });

  return descrieImprumut(imprumut);
}

// RF-12: imprumuturile proprii, cele deschise primele
export async function aleMele(utilizatorId) {
  const imprumuturi = await prisma.imprumut.findMany({
    where: { utilizatorId },
    orderBy: [{ dataReturnare: { sort: 'desc', nulls: 'first' } }, { dataScadenta: 'asc' }],
    include: cuCarte,
  });
  return imprumuturi.map((i) => descrieImprumut(i));
}

// Pentru administrator: toate imprumuturile nereturnate, cu cititorul
export async function toateDeschise() {
  const imprumuturi = await prisma.imprumut.findMany({
    where: { status: { in: STATUSURI_DESCHISE } },
    orderBy: { dataScadenta: 'asc' },
    include: { ...cuCarte, utilizator: { select: { id: true, numeComplet: true, email: true } } },
  });
  return imprumuturi.map((i) => descrieImprumut(i));
}
