using System.Collections.Generic;
using System.Linq;

namespace MIACopilotApp.Models
{
    public class Apprentice
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public int? CompanyId { get; set; }
        public int? VocationalTrainerId { get; set; }

        public List<WorkJournal> WorkJournals { get; set; } = new();

        // Creates a new apprentice
        public static void Create(Apprentice apprentice)
        {
            if (apprentice == null) return;
            apprentice.Id = DataStore.Data.Apprentices.Any() ? DataStore.Data.Apprentices.Max(a => a.Id) + 1 : 1;
            DataStore.Data.Apprentices.Add(apprentice);
            DataStore.SaveData();
        }

        // Reads an apprentice by id
        public static Apprentice? Read(int id)
        {
            return DataStore.Data.Apprentices.FirstOrDefault(a => a.Id == id);
        }

        // Updates an existing apprentice
        public static void Update(Apprentice apprentice)
        {
            if (apprentice == null) return;
            var existing = Read(apprentice.Id);
            if (existing != null)
            {
                existing.FirstName = apprentice.FirstName;
                existing.LastName = apprentice.LastName;
                existing.CompanyId = apprentice.CompanyId;
                existing.VocationalTrainerId = apprentice.VocationalTrainerId;
                existing.WorkJournals = apprentice.WorkJournals;
                DataStore.SaveData();
            }
        }

        // Deletes an apprentice by id
        public static void Delete(int id)
        {
            var apprentice = Read(id);
            if (apprentice != null)
            {
                DataStore.Data.Apprentices.Remove(apprentice);
                DataStore.SaveData();
            }
        }
    }
}
