namespace ApprenticeManagement.Models;

public enum ApprenticeStatus
{
    Active,
    OnLeave,
    Completed,
    Withdrawn
}

public class Apprentice
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ProgramName { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public ApprenticeStatus Status { get; set; } = ApprenticeStatus.Active;

    public string FullName => $"{FirstName} {LastName}";
}
