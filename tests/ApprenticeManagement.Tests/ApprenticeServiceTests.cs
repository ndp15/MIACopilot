using ApprenticeManagement.Models;
using ApprenticeManagement.Repositories;
using ApprenticeManagement.Services;

namespace ApprenticeManagement.Tests;

public class ApprenticeServiceTests
{
    private static ApprenticeService CreateService() =>
        new(new ApprenticeRepository());

    [Fact]
    public void AddApprentice_ValidData_ReturnsApprenticeWithId()
    {
        var service = CreateService();

        var result = service.AddApprentice("Jane", "Doe", "jane@example.com", "Software Dev", new DateOnly(2024, 1, 1), null);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Jane", result.FirstName);
        Assert.Equal("Doe", result.LastName);
        Assert.Equal(ApprenticeStatus.Active, result.Status);
    }

    [Fact]
    public void AddApprentice_EmptyFirstName_ThrowsArgumentException()
    {
        var service = CreateService();
        Assert.Throws<ArgumentException>(() =>
            service.AddApprentice("", "Doe", "jane@example.com", "SW Dev", new DateOnly(2024, 1, 1), null));
    }

    [Fact]
    public void AddApprentice_EmptyLastName_ThrowsArgumentException()
    {
        var service = CreateService();
        Assert.Throws<ArgumentException>(() =>
            service.AddApprentice("Jane", "", "jane@example.com", "SW Dev", new DateOnly(2024, 1, 1), null));
    }

    [Fact]
    public void AddApprentice_EmptyEmail_ThrowsArgumentException()
    {
        var service = CreateService();
        Assert.Throws<ArgumentException>(() =>
            service.AddApprentice("Jane", "Doe", "", "SW Dev", new DateOnly(2024, 1, 1), null));
    }

    [Fact]
    public void AddApprentice_EmptyProgramName_ThrowsArgumentException()
    {
        var service = CreateService();
        Assert.Throws<ArgumentException>(() =>
            service.AddApprentice("Jane", "Doe", "jane@example.com", "", new DateOnly(2024, 1, 1), null));
    }

    [Fact]
    public void AddApprentice_EndDateBeforeStartDate_ThrowsArgumentException()
    {
        var service = CreateService();
        Assert.Throws<ArgumentException>(() =>
            service.AddApprentice("Jane", "Doe", "jane@example.com", "SW Dev",
                new DateOnly(2024, 6, 1), new DateOnly(2024, 1, 1)));
    }

    [Fact]
    public void AddApprentice_TrimsWhitespace()
    {
        var service = CreateService();

        var result = service.AddApprentice("  Jane  ", "  Doe  ", "  jane@example.com  ", "  SW Dev  ", new DateOnly(2024, 1, 1), null);

        Assert.Equal("Jane", result.FirstName);
        Assert.Equal("Doe", result.LastName);
        Assert.Equal("jane@example.com", result.Email);
        Assert.Equal("SW Dev", result.ProgramName);
    }

    [Fact]
    public void GetAllApprentices_MultipleAdded_ReturnsAll()
    {
        var service = CreateService();
        service.AddApprentice("A", "A", "a@a.com", "P1", new DateOnly(2024, 1, 1), null);
        service.AddApprentice("B", "B", "b@b.com", "P2", new DateOnly(2024, 2, 1), null);

        Assert.Equal(2, service.GetAllApprentices().Count());
    }

    [Fact]
    public void GetApprenticeById_ExistingId_ReturnsApprentice()
    {
        var service = CreateService();
        var added = service.AddApprentice("Jane", "Doe", "jane@example.com", "SW Dev", new DateOnly(2024, 1, 1), null);

        var result = service.GetApprenticeById(added.Id);

        Assert.NotNull(result);
        Assert.Equal("Jane", result.FirstName);
    }

    [Fact]
    public void GetApprenticeById_NonExistingId_ReturnsNull()
    {
        var service = CreateService();
        Assert.Null(service.GetApprenticeById(999));
    }

    [Fact]
    public void UpdateApprentice_ExistingId_ReturnsTrueAndUpdates()
    {
        var service = CreateService();
        var added = service.AddApprentice("Jane", "Doe", "jane@example.com", "SW Dev", new DateOnly(2024, 1, 1), null);

        var result = service.UpdateApprentice(added.Id, "Janet", "Smith", "janet@example.com", "Data Science",
            new DateOnly(2024, 3, 1), null, ApprenticeStatus.OnLeave);

        Assert.True(result);
        var updated = service.GetApprenticeById(added.Id)!;
        Assert.Equal("Janet", updated.FirstName);
        Assert.Equal("Smith", updated.LastName);
        Assert.Equal(ApprenticeStatus.OnLeave, updated.Status);
    }

    [Fact]
    public void UpdateApprentice_NonExistingId_ReturnsFalse()
    {
        var service = CreateService();

        var result = service.UpdateApprentice(999, "Jane", "Doe", "jane@example.com", "SW Dev",
            new DateOnly(2024, 1, 1), null, ApprenticeStatus.Active);

        Assert.False(result);
    }

    [Fact]
    public void RemoveApprentice_ExistingId_ReturnsTrueAndRemoves()
    {
        var service = CreateService();
        var added = service.AddApprentice("Jane", "Doe", "jane@example.com", "SW Dev", new DateOnly(2024, 1, 1), null);

        var result = service.RemoveApprentice(added.Id);

        Assert.True(result);
        Assert.Empty(service.GetAllApprentices());
    }

    [Fact]
    public void RemoveApprentice_NonExistingId_ReturnsFalse()
    {
        var service = CreateService();
        Assert.False(service.RemoveApprentice(999));
    }

    [Fact]
    public void SearchByName_MatchingFirstName_ReturnsResults()
    {
        var service = CreateService();
        service.AddApprentice("Jane", "Doe", "jane@example.com", "SW Dev", new DateOnly(2024, 1, 1), null);
        service.AddApprentice("John", "Smith", "john@example.com", "Data", new DateOnly(2024, 2, 1), null);

        var results = service.SearchByName("Jane").ToList();

        Assert.Single(results);
        Assert.Equal("Jane", results[0].FirstName);
    }

    [Fact]
    public void SearchByName_MatchingLastName_ReturnsResults()
    {
        var service = CreateService();
        service.AddApprentice("Jane", "Doe", "jane@example.com", "SW Dev", new DateOnly(2024, 1, 1), null);
        service.AddApprentice("John", "Smith", "john@example.com", "Data", new DateOnly(2024, 2, 1), null);

        var results = service.SearchByName("Smith").ToList();

        Assert.Single(results);
        Assert.Equal("John", results[0].FirstName);
    }

    [Fact]
    public void SearchByName_CaseInsensitive_ReturnsResults()
    {
        var service = CreateService();
        service.AddApprentice("Jane", "Doe", "jane@example.com", "SW Dev", new DateOnly(2024, 1, 1), null);

        var results = service.SearchByName("JANE").ToList();

        Assert.Single(results);
    }

    [Fact]
    public void SearchByName_EmptyString_ReturnsAll()
    {
        var service = CreateService();
        service.AddApprentice("Jane", "Doe", "jane@example.com", "SW Dev", new DateOnly(2024, 1, 1), null);
        service.AddApprentice("John", "Smith", "john@example.com", "Data", new DateOnly(2024, 2, 1), null);

        var results = service.SearchByName("").ToList();

        Assert.Equal(2, results.Count);
    }

    [Fact]
    public void GetByStatus_FiltersByStatus_ReturnsMatchingOnly()
    {
        var service = CreateService();
        var a1 = service.AddApprentice("Jane", "Doe", "jane@example.com", "SW Dev", new DateOnly(2024, 1, 1), null);
        var a2 = service.AddApprentice("John", "Smith", "john@example.com", "Data", new DateOnly(2024, 2, 1), null);
        service.UpdateApprentice(a2.Id, a2.FirstName, a2.LastName, a2.Email, a2.ProgramName,
            a2.StartDate, a2.EndDate, ApprenticeStatus.Completed);

        var active = service.GetByStatus(ApprenticeStatus.Active).ToList();
        var completed = service.GetByStatus(ApprenticeStatus.Completed).ToList();

        Assert.Single(active);
        Assert.Equal("Jane", active[0].FirstName);
        Assert.Single(completed);
        Assert.Equal("John", completed[0].FirstName);
    }
}
