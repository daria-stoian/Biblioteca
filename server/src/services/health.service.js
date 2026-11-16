// Stratul de servicii: logica, fara nimic legat de HTTP (req/res).
import { prisma } from '../lib/prisma.js';

export async function verificaStare() {
  let bazaDeDate = 'conectata';
  try {
    await prisma.$queryRaw`SELECT 1`;
  } catch {
    bazaDeDate = 'indisponibila';
  }

  return {
    status: bazaDeDate === 'conectata' ? 'ok' : 'degradat',
    bazaDeDate,
    ora: new Date().toISOString(),
  };
}
