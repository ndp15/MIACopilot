using ApprenticeManagement.Models;

namespace ApprenticeManagement.Repositories;

public class ApprenticeRepository : IApprenticeRepository
{
    private readonly List<Apprentice> _apprentices = [];
    private int _nextId = 1;

    public IEnumerable<Apprentice> GetAll() => _apprentices.AsReadOnly();

    public Apprentice? GetById(int id) =>
        _apprentices.FirstOrDefault(a => a.Id == id);

    public Apprentice Add(Apprentice apprentice)
    {
        apprentice.Id = _nextId++;
        _apprentices.Add(apprentice);
        return apprentice;
    }

    public bool Update(Apprentice apprentice)
    {
        var index = _apprentices.FindIndex(a => a.Id == apprentice.Id);
        if (index < 0) return false;
        _apprentices[index] = apprentice;
        return true;
    }

    public bool Delete(int id)
    {
        var apprentice = _apprentices.FirstOrDefault(a => a.Id == id);
        if (apprentice is null) return false;
        _apprentices.Remove(apprentice);
        return true;
    }

    public bool Exists(int id) => _apprentices.Any(a => a.Id == id);
}
