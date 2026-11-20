import bcrypt from 'bcryptjs';
import jwt from 'jsonwebtoken';
import { prisma } from '../lib/prisma.js';
import { config } from '../config/env.js';
import { AppError } from '../utils/AppError.js';

const RUNDE_BCRYPT = 10;
const LUNGIME_MINIMA_PAROLA = 6;
const EMAIL_REGEX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

const campuriPublice = {
  id: true,
  numeComplet: true,
  email: true,
  rol: true,
  telefon: true,
  adresa: true,
  pozaProfil: true,
};

function genereazaToken(utilizator) {
  return jwt.sign({ rol: utilizator.rol }, config.jwt.secret, {
    subject: String(utilizator.id),
    expiresIn: config.jwt.expiresIn,
  });
}

export async function inregistrare({ numeComplet, email, parola }) {
  numeComplet = numeComplet?.trim();
  email = email?.trim().toLowerCase();

  if (!numeComplet) throw new AppError(400, 'Numele complet este obligatoriu.');
  if (!email || !EMAIL_REGEX.test(email)) throw new AppError(400, 'Adresa de email nu este validă.');
  if (!parola || parola.length < LUNGIME_MINIMA_PAROLA) {
    throw new AppError(400, `Parola trebuie să aibă cel puțin ${LUNGIME_MINIMA_PAROLA} caractere.`);
  }

  const existent = await prisma.utilizator.findUnique({ where: { email } });
  if (existent) throw new AppError(409, 'Există deja un cont cu această adresă de email.');

  const utilizator = await prisma.utilizator.create({
    data: {
      numeComplet,
      email,
      parolaHash: await bcrypt.hash(parola, RUNDE_BCRYPT),
      rol: 'Cititor',
    },
    select: campuriPublice,
  });

  return { token: genereazaToken(utilizator), utilizator };
}

export async function autentificare({ email, parola }) {
  email = email?.trim().toLowerCase();
  if (!email || !parola) throw new AppError(400, 'Emailul și parola sunt obligatorii.');

  const utilizator = await prisma.utilizator.findUnique({ where: { email } });

  // Mesaj pentru email inexistent si parola gresita
  const parolaCorecta = utilizator && (await bcrypt.compare(parola, utilizator.parolaHash));
  if (!parolaCorecta) throw new AppError(401, 'Email sau parolă incorectă.');

  const { parolaHash, ...faraParola } = utilizator;
  return { token: genereazaToken(utilizator), utilizator: faraParola };
}

export async function utilizatorCurent(id) {
  const utilizator = await prisma.utilizator.findUnique({ where: { id }, select: campuriPublice });
  if (!utilizator) throw new AppError(404, 'Utilizatorul nu mai există.');
  return utilizator;
}
