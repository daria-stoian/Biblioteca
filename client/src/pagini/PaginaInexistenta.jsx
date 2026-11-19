import { Link } from 'react-router';

export default function PaginaInexistenta() {
  return (
    <div className="card">
      <h1>Pagina nu există</h1>
      <p><Link to="/">Înapoi la catalog</Link></p>
    </div>
  );
}
