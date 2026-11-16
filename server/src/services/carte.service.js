// Catalogul de carti (RF-01, RF-02). Echivalent v1.0: CarteService.cs + CarteRepository.cs
import { prisma } from '../lib/prisma.js';
import { AppError } from '../utils/AppError.js';
import { raspunsPaginat } from '../utils/paginare.js';

// Ce se afiseaza pe un "card" din catalog
const campuriLista = {
  id: true,
  titlu: true,
  anPublicare: true,
  imagineCoperta: true,
  status: true,
  autor: { select: { id: true, numeAutor: true } },
  categorie: { select: { id: true, numeCategorie: true } },
};

// RF-01: cautare dupa titlu (fara diferenta intre litere mari/mici) si filtrare dupa categorie
export async function listaCarti({ cautare, categorieId, paginare }) {
  const where = {};
  if (cautare?.trim()) where.titlu = { contains: cautare.trim(), mode: 'insensitive' };
  if (categorieId) where.categorieId = categorieId;

  // Cele doua interogari pornesc in paralel: pagina curenta si numarul total de rezultate
  const [carti, total] = await Promise.all([
    prisma.carte.findMany({
      where,
      orderBy: { titlu: 'asc' },
      skip: paginare.skip,
      take: paginare.take,
      select: campuriLista,
    }),
    prisma.carte.count({ where }),
  ]);

  return raspunsPaginat(carti, total, paginare);
}

// RF-02: autor, categorie, descriere, coperta, disponibilitate si recenzii
export async function detaliiCarte(id) {
  const carte = await prisma.carte.findUnique({
    where: { id },
    include: {
      autor: { select: { id: true, numeAutor: true, biografie: true } },
      categorie: { select: { id: true, numeCategorie: true } },
      recenzii: {
        orderBy: { dataPostare: 'desc' },
        select: {
          id: true,
          nota: true,
          comentariu: true,
          dataPostare: true,
          utilizator: { select: { id: true, numeComplet: true } },
        },
      },
    },
  });

  if (!carte) throw new AppError(404, 'Cartea nu a fost găsită.');

  // Fisierul PDF nu se trimite: descarcarea e permisa doar cu imprumut activ (RF-13).
  const { fisierPdf, ...rest } = carte;
  const note = carte.recenzii.map((r) => r.nota);

  return {
    ...rest,
    arePdf: Boolean(fisierPdf),
    disponibila: carte.status === 'Disponibil',
    notaMedie: note.length ? Math.round((note.reduce((a, b) => a + b, 0) / note.length) * 10) / 10 : null,
    numarRecenzii: note.length,
  };
}
