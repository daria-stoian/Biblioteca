// adauga automat token-ul JWT in antetul Authorization

const CHEIE_TOKEN = 'biblioweb_token';

export const tokenStocat = {
  citeste: () => localStorage.getItem(CHEIE_TOKEN),
  salveaza: (token) => localStorage.setItem(CHEIE_TOKEN, token),
  sterge: () => localStorage.removeItem(CHEIE_TOKEN),
};

// Functie apelata cand serverul raspunde 401 (token expirat), setata de AuthContext
let laExpirareaSesiunii = () => {};
export function setLaExpirareaSesiunii(functie) {
  laExpirareaSesiunii = functie;
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
    if (raspuns.status === 401 && token) laExpirareaSesiunii();
    throw new EroareApi(raspuns.status, date?.eroare || `Eroare ${raspuns.status}`);
  }
  return date;
}

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
  autori: (cautare) => cerere(`/autori${interogare({ cautare })}`),

  // Imprumuturi
  imprumuta: (carteId) => cerere('/imprumuturi', { metoda: 'POST', corp: { carteId } }),
  imprumuturileMele: () => cerere('/imprumuturi/ale-mele'),
  returneaza: (id) => cerere(`/imprumuturi/${id}/returnare`, { metoda: 'POST' }),
  prelungeste: (id) => cerere(`/imprumuturi/${id}/prelungire`, { metoda: 'POST' }),

  // Administrare (doar cu token de administrator, serverul verifica rolul la fiecare cerere)
  admin: {
    coperti: () => cerere('/carti/coperti'),
    adaugaCarte: (date) => cerere('/carti', { metoda: 'POST', corp: date }),
    modificaCarte: (id, date) => cerere(`/carti/${id}`, { metoda: 'PUT', corp: date }),
    schimbaStatus: (id, status) => cerere(`/carti/${id}/status`, { metoda: 'PATCH', corp: { status } }),
    stergeCarte: (id) => cerere(`/carti/${id}`, { metoda: 'DELETE' }),

    adaugaAutor: (date) => cerere('/autori', { metoda: 'POST', corp: date }),
    modificaAutor: (id, date) => cerere(`/autori/${id}`, { metoda: 'PUT', corp: date }),
    stergeAutor: (id) => cerere(`/autori/${id}`, { metoda: 'DELETE' }),

    adaugaCategorie: (date) => cerere('/categorii', { metoda: 'POST', corp: date }),
    modificaCategorie: (id, date) => cerere(`/categorii/${id}`, { metoda: 'PUT', corp: date }),
    stergeCategorie: (id) => cerere(`/categorii/${id}`, { metoda: 'DELETE' }),

    imprumuturiDeschise: () => cerere('/imprumuturi'),
  },
};
