// Autorii (RF-03). Echivalent v1.0: AutorService.cs + AutorRepository.cs
import { prisma } from '../lib/prisma.js';
import { AppError } from '../utils/AppError.js';

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
