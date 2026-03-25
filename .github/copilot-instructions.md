# MIACopilot - Project Guidelines

## Project Overview
A C# console application for managing apprenticeships (Lernenden-Management-System) using .NET 10.
The application must include the following entities: `Apprentice`, `WorkJournal`, `Company`, and `VocationalTrainer`.

## Architecture & Data Persistence
- Use an in-memory list system for runtime data management.
- Data must be saved locally as a JSON file and automatically loaded when the application starts. 
- Create an `AppData` wrapper class containing lists of all entities to serialize/deserialize them into a single `app_data.json` file.
- The UI is a menu-driven console application with a clear layout (using loops and switch-cases).

## Code Conventions
- **No Emojis:** Do NOT use any emojis in the code, comments, or console output strings.
- **Comments:** Every method must have a short, descriptive comment explaining what it does.
- **Error Handling:** Implement basic error handling (e.g., using `TryParse` for numbers, checking for null values) to prevent crashes on invalid user inputs.
- **CRUD Operations:** Every domain class must have functions to Create, Read, Update, and Delete records.
- **Language:** Variable names, classes, and methods should be in English. Console outputs and user prompts should be in German.