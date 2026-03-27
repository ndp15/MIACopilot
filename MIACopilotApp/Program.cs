using System;
using System.Linq;
using MIACopilotApp.Models;

namespace MIACopilotApp
{
    class Program
    {
        private static System.Windows.Application? _wpfApp;

        // Entry point for the console application
        [STAThread]
        static void Main(string[] args)
        {
            DataStore.LoadData();
            bool running = true;

            while (running)
            {
                Console.Clear();
                PrintConsoleTitle("LMS");
                Console.WriteLine(new string('=', 58));
                Console.WriteLine("1. Lernende verwalten");
                Console.WriteLine("2. Firmen verwalten");
                Console.WriteLine("3. Berufsbildner verwalten");
                Console.WriteLine("4. Arbeitsjournale verwalten");
                Console.WriteLine("5. Noten verwalten");
                Console.WriteLine("6. Suchen & Filtern");
                
                var prevColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("7. GUI öffnen");
                Console.ForegroundColor = prevColor;
                
                Console.WriteLine("8. Beenden");
                Console.Write("Bitte wählen Sie eine Option (1-8): ");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ManageApprentices();
                        break;
                    case "2":
                        ManageCompanies();
                        break;
                    case "3":
                        ManageVocationalTrainers();
                        break;
                    case "4":
                        ManageWorkJournals();
                        break;
                    case "5":
                        ManageGrades();
                        break;
                    case "6":
                        SearchAndFilter();
                        break;
                    case "7":
                        LaunchGui();
                        break;
                    case "8":
                        running = false;
                        Console.WriteLine("Programm wird beendet.");
                        break;
                    default:
                        Console.WriteLine("Ungültige Eingabe. Bitte versuchen Sie es erneut.");
                        break;
                }
            }
        }

        // Prints a pixel-like ASCII title with a 3D shadow block effect
        static void PrintConsoleTitle(string title)
        {
            var previousColor = Console.ForegroundColor;
            Console.WriteLine();

            string[] lernenden = {
                @"  ██╗     ███████╗██████╗ ███╗   ██╗███████╗███╗   ██╗██████╗ ███████╗███╗   ██╗",
                @"  ██║     ██╔════╝██╔══██╗████╗  ██║██╔════╝████╗  ██║██╔══██╗██╔════╝████╗  ██║",
                @"  ██║     █████╗  ██████╔╝██╔██╗ ██║█████╗  ██╔██╗ ██║██║  ██║█████╗  ██╔██╗ ██║",
                @"  ██║     ██╔══╝  ██╔══██╗██║╚██╗██║██╔══╝  ██║╚██╗██║██║  ██║██╔══╝  ██║╚██╗██║",
                @"  ███████╗███████╗██║  ██║██║ ╚████║███████╗██║ ╚████║██████╔╝███████╗██║ ╚████║",
                @"  ╚══════╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═══╝╚═════╝ ╚══════╝╚═╝  ╚═══╝"
            };

            string[] management = {
                @"    ███╗   ███╗ █████╗ ███╗   ██╗ █████╗  ██████╗ ███████╗███╗   ███╗███████╗███╗   ██╗████████╗",
                @"    ████╗ ████║██╔══██╗████╗  ██║██╔══██╗██╔════╝ ██╔════╝████╗ ████║██╔════╝████╗  ██║╚══██╔══╝",
                @"    ██╔████╔██║███████║██╔██╗ ██║███████║██║  ███╗█████╗  ██╔████╔██║█████╗  ██╔██╗ ██║   ██║   ",
                @"    ██║╚██╔╝██║██╔══██║██║╚██╗██║██╔══██║██║   ██║██╔══╝  ██║╚██╔╝██║██╔══╝  ██║╚██╗██║   ██║   ",
                @"    ██║ ╚═╝ ██║██║  ██║██║ ╚████║██║  ██║╚██████╔╝███████╗██║ ╚═╝ ██║███████╗██║ ╚████║   ██║   ",
                @"    ╚═╝     ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚═╝     ╚═╝╚══════╝╚═╝  ╚═══╝   ╚═╝   "
            };

            string[] system = {
                @"      ██████╗ ██╗   ██╗ ██████╗ ████████╗███████╗███╗   ███╗",
                @"     ██╔════╝ ╚██╗ ██╔╝██╔════╝ ╚══██╔══╝██╔════╝████╗ ████║",
                @"     ╚█████╗   ╚████╔╝ ╚█████╗     ██║   █████╗  ██╔████╔██║",
                @"      ╚═══██╗   ╚██╔╝   ╚═══██╗    ██║   ██╔══╝  ██║╚██╔╝██║",
                @"     ██████╔╝    ██║   ██████╔╝    ██║   ███████╗██║ ╚═╝ ██║",
                @"     ╚═════╝     ╚═╝   ╚═════╝     ╚═╝   ╚══════╝╚═╝     ╚═╝"
            };

            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (var line in lernenden) Console.WriteLine(line);
            
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            foreach (var line in management) Console.WriteLine(line);
            
            Console.ForegroundColor = ConsoleColor.Blue;
            foreach (var line in system) Console.WriteLine(line);

            Console.ForegroundColor = previousColor;
            Console.WriteLine();
        }

        // Opens the WPF GUI and reloads data after closing it
        static void LaunchGui()
        {
            if (System.Windows.Application.Current == null)
            {
                _wpfApp = new System.Windows.Application();
                _wpfApp.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
            }
            Console.WriteLine("GUI wird geöffnet...");
            new MIACopilotApp.Gui.MainWindow().ShowDialog();
            DataStore.LoadData();
            Console.WriteLine("GUI geschlossen. Daten wurden neu geladen.");
        }

        // Helper method to read an integer from console to avoid crashes
        static int ReadInt(string prompt)
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (int.TryParse(input, out result))
                {
                    return result;
                }
                Console.WriteLine("Ungültige Eingabe. Bitte geben Sie eine Zahl ein.");
            }
        }

        // Helper method to read an optional integer (for nullable IDs)
        static int? ReadOptionalInt(string prompt)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return null;
            if (int.TryParse(input, out int result)) return result;
            Console.WriteLine("Ungültige Eingabe. Wert wird als leer gespeichert.");
            return null;
        }

        // Helper method to read a date from console
        static DateTime ReadDate(string prompt)
        {
            DateTime result;
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (DateTime.TryParse(input, out result))
                {
                    return result;
                }
                Console.WriteLine("Ungültige Eingabe. Bitte geben Sie ein gültiges Datum ein.");
            }
        }

        // Manages the apprentice operations (CRUD)
        static void ManageApprentices()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n-- Lernende verwalten --");
                Console.WriteLine("1. Alle anzeigen");
                Console.WriteLine("2. Hinzufügen");
                Console.WriteLine("3. Bearbeiten");
                Console.WriteLine("4. Löschen");
                Console.WriteLine("5. Zurück zum Hauptmenü");
                Console.Write("Option: ");
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ReadApprentices();
                        break;
                    case "2":
                        CreateApprentice();
                        break;
                    case "3":
                        UpdateApprentice();
                        break;
                    case "4":
                        DeleteApprentice();
                        break;
                    case "5":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Ungültige Eingabe.");
                        break;
                }
            }
        }

        // Displays all apprentices
        static void ReadApprentices()
        {
            Console.WriteLine("\nAlle Lernenden:");
            if (!DataStore.Data.Apprentices.Any())
            {
                Console.WriteLine("Keine Lernenden gefunden.");
                return;
            }
            foreach (var a in DataStore.Data.Apprentices)
            {
                Console.WriteLine($"ID: {a.Id} | Name: {a.FirstName} {a.LastName} | Firma ID: {a.CompanyId} | Berufsbildner ID: {a.VocationalTrainerId}");
            }
        }

        // Creates a new apprentice
        static void CreateApprentice()
        {
            Console.Write("Vorname: ");
            string firstName = Console.ReadLine() ?? string.Empty;
            Console.Write("Nachname: ");
            string lastName = Console.ReadLine() ?? string.Empty;
            int? companyId = ReadOptionalInt("Firma ID (leer für keine): ");
            int? trainerId = ReadOptionalInt("Berufsbildner ID (leer für keine): ");

            var apprentice = new Apprentice 
            { 
                FirstName = firstName, 
                LastName = lastName,
                CompanyId = companyId,
                VocationalTrainerId = trainerId
            };
            Apprentice.Create(apprentice);
            Console.WriteLine("Lernender erfolgreich hinzugefügt.");
        }

        // Updates an existing apprentice
        static void UpdateApprentice()
        {
            int id = ReadInt("ID des zu bearbeitenden Lernenden: ");
            var apprentice = Apprentice.Read(id);
            if (apprentice == null)
            {
                Console.WriteLine("Lernender nicht gefunden.");
                return;
            }

            Console.Write($"Neuer Vorname (aktuell: {apprentice.FirstName}): ");
            string firstName = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(firstName)) apprentice.FirstName = firstName;

            Console.Write($"Neuer Nachname (aktuell: {apprentice.LastName}): ");
            string lastName = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(lastName)) apprentice.LastName = lastName;

            int? cId = ReadOptionalInt($"Neue Firma ID (aktuell: {apprentice.CompanyId}): ");
            if (cId.HasValue) apprentice.CompanyId = cId;

            int? tId = ReadOptionalInt($"Neue Berufsbildner ID (aktuell: {apprentice.VocationalTrainerId}): ");
            if (tId.HasValue) apprentice.VocationalTrainerId = tId;

            Apprentice.Update(apprentice);
            Console.WriteLine("Lernender erfolgreich aktualisiert.");
        }

        // Deletes an apprentice
        static void DeleteApprentice()
        {
            int id = ReadInt("ID des zu Löschenden Lernenden: ");
            Apprentice.Delete(id);
            Console.WriteLine("Lernender (falls vorhanden) gelöscht.");
        }

        // Manages the company operations (CRUD)
        static void ManageCompanies()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n-- Firmen verwalten --");
                Console.WriteLine("1. Alle anzeigen");
                Console.WriteLine("2. Hinzufügen");
                Console.WriteLine("3. Bearbeiten");
                Console.WriteLine("4. Löschen");
                Console.WriteLine("5. Zurück zum Hauptmenü");
                Console.Write("Option: ");
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ReadCompanies();
                        break;
                    case "2":
                        CreateCompany();
                        break;
                    case "3":
                        UpdateCompany();
                        break;
                    case "4":
                        DeleteCompany();
                        break;
                    case "5":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Ungültige Eingabe.");
                        break;
                }
            }
        }

        // Displays all companies
        static void ReadCompanies()
        {
            Console.WriteLine("\nAlle Firmen:");
            if (!DataStore.Data.Companies.Any())
            {
                Console.WriteLine("Keine Firmen gefunden.");
                return;
            }
            foreach (var c in DataStore.Data.Companies)
            {
                Console.WriteLine($"ID: {c.Id} | Name: {c.Name} | Adresse: {c.Address}");
            }
        }

        // Creates a new company
        static void CreateCompany()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.Write("Adresse: ");
            string address = Console.ReadLine() ?? string.Empty;

            var company = new Company { Name = name, Address = address };
            Company.Create(company);
            Console.WriteLine("Firma erfolgreich hinzugefügt.");
        }

        // Updates an existing company
        static void UpdateCompany()
        {
            int id = ReadInt("ID der zu bearbeitenden Firma: ");
            var company = Company.Read(id);
            if (company == null)
            {
                Console.WriteLine("Firma nicht gefunden.");
                return;
            }

            Console.Write($"Neuer Name (aktuell: {company.Name}): ");
            string name = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(name)) company.Name = name;

            Console.Write($"Neue Adresse (aktuell: {company.Address}): ");
            string address = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(address)) company.Address = address;

            Company.Update(company);
            Console.WriteLine("Firma erfolgreich aktualisiert.");
        }

        // Deletes a company
        static void DeleteCompany()
        {
            int id = ReadInt("ID der zu Löschenden Firma: ");
            Company.Delete(id);
            Console.WriteLine("Firma (falls vorhanden) gelöscht.");
        }

        // Manages the vocational trainer operations (CRUD)
        static void ManageVocationalTrainers()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n-- Berufsbildner verwalten --");
                Console.WriteLine("1. Alle anzeigen");
                Console.WriteLine("2. Hinzufügen");
                Console.WriteLine("3. Bearbeiten");
                Console.WriteLine("4. Löschen");
                Console.WriteLine("5. Zurück zum Hauptmenü");
                Console.Write("Option: ");
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ReadVocationalTrainers();
                        break;
                    case "2":
                        CreateVocationalTrainer();
                        break;
                    case "3":
                        UpdateVocationalTrainer();
                        break;
                    case "4":
                        DeleteVocationalTrainer();
                        break;
                    case "5":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Ungültige Eingabe.");
                        break;
                }
            }
        }

        // Displays all vocational trainers
        static void ReadVocationalTrainers()
        {
            Console.WriteLine("\nAlle Berufsbildner:");
            if (!DataStore.Data.VocationalTrainers.Any())
            {
                Console.WriteLine("Keine Berufsbildner gefunden.");
                return;
            }
            foreach (var t in DataStore.Data.VocationalTrainers)
            {
                Console.WriteLine($"ID: {t.Id} | Name: {t.FirstName} {t.LastName} | Firma ID: {t.CompanyId}");
            }
        }

        // Creates a new vocational trainer
        static void CreateVocationalTrainer()
        {
            Console.Write("Vorname: ");
            string firstName = Console.ReadLine() ?? string.Empty;
            Console.Write("Nachname: ");
            string lastName = Console.ReadLine() ?? string.Empty;
            int? companyId = ReadOptionalInt("Firma ID (leer für keine): ");

            var trainer = new VocationalTrainer 
            { 
                FirstName = firstName, 
                LastName = lastName, 
                CompanyId = companyId 
            };
            VocationalTrainer.Create(trainer);
            Console.WriteLine("Berufsbildner erfolgreich hinzugefügt.");
        }

        // Updates an existing vocational trainer
        static void UpdateVocationalTrainer()
        {
            int id = ReadInt("ID des zu bearbeitenden Berufsbildners: ");
            var trainer = VocationalTrainer.Read(id);
            if (trainer == null)
            {
                Console.WriteLine("Berufsbildner nicht gefunden.");
                return;
            }

            Console.Write($"Neuer Vorname (aktuell: {trainer.FirstName}): ");
            string firstName = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(firstName)) trainer.FirstName = firstName;

            Console.Write($"Neuer Nachname (aktuell: {trainer.LastName}): ");
            string lastName = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(lastName)) trainer.LastName = lastName;

            int? cId = ReadOptionalInt($"Neue Firma ID (aktuell: {trainer.CompanyId}): ");
            if (cId.HasValue) trainer.CompanyId = cId;

            VocationalTrainer.Update(trainer);
            Console.WriteLine("Berufsbildner erfolgreich aktualisiert.");
        }

        // Deletes a vocational trainer
        static void DeleteVocationalTrainer()
        {
            int id = ReadInt("ID des zu Löschenden Berufsbildners: ");
            VocationalTrainer.Delete(id);
            Console.WriteLine("Berufsbildner (falls vorhanden) gelöscht.");
        }

        // Manages the work journal operations (CRUD)
        static void ManageWorkJournals()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n-- Arbeitsjournale verwalten --");
                Console.WriteLine("1. Alle anzeigen");
                Console.WriteLine("2. Hinzufügen");
                Console.WriteLine("3. Bearbeiten");
                Console.WriteLine("4. Löschen");
                Console.WriteLine("5. Zurück zum Hauptmenü");
                Console.Write("Option: ");
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ReadWorkJournals();
                        break;
                    case "2":
                        CreateWorkJournal();
                        break;
                    case "3":
                        UpdateWorkJournal();
                        break;
                    case "4":
                        DeleteWorkJournal();
                        break;
                    case "5":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Ungültige Eingabe.");
                        break;
                }
            }
        }

        // Displays all work journals
        static void ReadWorkJournals()
        {
            Console.WriteLine("\nAlle Arbeitsjournale:");
            if (!DataStore.Data.WorkJournals.Any())
            {
                Console.WriteLine("Keine Arbeitsjournale gefunden.");
                return;
            }
            foreach (var j in DataStore.Data.WorkJournals)
            {
                Console.WriteLine($"ID: {j.Id} | Titel: {j.Title} | Datum: {j.DateEntry.ToShortDateString()} | Lernender ID: {j.ApprenticeId}");
            }
        }

        // Creates a new work journal
        static void CreateWorkJournal()
        {
            Console.Write("Titel: ");
            string title = Console.ReadLine() ?? string.Empty;
            Console.Write("Inhalt: ");
            string content = Console.ReadLine() ?? string.Empty;
            DateTime date = ReadDate("Datum (z.B. 2026-03-25): ");
            int apprenticeId = ReadInt("Lernender ID: ");

            var journal = new WorkJournal 
            { 
                Title = title, 
                Content = content, 
                DateEntry = date, 
                ApprenticeId = apprenticeId 
            };
            WorkJournal.Create(journal);
            Console.WriteLine("Arbeitsjournal erfolgreich hinzugefügt.");
        }

        // Updates an existing work journal
        static void UpdateWorkJournal()
        {
            int id = ReadInt("ID des zu bearbeitenden Arbeitsjournals: ");
            var journal = WorkJournal.Read(id);
            if (journal == null)
            {
                Console.WriteLine("Arbeitsjournal nicht gefunden.");
                return;
            }

            Console.Write($"Neuer Titel (aktuell: {journal.Title}): ");
            string title = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(title)) journal.Title = title;

            Console.Write($"Neuer Inhalt (aktuell: {journal.Content}): ");
            string content = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(content)) journal.Content = content;

            Console.Write($"Neues Datum (aktuell: {journal.DateEntry.ToShortDateString()} - leer lassen zum Beibehalten): ");
            string dateInput = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(dateInput) && DateTime.TryParse(dateInput, out DateTime newDate))
            {
                journal.DateEntry = newDate;
            }

            int? newAId = ReadOptionalInt($"Neue Lernender ID (aktuell: {journal.ApprenticeId}): ");
            if (newAId.HasValue) journal.ApprenticeId = newAId.Value;

            WorkJournal.Update(journal);
            Console.WriteLine("Arbeitsjournal erfolgreich aktualisiert.");
        }

        // Deletes a work journal
        static void DeleteWorkJournal()
        {
            int id = ReadInt("ID des zu Löschenden Arbeitsjournals: ");
            WorkJournal.Delete(id);
            Console.WriteLine("Arbeitsjournal (falls vorhanden) gelöscht.");
        }

        // Manages search and filter operations
        static void SearchAndFilter()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n-- Suchen & Filtern --");
                Console.WriteLine("1. Lernenden suchen (Name)");
                Console.WriteLine("2. Lernende nach Firma filtern");
                Console.WriteLine("3. Zurück zum Hauptmenü");
                Console.Write("Option: ");
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        SearchApprenticeByName();
                        break;
                    case "2":
                        FilterApprenticeByCompany();
                        break;
                    case "3":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Ungültige Eingabe.");
                        break;
                }
            }
        }

        // Searches for an apprentice by name
        static void SearchApprenticeByName()
        {
            Console.Write("Bitte Suchbegriff (Vor- oder Nachname) eingeben: ");
            string query = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(query))
            {
                Console.WriteLine("Der Suchbegriff darf nicht leer sein.");
                return;
            }

            var results = DataStore.Data.Apprentices
                .Where(a => a.FirstName.Contains(query, StringComparison.OrdinalIgnoreCase) || 
                            a.LastName.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Console.WriteLine("\nSuchergebnisse:");
            if (!results.Any())
            {
                Console.WriteLine("Keine Lernenden gefunden, die diesem Suchbegriff entsprechen.");
                return;
            }

            foreach (var a in results)
            {
                Console.WriteLine($"ID: {a.Id} | Name: {a.FirstName} {a.LastName} | Firma ID: {(a.CompanyId.HasValue ? a.CompanyId.Value.ToString() : "Keine")} | Berufsbildner ID: {(a.VocationalTrainerId.HasValue ? a.VocationalTrainerId.Value.ToString() : "Keine")}");
            }
        }

        // Filters apprentices by company
        static void FilterApprenticeByCompany()
        {
            int companyId = ReadInt("Bitte die ID der Firma eingeben: ");

            var results = DataStore.Data.Apprentices
                .Where(a => a.CompanyId == companyId)
                .ToList();

            Console.WriteLine($"\nLernende, die der Firma {companyId} zugewiesen sind:");
            if (!results.Any())
            {
                Console.WriteLine("Keine Lernenden für diese Firma gefunden.");
                return;
            }

            foreach (var a in results)
            {
                Console.WriteLine($"ID: {a.Id} | Name: {a.FirstName} {a.LastName} | Firma ID: {a.CompanyId} | Berufsbildner ID: {(a.VocationalTrainerId.HasValue ? a.VocationalTrainerId.Value.ToString() : "Keine")}");
            }
        }

        // Manages grade operations (CRUD)
        static void ManageGrades()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n-- Noten verwalten --");
                Console.WriteLine("1. Alle anzeigen");
                Console.WriteLine("2. Note hinzufügen");
                Console.WriteLine("3. Note bearbeiten");
                Console.WriteLine("4. Note löschen");
                Console.WriteLine("5. Zurück zum Hauptmenü");
                Console.Write("Option: ");
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1": ReadGrades();   break;
                    case "2": CreateGrade();  break;
                    case "3": UpdateGrade();  break;
                    case "4": DeleteGrade();  break;
                    case "5": back = true;    break;
                    default: Console.WriteLine("Ungültige Eingabe."); break;
                }
            }
        }

        // Displays all grades with apprentice names
        static void ReadGrades()
        {
            Console.WriteLine("\nAlle Noten:");
            if (!DataStore.Data.Grades.Any())
            {
                Console.WriteLine("Keine Noten gefunden.");
                return;
            }
            foreach (var g in DataStore.Data.Grades)
            {
                var a = DataStore.Data.Apprentices.FirstOrDefault(x => x.Id == g.ApprenticeId);
                string name = a != null ? $"{a.FirstName} {a.LastName}" : $"ID {g.ApprenticeId}";
                Console.WriteLine($"ID: {g.Id} | Fach: {g.Subject} | Note: {g.Value:F1} | Lernender: {name} | Datum: {g.Date:dd.MM.yyyy}{(string.IsNullOrEmpty(g.Remarks) ? "" : $" | Bemerkung: {g.Remarks}")}");
            }
        }

        // Creates a new grade
        static void CreateGrade()
        {
            Console.Write("Fach: ");
            string subject = Console.ReadLine() ?? string.Empty;

            double value;
            while (true)
            {
                Console.Write("Note (1.0 – 6.0): ");
                string? input = Console.ReadLine();
                if (double.TryParse(input, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out value) &&
                    value >= 1.0 && value <= 6.0) break;
                Console.WriteLine("Ungültige Note. Bitte einen Wert zwischen 1.0 und 6.0 eingeben.");
            }

            int apprenticeId = ReadInt("Lernender ID: ");
            DateTime date    = ReadDate("Datum (z.B. 2026-03-25): ");
            Console.Write("Bemerkungen (optional, Enter für keine): ");
            string? remarks = Console.ReadLine();

            var grade = new MIACopilotApp.Models.Grade
            {
                Subject      = subject,
                Value        = value,
                ApprenticeId = apprenticeId,
                Date         = date,
                Remarks      = string.IsNullOrWhiteSpace(remarks) ? null : remarks
            };
            MIACopilotApp.Models.Grade.Create(grade);
            Console.WriteLine("Note erfolgreich hinzugefügt.");
        }

        // Updates an existing grade
        static void UpdateGrade()
        {
            int id = ReadInt("ID der zu bearbeitenden Note: ");
            var grade = MIACopilotApp.Models.Grade.Read(id);
            if (grade == null) { Console.WriteLine("Note nicht gefunden."); return; }

            Console.Write($"Neues Fach (aktuell: {grade.Subject}): ");
            string subject = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(subject)) grade.Subject = subject;

            Console.Write($"Neue Note (aktuell: {grade.Value:F1}, leer lassen zum Beibehalten): ");
            string? valInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(valInput) &&
                double.TryParse(valInput, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out double newVal) &&
                newVal >= 1.0 && newVal <= 6.0)
                grade.Value = newVal;

            Console.Write($"Neues Datum (aktuell: {grade.Date:dd.MM.yyyy}, leer lassen zum Beibehalten): ");
            string? dateInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dateInput) && DateTime.TryParse(dateInput, out DateTime newDate))
                grade.Date = newDate;

            Console.Write($"Neue Bemerkung (aktuell: {grade.Remarks ?? "keine"}): ");
            string? remarks = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(remarks)) grade.Remarks = remarks;

            MIACopilotApp.Models.Grade.Update(grade);
            Console.WriteLine("Note erfolgreich aktualisiert.");
        }

        // Deletes a grade
        static void DeleteGrade()
        {
            int id = ReadInt("ID der zu löschenden Note: ");
            MIACopilotApp.Models.Grade.Delete(id);
            Console.WriteLine("Note (falls vorhanden) gelöscht.");
        }
    }
}

