const obligatorii = ['DATABASE_URL', 'JWT_SECRET'];

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
  jwt: {
    secret: process.env.JWT_SECRET,
    expiresIn: process.env.JWT_EXPIRES_IN || '8h',
  },
};
