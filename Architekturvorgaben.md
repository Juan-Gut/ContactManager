# Projektstruktur: Contact Manager

Unsere Visual Studio Solution (`ContactManager.sln`) ist in vier separate Projekte aufgeteilt, um eine saubere 3-Schichten-Architektur abzubilden.

Hier ist die Übersicht des Dateisystems:

```text
ContactManager/
├── ContactManager.sln                 
├── ARCHITECTURE.md                    
├── .gitignore                         
│
├── ContactManager.Models/             # ---> DATENSTRUKTUR (ohne Logik)
│   ├── ContactManager.Models.csproj
│   ├── Person.cs                      # Basisklasse
│   ├── Kunde.cs                       # Erbt von Person
│   └── Mitarbeiter.cs                 # Erbt von Person
│
├── ContactManager.Data/               # ---> DATENZUGRIFF (Nur File I/O!)
│   ├── ContactManager.Data.csproj
│   └── FileRepository.cs              # Automatisches Speichern/Laden auf Festplatte
│
├── ContactManager.Logic/              # ---> GESCHÄFTSLOGIK 
│   ├── ContactManager.Logic.csproj
│   ├── PersonenManager.cs             # CRUD-Operationen & Suchlogik
│   ├── MitarbeiterNummerGenerator.cs  # Automatische Nummernvergabe
│   └── ValidationService.cs           # Prüft Eingaben (Fehler abfangen)
│
└── ContactManager.UI/ (Windows Forms) # ---> BENUTZEROBERFLÄCHE 
    ├── ContactManager.UI.csproj
    ├── Program.cs                     # Einstiegspunkt der Applikation
    ├── DashboardForm.cs               # Hauptfenster / Übersicht
    ├── DashboardForm.Designer.cs
    ├── PersonDetailForm.cs            # Fenster für Mutationen
    └── PersonDetailForm.Designer.cs

 ```

 ## Abhängigkeiten

Damit die Schichten korrekt miteinander kommunizieren, ohne sich zu vermischen, gelten folgende Referenzen innerhalb der Projekte:

- UI hat eine Referenz auf Logic und Models.
- Logic hat eine Referenz auf Data und Models.
- Data hat eine Referenz auf Models.
- Models hat keine Referenzen auf andere Projekte.