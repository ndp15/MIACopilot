using System;
using System.Linq;

namespace MIACopilotApp.Models
{
    public class WorkJournal
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime DateEntry { get; set; } = DateTime.Today;
        public int ApprenticeId { get; set; }

        // Creates a new work journal entry
        public static void Create(WorkJournal journal)
        {
            if (journal == null) return;
            journal.Id = DataStore.Data.WorkJournals.Any() ? DataStore.Data.WorkJournals.Max(j => j.Id) + 1 : 1;
            DataStore.Data.WorkJournals.Add(journal);
            DataStore.SaveData();
        }

        // Reads a work journal entry by id
        public static WorkJournal? Read(int id)
        {
            return DataStore.Data.WorkJournals.FirstOrDefault(j => j.Id == id);
        }

        // Updates an existing work journal entry
        public static void Update(WorkJournal journal)
        {
            if (journal == null) return;
            var existing = Read(journal.Id);
            if (existing != null)
            {
                existing.Title = journal.Title;
                existing.Content = journal.Content;
                existing.DateEntry = journal.DateEntry;
                existing.ApprenticeId = journal.ApprenticeId;
                DataStore.SaveData();
            }
        }

        // Deletes a work journal entry by id
        public static void Delete(int id)
        {
            var journal = Read(id);
            if (journal != null)
            {
                DataStore.Data.WorkJournals.Remove(journal);
                DataStore.SaveData();
            }
        }
    }
}
