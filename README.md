# MIACopilot

MIACopilot ist ein Lernenden-Management-System als C#-Anwendung mit Console-Menue und WPF-GUI.
Die App verwaltet zentrale Daten fuer den Ausbildungsalltag und speichert alles lokal in einer JSON-Datei.

## Projektziel

Das Projekt dient zur einfachen Verwaltung von:

- Lernenden (Apprentice)
- Firmen (Company)
- Berufsbildnern (VocationalTrainer)
- Arbeitsjournalen (WorkJournal)
- Noten (Grade)

Alle Daten werden zur Laufzeit in In-Memory-Listen gehalten und beim Speichern in eine gemeinsame Datei geschrieben.

## Funktionen

### 1) CRUD fuer alle Hauptobjekte

Fuer jede Domänenklasse stehen grundlegende Create-, Read-, Update- und Delete-Operationen zur Verfuegung.

### 2) Console-Menue

Beim Start oeffnet die Anwendung ein Menue mit folgenden Bereichen:

- Lernende verwalten
- Firmen verwalten
- Berufsbildner verwalten
- Arbeitsjournale verwalten
- Suchen und Filtern
- GUI oeffnen
- Noten verwalten

### 3) WPF-GUI

Die GUI bietet eine tab-aehnliche Navigation mit separaten Verwaltungsansichten fuer:

- Lernende
- Firmen
- Berufsbildner
- Arbeitsjournale
- Noten

Jede Ansicht hat Suchfeld, Datagrid und Aktionsbuttons (Hinzufuegen, Bearbeiten, Loeschen).

### 4) Persistenz

Die Daten werden in einer lokalen Datei gespeichert:

- `app_data.json`

Beim Programmstart werden vorhandene Daten geladen. Nach Aenderungen werden sie wieder gespeichert.

## Technischer Aufbau

- Sprache: C#
- Framework: .NET 10 (Windows, WPF)
- Projekttyp: Konsolenanwendung mit WPF-Fenster
- Datenhaltung:
	- Laufzeit: In-Memory (Listen)
	- Speicherung: JSON-Datei
- Zentraler Datentraeger: AppData (enthaelt Listen aller Entitaeten)

## Voraussetzungen

- Windows
- .NET SDK 10.0 oder neuer

## Projekt starten

Im Projektordner:

```powershell
dotnet restore
dotnet build MIACopilot.slnx
dotnet run --project MIACopilotApp
```

## Bedienung (kurz)

1. Anwendung starten.
2. Im Console-Menue einen Bereich waehlen.
3. Eintraege erfassen, aendern oder loeschen.
4. Optional die GUI ueber den Menuepunkt oeffnen.
5. Beim Speichern werden Daten in `app_data.json` persistiert.

## Datenmodell

Die wichtigsten Beziehungen:

- Ein Lernender kann einer Firma zugeordnet sein.
- Ein Lernender kann einem Berufsbildner zugeordnet sein.
- Arbeitsjournale gehoeren zu einem Lernenden.
- Noten gehoeren zu einem Lernenden.

## Hinweise

- Die Anwendung verwendet grundlegende Eingabepruefungen (z. B. TryParse bei numerischen Werten).
- Bei fehlender oder fehlerhafter JSON-Datei startet die App mit leeren Listen.

## Erweiterungsideen

- Exportfunktionen (CSV/PDF)
- Erweiterte Validierung und Plausibilitaetspruefungen
- Filter nach Zeitraum fuer Journale und Noten
- Rollen/Rechtesystem fuer unterschiedliche Benutzergruppen