// Formatare pentru afisare, in limba romana
export const formatData = (iso) =>
  new Date(iso).toLocaleDateString('ro-RO', { day: 'numeric', month: 'long', year: 'numeric' });

export const formatLei = (suma) =>
  new Intl.NumberFormat('ro-RO', { style: 'currency', currency: 'RON' }).format(suma);

// Textul afisat pentru fiecare status (in baza de date sunt fara diacritice)
export const textStatus = {
  Disponibil: 'Disponibilă',
  Indisponibil: 'Indisponibilă',
  Activ: 'Activ',
  Prelungit: 'Prelungit',
  Intarziat: 'Întârziat',
  Returnat: 'Returnat',
};
