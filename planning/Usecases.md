# ContactManager – Usecases / GUI-Testfälle

Zweck: Diese Datei ist die Checkliste, mit der wir vor der Abgabe (20.09.2026, 23:00 Uhr) die komplette Applikation manuell durchklicken und abhaken, damit wir wissen, was sauber funktioniert und was nicht. Genau diese Aufstellung hilft uns auch, in der Abgabe-Textdatei ehrlich zu beschreiben, "was funktioniert und was nicht" (Pflichtangabe laut Aufgabenstellung).

Legende: **Vorbedingung** = Zustand vor dem Test · **Schritte** = was wir in der GUI tun · **Erwartetes Ergebnis** = was passieren muss, damit der Test als bestanden gilt.

---

## 0. Grundsätzliches / Setup

- [ ] Applikation lässt sich ohne Visual Studio (z.B. via `.exe` aus dem Build-Ordner) starten.
- [ ] Applikation startet auch dann fehlerfrei, wenn noch keine Datendatei existiert (erster Start auf einem neuen Rechner).
- [ ] Applikation startet fehlerfrei, wenn schon Daten von einem vorherigen Testlauf vorhanden sind.
- [ ] Fenstergrösse/Layout ist beim Start sinnvoll lesbar (kein abgeschnittener Text, keine überlappenden Elemente).

## 1. Kunde erfassen (Create)

- [ ] Neuen Kunden mit allen Pflichtfeldern korrekt ausfüllen (Anrede, Vorname, Nachname, Geburtsdatum, Geschlecht, Firma, Titel/Berufsbezeichnung, Telefon Geschäft, Mobil, E-Mail) → Speichern → Kunde erscheint korrekt in der Liste/Übersicht.
- [ ] Kunde ohne Vorname erfassen → Fehlermeldung "Vorname ist erforderlich", kein Absturz, kein leerer Eintrag wird gespeichert.
- [ ] Kunde ohne Nachname erfassen → entsprechende Fehlermeldung.
- [ ] Kunde ohne Firma erfassen → Fehlermeldung "Firma ist für einen Kunden erforderlich".
- [ ] Kunde mit ungültiger E-Mail-Adresse (z.B. `max@`) erfassen → Fehlermeldung "E-Mail-Adresse ist ungültig".
- [ ] Kunde ohne E-Mail-Adresse erfassen → Fehlermeldung "E-Mail-Adresse ist erforderlich" (E-Mail ist Pflichtfeld).
- [ ] Kunde mit Geburtsdatum in der Zukunft erfassen → Fehlermeldung "Geburtsdatum darf nicht in der Zukunft liegen".
- [ ] Kunde mit Status "passiv" direkt bei der Erfassung anlegen (falls die GUI das zulässt) → wird korrekt als inaktiv gespeichert.
- [ ] Nach dem Erfassen: Applikation neu starten → neuer Kunde ist noch vorhanden (Persistenz).

## 2. Mitarbeiter erfassen (Create)

- [ ] Neuen Mitarbeiter mit allen Pflichtfeldern erfassen (inkl. Abteilung, AHV-Nummer, Wohnort, Nationalität, Adresse, PLZ, Eintrittsdatum, Beschäftigungsgrad, Kaderstufe, Bürostandort) → Speichern → erscheint korrekt in der Liste.
- [ ] Mitarbeiter erhält automatisch eine **Mitarbeiternummer ab 1000**, ohne dass man sie manuell eingeben muss.
- [ ] Zwei Mitarbeiter nacheinander erfassen → beide erhalten unterschiedliche, aufsteigende Mitarbeiternummern.
- [ ] Mitarbeiter mit Geburtsdatum jünger als 16 Jahre erfassen → Fehlermeldung "Mitarbeiter muss mindestens 16 Jahre alt sein".
- [ ] Mitarbeiter mit Beschäftigungsgrad `0%` erfassen → Fehlermeldung (gültiger Bereich ist 5–100 %).
- [ ] Mitarbeiter mit Beschäftigungsgrad `150%` erfassen → Fehlermeldung.
- [ ] Mitarbeiter mit Beschäftigungsgrad genau `5%` und genau `100%` erfassen → beide werden akzeptiert (Grenzwerte).
- [ ] Mitarbeiter ohne Eintrittsdatum erfassen → Fehlermeldung "Eintrittsdatum ist erforderlich".
- [ ] Mitarbeiter mit Austrittsdatum **vor** dem Eintrittsdatum erfassen → Fehlermeldung.
- [ ] Mitarbeiter ohne Austrittsdatum (noch aktiv angestellt) erfassen → wird akzeptiert, in der GUI wird **kein** unsinniges Datum wie `31.12.9999` angezeigt, sondern z.B. "-" oder "aktuell angestellt".
- [ ] Alle Kaderstufen (0 bis 5) einzeln durchtesten, ob sie in der GUI korrekt auswählbar und speicherbar sind.
- [ ] Alle Bürostandorte (Zürich, St. Gallen, Genf) einzeln testen.

## 3. Lernende/n erfassen (Create)

- [ ] Neue/n Lernende/n mit allen Mitarbeiterfeldern **plus** Lehrjahre (Gesamtdauer) und aktuellem Lehrjahr erfassen → wird korrekt gespeichert.
- [ ] Lernende/r erhält ebenfalls automatisch eine Mitarbeiternummer.
- [ ] Lehrjahre-Gesamtdauer `0` eingeben → Fehlermeldung (gültig ist 1–4 Jahre).
- [ ] Lehrjahre-Gesamtdauer `5` eingeben → Fehlermeldung (Maximum ist 4 Jahre).
- [ ] Aktuelles Lehrjahr grösser als Gesamtdauer eingeben (z.B. 3. Lehrjahr bei 2 Jahren Gesamtdauer) → Fehlermeldung.
- [ ] Aktuelles Lehrjahr `0` eingeben → Fehlermeldung.
- [ ] Lernende/r mit 16-jährigem Geburtsdatum (Grenzfall) erfassen → wird akzeptiert.

## 4. Mutieren (Update) von Kunde / Mitarbeiter / Lernende/r

- [ ] Bestehenden Kunden öffnen, ein Feld ändern (z.B. Telefonnummer), speichern → Änderung ist sofort in der Liste sichtbar.
- [ ] Bestehenden Mitarbeiter mutieren → **Mitarbeiternummer bleibt unverändert**, auch wenn man versucht, sie manuell zu ändern.
- [ ] Beim Mutieren ein Pflichtfeld leeren (z.B. Nachname löschen) und speichern versuchen → Fehlermeldung, alte Daten bleiben erhalten (kein Datenverlust).
- [ ] Beim Mutieren ungültige Werte eingeben (z.B. Beschäftigungsgrad 200%) → Fehlermeldung, nichts wird gespeichert.
- [ ] Nach Mutation: Applikation neu starten → Änderungen sind noch vorhanden.
- [ ] Abbrechen-Button beim Mutieren verwenden → keine Änderungen werden übernommen.

## 5. Aktivieren / Deaktivieren

- [ ] Aktiven Kunden/Mitarbeiter deaktivieren → Status wechselt sichtbar auf "passiv/inaktiv".
- [ ] Deaktivierten Kontakt wieder aktivieren → Status wechselt zurück auf "aktiv".
- [ ] Deaktivierter Kontakt bleibt in der Datenliste erhalten (wird nicht gelöscht) und ist weiterhin auffindbar/anzeigbar.
- [ ] Status-Änderung übersteht einen Neustart der Applikation.

## 6. Löschen

- [ ] Kontakt löschen → verschwindet aus der Liste.
- [ ] Vor dem endgültigen Löschen erscheint eine Sicherheitsabfrage ("Wirklich löschen?").
- [ ] Löschen abbrechen (Sicherheitsabfrage verneinen) → Kontakt bleibt erhalten.
- [ ] Gelöschter Kontakt ist nach Neustart der Applikation wirklich weg (nicht nur aus der Liste ausgeblendet).
- [ ] Kunden mit vorhandener Kontakt-Historie löschen → auch die Historie verschwindet mit, kein verwaister Datensatz, kein Absturz.

## 7. Suche

- [ ] Suche nach exaktem Nachnamen → korrekter Treffer.
- [ ] Suche nach Teilstring (z.B. nur "Mül" statt "Müller") → wird gefunden.
- [ ] Suche Gross-/Kleinschreibung ignorieren (z.B. "müller" findet "Müller").
- [ ] Suche nach Vornamen → korrekter Treffer.
- [ ] Suche nach Geburtsdatum → korrekter Treffer.
- [ ] Suche, die sowohl auf einen Kunden als auch auf einen Mitarbeiter passt → beide werden angezeigt, klar unterscheidbar in der GUI.
- [ ] Suche nach einem Begriff, der aus einer Kundenkontakt-Notiz stammt → Kunde wird gefunden.
- [ ] Suchfeld leeren → wieder alle Kontakte werden angezeigt.
- [ ] Suche nach einem nicht existierenden Begriff (z.B. "xyzxyz") → leere Liste, keine Fehlermeldung/Absturz.

## 8. Kundenkontakt-Historie

- [ ] Bei einem Kunden eine neue Kontaktnotiz erfassen → erscheint in der Historie mit Datum/Zeit.
- [ ] Mehrere Notizen nacheinander erfassen → alle bleiben in chronologischer Reihenfolge erhalten.
- [ ] Leere Notiz speichern versuchen → Fehlermeldung "Kontaktnotiz ist erforderlich", wird nicht gespeichert.
- [ ] Historie eines Kunden ansehen, ohne etwas zu verändern → bestehende Einträge werden korrekt und vollständig angezeigt.
- [ ] Kontakt-Historie übersteht einen Neustart der Applikation.
- [ ] Bei einem **Mitarbeiter** gibt es (bewusst) keine Möglichkeit, eine Kundenkontakt-Notiz zu erfassen (nur Kunden haben eine Historie).

## 9. Persistenz / Speichern & Laden

- [ ] Eine Änderung wird nur dann gespeichert, wenn man aktiv auf den jeweiligen Button klickt (z.B. "Speichern", "Ja" bei der Löschen-Sicherheitsabfrage) — es gibt kein automatisches Speichern im Hintergrund. Der Klick löst das Speichern sofort aus, ein separater Speichervorgang danach ist nicht nötig.
- [ ] Applikation normal schliessen und neu starten → alle Daten sind identisch zu vorher.
- [ ] Applikation über den Task-Manager hart beenden, während gerade **nicht** gespeichert wird, dann neu starten → letzter erfolgreich gespeicherter Stand ist vorhanden (kein Totalverlust der Datei).
- [ ] Datendatei manuell mit ungültigem Inhalt überschreiben (z.B. Text reinschreiben, der kein gültiges JSON ist), Applikation starten → **klare Fehlermeldung für den Benutzer** (nicht nur eine Konsolenausgabe, die niemand sieht), Applikation stürzt nicht ab.

## 10. Validierung / Fehlerbehandlung allgemein

- [ ] In jedem Formular: alle Pflichtfelder leer lassen und Speichern klicken → **eine** verständliche, zusammengefasste Fehlermeldung (nicht 10 einzelne Pop-ups nacheinander), kein Absturz.
- [ ] Sehr lange Texteingaben (z.B. 500 Zeichen im Namensfeld) → Applikation stürzt nicht ab, verhält sich sinnvoll.
- [ ] Sonderzeichen/Umlaute in Namen (ä, ö, ü, é) → werden korrekt gespeichert und wieder angezeigt (auch nach Neustart).
- [ ] Datumsfeld mit offensichtlich unsinnigem Wert (falls frei eingebbar) → wird abgefangen, kein Absturz.

## 11. Optionale Anforderungen (CSV-Import/Export, Login, Dashboard, Mutationshistorie)

- [ ] CSV-Import: Beispieldatei liegt bei, Import mit gültiger Datei getestet, Import mit fehlerhafter/beschädigter Datei getestet (keine Abstürze, klare Fehlermeldung).
- [ ] CSV-Export: Export einer bestehenden Kontaktliste getestet, exportierte Datei lässt sich anschliessend wieder erfolgreich importieren.
- [ ] Login: gültige Zugangsdaten getestet, ungültige Zugangsdaten getestet (klare Fehlermeldung, kein Zugriff), Zugangsdaten für die Abgabe-Textdatei notiert.
- [ ] Dashboard: Zahlen/Übersicht auf dem Dashboard stimmen mit den tatsächlich gespeicherten Daten überein.
- [ ] Mutationshistorie: eine Änderung durchführen, prüfen ob sie korrekt protokolliert und einsehbar ist.
- [ ] Alle vier optionalen Anforderungen übersteht einen Neustart der Applikation (Daten/Konfiguration bleiben erhalten).

## 12. Allgemeine Stabilität & Usability (Dozenten-Perspektive)

- [ ] Applikation crasht bei keinem der obigen Tests unerwartet (ohne Fehlermeldung) auf den Desktop.
- [ ] Alle Fehlermeldungen sind auf Deutsch (oder konsistent in einer Sprache) und für eine fachfremde Person verständlich formuliert.
- [ ] Buttons/Felder sind eindeutig beschriftet, Tab-Reihenfolge in Formularen ist logisch.
- [ ] Fenster lässt sich nicht in einen kaputten/unbedienbaren Zustand bringen (z.B. durch Grössenänderung, mehrfaches schnelles Klicken).
- [ ] Applikation mit den mitgelieferten Testdaten (`test-data/customer.json`, `test-data/employees.json`) einmal komplett durchgespielt, falls dafür eine Import-/Lademöglichkeit existiert.

---

**Hinweis:** Sobald ein Punkt bei einem Testlauf fehlschlägt, kurz notieren *was* genau passiert ist (Fehlermeldung, falsches Verhalten, Absturz) – das erleichtert das Beheben und ist die Grundlage für die "was funktioniert nicht"-Beschreibung in der Abgabe-Textdatei.
