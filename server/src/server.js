// Punctul de pornire: deschide portul si asculta cereri.
import { config } from './config/env.js';
import app from './app.js';

app.listen(config.port, () => {
  console.log(`API BiblioWeb pornit pe http://localhost:${config.port}/api`);
});
