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
import AdminLayout from './pagini/admin/AdminLayout.jsx';
import AdminCarti from './pagini/admin/AdminCarti.jsx';
import FormularCarte from './pagini/admin/FormularCarte.jsx';
import AdminAutori from './pagini/admin/AdminAutori.jsx';
import AdminCategorii from './pagini/admin/AdminCategorii.jsx';
import AdminImprumuturi from './pagini/admin/AdminImprumuturi.jsx';

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

          {/* Administrare: rute "imbricate" care au in comun AdminLayout (titlul si filele) */}
          <Route
            path="/admin"
            element={
              <RutaProtejata rol="Administrator">
                <AdminLayout />
              </RutaProtejata>
            }
          >
            <Route index element={<AdminCarti />} />
            <Route path="carti/noua" element={<FormularCarte />} />
            <Route path="carti/:id" element={<FormularCarte />} />
            <Route path="autori" element={<AdminAutori />} />
            <Route path="categorii" element={<AdminCategorii />} />
            <Route path="imprumuturi" element={<AdminImprumuturi />} />
          </Route>

          <Route path="*" element={<PaginaInexistenta />} />
        </Routes>
      </main>
    </>
  );
}
