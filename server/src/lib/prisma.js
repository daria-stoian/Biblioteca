// O singura instanta PrismaClient pentru toata aplicatia.
// Fiecare instanta deschide propriul set de conexiuni, deci nu cream una noua in fiecare serviciu.
import { PrismaClient } from '@prisma/client';

export const prisma = new PrismaClient();
