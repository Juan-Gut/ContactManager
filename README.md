# Contact Manager

Windows-Forms-Anwendung zur Verwaltung von **Kunden, Mitarbeitenden und Lernenden**.

Semesterprojekt im Modul *Programming Foundation II* (ZbW).

|                     |                                            |
|---------------------|--------------------------------------------|
| Sprache / Framework | C# / .NET 10 (`net10.0-windows`)           |
| Oberfläche          | Windows Forms                              |
| Architektur         | 4 Projekte: Models · Data · Logic · UI     |
| Persistenz          | JSON-Datei im lokalen App-Data-Verzeichnis |

---

## Team

|                   |                                                                 |
|-------------------|-----------------------------------------------------------------|
| Gruppenmitglieder | Juan Gutierrez Rangel<br/>Mika Hänsenberger<br/>Khaled Al Akoub |
| Teamleitung       | Juan Gutierrez Rangel                                           |
| Repository        | https://github.com/Juan-Gut/ContactManager                      |

---

### Anmeldung

Zugangsdaten des Benutzers:

```
Benutzername: admin
Passwort:     password
```

### Daten Speicherort

```
%LOCALAPPDATA%\ContactManager\contacts.json

Wird auf contacts.json.corrupt umbenannt, wenn die Datei beschädigt ist und nicht geladen werden kann.
```

---

## Funktionsumfang

### Pflichtanforderungen

| Anforderung                               | Funktioniert |
|-------------------------------------------|--------------|
| Erfassen von Mitarbeitenden und Kunden    | Ja           |
| Mutieren                                  | Ja           |
| Aktivieren / Deaktivieren                 | Ja           |
| Löschen                                   | Ja           |
| Automatische Mitarbeiternummer            | Ja           |
| Kontaktjournal                            | Ja           |
| Suche                                     | Ja           |
| Speichern und Laden auf Festplatte (JSON) | Ja           |

#### **Anmerkungen:**

**Suche:**
Die Suche ist bewusst auf die folgenden Felder beschränkt.

- First name
- Surname
- Date of birth
- Job title
- Email
- Business & mobile number
- Status
- Company (für Kunden)
- Department (für Mitarbeitende)
- AHV number (für Mitarbeitende)
- Nationality (für Mitarbeitende)
- Address (für Mitarbeitende)
- City (für Mitarbeitende)
- PLZ (für Mitarbeitende)

Andere Felder wie z.B. Gender würden Konflikte bei der Suche verursachen (Eine String Suche nach "Male" würde auch "
Female" finden oder auch Näme wie "Malene").
Die fehlende Felder könnten in der Zukunft als separate Filter Funktion implementiert werden.

**Kaderstufen:**
Wir haben uns auf die folgenden Kaderstufen entschieden:

- Team Leader
- Department Manager
- Senior Manager
- Executive Manager

Die Kaderstufen als Zahlen 0-5 hat für uns nicht viel Sinn gemacht, daher haben wir uns auf echte Bezeichnungen
entschieden.

Dies haben wir mit Jean-Pierre besprochen und er hat uns bestätigt, dass dies in Ordnung ist.

### Optionale Anforderungen

| Anforderung       | Funktioniert |
|-------------------|--------------|
| Login             | Ja           |
| Mutationshistorie | Ja           |
| Dashboard         | Ja           |
| Import            | Ja           |

#### **Anmerkungen:**

- Mutationshistorie: Wir haben uns entschieden, die Mutationshistorie auf die Aktion zu beschränken (z. B. `Customer
  created`), anstatt die einzelnen Feldwerte vorher/nachher zu protokollieren. Das wäre für uns zu verbose.
- Dashboard: Da haben wir uns entschieden, wichtige Metrics und hilfreiche Kontaktinformationen anzuzeigen.
- Import: Beispieldaten gibt es unter `test-data/import`. Zusätzlich gibt es dort eine README-Datei mit mehr
  Informationen.

---

## Sonstige Gruppenentscheidungen

- Nur ein Benutzerkonto implementiert. 
- Das ganze Code sowie XML Kommentare und UI Elemente sind auf Englisch geschrieben.
- Die Mutationshistorie protokolliert die Aktion, nicht die einzelnen Feldwerte vorher/nachher.
- Die Suche ist bewusst auf gewisse Felder beschränkt.
