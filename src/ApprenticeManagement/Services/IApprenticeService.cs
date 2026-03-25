using ApprenticeManagement.Models;

namespace ApprenticeManagement.Services;

public interface IApprenticeService
{
    IEnumerable<Apprentice> GetAllApprentices();
    Apprentice? GetApprenticeById(int id);
    Apprentice AddApprentice(string firstName, string lastName, string email, string programName, DateOnly startDate, DateOnly? endDate);
    bool UpdateApprentice(int id, string firstName, string lastName, string email, string programName, DateOnly startDate, DateOnly? endDate, ApprenticeStatus status);
    bool RemoveApprentice(int id);
    IEnumerable<Apprentice> SearchByName(string name);
    IEnumerable<Apprentice> GetByStatus(ApprenticeStatus status);
}
