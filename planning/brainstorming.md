# CONTACT MANAGER

This file is just for me to brainstorm at the start of our project. Things will change throughout the project, and as
such, it is not part of the final documentation.

## Arbeitvorgehen:

### Workspace:

#### GitHub: Repo

- Work on feature branches
    - no merge to main
    - code reviews
- Wiki for Hand-ins
    - documentation (alongside comments on functions, etc)
        - architecture documentation?
    - description of functional/broken features
    - other information as necessary

#### Jira: Task Management

- Parts of app are divided into "epics" (UI epic, data definition epic, customer mgmt epic, employee mgmt epic, etc)
- "epics" are divided into stories or tasks (Customer dashboard, new employee data handling, etc)
- tasks can have subtasks for further separation of work

### Arbeitszeiten

- wönd ihr scho jetzt ahfange? (damit mir kei stress hend)
- Individuel mit status update Mon, evtl Online Meeting
- 4 PF Lektionen Aug/Sept (evtl chönd mir vor ort zämme schaffe
- Ferien
  - Mika: 25.06 - 20.07, 01.08 - 09.08
  - Khaled: 13.07 - 02.08

### Sonstiges

- English -> Ask JP if it's fine
- journal (optional)
- Khaled Git explanation + practice
- GitHub Copilot

## APPLICATION:

### Architecture

#### Model-View-Presenter

- View (WinForms) handle UI events & display data
  (Multiple WinForm views, switch depending on context)
- Presenter contains logic & communication between View & Model

#### Data Definition

#### Data Storage on User's PC

- config for save file location and other stuff
- JSON file for storage ?? 

### Code Guidelines

- C# recommended naming convention
- WinForms elements: Semantic Naming (SaveButton, CancelButton, CustomerNameInput), not ZbW naming (btnSave, btnCancel)
- Exception handling is not optional (NF Anf)

#### Clean code strategy

- .editorconfig for code rules & formatting
- Roslyn Analyzers & StyleCop
    - force methods to have XML doc (NF Anf)
- dotnet format for automation ??
- Prettier for non c# files
- automate clean-up scripts
    1. fail github pipeline if unclean
    2. GitHub cleans up code automatically after push
    3. Husky.NET for local cleanup before push

## Questions / Uncertainties

### Anforderungen

#### Funktionale

#### Nicht funktionale

- Wa ish gmeint mit "Verebrungshierarchie der Daten ist Pflicht"?

## Next Steps:

1. Workspace setup, architecture plan, UI design, Git practice (Khaled), requirements list
2. Task handout, data definition plan
3. Coding ??
