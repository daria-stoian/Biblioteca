// Eroare "asteptata", cu cod HTTP si mesaj pentru utilizator.
// Exemplu (Pasul 5): throw new AppError(409, 'Ai deja această carte împrumutată.');
export class AppError extends Error {
  constructor(status, mesaj) {
    super(mesaj);
    this.status = status;
  }
}
