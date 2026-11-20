// Catalogul de carti (RF-01, RF-02). Echivalent v1.0: CarteService.cs + CarteRepository.cs
import { prisma } from '../lib/prisma.js';
import { AppError } from '../utils/AppError.js';
import { readdir } from 'node:fs/promises';
import { raspunsPaginat } from '../utils/paginare.js';
import { text, intreg, dinLista } from '../utils/validare.js';

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

// ---------------------------------------------------------------- ADMINISTRARE (RF-18, RF-23)
// Echivalent v1.0: actiunile Adauga / Editeaza / Sterge / Marcheaza* din CarteController.cs

const DOSAR_COPERTI = new URL('../../public/coperti/', import.meta.url);

// Copertele existente pe server, din care administratorul alege una.
// (Incarcarea de imagini noi vine in varianta finala.)
export async function copertiDisponibile() {
  const fisiere = await readdir(DOSAR_COPERTI);
  return fisiere.filter((f) => /\.(png|jpe?g|webp)$/i.test(f)).sort().map((f) => `/coperti/${f}`);
}

// Verifica si curata datele din formular; arunca 400 cu un mesaj clar la prima problema
async function dateCarte(corp) {
  const date = {
    titlu: text(corp.titlu, 'Titlul', { obligatoriu: true, max: 200 }),
    isbn: text(corp.isbn, 'ISBN-ul', { max: 20 }),
    anPublicare: intreg(corp.anPublicare, 'Anul publicării', { min: 1000, max: new Date().getFullYear() + 1 }),
    numarPagini: intreg(corp.numarPagini, 'Numărul de pagini', { min: 1, max: 20000 }),
    descriere: text(corp.descriere, 'Descrierea', { max: 2000 }),
    imagineCoperta: text(corp.imagineCoperta, 'Coperta', { max: 300 }),
    status: dinLista(corp.status ?? 'Disponibil', 'Statusul', ['Disponibil', 'Indisponibil']),
    autorId: intreg(corp.autorId, 'Autorul'),
    categorieId: intreg(corp.categorieId, 'Categoria'),
  };

  if (date.imagineCoperta && !(await copertiDisponibile()).includes(date.imagineCoperta)) {
    throw new AppError(400, 'Coperta aleasă nu există pe server.');
  }
  if (!(await prisma.autor.findUnique({ where: { id: date.autorId } }))) {
    throw new AppError(400, 'Autorul ales nu există.');
  }
  if (!(await prisma.categorie.findUnique({ where: { id: date.categorieId } }))) {
    throw new AppError(400, 'Categoria aleasă nu există.');
  }
  return date;
}

async function verificaIsbnUnic(isbn, idExclus) {
  if (!isbn) return;
  const existenta = await prisma.carte.findUnique({ where: { isbn } });
  if (existenta && existenta.id !== idExclus) {
    throw new AppError(409, `ISBN-ul ${isbn} aparține deja cărții „${existenta.titlu}”.`);
  }
}

export async function adaugaCarte(corp) {
  const date = await dateCarte(corp);
  await verificaIsbnUnic(date.isbn);
  return prisma.carte.create({ data: date, select: campuriLista });
}

export async function modificaCarte(id, corp) {
  if (!(await prisma.carte.findUnique({ where: { id } }))) throw new AppError(404, 'Cartea nu a fost găsită.');
  const date = await dateCarte(corp);
  await verificaIsbnUnic(date.isbn, id);
  return prisma.carte.update({ where: { id }, data: date, select: campuriLista });
}

// RF-23: marcarea rapida ca disponibila / indisponibila, fara formularul complet
export async function schimbaStatus(id, status) {
  dinLista(status, 'Statusul', ['Disponibil', 'Indisponibil']);
  if (!(await prisma.carte.findUnique({ where: { id } }))) throw new AppError(404, 'Cartea nu a fost găsită.');
  return prisma.carte.update({ where: { id }, data: { status }, select: campuriLista });
}

// O carte cu istoric (imprumuturi sau recenzii) nu se sterge: istoricul trebuie pastrat (RN-02).
export async function stergeCarte(id) {
  const carte = await prisma.carte.findUnique({
    where: { id },
    select: { titlu: true, _count: { select: { imprumuturi: true, recenzii: true } } },
  });
  if (!carte) throw new AppError(404, 'Cartea nu a fost găsită.');
  if (carte._count.imprumuturi > 0 || carte._count.recenzii > 0) {
    throw new AppError(
      409,
      `„${carte.titlu}” are împrumuturi sau recenzii și nu poate fi ștearsă. O poți marca drept indisponibilă.`,
    );
  }
  await prisma.carte.delete({ where: { id } });
}
