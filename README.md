# ContactManager

Eine Windows-Forms-Anwendung zur Verwaltung von Kunden, Mitarbeitern und Lernenden.

## Kontaktimport

Über die jederzeit sichtbare Schaltfläche **Import contacts…** oberhalb der Registerkarten (`Ctrl+I`) lassen sich Kontakte aus CSV- und vCard-Dateien einlesen. Eine Vorschau zeigt vor dem Speichern alle gültigen Kontakte und verständliche, auf die jeweilige CSV-Zeile oder vCard bezogene Probleme. Mögliche Duplikate mit derselben E-Mail-Adresse oder AHV-Nummer werden gewarnt und zunächst nicht ausgewählt; sie können nach bewusster Prüfung trotzdem importiert werden.

Beispieldateien sowie das vollständige unterstützte Schema befinden sich unter [`test-data/import`](test-data/import/README.md).
