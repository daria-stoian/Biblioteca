// Butoanele "Înapoi / Înainte" sub catalog (RN-03)
export default function Paginare({ pagina, totalPagini, laSchimbare }) {
  if (totalPagini <= 1) return null;

  return (
    <nav className="paginare" aria-label="Paginare">
      <button className="buton buton-secundar" disabled={pagina <= 1} onClick={() => laSchimbare(pagina - 1)}>
        ← Înapoi
      </button>
      <span>Pagina {pagina} din {totalPagini}</span>
      <button className="buton buton-secundar" disabled={pagina >= totalPagini} onClick={() => laSchimbare(pagina + 1)}>
        Înainte →
      </button>
    </nav>
  );
}
