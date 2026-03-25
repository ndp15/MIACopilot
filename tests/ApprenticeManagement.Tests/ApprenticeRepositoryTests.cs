using ApprenticeManagement.Models;
using ApprenticeManagement.Repositories;

namespace ApprenticeManagement.Tests;

public class ApprenticeRepositoryTests
{
    private static ApprenticeRepository CreateRepository() => new();

    [Fact]
    public void GetAll_EmptyRepository_ReturnsEmpty()
    {
        var repo = CreateRepository();
        Assert.Empty(repo.GetAll());
    }

    [Fact]
    public void Add_ValidApprentice_AssignsIdAndReturnsIt()
    {
        var repo = CreateRepository();
        var apprentice = new Apprentice { FirstName = "Jane", LastName = "Doe", Email = "jane@example.com", ProgramName = "Software Dev", StartDate = new DateOnly(2024, 1, 1) };

        var result = repo.Add(apprentice);

        Assert.Equal(1, result.Id);
        Assert.Single(repo.GetAll());
    }

    [Fact]
    public void Add_MultipleApprentices_AssignsIncrementalIds()
    {
        var repo = CreateRepository();

        var a1 = repo.Add(new Apprentice { FirstName = "A", LastName = "A", Email = "a@a.com", ProgramName = "P1", StartDate = new DateOnly(2024, 1, 1) });
        var a2 = repo.Add(new Apprentice { FirstName = "B", LastName = "B", Email = "b@b.com", ProgramName = "P2", StartDate = new DateOnly(2024, 2, 1) });

        Assert.Equal(1, a1.Id);
        Assert.Equal(2, a2.Id);
    }

    [Fact]
    public void GetById_ExistingId_ReturnsApprentice()
    {
        var repo = CreateRepository();
        var added = repo.Add(new Apprentice { FirstName = "Jane", LastName = "Doe", Email = "jane@example.com", ProgramName = "SW Dev", StartDate = new DateOnly(2024, 1, 1) });

        var result = repo.GetById(added.Id);

        Assert.NotNull(result);
        Assert.Equal("Jane", result.FirstName);
    }

    [Fact]
    public void GetById_NonExistingId_ReturnsNull()
    {
        var repo = CreateRepository();
        Assert.Null(repo.GetById(999));
    }

    [Fact]
    public void Update_ExistingApprentice_ReturnsTrueAndUpdates()
    {
        var repo = CreateRepository();
        var added = repo.Add(new Apprentice { FirstName = "Jane", LastName = "Doe", Email = "jane@example.com", ProgramName = "SW Dev", StartDate = new DateOnly(2024, 1, 1) });
        added.FirstName = "Janet";

        var result = repo.Update(added);

        Assert.True(result);
        Assert.Equal("Janet", repo.GetById(added.Id)!.FirstName);
    }

    [Fact]
    public void Update_NonExistingApprentice_ReturnsFalse()
    {
        var repo = CreateRepository();
        var apprentice = new Apprentice { Id = 999, FirstName = "X", LastName = "X", Email = "x@x.com", ProgramName = "P", StartDate = new DateOnly(2024, 1, 1) };

        Assert.False(repo.Update(apprentice));
    }

    [Fact]
    public void Delete_ExistingId_ReturnsTrueAndRemoves()
    {
        var repo = CreateRepository();
        var added = repo.Add(new Apprentice { FirstName = "Jane", LastName = "Doe", Email = "jane@example.com", ProgramName = "SW Dev", StartDate = new DateOnly(2024, 1, 1) });

        var result = repo.Delete(added.Id);

        Assert.True(result);
        Assert.Empty(repo.GetAll());
    }

    [Fact]
    public void Delete_NonExistingId_ReturnsFalse()
    {
        var repo = CreateRepository();
        Assert.False(repo.Delete(999));
    }

    [Fact]
    public void Exists_ExistingId_ReturnsTrue()
    {
        var repo = CreateRepository();
        var added = repo.Add(new Apprentice { FirstName = "Jane", LastName = "Doe", Email = "jane@example.com", ProgramName = "SW Dev", StartDate = new DateOnly(2024, 1, 1) });

        Assert.True(repo.Exists(added.Id));
    }

    [Fact]
    public void Exists_NonExistingId_ReturnsFalse()
    {
        var repo = CreateRepository();
        Assert.False(repo.Exists(999));
    }
}
