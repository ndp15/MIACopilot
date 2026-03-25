using ApprenticeManagement.Models;

namespace ApprenticeManagement.Repositories;

public interface IApprenticeRepository
{
    IEnumerable<Apprentice> GetAll();
    Apprentice? GetById(int id);
    Apprentice Add(Apprentice apprentice);
    bool Update(Apprentice apprentice);
    bool Delete(int id);
    bool Exists(int id);
}
