using System.Linq;

namespace MIACopilotApp.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // Creates a new company
        public static void Create(Company company)
        {
            if (company == null) return;
            company.Id = DataStore.Data.Companies.Any() ? DataStore.Data.Companies.Max(c => c.Id) + 1 : 1;
            DataStore.Data.Companies.Add(company);
            DataStore.SaveData();
        }

        // Reads a company by id
        public static Company? Read(int id)
        {
            return DataStore.Data.Companies.FirstOrDefault(c => c.Id == id);
        }

        // Updates an existing company
        public static void Update(Company company)
        {
            if (company == null) return;
            var existing = Read(company.Id);
            if (existing != null)
            {
                existing.Name = company.Name;
                existing.Address = company.Address;
                DataStore.SaveData();
            }
        }

        // Deletes a company by id
        public static void Delete(int id)
        {
            var company = Read(id);
            if (company != null)
            {
                DataStore.Data.Companies.Remove(company);
                DataStore.SaveData();
            }
        }
    }
}
