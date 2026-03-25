using System.Linq;

namespace MIACopilotApp.Models
{
    public class VocationalTrainer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? CompanyId { get; set; }

        // Creates a new vocational trainer
        public static void Create(VocationalTrainer trainer)
        {
            if (trainer == null) return;
            trainer.Id = DataStore.Data.VocationalTrainers.Any() ? DataStore.Data.VocationalTrainers.Max(t => t.Id) + 1 : 1;
            DataStore.Data.VocationalTrainers.Add(trainer);
            DataStore.SaveData();
        }

        // Reads a vocational trainer by id
        public static VocationalTrainer? Read(int id)
        {
            return DataStore.Data.VocationalTrainers.FirstOrDefault(t => t.Id == id);
        }

        // Updates an existing vocational trainer
        public static void Update(VocationalTrainer trainer)
        {
            if (trainer == null) return;
            var existing = Read(trainer.Id);
            if (existing != null)
            {
                existing.FirstName = trainer.FirstName;
                existing.LastName = trainer.LastName;
                existing.CompanyId = trainer.CompanyId;
                DataStore.SaveData();
            }
        }

        // Deletes a vocational trainer by id
        public static void Delete(int id)
        {
            var trainer = Read(id);
            if (trainer != null)
            {
                DataStore.Data.VocationalTrainers.Remove(trainer);
                DataStore.SaveData();
            }
        }
    }
}
