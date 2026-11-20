export class AppError extends Error {
  constructor(status, mesaj) {
    super(mesaj);
    this.status = status;
  }
}
