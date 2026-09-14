# Contact Manager

Windows-Forms-Anwendung zur Verwaltung von **Kunden, Mitarbeitenden und Lernenden** — inklusive
Kontaktjournal, Mutationshistorie, Dashboard und Kontaktimport aus CSV/vCard.

Semesterprojekt im Modul *Programming Foundation II* (ZbW).

| | |
|---|---|
| Sprache / Framework | C# / .NET 10 (`net10.0-windows`) |
| Oberfläche | Windows Forms |
| Architektur | 4 Projekte: Models · Data · Logic · UI |
| Persistenz | JSON-Datei im lokalen App-Data-Verzeichnis |
| Login | `admin` / `password` |

---

## Schnellstart

**Voraussetzungen:** Windows, .NET 10 SDK, Visual Studio 2022+ oder JetBrains Rider.

```bash
git clone https://github.com/Juan-Gut/ContactManager.git
cd ContactManager
dotnet run --project ContactManager.UI
```

Alternativ `ContactManager.sln` in der IDE öffnen, `ContactManager.UI` als Startprojekt setzen und
mit F5 starten.

### Anmeldung

Beim Start erscheint ein Login-Dialog. Zugangsdaten des Demo-Benutzers:

```
Benutzername: admin
Passwort:     password
```

### Wo liegen die Daten?

Der Datenstamm wird automatisch geladen und nach jeder Änderung gespeichert:

```
%LOCALAPPDATA%\ContactManager\contacts.json
```

Geschrieben wird zuerst in eine temporäre Datei, die anschliessend an ihren Platz verschoben wird —
ein Absturz mitten im Speichern kann die bestehenden Daten daher nicht beschädigen. Eine unlesbare
Datei wird nach `contacts.json.corrupt` weggesichert, damit die Anwendung trotzdem startet.

Zum Zurücksetzen auf einen leeren Stand genügt es, `contacts.json` zu löschen.

---

## Funktionsumfang

### Pflichtanforderungen

| Anforderung | Umsetzung |
|---|---|
| Erfassen von Mitarbeitenden und Kunden | Registerkarten **Customers** und **Employees**, Schaltfläche *Create* |
| Mutieren | *Edit* → Felder ändern → *Save* (oder *Cancel*) |
| Aktivieren / Deaktivieren | Kontrollkästchen *Active* im Detailbereich, Spalte *Status* in der Liste |
| Löschen | *Delete* mit Rückfrage |
| Automatische Mitarbeiternummer | `EmployeeNrGenerator` vergibt beim Anlegen die nächste freie Nummer |
| Kontaktjournal inkl. Historie | *View notes* bei Kunden: Notiz erfassen, alle Einträge mit Zeitstempel einsehen |
| Suche nach Name, Vorname und Geburtsdatum | Suchfeld über jeder Liste, siehe unten |
| Automatisches Speichern und Laden | JSON-Datei, siehe oben |

### Umgesetzte optionale Anforderungen

- **Login** — vorgelagerter Anmeldedialog mit gehashtem Passwort.
- **Mutationshistorie** — jede Änderung wird protokolliert und ist über *View history* pro Kontakt
  einsehbar (Zeitpunkt und Aktion, ohne Klartext-Inhalte).
- **Dashboard** — Anzahl Kunden, Mitarbeitende und aktive Kontakte, Geburtstage der nächsten
  30 Tage sowie Verträge, die innerhalb von sechs Monaten enden.
- **Import** — CSV und vCard 3.0/4.0 mit Vorschau, Duplikatwarnung und Teilimport.

---

## Bedienung

### Registerkarten

| Tab | Inhalt |
|---|---|
| **Dashboard** | Kennzahlen, bevorstehende Geburtstage, auslaufende Verträge |
| **Customers** | Kundenliste links, Detailformular rechts |
| **Employees** | Mitarbeitende und Lernende, Umschaltung über die Auswahl *Employee / Apprentice* |

Die Detailfelder sind ausserhalb des Bearbeitungsmodus gesperrt; *Edit* schaltet sie frei, *Save*
validiert und speichert, *Cancel* verwirft. Fehleingaben werden abgefangen und als Meldung erklärt,
statt die Anwendung zu beenden.

### Suche

Über jeder Liste liegt ein Suchfeld (*Search customers…* / *Search employees…*). Die Liste wird
bereits beim Tippen gefiltert, Gross-/Kleinschreibung spielt keine Rolle. Gesucht wird — wie im
Anforderungsblatt verlangt — über **Name, Vorname und Geburtsdatum**:

| Eingabe | Findet |
|---|---|
| `Muster` | alle Kontakte mit diesem Nachnamen |
| `Hans` | alle Kontakte mit diesem Vornamen |
| `Hans Muster` bzw. `Muster Hans` | den Kontakt über den vollständigen Namen |
| `01.01.2000` | alle Kontakte mit diesem Geburtsdatum |
| `1.1.2000` | dasselbe, ohne führende Nullen |
| `2000` | alle Geburtstage in diesem Jahr (Teiltreffer) |

Das Datum wird im Format `dd.MM.yyyy` gesucht, nicht im ISO-Format. Ein leeres Suchfeld zeigt
wieder alle Einträge. Der Filter bleibt auch nach Speichern oder Löschen aktiv.

Kunden und Mitarbeitende werden über die jeweilige Registerkarte getrennt: Das Kundensuchfeld
durchsucht nur Kunden, das Mitarbeitersuchfeld nur Mitarbeitende und Lernende.

### Kontaktimport

Die Schaltfläche **Import contacts…** über den Registerkarten (`Ctrl+I`) liest CSV- und
vCard-Dateien ein. Vor dem Speichern zeigt eine Vorschau alle gültigen Kontakte sowie verständliche,
auf die jeweilige Zeile bzw. Karte bezogene Fehler. Mögliche Duplikate — gleiche E-Mail-Adresse oder
AHV-Nummer — werden gewarnt und zunächst abgewählt; sie lassen sich nach bewusster Prüfung trotzdem
importieren.

Beispieldateien und das vollständige unterstützte Schema: [`test-data/import`](test-data/import/README.md).

---

## Projektstruktur

```
ContactManager.sln
├── ContactManager.Models   Datenmodell und Enums (keine Abhängigkeiten)
├── ContactManager.Data     Persistenz: IContactRepository, FileRepository (JSON)
├── ContactManager.Logic    Fachlogik: PersonManager, ValidationService,
│                           EmployeeNrGenerator, AuthenticationService,
│                           ContactImportService, ContactDuplicateDetector
├── ContactManager.UI       Windows Forms: LoginForm, MainForm (partial),
│                           ImportPreviewForm
├── planning/               Anforderungen, Use Cases, Architekturvorgaben
└── test-data/              Beispieldaten und Importdateien
```

Die Abhängigkeiten zeigen nur in eine Richtung: **UI → Logic → Data → Models**. Die UI kennt keine
Dateizugriffe, die Logik keine Formulare.

### Vererbungshierarchie

```
Person
├── Customer      + Firma, Kontaktjournal
└── Employee      + Mitarbeiternummer, Abteilung, AHV-Nr., Adresse,
    │               Ein-/Austritt, Beschäftigungsgrad, Kaderstufe
    └── Apprentice  + Lehrjahre, aktuelles Lehrjahr
```

`MainForm` ist bewusst auf mehrere `partial`-Dateien aufgeteilt, damit jede Datei einen Zweck hat:
`Navigation`, `DataPresentation`, `Editors`, `EditorState`, `ContactHistory`, `Import`, `ViewModels`
und die generierte `Designer`-Datei.

---

## Bekannte Einschränkungen

- Nur ein Benutzerkonto (Demo-Login), keine Rollen oder Rechteverwaltung.
- Die Oberfläche ist auf Englisch beschriftet, die Dokumentation auf Deutsch.
- Der Datenstamm liegt lokal pro Windows-Benutzer; es gibt keine Mehrplatzfähigkeit.
- Die Mutationshistorie protokolliert die Aktion, nicht die einzelnen Feldwerte vorher/nachher.
- Die Suche ist bewusst auf Name, Vorname und Geburtsdatum beschränkt; nach E-Mail, Firma oder
  Mitarbeiternummer lässt sich nicht suchen.

---

## Team und Abgabe

| | |
|---|---|
| Gruppenmitglieder | Juan Gutierrez Rangel, Mika Hänsenberger, Khaled Al Akoub |
| Teamleitung | Juan Gutierrez Rangel |
| Repository | https://github.com/Juan-Gut/ContactManager |
| Abgabefrist | 20.09.2026, 23:00 Uhr |
