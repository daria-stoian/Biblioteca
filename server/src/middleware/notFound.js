// Ruleaza cand nicio ruta nu a raspuns: adresa nu exista.
export function notFound(req, res) {
  res.status(404).json({ eroare: `Ruta ${req.method} ${req.originalUrl} nu există.` });
}
