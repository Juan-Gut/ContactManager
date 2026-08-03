# Requirements - Contact Manager

## Features

### Allgemein

| Information | Notizen / Ideen                           |
|-------------|-------------------------------------------|
| Verwaltung und Pflege von Mitarbeiter- und Kundeninformationen (nur Schweizer Kunden) |                                           |
| CRUD Möglichkeiten |                                           |
| Kontaktaufnahme mit Kunden dokumentieren | Eintrag in einem Journal (protokolliert)? |

### Informationen zu verwalten

Die folgende Matrix zeigt, welche Informationen den einzelnen Entitätstypen zugeordnet sind.
Kunde und Mitarbeiter erben von Person. Lernender erbt von Mitarbeiter und Person.

| Information | Person | Kunde | Mitarbeiter | Lernender |
|-------------|:------:|:-----:|:-----------:|:---------:|
| Anrede | ✓ | ✓ | ✓ | ✓ |
| Vorname | ✓ | ✓ | ✓ | ✓ |
| Nachname | ✓ | ✓ | ✓ | ✓ |
| Geburtsdatum | ✓ | ✓ | ✓ | ✓ |
| Geschlecht | ✓ | ✓ | ✓ | ✓ |
| Berufsbezeichnung | ✓ | ✓ | ✓ | ✓ |
| Telefon Gesch. | ✓ | ✓ | ✓ | ✓ |
| Mobil Tel | ✓ | ✓ | ✓ | ✓ |
| Email | ✓ | ✓ | ✓ | ✓ |
| Status (aktiv/passiv) | ✓ | ✓ | ✓ | ✓ |
| Firma | | ✓ | | |
| Mitarb.Nr | | | ✓ | ✓ |
| Abteilung | | | ✓ | ✓ |
| AHV Nr | | | ✓ | ✓ |
| Wohnort | | | ✓ | ✓ |
| Nationalität | | | ✓ | ✓ |
| Addresse | | | ✓ | ✓ |
| PLZ | | | ✓ | ✓ |
| Eintritt- & Austrittsdatum | | | ✓ | ✓ |
| Beschäftigungsgrad | | | ✓ | ✓ |
| Kaderstufe (0-5) | | | ✓ | ✓ |
| Lehrjahre | | | | ✓ |
| Aktuelles Lehrjahr | | | | ✓ |

### Funktional

| Bereich | Anforderung |
|---------|-------------|
| Allgemein | Erfassung und Mutieren von Daten |
| Allgemein | Personen Aktivieren & Deaktivieren |
| Allgemein | Personen Löschen |
| Allgemein | Suchmöglichkeiten über gespeicherte Infos |
| Allgemein | Auto. Speichern/Laden des Datenstamms auf Festplatte (JSON / CSV Datei / ähnliches?) |
| Kunden | Protokollieren Notizen in Kundenkontakt inkl. Historie |
| Mitarbeiter | Automatische Vergabe von Mitarb.Nr |

### Optional Funktional (Mind. 1 umgesetzt)

| Anforderung | Info / Ideen |
|-------------|--------------|
| Mutationshistorie von Kontaktdaten | append in Log Datei & anzeigen lassen? |
| Login | ein paar Accounts im JSON? |
| Sinnvolles Dashboard-View | |
| Import von Kontakten im CSV oder VCard-Format | Beispiel-Daten/Dateien beilegen |

### Nicht-Funktional

- Umsetzung C#, .NET, WindowsForms
- Durchdachte Applikationsarchitektur (Vererbungshierarchie der Daten ist Pflicht)
- Gute Benutzbarkeit
- Hohe Stabilität (Fehleingaben abfangen, Abstürze verhindern)
- Ausreichende In-Line Dokumentation (Public Classes, Methods, Properties, etc.)

### Abgabe

- Abgabefrist: 20.09.2026 @ 23:00!!
- Auf GitHub laden. Teamleiter sorgt dafür, dass Dozent den Link erhält (Email in Modulübersicht)
- Keine Commits nach Abgabezeitpunkt -> Ganze Projekt mit Note 1 bewertet
- Projekt enthält eine TXT-Datei mit:
  - Gruppenmitglieder, Vorname & Nachname
  - Beschreibung was funktioniert und was nicht
  - Zusatzinformationen wie Login-Accounts, Import-Dateien, etc.

#### Freiwillig

- Arbeitsjournal führen (muss nicht abgegeben werden)
