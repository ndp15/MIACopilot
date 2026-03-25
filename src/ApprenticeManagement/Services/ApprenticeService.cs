using ApprenticeManagement.Models;
using ApprenticeManagement.Repositories;

namespace ApprenticeManagement.Services;

public class ApprenticeService : IApprenticeService
{
    private readonly IApprenticeRepository _repository;

    public ApprenticeService(IApprenticeRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Apprentice> GetAllApprentices() => _repository.GetAll();

    public Apprentice? GetApprenticeById(int id) => _repository.GetById(id);

    public Apprentice AddApprentice(string firstName, string lastName, string email, string programName, DateOnly startDate, DateOnly? endDate)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));
        if (string.IsNullOrWhiteSpace(programName))
            throw new ArgumentException("Program name cannot be empty.", nameof(programName));
        if (endDate.HasValue && endDate.Value <= startDate)
            throw new ArgumentException("End date must be after start date.");

        var apprentice = new Apprentice
        {
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Email = email.Trim(),
            ProgramName = programName.Trim(),
            StartDate = startDate,
            EndDate = endDate,
            Status = ApprenticeStatus.Active
        };

        return _repository.Add(apprentice);
    }

    public bool UpdateApprentice(int id, string firstName, string lastName, string email, string programName, DateOnly startDate, DateOnly? endDate, ApprenticeStatus status)
    {
        var existing = _repository.GetById(id);
        if (existing is null) return false;

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));
        if (string.IsNullOrWhiteSpace(programName))
            throw new ArgumentException("Program name cannot be empty.", nameof(programName));
        if (endDate.HasValue && endDate.Value <= startDate)
            throw new ArgumentException("End date must be after start date.");

        existing.FirstName = firstName.Trim();
        existing.LastName = lastName.Trim();
        existing.Email = email.Trim();
        existing.ProgramName = programName.Trim();
        existing.StartDate = startDate;
        existing.EndDate = endDate;
        existing.Status = status;

        return _repository.Update(existing);
    }

    public bool RemoveApprentice(int id) => _repository.Delete(id);

    public IEnumerable<Apprentice> SearchByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return _repository.GetAll();
        var lower = name.Trim().ToLowerInvariant();
        return _repository.GetAll()
            .Where(a => a.FirstName.ToLowerInvariant().Contains(lower)
                     || a.LastName.ToLowerInvariant().Contains(lower));
    }

    public IEnumerable<Apprentice> GetByStatus(ApprenticeStatus status) =>
        _repository.GetAll().Where(a => a.Status == status);
}
