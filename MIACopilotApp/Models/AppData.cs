using System.Collections.Generic;

namespace MIACopilotApp.Models
{
    public class AppData
    {
        public List<Company> Companies { get; set; } = new();
        public List<VocationalTrainer> VocationalTrainers { get; set; } = new();
        public List<WorkJournal> WorkJournals { get; set; } = new();
        public List<Apprentice> Apprentices { get; set; } = new();
        public List<Grade>      Grades      { get; set; } = new();
    }
}
