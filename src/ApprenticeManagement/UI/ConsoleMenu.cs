using ApprenticeManagement.Models;
using ApprenticeManagement.Services;

namespace ApprenticeManagement.UI;

public class ConsoleMenu
{
    private readonly IApprenticeService _service;

    public ConsoleMenu(IApprenticeService service)
    {
        _service = service;
    }

    public void Run()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("    Apprentice Management System v1.0      ");
        Console.WriteLine("===========================================");

        bool running = true;
        while (running)
        {
            PrintMainMenu();
            var choice = Console.ReadLine()?.Trim();
            Console.WriteLine();

            running = choice switch
            {
                "1" => HandleListAll(),
                "2" => HandleAdd(),
                "3" => HandleView(),
                "4" => HandleUpdate(),
                "5" => HandleDelete(),
                "6" => HandleSearch(),
                "7" => HandleFilterByStatus(),
                "8" => false,
                _ => HandleInvalidOption()
            };

            if (running) Console.WriteLine();
        }

        Console.WriteLine("Goodbye!");
    }

    private static void PrintMainMenu()
    {
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine("Main Menu:");
        Console.WriteLine("  1. List all apprentices");
        Console.WriteLine("  2. Add apprentice");
        Console.WriteLine("  3. View apprentice details");
        Console.WriteLine("  4. Update apprentice");
        Console.WriteLine("  5. Delete apprentice");
        Console.WriteLine("  6. Search by name");
        Console.WriteLine("  7. Filter by status");
        Console.WriteLine("  8. Exit");
        Console.WriteLine("-------------------------------------------");
        Console.Write("Enter your choice: ");
    }

    private bool HandleListAll()
    {
        var apprentices = _service.GetAllApprentices().ToList();
        if (apprentices.Count == 0)
        {
            Console.WriteLine("No apprentices found.");
        }
        else
        {
            Console.WriteLine($"{"ID",-5} {"Name",-25} {"Program",-25} {"Start Date",-12} {"Status",-12}");
            Console.WriteLine(new string('-', 82));
            foreach (var a in apprentices)
            {
                Console.WriteLine($"{a.Id,-5} {a.FullName,-25} {a.ProgramName,-25} {a.StartDate,-12} {a.Status,-12}");
            }
        }
        return true;
    }

    private bool HandleAdd()
    {
        Console.WriteLine("--- Add New Apprentice ---");

        var firstName = PromptRequired("First name: ");
        if (firstName is null) return true;

        var lastName = PromptRequired("Last name: ");
        if (lastName is null) return true;

        var email = PromptRequired("Email: ");
        if (email is null) return true;

        var programName = PromptRequired("Program name: ");
        if (programName is null) return true;

        var startDate = PromptDate("Start date (yyyy-MM-dd): ");
        if (startDate is null) return true;

        var endDate = PromptOptionalDate("End date (yyyy-MM-dd, leave blank to skip): ");

        try
        {
            var apprentice = _service.AddApprentice(firstName, lastName, email, programName, startDate.Value, endDate);
            Console.WriteLine($"Apprentice '{apprentice.FullName}' added successfully with ID {apprentice.Id}.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return true;
    }

    private bool HandleView()
    {
        Console.Write("Enter apprentice ID: ");
        if (!TryReadId(out var id)) return true;

        var apprentice = _service.GetApprenticeById(id);
        if (apprentice is null)
        {
            Console.WriteLine($"Apprentice with ID {id} not found.");
            return true;
        }

        PrintApprenticeDetails(apprentice);
        return true;
    }

    private bool HandleUpdate()
    {
        Console.Write("Enter apprentice ID to update: ");
        if (!TryReadId(out var id)) return true;

        var existing = _service.GetApprenticeById(id);
        if (existing is null)
        {
            Console.WriteLine($"Apprentice with ID {id} not found.");
            return true;
        }

        Console.WriteLine($"Updating: {existing.FullName} (press Enter to keep current value)");

        var firstName = PromptWithDefault($"First name [{existing.FirstName}]: ", existing.FirstName);
        var lastName = PromptWithDefault($"Last name [{existing.LastName}]: ", existing.LastName);
        var email = PromptWithDefault($"Email [{existing.Email}]: ", existing.Email);
        var programName = PromptWithDefault($"Program name [{existing.ProgramName}]: ", existing.ProgramName);

        DateOnly startDate = existing.StartDate;
        Console.Write($"Start date [{existing.StartDate:yyyy-MM-dd}]: ");
        var startInput = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(startInput))
        {
            if (!DateOnly.TryParseExact(startInput, "yyyy-MM-dd", out startDate))
            {
                Console.WriteLine("Invalid date format. Update cancelled.");
                return true;
            }
        }

        DateOnly? endDate = existing.EndDate;
        Console.Write($"End date [{(existing.EndDate.HasValue ? existing.EndDate.Value.ToString("yyyy-MM-dd") : "none")}] (enter date or 'none' to clear): ");
        var endInput = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(endInput))
        {
            if (endInput.Equals("none", StringComparison.OrdinalIgnoreCase))
            {
                endDate = null;
            }
            else if (DateOnly.TryParseExact(endInput, "yyyy-MM-dd", out var parsedEnd))
            {
                endDate = parsedEnd;
            }
            else
            {
                Console.WriteLine("Invalid date format. Update cancelled.");
                return true;
            }
        }

        var status = existing.Status;
        Console.WriteLine($"Status [{existing.Status}]:");
        var statuses = Enum.GetValues<ApprenticeStatus>();
        for (int i = 0; i < statuses.Length; i++)
        {
            Console.WriteLine($"  {i + 1}. {statuses[i]}");
        }
        Console.Write("Choose status (or press Enter to keep current): ");
        var statusInput = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(statusInput) && int.TryParse(statusInput, out var statusChoice)
            && statusChoice >= 1 && statusChoice <= statuses.Length)
        {
            status = statuses[statusChoice - 1];
        }

        try
        {
            var updated = _service.UpdateApprentice(id, firstName, lastName, email, programName, startDate, endDate, status);
            Console.WriteLine(updated ? "Apprentice updated successfully." : "Update failed.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return true;
    }

    private bool HandleDelete()
    {
        Console.Write("Enter apprentice ID to delete: ");
        if (!TryReadId(out var id)) return true;

        var existing = _service.GetApprenticeById(id);
        if (existing is null)
        {
            Console.WriteLine($"Apprentice with ID {id} not found.");
            return true;
        }

        Console.Write($"Are you sure you want to delete '{existing.FullName}'? (y/N): ");
        var confirm = Console.ReadLine()?.Trim();
        if (confirm?.Equals("y", StringComparison.OrdinalIgnoreCase) != true)
        {
            Console.WriteLine("Deletion cancelled.");
            return true;
        }

        var deleted = _service.RemoveApprentice(id);
        Console.WriteLine(deleted ? "Apprentice deleted successfully." : "Deletion failed.");
        return true;
    }

    private bool HandleSearch()
    {
        Console.Write("Enter name to search: ");
        var name = Console.ReadLine()?.Trim() ?? string.Empty;

        var results = _service.SearchByName(name).ToList();
        if (results.Count == 0)
        {
            Console.WriteLine("No matching apprentices found.");
        }
        else
        {
            Console.WriteLine($"Found {results.Count} result(s):");
            Console.WriteLine($"{"ID",-5} {"Name",-25} {"Program",-25} {"Status",-12}");
            Console.WriteLine(new string('-', 70));
            foreach (var a in results)
            {
                Console.WriteLine($"{a.Id,-5} {a.FullName,-25} {a.ProgramName,-25} {a.Status,-12}");
            }
        }
        return true;
    }

    private bool HandleFilterByStatus()
    {
        Console.WriteLine("Select status to filter by:");
        var statuses = Enum.GetValues<ApprenticeStatus>();
        for (int i = 0; i < statuses.Length; i++)
        {
            Console.WriteLine($"  {i + 1}. {statuses[i]}");
        }
        Console.Write("Enter choice: ");
        var input = Console.ReadLine()?.Trim();

        if (!int.TryParse(input, out var choice) || choice < 1 || choice > statuses.Length)
        {
            Console.WriteLine("Invalid choice.");
            return true;
        }

        var status = statuses[choice - 1];
        var results = _service.GetByStatus(status).ToList();

        if (results.Count == 0)
        {
            Console.WriteLine($"No apprentices with status '{status}'.");
        }
        else
        {
            Console.WriteLine($"Apprentices with status '{status}':");
            Console.WriteLine($"{"ID",-5} {"Name",-25} {"Program",-25} {"Start Date",-12}");
            Console.WriteLine(new string('-', 70));
            foreach (var a in results)
            {
                Console.WriteLine($"{a.Id,-5} {a.FullName,-25} {a.ProgramName,-25} {a.StartDate,-12}");
            }
        }
        return true;
    }

    private static bool HandleInvalidOption()
    {
        Console.WriteLine("Invalid option. Please try again.");
        return true;
    }

    private static void PrintApprenticeDetails(Apprentice a)
    {
        Console.WriteLine($"ID:           {a.Id}");
        Console.WriteLine($"Name:         {a.FullName}");
        Console.WriteLine($"Email:        {a.Email}");
        Console.WriteLine($"Program:      {a.ProgramName}");
        Console.WriteLine($"Start Date:   {a.StartDate:yyyy-MM-dd}");
        Console.WriteLine($"End Date:     {(a.EndDate.HasValue ? a.EndDate.Value.ToString("yyyy-MM-dd") : "N/A")}");
        Console.WriteLine($"Status:       {a.Status}");
    }

    private static string? PromptRequired(string prompt)
    {
        Console.Write(prompt);
        var value = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(value))
        {
            Console.WriteLine("This field is required. Operation cancelled.");
            return null;
        }
        return value;
    }

    private static string PromptWithDefault(string prompt, string defaultValue)
    {
        Console.Write(prompt);
        var value = Console.ReadLine()?.Trim();
        return string.IsNullOrEmpty(value) ? defaultValue : value;
    }

    private static DateOnly? PromptDate(string prompt)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim();
        if (DateOnly.TryParseExact(input, "yyyy-MM-dd", out var date))
            return date;
        Console.WriteLine("Invalid date format (expected yyyy-MM-dd). Operation cancelled.");
        return null;
    }

    private static DateOnly? PromptOptionalDate(string prompt)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(input)) return null;
        if (DateOnly.TryParseExact(input, "yyyy-MM-dd", out var date))
            return date;
        Console.WriteLine("Invalid date format. End date ignored.");
        return null;
    }

    private static bool TryReadId(out int id)
    {
        var input = Console.ReadLine()?.Trim();
        if (int.TryParse(input, out id) && id > 0) return true;
        Console.WriteLine("Invalid ID.");
        id = 0;
        return false;
    }
}
