import * as authService from '../services/auth.service.js';

export async function register(req, res) {
  const rezultat = await authService.inregistrare(req.body ?? {});
  res.status(201).json(rezultat);
}

export async function login(req, res) {
  const rezultat = await authService.autentificare(req.body ?? {});
  res.json(rezultat);
}

export async function me(req, res) {
  const utilizator = await authService.utilizatorCurent(req.utilizator.id);
  res.json(utilizator);
}
