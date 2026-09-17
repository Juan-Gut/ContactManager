# Beispiel-Dateien für den Kontaktimport

Der Import ist über die jederzeit sichtbare Schaltfläche **Import contacts…** oberhalb der Registerkarten (`Ctrl+I`) erreichbar. Dort können CSV- und vCard-Dateien ausgewählt werden. Vor dem Speichern zeigt die Anwendung alle gültigen Kontakte sowie Warnungen und Fehler. Fehlerhafte Zeilen bzw. Karten werden übersprungen; gültige Kontakte werden erst nach der Bestätigung gemeinsam gespeichert. Kontakte mit einer E-Mail-Adresse oder AHV-Nummer, die bereits gespeichert ist oder vorher in derselben Datei vorkommt, werden als mögliche Duplikate markiert und standardmässig abgewählt. Bei Bedarf lassen sie sich bewusst wieder auswählen.

## Dateien

- `contacts-valid.csv`: gültige Beispiele für Kunde, Mitarbeiter und Lernende; Semikolon als Trennzeichen (Excel-kompatibel).
- `contacts-valid.vcf`: dieselben Kontaktarten als vCard 4.0; mit anwendungsspezifischen `X-CONTACTMANAGER-*`-Feldern.
- `contacts-with-errors.csv`: ein gültiger und drei absichtlich fehlerhafte Datensätze zum Prüfen der Fehleranzeige und des Teilimports.
