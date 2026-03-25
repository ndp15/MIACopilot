using System;
using MIACopilotApp;

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
                Console.Write("Bitte wahlen Sie eine Option (1-5): ");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ManageApprentices();
                        break;
                    case "2":
                        Console.WriteLine("Firmenverwaltung gestartet.");
                        break;
                    case "3":
                        Console.WriteLine("Berufsbildnerverwaltung gestartet.");
                        break;
                    case "4":
                        Console.WriteLine("Arbeitsjournalverwaltung gestartet.");
                        break;
                    case "5":
                        running = false;
                        Console.WriteLine("Programm wird beendet.");
                        break;
                    default:
                        Console.WriteLine("Ungultige Eingabe. Bitte versuchen Sie es erneut.");
                        break;
                }
            }
        }

        // Placeholder method to demonstrate apprentice management menu
        static void ManageApprentices()
        {
            Console.WriteLine("\n-- Lernende verwalten --");
            Console.WriteLine("1. Lernenden hinzufugen");
            Console.WriteLine("2. Zuruck zum Hauptmenue");
            Console.Write("Option: ");
            string? input = Console.ReadLine();

            if (input == "1")
            {
                Console.Write("Vorname: ");
                string firstName = Console.ReadLine() ?? string.Empty;
                Console.Write("Nachname: ");
                string lastName = Console.ReadLine() ?? string.Empty;

                var apprentice = new Models.Apprentice { FirstName = firstName, LastName = lastName };
                Models.Apprentice.Create(apprentice);
                Console.WriteLine("Lernender erfolgreich hinzugefuegt.");
            }
        }
    }
}
