// Citeste configurarea din variabilele de mediu (fisierul .env, RN-01).
// Daca lipseste o valoare obligatorie, serverul se opreste imediat cu un mesaj clar.

const obligatorii = ['DATABASE_URL'];

for (const nume of obligatorii) {
  if (!process.env[nume]) {
    console.error(`Lipseste variabila de mediu ${nume}. Verifica fisierul server/.env.`);
    process.exit(1);
  }
}

export const config = {
  port: Number(process.env.PORT) || 3000,
  clientUrl: process.env.CLIENT_URL || 'http://localhost:5173',
  nodeEnv: process.env.NODE_ENV || 'development',
};
