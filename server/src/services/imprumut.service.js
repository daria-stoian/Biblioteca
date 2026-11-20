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

// Cauta imprumutul si verifica dreptul
async function gasesteImprumut(tx, id, utilizator) {
  const imprumut = await tx.imprumut.findUnique({ where: { id } });
  if (!imprumut) throw new AppError(404, 'Împrumutul nu a fost găsit.');
  if (imprumut.utilizatorId !== utilizator.id && utilizator.rol !== 'Administrator') {
    throw new AppError(403, 'Nu poți modifica împrumutul altui utilizator.');
  }
  return imprumut;
}

// Returnare cu taxa de 1 RON pentru fiecare zi de intarziere
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

// O singura prelungire, cu 14 zile, doar inainte de termen
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

// Imprumuturile proprii
export async function aleMele(utilizatorId) {
  const imprumuturi = await prisma.imprumut.findMany({
    where: { utilizatorId },
    orderBy: [{ dataReturnare: { sort: 'desc', nulls: 'first' } }, { dataScadenta: 'asc' }],
    include: cuCarte,
  });
  return imprumuturi.map((i) => descrieImprumut(i));
}

// Administrator: toate imprumuturile nereturnate
export async function toateDeschise() {
  const imprumuturi = await prisma.imprumut.findMany({
    where: { status: { in: STATUSURI_DESCHISE } },
    orderBy: { dataScadenta: 'asc' },
    include: { ...cuCarte, utilizator: { select: { id: true, numeComplet: true, email: true } } },
  });
  return imprumuturi.map((i) => descrieImprumut(i));
}
