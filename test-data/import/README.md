# Beispiel-Dateien für den Kontaktimport

Der Import befindet sich global unter **File > Import contacts…** (`Ctrl+I`). Dort können CSV- und vCard-Dateien ausgewählt werden. Vor dem Speichern zeigt die Anwendung alle gültigen Kontakte sowie Warnungen und Fehler. Fehlerhafte Zeilen bzw. Karten werden übersprungen; gültige Kontakte werden erst nach der Bestätigung gemeinsam gespeichert.

## Dateien

- `contacts-valid.csv`: gültige Beispiele für Kunde, Mitarbeiter und Lernende; Semikolon als Trennzeichen (Excel-kompatibel).
- `contacts-valid.vcf`: dieselben Kontaktarten als vCard 4.0; mit anwendungsspezifischen `X-CONTACTMANAGER-*`-Feldern.
- `contacts-with-errors.csv`: ein gültiger und drei absichtlich fehlerhafte Datensätze zum Prüfen der Fehleranzeige und des Teilimports.

## Unterstützte Formate und Datentypen

CSV unterstützt Komma oder Semikolon als Trennzeichen, UTF-8, eine Kopfzeile sowie in Anführungszeichen gesetzte oder mehrzeilige Werte. Die Spaltennamen sind nicht gross-/kleinschreibungsabhängig. Pflichtspalten sind `Type`, `FirstName`, `LastName` und `DateOfBirth`. Importdateien sind aus Stabilitätsgründen auf 5 MB begrenzt.

| Datentyp | Eingabe |
|---|---|
| Kontaktart | `Customer`, `Employee`, `Apprentice` (auch `Kunde`, `Mitarbeiter`, `Lernender`) |
| Datum | `yyyy-MM-dd` oder `dd.MM.yyyy`; in vCard zusätzlich `yyyyMMdd` |
| Boolean | `true`/`false`, `yes`/`no`, `1`/`0`, `active`/`inactive` |
| Ganzzahl | Ziffern ohne Dezimalstellen, z. B. `80` |
| Aufzählungen | Namen aus dem Modell, ohne Beachtung der Gross-/Kleinschreibung |
| Text | UTF-8-Text; in CSV bei Trennzeichen, Zeilenumbruch oder `"` in doppelte Anführungszeichen setzen |

Unterstützte CSV-Spalten:

```text
Type, Title, FirstName, LastName, DateOfBirth, Gender, JobTitle,
BusinessNumber, MobileNumber, EmailAddress, IsActive, Company,
Department, AhvNumber, Nationality, City, Address, Plz,
EmploymentStartDate, EmploymentEndDate, EmploymentPercentage,
OfficeLocation, ManagementLevel, ApprenticeshipDuration,
CurrentApprenticeshipYear
```

Zulässige Aufzählungswerte:

- `Title`: `Unknown`, `Mr`, `Mrs`, `Ms`
- `Gender`: `Unknown`, `Male`, `Female`, `Other`
- `OfficeLocation`: `Unknown`, `Zurich`, `StGallen`, `Genf`
- `ManagementLevel`: `None`, `TeamLeader`, `DepartmentManager`, `SeniorManager`, `ExecutiveManager`

`EmployeeNumber`, `Id` und `CreatedAt` werden bewusst nicht importiert: Die Anwendung erzeugt diese Werte selbst. Leere optionale Werte erhalten die gleichen Standardwerte wie bei der manuellen Erfassung (`IsActive=true`, `EmploymentPercentage=100`, unbefristetes Austrittsdatum).

vCard 3.0 und 4.0 unterstützen die Standardfelder `N`, `FN`, `BDAY`, `TITLE`, `ORG`, `TEL`, `EMAIL` und `ADR`. Ohne `X-CONTACTMANAGER-TYPE` wird eine vCard als Kunde interpretiert. Für Mitarbeiter und Lernende werden die im gültigen Beispiel gezeigten `X-CONTACTMANAGER-*`-Felder benötigt. Die normalen Validierungsregeln der Anwendung gelten auch beim Import, insbesondere Firma bei Kunden, Geburtsdatum, Mindestalter und Beschäftigungsdaten.
