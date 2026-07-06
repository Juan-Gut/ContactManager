# Requirements List – Semesterprojekt "Contact Manager"

---

## 1. Funktionale Anforderungen (Must-Have)

| ID  | Anforderung                                               | Priorität |
|-----|-----------------------------------------------------------|-----------|
| 01  | Kunde erfassen                                            | Hoch      |
| 02  | Kunde bearbeiten (Mutieren z.B. in eine andere Abteilung) | Hoch      |
| 03  | Kunde löschen                                             | Hoch      |
| 04  | Kunde aktivieren/deaktivieren                             | Hoch      |
| 05  | Mitarbeiter erfassen                                      | Hoch      |
| 06  | Mitarbeiter bearbeiten (Mutieren)                         | Hoch      |
| 07  | Mitarbeiter löschen                                       | Hoch      |
| 08  | Mitarbeiter aktivieren/deaktivieren                       | Hoch      |
| 09  | Mitarbeiternummer automatisch vergeben                    | Hoch      |
| 10  | Kundenkontakte als Notizen erfassen                       | Hoch      |
| 11  | Historie aller Kundenkontakte anzeigen                    | Hoch      |
| 12  | Suche nach Nachname                                       | Hoch      |
| 13  | Suche nach Vorname                                        | Hoch      |
| 14  | Suche nach Geburtsdatum                                   | Hoch      |
| 15  | Suche nach Typ (Kunde/Mitarbeiter)                        | Hoch      |
| 16  | Daten automatisch auf Festplatte speichern                | Hoch      |
| 17  | Daten beim Start automatisch laden                        | Hoch      |

---

## 2. Datenanforderungen

### Kunde

| Feld                   | Pflicht |
|------------------------|---------|
| Anrede                 | Ja      |
| Vorname                | Ja      |
| Nachname               | Ja      |
| Geburtsdatum           | Ja      |
| Geschlecht             | Ja      |
| Titel                  | Nein    |
| Telefonnummer Geschäft | Nein    |
| Mobiltelefonnummer     | Ja      |
| E-Mail-Adresse         | Ja      |
| Status (aktiv/passiv)  | Ja      |

### Mitarbeiter

| Feld                   | Pflicht                                    |
|------------------------|--------------------------------------------|
| Mitarbeiternummer      | Automatisch                                |
| Abteilung              | Ja                                         |
| AHV-Nummer             | Ja                                         |
| Wohnort                | Ja                                         |
| Nationalität           | Ja                                         |
| Adresse                | Ja                                         |
| Postleitzahl           | Ja                                         |
| Mobiltelefonnummer     | Ja                                         |
| Eintrittsdatum         | Ja (Status kann dann berechnet werden)     |
| Austrittsdatum         | Nein                                       |
| Beschäftigungsgrad     | Ja                                         |
| Rolle                  | Ja                                         |
| Kaderstufe (0–5)       | Ja                                         |
| Lehrjahre              | Ja, wenn Lernender                         |
| Aktuelles Lehrjahr     | Ja, wenn Lernender                         |
| Geschäftsadresse       | Ja                                         |
| Telefonnummer Geschäft | Ja                                         |
| E-Mail-Adresse         | Ja                                         |

---

## 3. Kundenkontakt-Anforderungen

| ID    | Anforderung                                |
|-------|--------------------------------------------|
| CR-01 | Kunde kann mehrere Kontakte besitzen       |
| CR-02 | Kontakt enthält Datum/Zeit                 |
| CR-03 | Kontakt enthält Notiztext                  |
| CR-04 | Kontakte werden chronologisch gespeichert  |
| CR-05 | Kontakt-Historie kann angezeigt werden     |

---

## 4. Nichtfunktionale Anforderungen

| ID  | Anforderung                              |
|-----|------------------------------------------|
| N01 | Umsetzung in C#                          |
| N02 | Verwendung von .NET Windows Forms        |
| N03 | Vererbungshierarchie muss vorhanden sein |
| N04 | Benutzerfreundliche Oberfläche           |
| N05 | Fehlerhafte Eingaben abfangen            |
| N06 | Anwendung darf nicht abstürzen           |
| N07 | Public Klassen dokumentieren             |
| N08 | Public Methoden dokumentieren            |
| N09 | Public Properties dokumentieren          |

---

## 5. Optionale Anforderungen (Nice-to-Have)

| ID    | Anforderung                        |
|-------|------------------------------------|
| OR-01 | Mutationshistorie von Kontaktdaten |
| OR-02 | Login-System                       |
| OR-03 | Dashboard-Ansicht                  |
| OR-04 | CSV-Import                         |
| OR-05 | VCard-Import                       |

---

## 6. Empfohlene Klassenstruktur

> Laut Aufgabenstellung ist eine Vererbungshierarchie Pflicht.

```
Person
│
├── Kunde
│    └── List<Kundenkontakt>
│
└── Mitarbeiter
```

**Weitere Klassen:**

- `Person`
- `Kunde`
- `Mitarbeiter`
- `Kundenkontakt`
- `DataManager`
- `FileManager`
- `MainForm`
- `CustomerForm`
- `EmployeeForm`
- `SearchForm`

---

## 7. Akzeptanzkriterien

Das Projekt gilt als erfüllt, wenn:

- [ ] Kunden erstellt, bearbeitet und gelöscht werden können
- [ ] Mitarbeiter erstellt, bearbeitet und gelöscht werden können
- [ ] Mitarbeiternummern automatisch erzeugt werden
- [ ] Kundenkontakte protokolliert werden können
- [ ] Kontakt-Historie sichtbar ist
- [ ] Suchfunktion funktioniert
- [ ] Daten nach Neustart erhalten bleiben
- [ ] Fehlerhafte Eingaben behandelt werden
- [ ] Vererbung eingesetzt wird
- [ ] Windows Forms verwendet wird
