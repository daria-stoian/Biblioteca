-- CreateEnum
CREATE TYPE "Rol" AS ENUM ('Administrator', 'Cititor');

-- CreateEnum
CREATE TYPE "StatusDisponibilitate" AS ENUM ('Disponibil', 'Indisponibil');

-- CreateEnum
CREATE TYPE "StatusImprumut" AS ENUM ('Activ', 'Prelungit', 'Returnat', 'Intarziat');

-- CreateEnum
CREATE TYPE "StatusRezervare" AS ENUM ('Activa', 'Anulata', 'Finalizata');

-- CreateEnum
CREATE TYPE "StatusMesaj" AS ENUM ('Necitit', 'Citit', 'Raspuns_Trimis');

-- CreateTable
CREATE TABLE "utilizatori" (
    "id" SERIAL NOT NULL,
    "numeComplet" VARCHAR(150) NOT NULL,
    "email" VARCHAR(150) NOT NULL,
    "parolaHash" TEXT NOT NULL,
    "rol" "Rol" NOT NULL DEFAULT 'Cititor',
    "telefon" VARCHAR(20),
    "adresa" VARCHAR(250),
    "pozaProfil" TEXT,
    "dataCreare" TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT "utilizatori_pkey" PRIMARY KEY ("id")
);

-- CreateTable
CREATE TABLE "autori" (
    "id" SERIAL NOT NULL,
    "numeAutor" VARCHAR(150) NOT NULL,
    "biografie" TEXT,

    CONSTRAINT "autori_pkey" PRIMARY KEY ("id")
);

-- CreateTable
CREATE TABLE "categorii" (
    "id" SERIAL NOT NULL,
    "numeCategorie" VARCHAR(100) NOT NULL,

    CONSTRAINT "categorii_pkey" PRIMARY KEY ("id")
);

-- CreateTable
CREATE TABLE "carti" (
    "id" SERIAL NOT NULL,
    "titlu" VARCHAR(200) NOT NULL,
    "isbn" VARCHAR(20),
    "anPublicare" INTEGER NOT NULL,
    "numarPagini" INTEGER NOT NULL,
    "descriere" VARCHAR(2000),
    "imagineCoperta" TEXT,
    "fisierPdf" TEXT,
    "status" "StatusDisponibilitate" NOT NULL DEFAULT 'Disponibil',
    "autorId" INTEGER NOT NULL,
    "categorieId" INTEGER NOT NULL,

    CONSTRAINT "carti_pkey" PRIMARY KEY ("id")
);

-- CreateTable
CREATE TABLE "imprumuturi" (
    "id" SERIAL NOT NULL,
    "dataImprumut" TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "dataScadenta" TIMESTAMP(3) NOT NULL,
    "dataReturnare" TIMESTAMP(3),
    "taxaPenalizare" DECIMAL(10,2) NOT NULL DEFAULT 0,
    "status" "StatusImprumut" NOT NULL DEFAULT 'Activ',
    "utilizatorId" INTEGER NOT NULL,
    "carteId" INTEGER NOT NULL,

    CONSTRAINT "imprumuturi_pkey" PRIMARY KEY ("id")
);

-- CreateTable
CREATE TABLE "recenzii" (
    "id" SERIAL NOT NULL,
    "nota" INTEGER NOT NULL,
    "comentariu" VARCHAR(2000),
    "dataPostare" TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "utilizatorId" INTEGER NOT NULL,
    "carteId" INTEGER NOT NULL,

    CONSTRAINT "recenzii_pkey" PRIMARY KEY ("id")
);

-- CreateTable
CREATE TABLE "lista_dorinte" (
    "dataAdaugare" TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "utilizatorId" INTEGER NOT NULL,
    "carteId" INTEGER NOT NULL,

    CONSTRAINT "lista_dorinte_pkey" PRIMARY KEY ("utilizatorId","carteId")
);

-- CreateTable
CREATE TABLE "evenimente" (
    "id" SERIAL NOT NULL,
    "titluEveniment" VARCHAR(200) NOT NULL,
    "descriere" VARCHAR(2000),
    "dataOra" TIMESTAMP(3) NOT NULL,
    "locatie" VARCHAR(200),
    "locuriDisponibile" INTEGER NOT NULL,
    "locuriRezervate" INTEGER NOT NULL DEFAULT 0,

    CONSTRAINT "evenimente_pkey" PRIMARY KEY ("id")
);

-- CreateTable
CREATE TABLE "rezervari_evenimente" (
    "id" SERIAL NOT NULL,
    "dataRezervare" TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "status" "StatusRezervare" NOT NULL DEFAULT 'Activa',
    "utilizatorId" INTEGER NOT NULL,
    "evenimentId" INTEGER NOT NULL,

    CONSTRAINT "rezervari_evenimente_pkey" PRIMARY KEY ("id")
);

-- CreateTable
CREATE TABLE "contacte" (
    "id" SERIAL NOT NULL,
    "numeExpeditor" VARCHAR(100) NOT NULL,
    "emailExpeditor" VARCHAR(150) NOT NULL,
    "mesaj" VARCHAR(2000) NOT NULL,
    "dataTrimitere" TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "status" "StatusMesaj" NOT NULL DEFAULT 'Necitit',
    "utilizatorId" INTEGER,

    CONSTRAINT "contacte_pkey" PRIMARY KEY ("id")
);

-- CreateIndex
CREATE UNIQUE INDEX "utilizatori_email_key" ON "utilizatori"("email");

-- CreateIndex
CREATE UNIQUE INDEX "categorii_numeCategorie_key" ON "categorii"("numeCategorie");

-- CreateIndex
CREATE UNIQUE INDEX "carti_isbn_key" ON "carti"("isbn");

-- CreateIndex
CREATE INDEX "carti_titlu_idx" ON "carti"("titlu");

-- CreateIndex
CREATE INDEX "carti_categorieId_idx" ON "carti"("categorieId");

-- CreateIndex
CREATE INDEX "imprumuturi_utilizatorId_status_idx" ON "imprumuturi"("utilizatorId", "status");

-- CreateIndex
CREATE UNIQUE INDEX "recenzii_utilizatorId_carteId_key" ON "recenzii"("utilizatorId", "carteId");

-- CreateIndex
CREATE INDEX "rezervari_evenimente_evenimentId_status_idx" ON "rezervari_evenimente"("evenimentId", "status");

-- AddForeignKey
ALTER TABLE "carti" ADD CONSTRAINT "carti_autorId_fkey" FOREIGN KEY ("autorId") REFERENCES "autori"("id") ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "carti" ADD CONSTRAINT "carti_categorieId_fkey" FOREIGN KEY ("categorieId") REFERENCES "categorii"("id") ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "imprumuturi" ADD CONSTRAINT "imprumuturi_utilizatorId_fkey" FOREIGN KEY ("utilizatorId") REFERENCES "utilizatori"("id") ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "imprumuturi" ADD CONSTRAINT "imprumuturi_carteId_fkey" FOREIGN KEY ("carteId") REFERENCES "carti"("id") ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "recenzii" ADD CONSTRAINT "recenzii_utilizatorId_fkey" FOREIGN KEY ("utilizatorId") REFERENCES "utilizatori"("id") ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "recenzii" ADD CONSTRAINT "recenzii_carteId_fkey" FOREIGN KEY ("carteId") REFERENCES "carti"("id") ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "lista_dorinte" ADD CONSTRAINT "lista_dorinte_utilizatorId_fkey" FOREIGN KEY ("utilizatorId") REFERENCES "utilizatori"("id") ON DELETE CASCADE ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "lista_dorinte" ADD CONSTRAINT "lista_dorinte_carteId_fkey" FOREIGN KEY ("carteId") REFERENCES "carti"("id") ON DELETE CASCADE ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "rezervari_evenimente" ADD CONSTRAINT "rezervari_evenimente_utilizatorId_fkey" FOREIGN KEY ("utilizatorId") REFERENCES "utilizatori"("id") ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "rezervari_evenimente" ADD CONSTRAINT "rezervari_evenimente_evenimentId_fkey" FOREIGN KEY ("evenimentId") REFERENCES "evenimente"("id") ON DELETE CASCADE ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "contacte" ADD CONSTRAINT "contacte_utilizatorId_fkey" FOREIGN KEY ("utilizatorId") REFERENCES "utilizatori"("id") ON DELETE SET NULL ON UPDATE CASCADE;
