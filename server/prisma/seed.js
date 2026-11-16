// Date initiale pentru BiblioWeb v2.0. Rulare: npm run db:seed
//
// 1. Contul de administrator, din variabilele ADMIN_* din .env (RN-01).
//    In v1.0, parola era scrisa direct in Program.cs ("Admin123!").
// 2. Un catalog demonstrativ: categoriile, autorii si cartile ale caror coperti
//    existau in v1.0 (version-dotnet/Biblioteca/wwwroot/imagini).
//    Anii si numarul de pagini sunt orientative. Fisierele PDF nu sunt incluse.
//
// Scriptul poate fi rulat de mai multe ori: nu creeaza duplicate.
import { PrismaClient } from '@prisma/client';
import bcrypt from 'bcryptjs';

const prisma = new PrismaClient();

const categorii = [
  'Fantasy',
  'Științifico-fantastic',
  'Thriller',
  'Ficțiune istorică',
  'Dezvoltare personală',
  'Clasici',
];

// [titlu, autor, categorie, an, pagini, fisier coperta, descriere]
const carti = [
  ['Harry Potter și Piatra Filozofală', 'J.K. Rowling', 'Fantasy', 1997, 336, 'harry_potter1.png',
    'Primul an al lui Harry la Școala de Magie și Vrăjitorie Hogwarts.'],
  ['Harry Potter și Camera Secretelor', 'J.K. Rowling', 'Fantasy', 1998, 352, 'harry_potter2.png',
    'Al doilea an la Hogwarts, umbrit de atacuri misterioase asupra elevilor.'],
  ['Harry Potter și prizonierul din Azkaban', 'J.K. Rowling', 'Fantasy', 1999, 448, 'harry_potter3.png',
    'Un prizonier periculos evadează din Azkaban, iar urmele duc spre Harry.'],
  ['Harry Potter și copilul blestemat', 'J.K. Rowling', 'Fantasy', 2016, 336, 'harry_potter8.png',
    'Povestea continuă la nouăsprezece ani după bătălia de la Hogwarts.'],
  ['Hobbitul', 'J.R.R. Tolkien', 'Fantasy', 1937, 320, 'hobbitul.png',
    'Bilbo Baggins pleacă alături de treisprezece pitici să recupereze o comoară păzită de un dragon.'],
  ['Cronicile din Narnia: Nepotul magicianului', 'C.S. Lewis', 'Fantasy', 1955, 208, 'croniciledinnarnia1.png',
    'Doi copii ajung, printr-un experiment magic, la începuturile lumii Narnia.'],
  ['Cronicile din Narnia: Leul, vrăjitoarea și dulapul', 'C.S. Lewis', 'Fantasy', 1950, 192, 'croniciledinnarnia2.png',
    'Patru frați descoperă Narnia printr-un dulap și ajută la eliberarea ei.'],
  ['Dune', 'Frank Herbert', 'Științifico-fantastic', 1965, 688, 'dune.png',
    'Lupta pentru controlul planetei deșertice Arrakis, singura sursă a mirodeniei.'],
  ['Fundația', 'Isaac Asimov', 'Științifico-fantastic', 1951, 256, 'fundatia.png',
    'Un matematician prevede căderea Imperiului Galactic și încearcă să scurteze epoca de haos.'],
  ['Pacienta tăcută', 'Alex Michaelides', 'Thriller', 2019, 352, 'pacienta_tacuta.png',
    'O pictoriță încetează să vorbească după ce își împușcă soțul; un psihoterapeut vrea să afle de ce.'],
  ['Black Out', 'John Lawton', 'Thriller', 1995, 368, 'black_out.png',
    'Un inspector de poliție investighează o crimă în Londra bombardată din 1944.'],
  ['Hoțul de cărți', 'Markus Zusak', 'Ficțiune istorică', 2005, 576, 'hotul_de_carti.png',
    'O fetiță din Germania nazistă găsește alinare în cărțile pe care le fură.'],
  ['Râul înghețat', 'Ariel Lawhon', 'Ficțiune istorică', 2023, 432, 'raul_inghetat.png',
    'O moașă din Maine, anul 1789, investighează o moarte suspectă.'],
  ['Micul Prinț', 'Antoine de Saint-Exupéry', 'Clasici', 1943, 96, 'micul_print.png',
    'Un aviator rătăcit în deșert întâlnește un prinț venit de pe un asteroid.'],
  ['Atomic Habits', 'James Clear', 'Dezvoltare personală', 2018, 320, 'atomic_habits.png',
    'Cum se formează obiceiurile bune și cum se renunță la cele rele, prin schimbări mici.'],
  ['Inteligența emoțională', 'Daniel Goleman', 'Dezvoltare personală', 1995, 512, 'inteligenta_emotionala.png',
    'De ce capacitatea de a înțelege emoțiile contează la fel de mult ca IQ-ul.'],
  ['Omul devine ceea ce gândește', 'James Allen', 'Dezvoltare personală', 1903, 64, 'omul_devine_ceea_ce_gandeste.png',
    'Un eseu scurt despre legătura dintre gânduri și caracter.'],
];

async function seedAdmin() {
  const { ADMIN_EMAIL, ADMIN_PASSWORD, ADMIN_NAME } = process.env;
  if (!ADMIN_EMAIL || !ADMIN_PASSWORD) {
    console.warn('ADMIN_EMAIL sau ADMIN_PASSWORD lipsesc din .env – contul de admin nu a fost creat.');
    return;
  }

  const email = ADMIN_EMAIL.trim().toLowerCase();
  await prisma.utilizator.upsert({
    where: { email },
    update: { rol: 'Administrator' }, // parola existenta nu se suprascrie la rulari repetate
    create: {
      email,
      numeComplet: ADMIN_NAME || 'Administrator',
      parolaHash: await bcrypt.hash(ADMIN_PASSWORD, 10),
      rol: 'Administrator',
    },
  });
  console.log(`Administrator: ${email}`);
}

async function seedCatalog() {
  const idCategorie = {};
  for (const nume of categorii) {
    const c = await prisma.categorie.upsert({
      where: { numeCategorie: nume },
      update: {},
      create: { numeCategorie: nume },
    });
    idCategorie[nume] = c.id;
  }

  const idAutor = {};
  for (const numeAutor of new Set(carti.map((c) => c[1]))) {
    const existent = await prisma.autor.findFirst({ where: { numeAutor } });
    idAutor[numeAutor] = (existent ?? (await prisma.autor.create({ data: { numeAutor } }))).id;
  }

  let adaugate = 0;
  for (const [titlu, autor, categorie, an, pagini, coperta, descriere] of carti) {
    const existenta = await prisma.carte.findFirst({ where: { titlu } });
    if (existenta) continue;

    await prisma.carte.create({
      data: {
        titlu,
        anPublicare: an,
        numarPagini: pagini,
        descriere,
        imagineCoperta: `/coperti/${coperta}`,
        autorId: idAutor[autor],
        categorieId: idCategorie[categorie],
      },
    });
    adaugate++;
  }

  console.log(
    `Catalog: ${categorii.length} categorii, ${Object.keys(idAutor).length} autori, ` +
      `${adaugate} cărți noi (${carti.length - adaugate} existau deja).`,
  );
}

try {
  await seedAdmin();
  await seedCatalog();
} catch (err) {
  console.error('Seed eșuat:', err);
  process.exitCode = 1;
} finally {
  await prisma.$disconnect();
}
