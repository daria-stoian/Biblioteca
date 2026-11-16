// Paginarea listelor (RN-03): ?pagina=2&limita=12
// Valorile lipsa sau invalide primesc valori implicite; limita are un plafon,
// ca un client sa nu poata cere toate inregistrarile deodata.
const LIMITA_IMPLICITA = 12;
const LIMITA_MAXIMA = 50;

export function citestePaginare(query) {
  const pagina = Math.max(1, parseInt(query.pagina, 10) || 1);
  const limita = Math.min(LIMITA_MAXIMA, Math.max(1, parseInt(query.limita, 10) || LIMITA_IMPLICITA));
  return { pagina, limita, skip: (pagina - 1) * limita, take: limita };
}

export function raspunsPaginat(elemente, total, { pagina, limita }) {
  return {
    elemente,
    total,
    pagina,
    limita,
    totalPagini: Math.max(1, Math.ceil(total / limita)),
  };
}
