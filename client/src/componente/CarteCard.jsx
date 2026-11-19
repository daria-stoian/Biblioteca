// O carte din grila catalogului
import { Link } from 'react-router';
import Coperta from './Coperta.jsx';
import { textStatus } from '../format.js';

export default function CarteCard({ carte }) {
  return (
    <Link to={`/carti/${carte.id}`} className="carte-card">
      <Coperta src={carte.imagineCoperta} titlu={carte.titlu} />
      <div className="info">
        <span className="titlu">{carte.titlu}</span>
        <span className="autor">{carte.autor.numeAutor}</span>
        <span className={`eticheta eticheta-${carte.status}`}>{textStatus[carte.status]}</span>
      </div>
    </Link>
  );
}
