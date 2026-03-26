using System.Linq;

namespace MIACopilotApp.Models
{
    public class Grade
    {
        public int    Id           { get; set; }
        public string Subject      { get; set; } = string.Empty;
        public double Value        { get; set; }
        public int    ApprenticeId { get; set; }
        public DateTime Date       { get; set; } = DateTime.Today;
        public string? Remarks     { get; set; }

        // Creates a new grade
        public static void Create(Grade grade)
        {
            if (grade == null) return;
            grade.Id = DataStore.Data.Grades.Any()
                ? DataStore.Data.Grades.Max(g => g.Id) + 1
                : 1;
            DataStore.Data.Grades.Add(grade);
            DataStore.SaveData();
        }

        // Reads a grade by id
        public static Grade? Read(int id)
            => DataStore.Data.Grades.FirstOrDefault(g => g.Id == id);

        // Updates an existing grade
        public static void Update(Grade grade)
        {
            if (grade == null) return;
            var existing = Read(grade.Id);
            if (existing != null)
            {
                existing.Subject      = grade.Subject;
                existing.Value        = grade.Value;
                existing.ApprenticeId = grade.ApprenticeId;
                existing.Date         = grade.Date;
                existing.Remarks      = grade.Remarks;
                DataStore.SaveData();
            }
        }

        // Deletes a grade by id
        public static void Delete(int id)
        {
            var grade = Read(id);
            if (grade != null)
            {
                DataStore.Data.Grades.Remove(grade);
                DataStore.SaveData();
            }
        }
    }
}
