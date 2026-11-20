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

app.use(cors({ origin: config.clientUrl }));
app.use(express.json());

// Copertele cartilor
app.use('/coperti', express.static(fileURLToPath(new URL('../public/coperti', import.meta.url))));

app.use('/api/docs', swaggerUi.serve, swaggerUi.setup(swaggerSpec, { swaggerOptions: { persistAuthorization: true } }));
app.use('/api', routes);

app.use(notFound);
app.use(errorHandler);

export default app;
