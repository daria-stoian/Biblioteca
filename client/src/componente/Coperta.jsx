// Imaginea copertei; daca lipseste, se afiseaza un dreptunghi gol de aceeasi forma.
export default function Coperta({ src, titlu }) {
  return src ? (
    <img className="coperta" src={src} alt={`Coperta: ${titlu}`} loading="lazy" />
  ) : (
    <div className="coperta" role="img" aria-label="Fără copertă" />
  );
}
