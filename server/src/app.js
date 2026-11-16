// Construieste aplicatia Express (fara sa porneasca serverul).
// Separarea de server.js permite testarea aplicatiei fara a deschide un port.
import { fileURLToPath } from 'node:url';
import express from 'express';
import cors from 'cors';
import swaggerUi from 'swagger-ui-express';
import { config } from './config/env.js';
import routes from './routes/index.js';
import { swaggerSpec } from './docs/swagger.js';
import { notFound } from './middleware/notFound.js';
import { errorHandler } from './middleware/errorHandler.js';

const app = express();

app.use(cors({ origin: config.clientUrl })); // doar frontend-ul React are voie sa apeleze API-ul
app.use(express.json()); // transforma corpul cererilor JSON in req.body

// Copertele cartilor: /coperti/dune.png -> server/public/coperti/dune.png
app.use('/coperti', express.static(fileURLToPath(new URL('../public/coperti', import.meta.url))));

app.use('/api/docs', swaggerUi.serve, swaggerUi.setup(swaggerSpec));
app.use('/api', routes);

// Ordinea conteaza: acestea doua stau la final, dupa toate rutele.
app.use(notFound);
app.use(errorHandler);

export default app;
