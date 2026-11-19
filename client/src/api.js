// Un singur loc prin care React vorbeste cu API-ul Express.
// - adauga automat token-ul JWT in antetul Authorization
// - transforma raspunsurile de eroare ({ eroare: "..." }) in exceptii cu mesajul serverului

const CHEIE_TOKEN = 'biblioweb_token';

export const tokenStocat = {
  citeste: () => localStorage.getItem(CHEIE_TOKEN),
  salveaza: (token) => localStorage.setItem(CHEIE_TOKEN, token),
  sterge: () => localStorage.removeItem(CHEIE_TOKEN),
};

// Functie apelata cand serverul raspunde 401 (token expirat); o seteaza AuthContext.
let laSesiuneExpirata = () => {};
export function setLaSesiuneExpirata(functie) {
  laSesiuneExpirata = functie;
}

export class EroareApi extends Error {
  constructor(status, mesaj) {
    super(mesaj);
    this.status = status;
  }
}

async function cerere(cale, { metoda = 'GET', corp } = {}) {
  const antete = {};
  const token = tokenStocat.citeste();
  if (token) antete.Authorization = `Bearer ${token}`;
  if (corp !== undefined) antete['Content-Type'] = 'application/json';

  let raspuns;
  try {
    raspuns = await fetch(`/api${cale}`, {
      method: metoda,
      headers: antete,
      body: corp !== undefined ? JSON.stringify(corp) : undefined,
    });
  } catch {
    throw new EroareApi(0, 'Serverul nu răspunde. Verifică dacă API-ul este pornit.');
  }

  const date = await raspuns.json().catch(() => null);

  if (!raspuns.ok) {
    if (raspuns.status === 401 && token) laSesiuneExpirata();
    throw new EroareApi(raspuns.status, date?.eroare || `Eroare ${raspuns.status}`);
  }
  return date;
}

// Construieste "?cautare=x&pagina=2", ignorand valorile goale
function interogare(parametri) {
  const p = new URLSearchParams();
  for (const [cheie, valoare] of Object.entries(parametri)) {
    if (valoare !== undefined && valoare !== null && valoare !== '') p.set(cheie, valoare);
  }
  const text = p.toString();
  return text ? `?${text}` : '';
}

export const api = {
  // Autentificare
  login: (email, parola) => cerere('/auth/login', { metoda: 'POST', corp: { email, parola } }),
  inregistrare: (numeComplet, email, parola) =>
    cerere('/auth/register', { metoda: 'POST', corp: { numeComplet, email, parola } }),
  eu: () => cerere('/auth/me'),

  // Catalog
  carti: (filtre) => cerere(`/carti${interogare(filtre)}`),
  carte: (id) => cerere(`/carti/${id}`),
  categorii: () => cerere('/categorii'),

  // Imprumuturi
  imprumuta: (carteId) => cerere('/imprumuturi', { metoda: 'POST', corp: { carteId } }),
  imprumuturileMele: () => cerere('/imprumuturi/ale-mele'),
  returneaza: (id) => cerere(`/imprumuturi/${id}/returnare`, { metoda: 'POST' }),
  prelungeste: (id) => cerere(`/imprumuturi/${id}/prelungire`, { metoda: 'POST' }),
};
