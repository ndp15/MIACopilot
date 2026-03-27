using MIACopilotApp.Models;
using System.Windows;

namespace MIACopilotApp.Gui.Dialogs
{
    public partial class ApprenticeDialog : Window
    {
        public Apprentice? Result { get; private set; }

        private class ComboItem
        {
            public int?   Id          { get; set; }
            public string DisplayName { get; set; } = "";
        }

        // Add mode
        public ApprenticeDialog()
        {
            InitializeComponent();
            LoadCombos();
        }

        // Edit mode
        public ApprenticeDialog(Apprentice existing)
        {
            InitializeComponent();
            TitleText.Text = "Lernenden bearbeiten";
            Title          = "Lernenden bearbeiten";

            TbFirstName.Text = existing.FirstName;
            TbLastName .Text = existing.LastName;
            LoadCombos(existing.CompanyId, existing.VocationalTrainerId);

            // Keep the original so WorkJournals are preserved
            Result = existing;
        }

        // Loads the company and trainer combo boxes and preselects values when provided
        private void LoadCombos(int? selCompany = null, int? selTrainer = null)
        {
            CbCompany.Items.Add(new ComboItem { Id = null, DisplayName = "(keine)" });
            foreach (var c in DataStore.Data.Companies)
                CbCompany.Items.Add(new ComboItem { Id = c.Id, DisplayName = c.Name });
            CbCompany.SelectedIndex = 0;
            if (selCompany.HasValue)
                SelectById(CbCompany, selCompany.Value);

            CbTrainer.Items.Add(new ComboItem { Id = null, DisplayName = "(keine)" });
            foreach (var t in DataStore.Data.VocationalTrainers)
                CbTrainer.Items.Add(new ComboItem { Id = t.Id, DisplayName = $"{t.FirstName} {t.LastName}" });
            CbTrainer.SelectedIndex = 0;
            if (selTrainer.HasValue)
                SelectById(CbTrainer, selTrainer.Value);
        }

        // Selects an entry in a combo box by id
        private static void SelectById(System.Windows.Controls.ComboBox cb, int id)
        {
            for (int i = 0; i < cb.Items.Count; i++)
            {
                if (cb.Items[i] is ComboItem item && item.Id == id)
                {
                    cb.SelectedIndex = i;
                    return;
                }
            }
        }

        // Validates and saves the apprentice data from the dialog
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbFirstName.Text) ||
                string.IsNullOrWhiteSpace(TbLastName.Text))
            {
                MessageBox.Show("Vorname und Nachname sind Pflichtfelder.",
                    "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int? companyId = (CbCompany.SelectedItem as ComboItem)?.Id;
            int? trainerId = (CbTrainer.SelectedItem as ComboItem)?.Id;

            if (Result != null)
            {
                // Edit mode: update existing object in place
                Result.FirstName           = TbFirstName.Text.Trim();
                Result.LastName            = TbLastName .Text.Trim();
                Result.CompanyId           = companyId;
                Result.VocationalTrainerId = trainerId;
            }
            else
            {
                // Add mode: new object (ID is set by Create())
                Result = new Apprentice
                {
                    FirstName           = TbFirstName.Text.Trim(),
                    LastName            = TbLastName .Text.Trim(),
                    CompanyId           = companyId,
                    VocationalTrainerId = trainerId
                };
            }

            DialogResult = true;
        }

        // Closes the dialog without saving changes
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
