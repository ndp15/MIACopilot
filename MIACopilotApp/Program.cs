using System;
using System.Linq;
using MIACopilotApp.Models;

namespace MIACopilotApp
{
    class Program
    {
        // Entry point for the console application
        static void Main(string[] args)
        {
            DataStore.LoadData();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n--- Lernenden-Management-System ---");
                Console.WriteLine("1. Lernende verwalten");
                Console.WriteLine("2. Firmen verwalten");
                Console.WriteLine("3. Berufsbildner verwalten");
                Console.WriteLine("4. Arbeitsjournale verwalten");
                Console.WriteLine("5. Beenden");
                Console.Write("Bitte waehlen Sie eine Option (1-5): ");

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
                        running = false;
                        Console.WriteLine("Programm wird beendet.");
                        break;
                    default:
                        Console.WriteLine("Ungueltige Eingabe. Bitte versuchen Sie es erneut.");
                        break;
                }
            }
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
                Console.WriteLine("Ungueltige Eingabe. Bitte geben Sie eine Zahl ein.");
            }
        }

        // Helper method to read an optional integer (for nullable IDs)
        static int? ReadOptionalInt(string prompt)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return null;
            if (int.TryParse(input, out int result)) return result;
            Console.WriteLine("Ungueltige Eingabe. Wert wird als leer gespeichert.");
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
                Console.WriteLine("Ungueltige Eingabe. Bitte geben Sie ein gueltiges Datum ein.");
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
                Console.WriteLine("2. Hinzufuegen");
                Console.WriteLine("3. Bearbeiten");
                Console.WriteLine("4. Loeschen");
                Console.WriteLine("5. Zurueck zum Hauptmenue");
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
                        Console.WriteLine("Ungueltige Eingabe.");
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
            int? companyId = ReadOptionalInt("Firma ID (leer fuer keine): ");
            int? trainerId = ReadOptionalInt("Berufsbildner ID (leer fuer keine): ");

            var apprentice = new Apprentice 
            { 
                FirstName = firstName, 
                LastName = lastName,
                CompanyId = companyId,
                VocationalTrainerId = trainerId
            };
            Apprentice.Create(apprentice);
            Console.WriteLine("Lernender erfolgreich hinzugefuegt.");
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
            int id = ReadInt("ID des zu loeschenden Lernenden: ");
            Apprentice.Delete(id);
            Console.WriteLine("Lernender (falls vorhanden) geloescht.");
        }

        // Manages the company operations (CRUD)
        static void ManageCompanies()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n-- Firmen verwalten --");
                Console.WriteLine("1. Alle anzeigen");
                Console.WriteLine("2. Hinzufuegen");
                Console.WriteLine("3. Bearbeiten");
                Console.WriteLine("4. Loeschen");
                Console.WriteLine("5. Zurueck zum Hauptmenue");
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
                        Console.WriteLine("Ungueltige Eingabe.");
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
            Console.WriteLine("Firma erfolgreich hinzugefuegt.");
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
            int id = ReadInt("ID der zu loeschenden Firma: ");
            Company.Delete(id);
            Console.WriteLine("Firma (falls vorhanden) geloescht.");
        }

        // Manages the vocational trainer operations (CRUD)
        static void ManageVocationalTrainers()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n-- Berufsbildner verwalten --");
                Console.WriteLine("1. Alle anzeigen");
                Console.WriteLine("2. Hinzufuegen");
                Console.WriteLine("3. Bearbeiten");
                Console.WriteLine("4. Loeschen");
                Console.WriteLine("5. Zurueck zum Hauptmenue");
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
                        Console.WriteLine("Ungueltige Eingabe.");
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
            int? companyId = ReadOptionalInt("Firma ID (leer fuer keine): ");

            var trainer = new VocationalTrainer 
            { 
                FirstName = firstName, 
                LastName = lastName, 
                CompanyId = companyId 
            };
            VocationalTrainer.Create(trainer);
            Console.WriteLine("Berufsbildner erfolgreich hinzugefuegt.");
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
            int id = ReadInt("ID des zu loeschenden Berufsbildners: ");
            VocationalTrainer.Delete(id);
            Console.WriteLine("Berufsbildner (falls vorhanden) geloescht.");
        }

        // Manages the work journal operations (CRUD)
        static void ManageWorkJournals()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n-- Arbeitsjournale verwalten --");
                Console.WriteLine("1. Alle anzeigen");
                Console.WriteLine("2. Hinzufuegen");
                Console.WriteLine("3. Bearbeiten");
                Console.WriteLine("4. Loeschen");
                Console.WriteLine("5. Zurueck zum Hauptmenue");
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
                        Console.WriteLine("Ungueltige Eingabe.");
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
            Console.WriteLine("Arbeitsjournal erfolgreich hinzugefuegt.");
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
            int id = ReadInt("ID des zu loeschenden Arbeitsjournals: ");
            WorkJournal.Delete(id);
            Console.WriteLine("Arbeitsjournal (falls vorhanden) geloescht.");
        }
    }
}
