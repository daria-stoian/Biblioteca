// Teste pentru regulile de imprumut. Rulare: npm test
import { test } from 'node:test';
import assert from 'node:assert/strict';
import {
  adaugaZile,
  zileIntarziere,
  calculeazaPenalizare,
  statusEfectiv,
  descrieImprumut,
} from '../src/services/imprumut.reguli.js';

const scadenta = new Date('2026-11-01T12:00:00Z');
const ore = (h) => new Date(scadenta.getTime() + h * 60 * 60 * 1000);

test('termenul de 14 zile', () => {
  assert.equal(adaugaZile(new Date('2026-11-01T10:00:00Z'), 14).toISOString(), '2026-11-15T10:00:00.000Z');
});

test('fara taxa daca returnarea e inainte sau exact la termen', () => {
  assert.equal(calculeazaPenalizare(scadenta, ore(-5)), 0);
  assert.equal(calculeazaPenalizare(scadenta, scadenta), 0);
});

test('orice zi inceputa de intarziere se plateste (diferenta fata de v1.0)', () => {
  assert.equal(zileIntarziere(scadenta, ore(1)), 1); // in v1.0: 0 zile
  assert.equal(zileIntarziere(scadenta, ore(23)), 1); // in v1.0: 0 zile
  assert.equal(zileIntarziere(scadenta, ore(24)), 1);
  assert.equal(zileIntarziere(scadenta, ore(25)), 2);
  assert.equal(calculeazaPenalizare(scadenta, ore(24 * 3)), 3);
});

test('statusul Intarziat se calculeaza (in v1.0 nu era setat niciodata)', () => {
  assert.equal(statusEfectiv({ status: 'Activ', dataScadenta: scadenta }, ore(-1)), 'Activ');
  assert.equal(statusEfectiv({ status: 'Activ', dataScadenta: scadenta }, ore(1)), 'Intarziat');
  assert.equal(statusEfectiv({ status: 'Prelungit', dataScadenta: scadenta }, ore(1)), 'Intarziat');
  assert.equal(statusEfectiv({ status: 'Returnat', dataScadenta: scadenta }, ore(500)), 'Returnat');
});

test('prelungirea e permisa doar pentru un imprumut Activ, inainte de termen', () => {
  const baza = { taxaPenalizare: 0, dataScadenta: scadenta };
  assert.equal(descrieImprumut({ ...baza, status: 'Activ' }, ore(-1)).poatePrelungi, true);
  assert.equal(descrieImprumut({ ...baza, status: 'Activ' }, ore(1)).poatePrelungi, false);
  assert.equal(descrieImprumut({ ...baza, status: 'Prelungit' }, ore(-1)).poatePrelungi, false);
});

test('taxa estimata pentru un imprumut intarziat nereturnat', () => {
  const d = descrieImprumut({ status: 'Activ', taxaPenalizare: 0, dataScadenta: scadenta }, ore(24 * 2 + 3));
  assert.equal(d.status, 'Intarziat');
  assert.equal(d.zileIntarziere, 3);
  assert.equal(d.taxaEstimata, 3);
});
