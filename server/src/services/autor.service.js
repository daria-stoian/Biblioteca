import { prisma } from '../lib/prisma.js';
import { AppError } from '../utils/AppError.js';
import { text } from '../utils/validare.js';

export async function listaAutori({ cautare } = {}) {
  const where = cautare?.trim()
    ? { numeAutor: { contains: cautare.trim(), mode: 'insensitive' } }
    : {};

  return prisma.autor.findMany({
    where,
    orderBy: { numeAutor: 'asc' },
    select: {
      id: true,
      numeAutor: true,
      _count: { select: { carti: true } }, // cate carti are fiecare autor
    },
  });
}

export async function detaliiAutor(id) {
  const autor = await prisma.autor.findUnique({
    where: { id },
    include: {
      carti: {
        orderBy: { titlu: 'asc' },
        select: { id: true, titlu: true, anPublicare: true, imagineCoperta: true, status: true },
      },
    },
  });
  if (!autor) throw new AppError(404, 'Autorul nu a fost găsit.');
  return autor;
}

// ADMINISTRARE
function dateAutor(corp) {
  return {
    numeAutor: text(corp.numeAutor, 'Numele autorului', { obligatoriu: true, max: 150 }),
    biografie: text(corp.biografie, 'Biografia', { max: 2000 }),
  };
}

export async function adaugaAutor(corp) {
  return prisma.autor.create({ data: dateAutor(corp) });
}

export async function modificaAutor(id, corp) {
  if (!(await prisma.autor.findUnique({ where: { id } }))) throw new AppError(404, 'Autorul nu a fost găsit.');
  return prisma.autor.update({ where: { id }, data: dateAutor(corp) });
}

// Un autor cu carti nu poate fi sters
export async function stergeAutor(id) {
  const autor = await prisma.autor.findUnique({ where: { id }, include: { _count: { select: { carti: true } } } });
  if (!autor) throw new AppError(404, 'Autorul nu a fost găsit.');
  if (autor._count.carti > 0) {
    throw new AppError(409, `${autor.numeAutor} are cărți în catalog (${autor._count.carti}). Șterge sau mută întâi cărțile.`);
  }
  await prisma.autor.delete({ where: { id } });
}
