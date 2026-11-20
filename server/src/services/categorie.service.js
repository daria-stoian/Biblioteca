import { prisma } from '../lib/prisma.js';
import { AppError } from '../utils/AppError.js';
import { text } from '../utils/validare.js';

export async function listaCategorii() {
  return prisma.categorie.findMany({
    orderBy: { numeCategorie: 'asc' },
    select: {
      id: true,
      numeCategorie: true,
      _count: { select: { carti: true } },
    },
  });
}

// ADMINISTRARE
async function numeUnic(corp, idExclus) {
  const numeCategorie = text(corp.numeCategorie, 'Numele categoriei', { obligatoriu: true, max: 100 });
  const existenta = await prisma.categorie.findUnique({ where: { numeCategorie } });
  if (existenta && existenta.id !== idExclus) throw new AppError(409, `Categoria „${numeCategorie}” există deja.`);
  return numeCategorie;
}

export async function adaugaCategorie(corp) {
  return prisma.categorie.create({ data: { numeCategorie: await numeUnic(corp) } });
}

export async function modificaCategorie(id, corp) {
  if (!(await prisma.categorie.findUnique({ where: { id } }))) throw new AppError(404, 'Categoria nu a fost găsită.');
  return prisma.categorie.update({ where: { id }, data: { numeCategorie: await numeUnic(corp, id) } });
}

export async function stergeCategorie(id) {
  const categorie = await prisma.categorie.findUnique({
    where: { id },
    include: { _count: { select: { carti: true } } },
  });
  if (!categorie) throw new AppError(404, 'Categoria nu a fost găsită.');
  if (categorie._count.carti > 0) {
        throw new AppError(409, `Categoria „${categorie.numeCategorie}” conține cărți (${categorie._count.carti}) și nu poate fi ștearsă.`);
  }
  await prisma.categorie.delete({ where: { id } });
}
