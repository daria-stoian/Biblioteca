import { verificaStare } from '../services/health.service.js';

export async function getHealth(req, res) {
  const stare = await verificaStare();
  res.status(stare.status === 'ok' ? 200 : 503).json(stare);
}
