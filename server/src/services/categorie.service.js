// Categoriile (RF-03). Echivalent v1.0: CategorieService.cs
import { prisma } from '../lib/prisma.js';

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
