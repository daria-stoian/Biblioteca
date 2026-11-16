// Documentatia interactiva a API-ului (RF-26), disponibila la /api/docs.
// swagger-jsdoc citeste comentariile @openapi scrise deasupra rutelor din src/routes/.
import swaggerJsdoc from 'swagger-jsdoc';

export const swaggerSpec = swaggerJsdoc({
  definition: {
    openapi: '3.0.3',
    info: {
      title: 'BiblioWeb API',
      version: '0.1.0',
      description: 'API REST pentru aplicația BiblioWeb (versiunea 2.0).',
    },
    servers: [{ url: '/api' }],
  },
  apis: ['./src/routes/*.js'],
});
