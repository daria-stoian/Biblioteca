// Harta paginilor aplicatiei: ce componenta se afiseaza la fiecare adresa
import { Route, Routes } from 'react-router';
import Antet from './componente/Antet.jsx';
import RutaProtejata from './auth/RutaProtejata.jsx';
import Catalog from './pagini/Catalog.jsx';
import DetaliiCarte from './pagini/DetaliiCarte.jsx';
import Login from './pagini/Login.jsx';
import Inregistrare from './pagini/Inregistrare.jsx';
import ImprumuturileMele from './pagini/ImprumuturileMele.jsx';
import PaginaInexistenta from './pagini/PaginaInexistenta.jsx';

export default function App() {
  return (
    <>
      <Antet />
      <main className="continut">
        <Routes>
          <Route path="/" element={<Catalog />} />
          <Route path="/carti/:id" element={<DetaliiCarte />} />
          <Route path="/login" element={<Login />} />
          <Route path="/inregistrare" element={<Inregistrare />} />
          <Route
            path="/imprumuturi"
            element={
              <RutaProtejata>
                <ImprumuturileMele />
              </RutaProtejata>
            }
          />
          <Route path="*" element={<PaginaInexistenta />} />
        </Routes>
      </main>
    </>
  );
}
